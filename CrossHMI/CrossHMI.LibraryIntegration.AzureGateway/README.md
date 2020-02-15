# CrossHMI.LibraryIntegration.AzureGateway

This component builds on top of `CrossHMI.LibraryIntegration` allowing easy routage of data to Azure IoT Hub, which can be later propagated to Azure IoT Central for example.

## Data Publishing

Data is published by `AzurePublisher` in specified intervals, it is assumed that asynchonous data is gathered within device object and sent in form of batch payload to Azure. Sending every value update is not viable solution due to Azure specification as well as message pricing.

The publisher serves as a gateway for various devices which can be added or removed on the fly. In the exercised sample there are two devices which are associated with two different independent simulators.

## How to

> Sample on how to integrate can be found in `CrossHMI.AzureGatewayService` on this very repository.

Basically, your `NetworkDevice` class needs to additionaly implement `IAzureEnabledNetworkDevice` interface.

```cs
public class Boiler : NetworkDeviceBase, IAzureEnabledNetworkDevice
```

This interface will:
* Request `IAzureDeviceParameters` providing your azure credentials and other various Azure tidbits.
* Provide you with direct `DeviceClient` instace allowing to work with Azure directly in custom scenarios.
* Require `CreateMessagePayload` method which should create the payload that will be sent to Azure. In the very basic case it'd be just JSON serialization:
    ```cs
    public string CreateMessagePayload()
    {
        return JsonConvert.SerializeObject(this);
    }
    ```

Once you have your device class all you need is to initialize the library and register the device for publishing:

```cs
await _networkEventsManager.Initialize();

var boilerSource =
    _networkEventsManager.ObtainEventSourceForDevice(
        repository: "BoilersArea_Boiler #1",
        _serviceProvider.GetService<Boiler>);

await _azurePublisher.RegisterDeviceForPublishingAsync(boilerSource.Device);
```

One final piece is to actually start the publisher. This is achieved with `StartAsync` method present on `IAzurePublisher`. This awaited method won't exit until cancelled with passed `CancellationToken` it is advised to run it on separate Task/Thread. 

```cs
await _azurePublisher.StartAsync(CancellationToken.None);
```

### Logging

Optional logging is available with `Microsoft.Extensions.Logging.Abstractions` you can pass your implementations to `AzurePublisher` constructor. It will log ecountered errors and warnings as well as some debug data.