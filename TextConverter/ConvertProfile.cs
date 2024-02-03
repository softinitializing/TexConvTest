using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectXTexNet;

namespace TexConverter
{
    public struct ConvertProfile(
        string name = "Default",
        DXGI_FORMAT format = DXGI_FORMAT.R32G32B32A32_FLOAT,
        TEX_COMPRESS_FLAGS compressFlags = TEX_COMPRESS_FLAGS.DEFAULT | TEX_COMPRESS_FLAGS.PARALLEL,
        TEX_FILTER_FLAGS filterFlags = TEX_FILTER_FLAGS.LINEAR,
        WIC_FLAGS wicFlags = WIC_FLAGS.NONE,
        DDS_FLAGS dDS_FLAGS = DDS_FLAGS.NONE,
        bool generateMips = true,
        int levels = 0,
        float threshold = 0.5f,
        float alphaWeight = 0
    )
    {
        public string Name { get; set; } = name;
        public DXGI_FORMAT Format { get; set; } = format;
        public TEX_COMPRESS_FLAGS CompressFlags { get; set; } = compressFlags;
        public TEX_FILTER_FLAGS FilterFlags { get; set; } = filterFlags;
        public WIC_FLAGS WICFlags { get; set; } = wicFlags;
        public DDS_FLAGS DDSFlags { get; set; } = dDS_FLAGS;
        public bool GenerateMips { get; set; } = generateMips;
        public int Levels { get; set; } = levels;
        public float Threshold { get; set; } = threshold;
        public float AlphaWeight { get; set; } = alphaWeight;
    }
}
