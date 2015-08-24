using System;
using JetBrains.Annotations;

namespace Microsoft.AspNet.SignalR.Client
{
    [PublicAPI]
    internal class ActionDetail
    {
        public string MethodName { get; set; }
        public object[] Parameters { get; set; }
        public Type ReturnType { get; set; }
    }
}