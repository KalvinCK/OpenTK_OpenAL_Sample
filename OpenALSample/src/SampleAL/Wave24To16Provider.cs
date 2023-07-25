using NAudio.Utils;

namespace NAudio.Wave
{
    public class Wave24To16Provider
    {
        private readonly IWaveProvider sourceProvider;
        private volatile float volume;
        private byte[]? sourceBuffer;

        /// <summary>
        /// Creates a new Wave24toFloatProvider
        /// </summary>
        /// <param name="sourceProvider">the source provider</param>
        public Wave24To16Provider(IWaveProvider sourceProvider)
        {
            if (sourceProvider.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
                throw new ArgumentException("Only PCM supported");
            if (sourceProvider.WaveFormat.BitsPerSample != 24)
                throw new ArgumentException("Only 24 bit audio supported");

            this.WaveFormat = new WaveFormat(
                sourceProvider.WaveFormat.SampleRate,
                16,
                sourceProvider.WaveFormat.Channels);

            this.sourceProvider = sourceProvider;
            this.volume = 1f;
        }

        /// <summary>
        /// <see cref="IWaveProvider.WaveFormat"/>
        /// </summary>
        public WaveFormat WaveFormat { get; private set; }

        /// <summary>
        /// Volume of this channel. 1.0 = full scale
        /// </summary>
        public float Volume
        {
            get => volume;
            set => volume = value;
        }
        /// <summary>
        /// Reads bytes from this wave stream
        /// </summary>
        /// <param name="destBuffer">The destination buffer</param>
        /// <param name="offset">Offset into the destination buffer</param>
        /// <param name="numBytes">Number of bytes read</param>
        /// <returns>Number of bytes read.</returns>
        public int Read(byte[] destBuffer, int offset, int numBytes)
        {
            var sourceBytesRequired = numBytes + (numBytes / 2);
            sourceBuffer = BufferHelpers.Ensure(sourceBuffer, sourceBytesRequired);
            int sourceBytesRead = this.sourceProvider.Read(sourceBuffer, offset, sourceBytesRequired);

            var destWaveBuffer = new WaveBuffer(destBuffer);

            int lastSampleOffset = sourceBuffer.Length / 3;
            int writeIndex = 0;
            int readIndex = 0;
            for (var sourceByteIndex = 0; sourceByteIndex < lastSampleOffset; ++sourceByteIndex)
            {
                readIndex++; //skip the least significant bytes
                                        //read the two most significant bytes
                destWaveBuffer.ByteBuffer[writeIndex++] = sourceBuffer[readIndex++];
                destWaveBuffer.ByteBuffer[writeIndex++] = sourceBuffer[readIndex++];
            }
            var sourceSamples = sourceBytesRead / 3;
            return sourceSamples * 4;
        }
    }
}
