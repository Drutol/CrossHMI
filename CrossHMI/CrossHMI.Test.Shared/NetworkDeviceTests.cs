using System.Linq;
using AoLibs.Adapters.Core.Interfaces;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.BL;
using CrossHMI.Shared.BL.Consumer;
using CrossHMI.Shared.Configuration;
using CrossHMI.Shared.Devices;
using CrossHMI.Test.Shared.Devices;
using CrossHMI.Test.Shared.Helpers;
using Moq;
using UAOOI.Networking.Encoding;
using UAOOI.Networking.UDPMessageHandler;
using Xunit;

namespace CrossHMI.Test.Shared
{
    public class NetworkDeviceTests
    {
        private const string Repository1 = "BoilersArea_Boiler #1";

        private readonly NetworkEventsManager _networkEventsManager;

        public NetworkDeviceTests()
        {
            var configurationFactory = new ConfigurationFactory(new ConfigurationResourcesProvider(),
                new Mock<ILogAdapter<ConfigurationFactory>>().Object);

            _networkEventsManager = new NetworkEventsManager(
                new ConsumerBindingFactory(),
                configurationFactory,
                configurationFactory,
                new MessageHandlerFactory(),
                new EncodingFactoryBinarySimple(),
                new Mock<IDispatcherAdapter>().Object,
                new Mock<ILogAdapter<NetworkEventsManager>>().Object);

            _networkEventsManager.Initialize().GetAwaiter().GetResult();
        }

        [Fact]
        public void TestDeviceCreation()
        {
            //Arrange
            var deviceMock = new Mock<Boiler>();
            var loggerMock =
                new Mock<ILogAdapter<NetworkEventsManager.NetworkDeviceDefinitionBuilder<
                    Boiler>>>();
            NetworkEventsManager.NetworkDeviceDefinitionBuilder<Boiler>
                .OverrideDefaultDeviceFactory(() => deviceMock.Object);
            NetworkEventsManager.NetworkDeviceDefinitionBuilder<Boiler>
                .OverrideDefaultLogger(loggerMock.Object);

            //Act
            var boilerUpdateSource = _networkEventsManager.ObtainEventSourceForDevice<Boiler>(Repository1);

            //Assert
            Assert.NotNull(boilerUpdateSource);
            deviceMock.Verify(boiler =>
                boiler.DefineDevice(It.IsAny<INetworkDeviceDefinitionBuilder>()));
            deviceMock.Verify(boiler => boiler.AssignRepository(It.Is<string>(s => s == Repository1)));
        }

        [Fact]
        public void TestDeviceDefinition()
        {
            //Arrange
            var deviceMock = new TestBoiler();
            var loggerMock =
                new Mock<ILogAdapter<NetworkEventsManager.NetworkDeviceDefinitionBuilder<
                    TestBoiler>>>();
            NetworkEventsManager.NetworkDeviceDefinitionBuilder<TestBoiler>
                .OverrideDefaultDeviceFactory(() => deviceMock);
            NetworkEventsManager.NetworkDeviceDefinitionBuilder<TestBoiler>
                .OverrideDefaultLogger(loggerMock.Object);

            //Act
            var boilerUpdateSource = _networkEventsManager.ObtainEventSourceForDevice<TestBoiler>(Repository1);
            boilerUpdateSource.Device.ProcessPropertyUpdate<double>(nameof(TestBoiler.CCX001_ControlOut), 1);
            boilerUpdateSource.Device.ProcessPropertyUpdate<double>(nameof(TestBoiler.CCX001_Input1), 1);
            boilerUpdateSource.Device.ProcessPropertyUpdate<double>(nameof(TestBoiler.CCX001_Input2), 1);
            boilerUpdateSource.Device.ProcessPropertyUpdate<double>("CCX001_Input3", 1);

            //Assert
            Assert.Equal(new[] {"CCX001_ControlOut"}, deviceMock.ChangedProperties.ToArray());
            Assert.Equal(new[] {"CCX001_ControlOut", "CCX001_Input1", "CCX001_Input2", "CCX001_Input3"},
                deviceMock.UpdatedProperties.ToArray());
            Assert.NotNull(deviceMock.AssignedExtension);
            Assert.Equal(deviceMock.AssignedExtension.Repository, Repository1);
        }
    }
}