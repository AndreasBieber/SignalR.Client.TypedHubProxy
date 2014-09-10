using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace Microsoft.AspNet.SignalR.Client
{
    public static class HubConnectionExtension
    {
        private const string ERR_INACCESSABLE = "\"{0}\" is inaccessible from outside due to its protection level.";

        private static readonly Dictionary<Type, Type> _compiledProxyClasses = new Dictionary<Type, Type>();

        public static T CreateHubProxy<T>(this HubConnection connection, string hubName)
        {
            Type interfaceType = typeof(T);

            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException(string.Format("\"{0}\" is not an interface.", interfaceType.Name));
            }

            if (!interfaceType.IsVisible)
            {
                throw new ConstraintException(string.Format(ERR_INACCESSABLE, interfaceType.FullName.Replace("+", ".")));
            }

            if (_compiledProxyClasses.ContainsKey(typeof(T)))
            {
                return (T)Activator.CreateInstance(_compiledProxyClasses[typeof(T)], connection.CreateHubProxy(hubName));
            }

            MethodInfo[] methodInfos = interfaceType.GetMethods();
            var assembliesToReference = new List<string>
            {
                interfaceType.Assembly.Location,
                typeof (IHubProxy).Assembly.Location
            };

            foreach (MethodInfo methodInfo in methodInfos)
            {
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();

                if (methodInfo.ReturnType != typeof(Task) && methodInfo.ReturnType.BaseType != typeof(Task))
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
                            methodInfo.ReturnType == typeof(void) ? string.Empty : string.Format("<{0}>", methodInfo.ReturnType.FullName.Replace("+", "."))));
                }

                if (!methodInfo.ReturnType.IsVisible)
                {
                    throw new ConstraintException(string.Format(ERR_INACCESSABLE, methodInfo.ReturnType.FullName.Replace("+", ".")));
                }

                ParameterInfo noPublicParam = parameterInfos.FirstOrDefault(p => !p.ParameterType.IsVisible);
                if (noPublicParam != null)
                {
                    throw new ConstraintException(string.Format(ERR_INACCESSABLE, noPublicParam.ParameterType.FullName.Replace("+", ".")));
                }

                List<string> assemblies = parameterInfos.Select(p => p.ParameterType.Assembly.Location).Distinct().ToList();
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
            var compilerParameters = new CompilerParameters { GenerateInMemory = true, GenerateExecutable = false };

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

            Type generatedProxyClassType = compiledAssembly.GetType(string.Concat(interfaceType.Namespace, ".", interfaceType.Name, "Proxy"));

            _compiledProxyClasses.Add(interfaceType, generatedProxyClassType);

            return (T)Activator.CreateInstance(generatedProxyClassType, connection.CreateHubProxy(hubName));
        }
    }
}
