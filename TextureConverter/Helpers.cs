using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DirectXTexNet;

namespace TexConverter
{
    public static class Helpers
    {
        public static byte[] ConvertTo32(IntPtr originalPixels, int pixelCount)
        {
            // Calculate the size of the new byte array (4 bytes per pixel for R8G8B8A8 format)
            int newSize = pixelCount * 4;

            // Create a new byte array to hold the converted pixels
            byte[] rgbaPixels = new byte[newSize];

            // Copy the original pixels to the new array, converting each 8-bit pixel to R8G8B8A8 format
            unsafe
            {
                byte* src = (byte*)originalPixels.ToPointer();
                fixed (byte* dest = rgbaPixels)
                {
                    byte* pDest = dest;
                    for (int i = 0; i < pixelCount; i++)
                    {
                        byte pixelValue = *src;

                        // Set R, G, B channels to the same value as the original pixel
                        pDest[0] = pDest[1] = pDest[2] = pixelValue;

                        // Set alpha channel to 255 (fully opaque)
                        pDest[3] = 255;

                        // Move to the next pixel
                        src++;
                        pDest += 4;
                    }
                }
            }

            
            return rgbaPixels;
        }
        static MemoryStream ConvertMemoryStream(UnmanagedMemoryStream unmanagedStream, bool isRonly = false)
        {
            byte[] buffer = new byte[unmanagedStream.Length];
            unmanagedStream.Read(buffer, 0, buffer.Length);

            if (isRonly)
            {
                // Create a new buffer to represent a 3-channel RGB image
                byte[] rgbBuffer = new byte[buffer.Length * 3]; // Three times larger
                for (int i = 0; i < buffer.Length; i++)
                {
                    // Copy R channel data to all three channels (R, G, B)
                    rgbBuffer[i * 3] = buffer[i];     // R channel
                    rgbBuffer[i * 3 + 1] = buffer[i]; // G channel (same as R)
                    rgbBuffer[i * 3 + 2] = buffer[i]; // B channel (same as R)
                }

                return new MemoryStream(rgbBuffer);
            }

            return new MemoryStream(buffer);
        }
        public static IntPtr ToUnmanegedPointer(this byte[] data)
        {
            // Pin the managed array in memory
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            // Get the address of the pinned object
            IntPtr ptr = handle.AddrOfPinnedObject();

            // Free the GCHandle once you're done with the unmanaged memory
            // This allows the garbage collector to move the array again
            // after the GCHandle is released
            handle.Free();

            return ptr;
        }
        public static byte[] GetDataFromStream(this UnmanagedMemoryStream unmanagedStream)
        {
            // Check if the stream is not null and is readable
            if (unmanagedStream == null || !unmanagedStream.CanRead)
            {
                throw new ArgumentException("Invalid UnmanagedMemoryStream or not readable.");
            }

            // Create a byte array to hold the stream data
            byte[] buffer = new byte[unmanagedStream.Length];

            // Read the data from the stream into the byte array
            unmanagedStream.Read(buffer, 0, buffer.Length);

            return buffer;
        }
        public static bool IsSingleChannelImage(DXGI_FORMAT format)
        {
            return format switch
            {
                DXGI_FORMAT.R32_FLOAT => true,
                DXGI_FORMAT.R32_UINT => true,
                DXGI_FORMAT.R32_SINT => true,
                DXGI_FORMAT.R16_FLOAT => true,
                DXGI_FORMAT.R16_UNORM => true,
                DXGI_FORMAT.R16_UINT => true,
                DXGI_FORMAT.R16_SNORM => true,
                DXGI_FORMAT.R16_SINT => true,
                DXGI_FORMAT.R8_UNORM => true,
                DXGI_FORMAT.R8_UINT => true,
                DXGI_FORMAT.R8_SNORM => true,
                DXGI_FORMAT.R8_SINT => true,
                DXGI_FORMAT.A8_UNORM => true,
                DXGI_FORMAT.R1_UNORM => true,
                DXGI_FORMAT.BC4_UNORM => true,
                DXGI_FORMAT.BC4_SNORM => true,
                //TODO Add other single-channel formats here if necessary.
                _ => false,
            };
        }

        public static int Depth(this DXGI_FORMAT format)
        {
            switch (format)
            {
                case DXGI_FORMAT.R32G32B32A32_TYPELESS:
                case DXGI_FORMAT.R32G32B32A32_FLOAT:
                case DXGI_FORMAT.R32G32B32A32_UINT:
                case DXGI_FORMAT.R32G32B32A32_SINT:
                    return 128;

                case DXGI_FORMAT.R16G16B16A16_TYPELESS:
                case DXGI_FORMAT.R16G16B16A16_FLOAT:
                case DXGI_FORMAT.R16G16B16A16_UNORM:
                case DXGI_FORMAT.R16G16B16A16_UINT:
                case DXGI_FORMAT.R16G16B16A16_SNORM:
                case DXGI_FORMAT.R16G16B16A16_SINT:
                    return 64;

                case DXGI_FORMAT.R32G32_TYPELESS:
                case DXGI_FORMAT.R32G32_FLOAT:
                case DXGI_FORMAT.R32G32_UINT:
                case DXGI_FORMAT.R32G32_SINT:
                    return 64;

                case DXGI_FORMAT.R32G8X24_TYPELESS:
                case DXGI_FORMAT.D32_FLOAT_S8X24_UINT:
                case DXGI_FORMAT.R32_FLOAT_X8X24_TYPELESS:
                case DXGI_FORMAT.X32_TYPELESS_G8X24_UINT:
                    return 64;

                case DXGI_FORMAT.R10G10B10A2_TYPELESS:
                case DXGI_FORMAT.R10G10B10A2_UNORM:
                case DXGI_FORMAT.R10G10B10A2_UINT:
                    return 32;

                case DXGI_FORMAT.R8G8_TYPELESS:
                case DXGI_FORMAT.R8G8_UNORM:
                case DXGI_FORMAT.R8G8_UINT:
                case DXGI_FORMAT.R8G8_SNORM:
                case DXGI_FORMAT.R8G8_SINT:
                    return 16;

                case DXGI_FORMAT.R16_TYPELESS:
                case DXGI_FORMAT.R16_FLOAT:
                case DXGI_FORMAT.D16_UNORM:
                case DXGI_FORMAT.R16_UNORM:
                case DXGI_FORMAT.R16_UINT:
                case DXGI_FORMAT.R16_SNORM:
                case DXGI_FORMAT.R16_SINT:
                    return 16;

                case DXGI_FORMAT.R8_TYPELESS:
                case DXGI_FORMAT.R8_UNORM:
                case DXGI_FORMAT.R8_UINT:
                case DXGI_FORMAT.R8_SNORM:
                case DXGI_FORMAT.R8_SINT:
                    return 8;

                case DXGI_FORMAT.A8_UNORM:
                    return 8;

                case DXGI_FORMAT.R1_UNORM:
                    return 1;
                case DXGI_FORMAT.R9G9B9E5_SHAREDEXP:
                    return 32;

                case DXGI_FORMAT.R8G8_B8G8_UNORM:
                case DXGI_FORMAT.G8R8_G8B8_UNORM:
                    return 32;

                default:
                    return -1; // Unknown format or unsupported format
            }
        }

       
    }
}
