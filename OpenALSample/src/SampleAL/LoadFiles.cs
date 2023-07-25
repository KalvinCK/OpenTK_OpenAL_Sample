using NAudio.Vorbis;
using NAudio.Wave;
using OpenTK.Audio.OpenAL;
using NAudio.Flac;

namespace OpenAL.Utils
{
    public class LoadFiles
    {
        
        public unsafe static void WAVE(string filePath, out int bufferID, out WaveFileInfo waveFileInfo)
        {
            using (var waveFileReader = new WaveFileReader(filePath))
            {
                int length = (int)waveFileReader.Length;
                byte[] data;
                WaveFormat format;

                int bits = waveFileReader.WaveFormat.BitsPerSample;
                if (bits == 24)
                {
                    var waveProvider16 = new Wave24To16Provider(waveFileReader);

                    data = new byte[length - (length / 3)];
                    waveProvider16.Read(data, 0, data.Length);

                    format = waveProvider16.WaveFormat;
                }
                else if (bits == 32)
                {
                    var waveProvider16 = new WaveFloatTo16Provider(waveFileReader);

                    data = new byte[length / 2];
                    var result = waveProvider16.Read(data, 0, data.Length);

                    format = waveProvider16.WaveFormat;
                }
                else
                {
                    data = new byte[length];
                    waveFileReader.Read(data, 0, data.Length);

                    format = waveFileReader.WaveFormat;

                }

                waveFileInfo = new WaveFileInfo()
                {
                    Name = Path.GetFileName(filePath),
                    Encoding = format.Encoding.ToString(),
                    BitsPerSample = format.BitsPerSample,
                    SampleRate = format.SampleRate,
                    Channels = GetFormat(format, Path.GetFileNameWithoutExtension(filePath)),
                    FileFormat = HelperFormat.Get(Path.GetExtension(filePath)),
                    TotalTime = waveFileReader.TotalTime
                };

                bufferID = AL.GenBuffer();

                fixed (void* d = data)
                {
                    AL.BufferData(bufferID, waveFileInfo.Channels, d, data.Length, waveFileInfo.SampleRate);
                }
            }
        }
        public unsafe static void FLAC(string filePath, out int bufferID, out WaveFileInfo waveFileInfo)
        {
            using(var reader = new FlacReader(filePath))
            using (var waveStream = new WaveFormatConversionStream(reader.WaveFormat, reader))
            {
                WaveFormat format = reader.WaveFormat;
                byte[] data;
                int length = (int)reader.Length;

                int bits = waveStream.WaveFormat.BitsPerSample;
                if (bits == 24)
                {
                    var waveProvider16 = new Wave24To16Provider(waveStream);

                    data = new byte[length - (length / 3)];
                    waveProvider16.Read(data, 0, data.Length);

                    format = waveProvider16.WaveFormat;
                }
                else if (bits == 32)
                {
                    var waveProvider16 = new WaveFloatTo16Provider(waveStream);

                    data = new byte[length / 2];
                    var result = waveProvider16.Read(data, 0, data.Length);

                    format = waveProvider16.WaveFormat;
                }
                else
                {
                    data = new byte[length];
                    waveStream.Read(data, 0, data.Length);

                    format = waveStream.WaveFormat;

                }

                waveFileInfo = new WaveFileInfo()
                {
                    Name = Path.GetFileName(filePath),
                    Encoding = format.Encoding.ToString(),
                    BitsPerSample = format.BitsPerSample,
                    SampleRate = format.SampleRate,
                    Channels = GetFormat(format, Path.GetFileNameWithoutExtension(filePath)),
                    FileFormat = HelperFormat.Get(Path.GetExtension(filePath)),
                    TotalTime = waveStream.TotalTime
                };

                bufferID = AL.GenBuffer();

                fixed (void* d = data)
                {
                    AL.BufferData(bufferID, waveFileInfo.Channels, d, data.Length, waveFileInfo.SampleRate);
                }
            }
        }
        public unsafe static void MP3(string filePath, out int bufferID, out WaveFileInfo waveFileInfo)
        {
            using (var reader = new Mp3FileReader(filePath))
            using (var waveStream = new WaveFormatConversionStream(reader.WaveFormat, reader))
            {
                WaveFormat format;
                int length = (int)waveStream.Length;
                byte[] data;

                int bits = waveStream.WaveFormat.BitsPerSample;
                if (bits == 24)
                {
                    var waveProvider16 = new Wave24To16Provider(waveStream);

                    data = new byte[length - (length / 3)];
                    waveProvider16.Read(data, 0, data.Length);

                    format = waveProvider16.WaveFormat;
                }
                else if (bits == 32)
                {
                    var waveProvider16 = new WaveFloatTo16Provider(waveStream);

                    data = new byte[length / 2];
                    var result = waveProvider16.Read(data, 0, data.Length);

                    format = waveProvider16.WaveFormat;
                }
                else
                {
                    data = new byte[length];
                    waveStream.Read(data, 0, data.Length);

                    format = waveStream.WaveFormat;
                }

                waveFileInfo = new WaveFileInfo()
                {
                    Name = Path.GetFileName(filePath),
                    Encoding = format.Encoding.ToString(),
                    BitsPerSample = format.BitsPerSample,
                    SampleRate = format.SampleRate,
                    Channels = GetFormat(format, Path.GetFileNameWithoutExtension(filePath)),
                    FileFormat = HelperFormat.Get(Path.GetExtension(filePath)),
                    TotalTime = waveStream.TotalTime
                };

                bufferID = AL.GenBuffer();

                fixed (void* d = data)
                {
                    AL.BufferData(bufferID, waveFileInfo.Channels, d, data.Length, waveFileInfo.SampleRate);
                }
            }
        }
        public unsafe static void MediaFoundation(string filePath, out int bufferID, out WaveFileInfo waveFileInfo)
		{
            using (var reader = new MediaFoundationReader(filePath))
            using (var waveStream = new WaveFormatConversionStream(reader.WaveFormat, reader))
            {
                WaveFormat format;
                int length = (int)waveStream.Length;
                byte[] data;

                int bits = waveStream.WaveFormat.BitsPerSample;
                if (bits == 24)
                {
                    var waveProvider16 = new Wave24To16Provider(waveStream);

                    data = new byte[length - (length / 3)];
                    waveProvider16.Read(data, 0, data.Length);

                    format = waveProvider16.WaveFormat;
                }
                else if (bits == 32)
                {
                    var waveProvider16 = new WaveFloatTo16Provider(waveStream);

                    data = new byte[length / 2];
                    var result = waveProvider16.Read(data, 0, data.Length);

                    format = waveProvider16.WaveFormat;
                }
                else
                {
                    data = new byte[length];
                    waveStream.Read(data, 0, data.Length);

                    format = waveStream.WaveFormat;
                }

                waveFileInfo = new WaveFileInfo()
                {
                    Name = Path.GetFileName(filePath),
                    Encoding = format.Encoding.ToString(),
                    BitsPerSample = format.BitsPerSample,
                    SampleRate = format.SampleRate,
                    Channels = GetFormat(format, Path.GetFileNameWithoutExtension(filePath)),
                    FileFormat = HelperFormat.Get(Path.GetExtension(filePath)),
                    TotalTime = reader.TotalTime
                };

                bufferID = AL.GenBuffer();

                fixed (void* d = data)
                {
                    AL.BufferData(bufferID, waveFileInfo.Channels, d, data.Length, waveFileInfo.SampleRate);
                }
            }
		}
        public unsafe static void OGG(string filePath, out int bufferID, out WaveFileInfo waveFileInfo)
        {
            using (var reader = new VorbisWaveReader(filePath))
            {
                var waveProvider16 = new WaveFloatTo16Provider(reader);

                int length = (int)reader.Length;
                WaveFormat format = waveProvider16.WaveFormat;

                byte[] data = new byte[length / 2];
                waveProvider16.Read(data, 0, data.Length);

                waveFileInfo = new WaveFileInfo()
                {
                    Name = Path.GetFileName(filePath),
                    BitsPerSample = format.BitsPerSample,
                    Channels = GetFormat(format, Path.GetFileNameWithoutExtension(filePath)),
                    Encoding = format.Encoding.ToString(),
                    FileFormat = HelperFormat.Get(Path.GetExtension(filePath)),
                    SampleRate = format.SampleRate,
                    TotalTime = reader.TotalTime
                };

                bufferID = AL.GenBuffer();

                fixed (void* d = data)
                {
                    AL.BufferData(bufferID, waveFileInfo.Channels, d, data.Length, waveFileInfo.SampleRate);
                }
            }
        }
        private static ALFormat GetFormat(WaveFormat format, string fileName)
        {

            ALFormat alFormat;
            var bits = format.BitsPerSample;

            if (bits == 8)
            {
                switch (format.Channels)
                {
                    case 1:
                        alFormat = ALFormat.Mono8;
                        break;
                    case 2:
                        alFormat = ALFormat.Stereo8;
                        break;

                    case 6:
                        alFormat = ALFormat.Multi51Chn8Ext;
                        break;
                    case 8:
                        alFormat = ALFormat.Multi71Chn8Ext;
                        break;

                    default:
                        throw new Exception($"({fileName})Unknown channels.");
                }
            }
            else if (bits == 16 || bits == 24 || bits == 32)
            {
                switch (format.Channels)
                {
                    case 1:
                        alFormat = ALFormat.Mono16;
                        break;
                    case 2:
                        alFormat = ALFormat.Stereo16;
                        break;
                    case 6:
                        alFormat = ALFormat.Multi51Chn16Ext;
                        break;
                    case 8:
                        alFormat = ALFormat.Multi71Chn16Ext;
                        break;

                    default:
                        throw new Exception($"({fileName})Unknown channels.");
                }
            }
            else
            {
                throw new Exception($"({fileName})Unsupported {bits} bit per sample.");
            }
            
            return alFormat;
        }
    }
}