# SignalR.Client.TypedHubProxy
SignalR.Client.TypedHubProxy is a library which extends the SignalR.Client components.

## What can it be used for?
One thing that really bugged me out about SignalR is the lack of strongly typed hub proxies.
This library will enable this feature via interface implementations.

## Get it on NuGet!
    https://www.nuget.org/packages/SignalR.Client.TypedHubProxy
or

    Install-Package SignalR.Client.TypedHubProxy

## LICENSE
[Apache 2.0 License](https://github.com/Gandalis/SignalR.Client.TypedHubProxy/blob/master/LICENSE.md)

## Example
First of all we have to declare the interfaces:

```csharp
public interface IServerHub
{
    void DoSomething();
    void DoSomethingWithParam(int id);
    int DoSomethingWithParamAndResult(int id);
}

public interface IClientContract
{
    void SomeInformation();
    void SomeInformationWithParam(int id);
}
```

After that, we have to implement the ServerHub which implements IServerHub and uses IClientContract:

```csharp
public class ServerHub : Microsoft.AspNet.SignalR.Hub<IClientContract>, IServerHub
{
    public void DoSomething()
    {
        System.Console.WriteLine("DoSomething called.");
    }

    public void DoSomethingWithParam(int id)
    {
        System.Console.WriteLine("DoSomethingWithParam called.");
    }

    public int DoSomethingWithParamAndResult(int id)
    {
        System.Console.WriteLine("DoSomethingWithParamAndResult called.");
        return id;
    }
}
```
At the client, we have to set up the connection: 

```csharp
var hubConnection = new Microsoft.AspNet.SignalR.Client.HubConnection("http://localhost:1337/signalr");
```
### Invocations

The next part is the interesting one - The usage of the strongly typed HubProxy.
To understand exactly what the TypedHubProxy does, here an example of how it is used normally:

```csharp
Microsoft.AspNet.SignalR.Client.IHubProxy hubProxy = hubConnection.CreateHubProxy("serverHub");
hubProxy.Invoke("DoSomething", new object[0]);
```
The part of the Invoke had really bugged me out was the lack of strongly typed invocation.
So SignalR.Client.TypedHubProxy enables strongly typed invocation and much more. The following code sample shows you exactly what can it be used for:
```csharp
IHubProxy<IServerHub, IClientContract> hubProxy = hubConnection.CreateHubProxy<IServerHub, IClientContract>("serverHub");

// Here you can define the method which should be called - strongly typed.
hubProxy.Call(hub => hub.DoSomething());

// For methods with parameters, just handover the parameter.
hubProxy.Call(hub => hub.DoSomethingWithParam(5));

// For methods with a result, you can receive it as expected.
int result = hubProxy.Call<int>(hub => hub.DoSomethingWithParamAndResult(5));
```
Async calls are also possible:
```csharp
hubProxy.CallAsync(hub => hub.DoSomething());
hubProxy.CallAsync(hub => hub.DoSomethingWithParam(5));
System.Threading.Tasks.Task<int> asyncResult = hubProxy.CallAsync(hub => hub.DoSomethingWithParamAndResult(5));
int result = asyncResult.Result; // or just: int result = hubProxy.CallAsync(hub => hub.DoSomethingWithParamAndResult(5)).Result;
```

### Subscriptions
The old way:
```csharp
Microsoft.AspNet.SignalR.Client.IHubProxy hubProxy = hubConnection.CreateHubProxy("serverHub");
hubProxy.On<int>("SomeInformationWithParam", i => System.Console.WriteLine("Got new information: {0}", i));
```

And the new way with SignalR.Client.TypedHubProxy:
```csharp
IHubProxy<IServerHub, IClientContract> hubProxy = hubConnection.CreateHubProxy<IServerHub, IClientContract>("serverHub");
hubProxy.SubscribeOn(hub => hub.SomeInformation, () => System.Console.WriteLine("Got some new information."));
hubProxy.SubscribeOn<int>(hub => hub.SomeInformationWithParam, i => System.Console.WriteLine("Got new information: {0}", i));
```

It's also possible to define a condition on the subscription, so that you will be only called if the condition is true:
```csharp
IHubProxy<IServerHub, IClientContract> hubProxy = hubConnection.CreateHubProxy<IServerHub, IClientContract>("serverHub");
hubProxy.SubscribeOn<int>(hub => hub.SomeInformationWithParam, i => i == 5, i => System.Console.WriteLine("Got new information where data == 5"));
```

### Observable
```csharp
System.IObservable<int> observableResult = hubProxy.Observe<int>(hub => hub.SomeInformationWithParam);
```