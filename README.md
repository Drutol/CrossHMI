# CrossHMI

This project implements simple HMI (Human Machine Interface) for process data in cross-platform fashion. For reference Android mobile application has been implemented.

The application works based upon [OPC-UA-OOI](https://github.com/mpostol/OPC-UA-OOI) library which provides necessary tools to obtain data from network attached devices.

The data itself in this exercised example originates from simulator application which is bundled as a [reference application](https://github.com/mpostol/OPC-UA-OOI/tree/master/Networking/ReferenceApplication) in the above mentioned library.

## Scenario

Currently implemented scenario includes a set of boilers about which data is presented.

Boiler list        |  Boiler device details
:-------------------------:|:-------------------------:
![](https://raw.githubusercontent.com/Drutol/CrossHMI/master/.github/images/boilers.png)  |  ![](https://raw.githubusercontent.com/Drutol/CrossHMI/master/.github/images/details.png)



The details page presents simple user interface mapping selected values in form of text and animated control onto a picture.

## Implementation

### Aplication

#### Overall architecture

The aplication itself is written incroprating cross-platform patterns and techniques like MVVM and dependency injection.

* There's a domain layer composing of `CrossHMI.Models` and `CrossHMI.Interfaces` namespaces housing universal interfaces and classes for storing data.
* Business logic which implements domain interfaces is contained within `CrossHMI.Shared` namespace and it includes both ViewModels used directly by user interface as well as logic for processing asynchnous data obtained through the OPC-UA-OOI library.
* The actual platform project `CrossHMI.Android` implements Android user interface using previously created ViewModels and serves as an entry point for the whole end application.

With this architecture it is easy to expand onto any other platform like Windows, Linux, iOS or even Web (Blazor WebAssembly).

<p align="center">
  <img width="460" height="300" src="https://raw.githubusercontent.com/Drutol/CrossHMI/master/.github/images/arch.png">
</p>


#### OPC-UA-OOI library interfacing

As it's out of scope of the OPC-UA-OOI library it does not provide any easy way to map data onto properties within objects which could be later used for example to display some values to the user. For this reason CrossHMI offers a proxy which will allow to manage the process. The proxy part is easily detachable from the whole project and could be later provided as a separate NuGet package.

Going from the very top level of "device" class definition. Let's assume boiler model we are dealing with:
```cs
public class Boiler : NetworkDeviceBaseWithConfiguration<BoilersConfigurationData>
```
`NetworkDeviceBaseWithConfiguration` class is a helper class for casting configuration data to a proper type, the base class which actually does the heavy handling is `NetworkDeviceBase`. It is responsible for configuring dependent components to provide appropriate data to appropriate instances of our device class.

The class provides the tools to streamline definition process, where providing properties marked with `[ProcessVariable]` attribute is enough to get it working.

```cs
[ProcessVariable] public double PipeX001_FTX001_Output { get; private set; }
[ProcessVariable] public double PipeX001_ValveX001_Input { get; private set; }
```

Obviously it's possible not to use the attributes and do it manually or in runtime using `INetworkDeviceDefinitionBuilder` passed to the `DefineDevice` method of `INetworkDevice` interface.

The whole orchestration process is handled by `NetworkEventsManager` class which instantiates given device classes.

```cs
var boiler = _networkEventsManager.ObtainEventSourceForDevice<Boiler>(RepositoryName)
```

The returned instance will be provided with any new data that appears from repository passed in `RepositoryName`.

There are also other aspects of library configuration that need to be handled, you can find an example in this very repository or directly in the library [documentation](https://commsvr.gitbook.io/ooi/reactive-communication/readmegettingstartedtutorial).


### Networking

For obtaining asynchronous data the application is listening for UDP broadcast packets, as simple as it gets. It's possible to inject other transports from OPC-UA-OOI's SDK.

### Comparison to original library scructure

Underlying library proposes [following architecture](https://commsvr.gitbook.io/ooi/reactive-communication/semanticdata): ![](https://gblobscdn.gitbook.com/assets%2F-LC0CZRWqxXUp1c0m4su%2F-LhqLqzedKjqeUzWKWJu%2F-LEUl1qtjbZoS9c9Na_4%2FDataManagementExternalLibraries.png?alt=media)

Here I'll compare how proposed architecture translates onto the original:

1. The core of the OOI library architecture is `DataManagementSetup` which gathers all dependencies required and initializes them. Respective component which directly inherits from it is called `NetworkEventsManager`. Its purpose is the same as of `DataManagementSetup` plus managing the creation and binding process of device objects. It's part of the CrossHMI SDK.
2. The `Configuration` component is to be injected via `NetworkEventsManager`'s constructor, given the nature of the configuration its form may vary hence actual implementation of `ConfigurationFactoryBase<TConfiguration>` is part of the startup project and is not part of the CrossHMI SDK.
3. `MessageHandling` and `Encoding` components hiding behind `IMessageHandlerFactory` and `IEncodingFactory` interfaces respectively are to be injected to `NetworkEventsManager`, the OOI library provides various implemenations in its SDK.
4. `DataRepository` describes what the final application is doing with the obtained data. The objects that are receiving it are created and bound by the `NetworkEventsManager` in accordance to passed `Configuration`. In case of Azure integration this is the part which then proceeds to funnel the state of the device to the Azure via `AzurePublisher` which is part of the plugin to the CrossHMI SDK. In case of Android it'd be the device classes which are ViewModel's on their own.

## Notes 

Additional NuGet packages used by this project can be obtained from following feed. These are my personal tools used to streamline Xamarin development.
```
https://www.myget.org/F/drutol/api/v3/index.json
```

The libraries in question are descibed [here](https://drutol.github.io/AoLibs/)
