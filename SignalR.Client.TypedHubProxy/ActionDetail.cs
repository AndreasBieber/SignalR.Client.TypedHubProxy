using System;

namespace Microsoft.AspNet.SignalR.Client
{
    internal class ActionDetail
    {
        public string MethodName { get; set; }
        public object[] Parameters { get; set; }
        public Type ReturnType { get; set; }
    }
}
