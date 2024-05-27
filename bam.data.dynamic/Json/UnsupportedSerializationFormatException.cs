using System;

namespace Bam.Schema.Json
{
    public class UnsupportedSerializationFormatException: Exception
    {
        public UnsupportedSerializationFormatException(SerializationFormat format) : base(
            $"The specified format is not supported: {format.ToString()}")
        {
        }
    }
}