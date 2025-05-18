using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HidSharp;

namespace ClientApp.Services
{
    public class DeviceInfo
    {
        public string DevicePath { get; init; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int VendorId { get; init; }
        public int ProductId { get; init; }

        public override bool Equals(object? obj)
        {
            if (obj is not DeviceInfo other) return false;
            return DevicePath == other.DevicePath &&
                   VendorId == other.VendorId &&
                   ProductId == other.ProductId;
        }

        public override int GetHashCode() =>
            HashCode.Combine(DevicePath, VendorId, ProductId);
    }

    public class DeviceManager : IDisposable
    {
        private readonly List<DeviceInfo> _devices = new();
        private CancellationTokenSource? _cts;
        private Task? _monitoringTask;

        public event EventHandler<IReadOnlyList<DeviceInfo>>? DevicesChanged;

        public IReadOnlyList<DeviceInfo> Devices
        {
            get
            {
                lock (_devices)
                {
                    return _devices.ToList();
                }
            }
        }

        public async Task InitializeAsync()
        {
            _cts = new CancellationTokenSource();
            await DetectConnectedDevicesAsync();
            _monitoringTask = MonitorDevicesAsync(_cts.Token);
        }

        private async Task MonitorDevicesAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(2000, token);
                await DetectConnectedDevicesAsync();
            }
        }

        private Task DetectConnectedDevicesAsync()
        {
            var foundDevices = new List<DeviceInfo>();
            try
            {
                var deviceList = DeviceList.Local;
                var hidDevices = deviceList.GetHidDevices();

                foreach (var device in hidDevices)
                {
                    foundDevices.Add(new DeviceInfo
                    {
                        DevicePath = device.DevicePath,
                        ProductName = device.GetProductName() ?? string.Empty,
                        VendorId = device.VendorID,
                        ProductId = device.ProductID
                    });
                }
            }
            catch (Exception)
            {
                // Optionally handle/log errors here
            }

            bool changed;
            lock (_devices)
            {
                changed = !foundDevices.SequenceEqual(_devices);

                if (changed)
                {
                    _devices.Clear();
                    _devices.AddRange(foundDevices);
                }
            }

            if (changed)
            {
                DevicesChanged?.Invoke(this, Devices);
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _monitoringTask?.Wait();
                _cts.Dispose();
                _cts = null;
            }
        }
    }
}
