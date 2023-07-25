using OpenTK.Audio.OpenAL;

namespace OpenAL
{
    /// <summary>
    /// Represents the OpenAL context.
    /// </summary>
    public class ContextAudio : IDisposable
    {
        public string Version    { get => "Version: " + AL.Get(ALGetString.Version); }
        public string Vendor     { get => "Vendor: " + AL.Get(ALGetString.Vendor); }
        public string Renderer   { get => "Renderer: " + AL.Get(ALGetString.Renderer); }

        /// <summary>
        /// OpenAL context.
        /// </summary>
        /// <value></value>
        public static ALContext Context { get; private set; }


        /// <summary>
        /// Default audio device.
        /// </summary>
        /// <value></value>
        //private ALDevice DeviceStandard;

        public int IndexDevice { get; set; } = 0;


        public readonly Dictionary<int, ALDevice> Devices = new();

        public readonly List<string> NamesDevices = new();

        public string[] Extensions
        {
            get
            {
                var result = AL.Get(ALGetString.Extensions).Split(" ").ToArray();
                if(result.Length < 0)
                {
                    result = new string[0];
                    Console.WriteLine("Extensions Error, The device has a problem");
                }
                return result;
            }
        }


        public List<string> AllInputDevices
            => ALC.GetStringList(GetEnumerationStringList.CaptureDeviceSpecifier).ToList();

        public string CurrentInputDevice
            => ALC.GetString(Devices[IndexDevice], AlcGetString.CaptureDefaultDeviceSpecifier);


        public List<string> AllOutputDevices
            => ALC.GetString(AlcGetStringList.AllDevicesSpecifier).ToList();


        public string CurrentOutputDevice
            => ALC.GetString(Devices[IndexDevice], AlcGetString.DeviceSpecifier);

        /// <summary>
        /// Starts the OpenAL context.
        /// </summary>
        public unsafe ContextAudio(string deviceName = "")
        {
            var DeviceStandard = ALC.OpenDevice(string.IsNullOrEmpty(deviceName) ? null : deviceName);
            Context = ALC.CreateContext(DeviceStandard, (int*)null);
            CheckContext();

            Devices.Add(0, DeviceStandard);

            for(int i = 0; i < AllOutputDevices.Count; i++)
            {
                var _device = ALC.OpenDevice(AllOutputDevices[i]);

                if (_device.Handle == 0)
                    Console.WriteLine($"Error in Open OpenAL Device {ALC.GetError(ALDevice.Null)}");

                Devices.Add(i + 1, _device);
            }
        }
        private void CheckContext()
        {
            if (!ALC.MakeContextCurrent(Context))
                throw new Exception($"Failed to create context.");
        }
        public unsafe void SelectDeviceIndex(int indexDevice)
        {
            IndexDevice = indexDevice;

            ALC.DestroyContext(Context);
            Context = ALC.CreateContext(Devices[IndexDevice], (int*)null);
            CheckContext();
        }

        /// <summary>
        /// Delete the OpenAL context.
        /// </summary>
        public void Dispose()
        {
            ALC.CloseDevice(Devices[IndexDevice]);
            ALC.DestroyContext(Context);
        }
    }
}