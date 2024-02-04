using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectXTexNet;

namespace TexConverter
{
    public static class ConvertProfiles
    {
        public static ConvertProfile DefaultProfile { get; } =
            new ConvertProfile(name: "DefaultProfile");
        public static ConvertProfile BC1LinearFast { get; } =
            new ConvertProfile(
                name: "BC1 Linear Fast",
                format: DXGI_FORMAT.BC1_UNORM,
                compressFlags: TEX_COMPRESS_FLAGS.DEFAULT | TEX_COMPRESS_FLAGS.SRGB,
                filterFlags: TEX_FILTER_FLAGS.DEFAULT,
                wicFlags: WIC_FLAGS.FORCE_SRGB,
                dDS_FLAGS: DDS_FLAGS.NONE,
                generateMips: true
            );

        public static ConvertProfile BC4Gray { get; } =
            new ConvertProfile(
                name: "BC4 Gray",
                format: DXGI_FORMAT.BC4_UNORM,
                compressFlags: TEX_COMPRESS_FLAGS.PARALLEL,
                filterFlags: TEX_FILTER_FLAGS.LINEAR,
                wicFlags: WIC_FLAGS.DEFAULT_SRGB,
                dDS_FLAGS: DDS_FLAGS.NONE,
                generateMips: true
            );

        public static ConvertProfile BC7LinearFast { get; } =
            new ConvertProfile(
                name: "BC7 Linear Fast",
                format: DXGI_FORMAT.BC7_UNORM,
                compressFlags: TEX_COMPRESS_FLAGS.DEFAULT
                    | TEX_COMPRESS_FLAGS.PARALLEL
                    | TEX_COMPRESS_FLAGS.BC7_QUICK,
                filterFlags: TEX_FILTER_FLAGS.LINEAR,
                wicFlags: WIC_FLAGS.FORCE_SRGB,
                dDS_FLAGS: DDS_FLAGS.NONE,
                generateMips: true
            );
        public static ConvertProfile BC7LinearFine { get; } =
            new ConvertProfile(
                name: "BC7 Linear Fine",
                format: DXGI_FORMAT.BC7_UNORM,
                compressFlags: TEX_COMPRESS_FLAGS.DEFAULT
                    | TEX_COMPRESS_FLAGS.PARALLEL
                    | TEX_COMPRESS_FLAGS.BC7_USE_3SUBSETS,
                filterFlags: TEX_FILTER_FLAGS.LINEAR,
                wicFlags: WIC_FLAGS.FORCE_SRGB,
                dDS_FLAGS: DDS_FLAGS.NONE,
                generateMips: true
            );
        public static ConvertProfile BC7sRgbFast { get; } =
            new ConvertProfile(
                name: "BC7 sRGB Fast",
                format: DXGI_FORMAT.BC7_UNORM_SRGB,
                compressFlags: TEX_COMPRESS_FLAGS.PARALLEL | TEX_COMPRESS_FLAGS.BC7_QUICK,
                filterFlags: TEX_FILTER_FLAGS.LINEAR,
                wicFlags: WIC_FLAGS.NONE | WIC_FLAGS.DEFAULT_SRGB,
                dDS_FLAGS: DDS_FLAGS.NONE | DDS_FLAGS.ALLOW_LARGE_FILES,
                generateMips: true,
                threshold: 0.5f
            );
        public static ConvertProfile BC7sRgbFine { get; } =
            new ConvertProfile(
                name: "BC7 sRGB Fine",
                format: DXGI_FORMAT.BC7_UNORM_SRGB,
                compressFlags: TEX_COMPRESS_FLAGS.PARALLEL | TEX_COMPRESS_FLAGS.BC7_USE_3SUBSETS,
                filterFlags: TEX_FILTER_FLAGS.LINEAR,
                wicFlags: WIC_FLAGS.NONE | WIC_FLAGS.DEFAULT_SRGB,
                dDS_FLAGS: DDS_FLAGS.NONE,
                generateMips: true,
                threshold: 0f
            );
        public static ConvertProfile HighQualityProfile { get; } =
            new ConvertProfile(
                name: "HighQualityProfile",
                format: DXGI_FORMAT.R16G16B16A16_FLOAT,
                compressFlags: TEX_COMPRESS_FLAGS.DEFAULT | TEX_COMPRESS_FLAGS.SRGB,
                filterFlags: TEX_FILTER_FLAGS.DEFAULT,
                wicFlags: WIC_FLAGS.NONE | WIC_FLAGS.DEFAULT_SRGB,
                dDS_FLAGS: DDS_FLAGS.NONE,
                generateMips: true,
                threshold: 0.5f,
                alphaWeight: 0.2f
            );

        // Add more predefined profiles as needed

        // You can also define a method to retrieve all predefined profiles
        public static IEnumerable<ConvertProfile> GetAllProfiles()
        {
            yield return DefaultProfile;
            yield return BC1LinearFast;
            yield return BC4Gray;
            yield return BC7LinearFast;
            yield return BC7LinearFine;
            yield return BC7sRgbFast;
            yield return BC7sRgbFine;
            yield return HighQualityProfile;

            // Add other predefined profiles here
        }
    }
}
