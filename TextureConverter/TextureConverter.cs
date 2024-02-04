using DirectXTexNet;

namespace TexConverter
{
    using System.IO;
    using System.Threading.Tasks;
    using SharpDX.Direct3D;
    using SharpDX.Direct3D11;

    public class TextureConverter : IDisposable
    {
        private Device _Device { get; set; }
        public static TextureConverter? Instance { get; set; }

        public TextureConverter()
        {
            _Device = new Device(DriverType.Hardware, DeviceCreationFlags.None);
            Instance = this;
        }

        public async Task ConvertToDdsFileAsync(
            string source,
            string destination,
            ConvertProfile profile
        )
        {
            await Task.Run(() =>
            {
                ConvertToDdsFile(source, destination, profile);
            });
        }

        public async Task<byte[]> ConvertToDdsStreamAsync(string source, ConvertProfile profile)
        {
            return await Task.Run(() =>
            {
                return ConvertToDdsStream(source, profile);
            });
        }

        private void ConvertToDdsFile(string source, string destination, ConvertProfile profile)
        {
            // Access the native pointer of the SharpDX.Direct3D11 device
            IntPtr devicePointer = _Device.NativePointer;
           
            // Load Png/jpg images with DirectXTexNet.
            using var orig = TexHelper.Instance.LoadFromWICFile(source, profile.WICFlags);
           

            using var withMips = orig.GenerateMipMaps(profile.FilterFlags, profile.Levels);

            using var compressed = orig.Compress(
                devicePointer,
                profile.Format,
                profile.CompressFlags,
                profile.Threshold
            );
            compressed.SaveToDDSFile(profile.DDSFlags, destination);
        }

        private byte[] ConvertToDdsStream(string source, ConvertProfile profile)
        {
            using var orig = TexHelper.Instance.LoadFromWICFile(source, profile.WICFlags);

            using var withMips = orig.GenerateMipMaps(profile.FilterFlags, profile.Levels);
            IntPtr devicePointer = _Device.NativePointer;
            using var compressed = withMips.Compress(
                devicePointer,
                profile.Format,
                profile.CompressFlags,
                profile.Threshold
            );
            using var stream = compressed.SaveToDDSMemory(profile.DDSFlags);
            return stream.GetDataFromStream();
        }

        public void Dispose()
        {
            _Device?.Dispose();
            _Device= null;
            GC.SuppressFinalize(this);
        }
    }
}
