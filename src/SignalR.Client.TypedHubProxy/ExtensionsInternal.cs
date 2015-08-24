using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.AspNet.SignalR.Client
{
    internal static class ExtensionsInternal
    {
        private const string ERR_ACTION_MUST_BE_METHODCALL = "Action must be a method call";
        private const string ERR_CANT_GET_METHODINFO = "Can't get method info of expression.";

        internal static ActionDetail GetActionDetails<T>(this Expression<Action<T>> action)
        {
            if (!(action.Body is MethodCallExpression))
            {
                throw new ArgumentException(ERR_ACTION_MUST_BE_METHODCALL, nameof(action));
            }

            var callExpression = (MethodCallExpression) action.Body;

            var actionDetail = new ActionDetail
            {
                MethodName = callExpression.Method.Name,
                Parameters = callExpression.Arguments.Select(ConvertToConstant).ToArray()
            };

            return actionDetail;
        }

        internal static ActionDetail GetActionDetails<TInput, TResult>(this Expression<Func<TInput, TResult>> action)
        {
            if (!(action.Body is MethodCallExpression))
            {
                throw new ArgumentException(ERR_ACTION_MUST_BE_METHODCALL, nameof(action));
            }

            var callExpression = (MethodCallExpression) action.Body;

            var actionDetail = new ActionDetail
            {
                MethodName = callExpression.Method.Name,
                Parameters = callExpression.Arguments.Select(ConvertToConstant).ToArray(),
                ReturnType = typeof (TResult)
            };

            return actionDetail;
        }

        internal static string GetMethodName(this LambdaExpression lambdaExpression)
        {
            var unaryExpression = (UnaryExpression) lambdaExpression.Body;
            var methodCallExpression = (MethodCallExpression) unaryExpression.Operand;

            if (methodCallExpression.Object == null)
            {
                throw new Exception(ERR_CANT_GET_METHODINFO);
            }

            var methodInfo = (MethodInfo) ((ConstantExpression) methodCallExpression.Object).Value;

            return methodInfo.Name;
        }

        private static object ConvertToConstant(Expression expression)
        {
            var objectMember = Expression.Convert(expression, typeof (object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();

            return getter();
        }
    }
}