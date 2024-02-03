using DirectXTexNet;

namespace TexConverter
{
    using System.Threading.Tasks;

    public class TextureConverter
    {
        public static async Task ConvertToDdsFileAsync(
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

        public static async Task<byte[]> ConvertToDdsStreamAsync(
            string source,
            ConvertProfile profile
        )
        {
            return await Task.Run(() =>
            {
                return ConvertToDdsStream(source, profile);
            });
        }

        private static void ConvertToDdsFile(
            string source,
            string destination,
            ConvertProfile profile
        )
        {
            using var orig = TexHelper.Instance.LoadFromWICFile(source, profile.WICFlags);

            var index = orig.ComputeImageIndex(0, 0, 0);
        
            using var withMips = orig.GenerateMipMaps(profile.FilterFlags, profile.Levels);

            using var compressed = withMips.Compress(
                index,
                profile.Format,
                profile.CompressFlags,
                profile.Threshold
            );
            compressed.SaveToDDSFile(profile.DDSFlags, destination);
        }

        private static byte[] ConvertToDdsStream(string source, ConvertProfile profile)
        {
            using var orig = TexHelper.Instance.LoadFromWICFile(source, profile.WICFlags);

            using var withMips = orig.GenerateMipMaps(profile.FilterFlags, profile.Levels);

            using var compressed = withMips.Compress(
                profile.Format,
                profile.CompressFlags,
                profile.Threshold
            );
            using var stream = compressed.SaveToDDSMemory(profile.DDSFlags);
            return stream.GetDataFromStream();
        }
    }
}
