using OpenTK.Audio.OpenAL;
using OpenAL.Utils;

namespace OpenAL
{
    /// <summary>
    /// Fire and Forget Audio Playback.
    /// </summary>
    class AudioBuffer : SourceControll
    {
        private readonly int BufferID;

        private readonly List<SourceControll> sourceBuffers = new();
        public AudioBuffer(string filePath) : base()
        {
            if (!File.Exists(filePath))
                throw new Exception($"({Path.GetFileNameWithoutExtension(filePath)})File not found");

            if (!ALC.MakeContextCurrent(ContextAudio.Context))
                throw new Exception("The context was not properly initialized.");


            WaveFileInfo waveFileInfo;

            switch (HelperFormat.Get(Path.GetExtension(filePath)))
            {
                case FileTypes.WAV:
                    LoadFiles.WAVE(filePath, out BufferID, out waveFileInfo);
                    break;

                case FileTypes.MP3:
                    LoadFiles.MP3(filePath, out BufferID, out waveFileInfo);
                    break;

                case FileTypes.OGG:
                    LoadFiles.OGG(filePath, out BufferID, out waveFileInfo);
                    break;

                case FileTypes.FLAC:
                    LoadFiles.FLAC(filePath, out BufferID, out waveFileInfo);
                    break;

                default:
                    throw new Exception($"({this.Name})Unsupported file type.");
            }

            this.Name = waveFileInfo.Name;
            this.Encoding = waveFileInfo.Encoding;
            this.BitsPerSample = waveFileInfo.BitsPerSample;
            this.SampleRate = waveFileInfo.SampleRate;
            this.Channels = waveFileInfo.Channels;
            this.FileFormat = waveFileInfo.FileFormat;
            this.TotalTime = waveFileInfo.TotalTime;


            if (this.TotalTime.Minutes > 1f || this.TotalTime.Seconds > 40f)
            {
                Console.WriteLine($"WARNING:: The audio buffer({this.Name}) is too large for a sound buffer, try using the buffer designed for music.");
            }


        }
		public override void Dispose()
        {
            AL.DeleteBuffer(BufferID);
            sourceBuffers.ForEach(i => i.Dispose());
            base.Dispose();
        }
        /// <summary>
		/// Start the audio.
		/// </summary>
        public new void Play()
        {
            sourceBuffers.Add(new SourceControll
            {
                Looping = Looping,
                Volume = Volume,
            });

            var lenght = sourceBuffers.Count - 1;

            AL.Source(sourceBuffers[lenght].SourceID, ALSourcei.Buffer, BufferID);
            sourceBuffers[lenght].Play();

            if (!Looping)
                RemoveStackCache();

        }
        /// <summary>
        /// Pause audio.
        /// </summary>
        public new void Pause()
        {
            sourceBuffers.ForEach(i => i.Pause());
        }

        /// <summary>
        /// Stop and rewind the audio.
        /// </summary>
        public new void Stop()
        {
            sourceBuffers.ForEach(i => i.Stop());
        }
        private async void RemoveStackCache()
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < sourceBuffers.Count; i++)
                {
                    if (sourceBuffers[i].State == ALSourceState.Stopped)
                    {
                        sourceBuffers[i].Dispose();
                        sourceBuffers.RemoveAt(i);
                    }
                }
            });
        }
        public new string TotalTimeString
             => $"{TotalTime.Minutes:D2}:{TotalTime.Seconds:D2}:{TotalTime.Milliseconds / 10:D2}:{TotalTime.Microseconds / 10:D2}";

        public override ALSourceState State
        {
            get
            {
                if (sourceBuffers.All(x => x.State == ALSourceState.Playing))
                {
                    return ALSourceState.Playing;
                }
                else
                {
                    return ALSourceState.Stopped;
                }
            }
        }
        public override float Volume
        {
            get => base.Volume;
            set
            {
                var vol = value;
                base.Volume = vol;

                for (int i = 0; i < sourceBuffers.Count; i++)
                    sourceBuffers[i].Volume = vol;
            }
        }
        public override bool Looping
        {
            get
            {
                if(sourceBuffers.All(x => x.Looping == true) && base.Looping)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                var loop = value;

                base.Looping = loop;

                for (int i = 0; i < sourceBuffers.Count; i++)
                {
                    sourceBuffers[i].Looping = loop;
                }
            }
        }
    }
}