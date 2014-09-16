using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.CSharp;

namespace Microsoft.AspNet.SignalR.Client
{
    public static partial class TypedHubProxyExtensions
    {
        private const string ERR_INACCESSABLE = "\"{0}\" is inaccessible from outside due to its protection level.";

        private static readonly Dictionary<Type, Type> _compiledProxyClasses = new Dictionary<Type, Type>();

        internal static IHubProxy GetHubProxy(this HubConnection hubConnection, string hubName)
        {
            FieldInfo hubsField = hubConnection.GetType()
                .GetField("_hubs", BindingFlags.NonPublic | BindingFlags.Instance);

            if (hubsField == null)
            {
                throw new ConstraintException("Couldn't find \"_hubs\" field inside of the HubConnection.");
            }

            var hubs = (Dictionary<string, HubProxy>) hubsField.GetValue(hubConnection);

            if (hubs.ContainsKey(hubName))
            {
                return hubs[hubName];
            }

            return null;
        }

        /// <summary>
        ///     Creates a strongly typed proxy for the hub with the specified name.
        /// </summary>
        /// <param name="connection">The <see cref="T:Microsoft.AspNet.SignalR.Client.HubConnection" />HubConnection.</param>
        /// <param name="hubName">The name of the hub.</param>
        /// <typeparam name="TServerHubInterface"></typeparam>
        /// <typeparam name="TClientInterface"></typeparam>
        public static ITypedHubProxy<TServerHubInterface, TClientInterface> CreateHubProxy
            <TServerHubInterface, TClientInterface>(this HubConnection connection,
                string hubName)
            where TServerHubInterface : class
            where TClientInterface : class
        {
            Type typedHubProxy = typeof (TypedHubProxy<,>).MakeGenericType(typeof (TServerHubInterface),
                typeof (TClientInterface));
            return
                (ITypedHubProxy<TServerHubInterface, TClientInterface>)
                    Activator.CreateInstance(typedHubProxy, BindingFlags.NonPublic | BindingFlags.Instance, null,
                        new object[] {connection, hubName}, null);
        }

        /// <summary>
        ///     Creates a typed hubproxy of type T.
        /// </summary>
        /// <typeparam name="T">The interface to proxy.</typeparam>
        /// <param name="connection">The HubConnection.</param>
        /// <param name="hubName">The name of the hub.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ConstraintException"></exception>
        /// <returns>Returns the typed hubproxy.</returns>
        [Obsolete(
            "This method will be removed in the next version of SignalR.Client.TypedHubProxy. Please use hubConnection.CreateHubProxy<TServerHubInterface, TClientInterface> instead.",
            false)]
        public static T CreateHubProxy<T>(this HubConnection connection, string hubName) where T : class
        {
            Type interfaceType = typeof (T);

            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException(string.Format("\"{0}\" is not an interface.", interfaceType.Name));
            }

            if (!interfaceType.IsVisible)
            {
                throw new ConstraintException(string.Format(ERR_INACCESSABLE, interfaceType.FullName.Replace("+", ".")));
            }

            if (_compiledProxyClasses.ContainsKey(typeof (T)))
            {
                return
                    (T) Activator.CreateInstance(_compiledProxyClasses[typeof (T)], connection.CreateHubProxy(hubName));
            }

            MethodInfo[] methodInfos = interfaceType.GetMethods();
            var assembliesToReference = new List<string>
            {
                Assembly.GetExecutingAssembly().Location,
                interfaceType.Assembly.Location,
                typeof (IHubProxy).Assembly.Location
            };

            foreach (MethodInfo methodInfo in methodInfos)
            {
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();

                if (methodInfo.ReturnType != typeof (Task) && methodInfo.ReturnType.BaseType != typeof (Task))
                {
                    if (methodInfo.DeclaringType == null)
                    {
                        throw new ConstraintException(string.Format("DeclaringType is null."));
                    }

                    string methodParams = string.Join(", ",
                        parameterInfos.Select(
                            pi => string.Format("{0} {1}", pi.ParameterType.Name, pi.ParameterType.Name)));

                    throw new ConstraintException(
                        string.Format(
                            "The returntype of {0}.{1}({2}) must be System.Threading.Tasks.Task{3}.",
                            methodInfo.DeclaringType.FullName.Replace("+", "."),
                            methodInfo.Name,
                            methodParams,
                            methodInfo.ReturnType == typeof (void)
                                ? string.Empty
                                : string.Format("<{0}>", methodInfo.ReturnType.FullName.Replace("+", "."))));
                }

                if (!methodInfo.ReturnType.IsVisible)
                {
                    throw new ConstraintException(string.Format(ERR_INACCESSABLE,
                        methodInfo.ReturnType.FullName.Replace("+", ".")));
                }

                ParameterInfo noPublicParam = parameterInfos.FirstOrDefault(p => !p.ParameterType.IsVisible);
                if (noPublicParam != null)
                {
                    throw new ConstraintException(string.Format(ERR_INACCESSABLE,
                        noPublicParam.ParameterType.FullName.Replace("+", ".")));
                }

                List<string> assemblies =
                    parameterInfos.Select(p => p.ParameterType.Assembly.Location).Distinct().ToList();
                assemblies.Add(methodInfo.ReturnType.Assembly.Location);

                foreach (string assembly in assemblies)
                {
                    if (!assembliesToReference.Contains(assembly))
                    {
                        assembliesToReference.Add(assembly);
                    }
                }
            }

            var template = new InterfaceHubProxyTemplate
            {
                Interface = interfaceType
            };

            string code = template.TransformText();

            var codeProvider = new CSharpCodeProvider();
            var compilerParameters = new CompilerParameters {GenerateInMemory = true, GenerateExecutable = false};

            foreach (string assemblyToReference in assembliesToReference)
            {
                compilerParameters.ReferencedAssemblies.Add(assemblyToReference);
            }

            CompilerResults results = codeProvider.CompileAssemblyFromSource(compilerParameters, code);

            if (results.Errors.HasErrors)
            {
                throw new Exception("Unknown error occured during proxy generation: " + Environment.NewLine +
                                    string.Join(Environment.NewLine,
                                        results.Errors.OfType<CompilerError>().Select(ce => ce.ToString())));
            }

            Assembly compiledAssembly = results.CompiledAssembly;

            Type generatedProxyClassType =
                compiledAssembly.GetType(string.Concat(interfaceType.Namespace, ".", interfaceType.Name, "Proxy"));

            _compiledProxyClasses.Add(interfaceType, generatedProxyClassType);

            return (T) Activator.CreateInstance(generatedProxyClassType, connection.CreateHubProxy(hubName));
        }
    }
}