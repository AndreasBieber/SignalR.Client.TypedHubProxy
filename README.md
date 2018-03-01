# SignalR.Client.TypedHubProxy
SignalR.Client.TypedHubProxy is a library which extends the SignalR.Client components.

## What can it be used for?
One thing that really bugged me out about SignalR is the lack of strongly typed hub proxies.
This library will enable this feature via interface implementations.

## Get it on NuGet!
    https://www.nuget.org/packages/SignalR.Client.TypedHubProxy
or

    Install-Package SignalR.Client.TypedHubProxy

## Example
First of all we have to declare the interfaces:

```csharp
public interface IServerHub
{
    void DoSomething();
    void DoSomethingWithParam(int id);
    System.Threading.Tasks.Task DoSomethingAsync();
    int DoSomethingWithParamAndResult(int id);
}

public interface IClientContract
{
    void SomeInformation();
    void SomeInformationWithParam(int id);
    void SomeInformationWithText(string text);
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

    public System.Threading.Tasks.Task DoSomethingAsync()
    {
        return System.Threading.Tasks.Task.Run(
            () => System.Console.WriteLine("DoSomethingAsync called."));
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
To understand exactly what the TypedHubProxy and ObservableHubProxy do, here an example of how it is used normally:

```csharp
Microsoft.AspNet.SignalR.Client.IHubProxy hubProxy = hubConnection.CreateHubProxy("serverHub");
hubProxy.Invoke("DoSomething", new object[0]);
```
What really bugged me out was the lack of strongly typed invocation.
So SignalR.Client.TypedHubProxy enables strongly typed invocation and much more. The following code sample shows you exactly what can it be used for:

```csharp
await hubProxy.CallAsync(hub => hub.DoSomething());
await hubProxy.CallAsync(hub => hub.DoSomethingWithParam(5));
await hubProxy.CallAsync(hub => hub.DoSomethingAsync());
int asyncResult = await hubProxy.CallAsync(hub => hub.DoSomethingWithParamAndResult(5));
```

Invocations on IObservableHubProxy work exactly the same way. The only difference is how
to create that type of proxy:
```csharp
IObservableHubProxy<IServerHub, IClientContract> hubProxy = hubConnection.CreateObservableHubProxy<IServerHub, IClientContract>("serverHub");
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

With IObservableHubProxy you can treat your subscriptions as IObservable<T>
(in other words you can treat them as streams of data that is pushed to you).
It means that you can do with them anything that
[Reactive Extensions](https://rx.codeplex.com/) provide for you.
```csharp
IObservableHubProxy<IServerHub, IClientContract> hubProxy =
    hubConnection.CreateObservableHubProxy<IServerHub, IClientContract>("serverHub");
IObservable<int> ids = hubProxy.Observe<int>(hub => hub.SomeInformationWithParam);
IObservable<string> texts = hubProxy.Observe<string>(hub => hub.SomeInformationWithText);
IDisposable subscription = ids.Where(i => i >= 5)
    .CombineLatest(texts.Where(text => !String.IsNullOrEmpty(text)), (id, text) => new { id, text })
    .Subscribe(x => System.Console.WriteLine("Latest id = {0} and text = {1}", x.id, x.text));
...
// Unsubscribe
subscription.Dispose();
```
