using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexConverter
{
    public static class Helpers
    {
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
    }
}
