using OpenTK.Audio.OpenAL;
using System.Diagnostics;
using OpenAL.Utils;

namespace OpenAL
{
    public class Music : SourceControll
    {
        private readonly int BufferID;

        private readonly Stopwatch stopWatch;

        private TimeSpan CurrentTime
        {
            get
            {
                if (State == ALSourceState.Stopped && stopWatch.IsRunning)
                    stopWatch.Reset();

                return stopWatch.Elapsed;
            }
        }
        public string CurrentTimeStrig
            => $"{CurrentTime.Minutes:D2}:{CurrentTime.Seconds:D2}:{CurrentTime.Milliseconds / 10:D2}";

        /// <summary>
        /// The context must have been created before, otherwise an exception is thrown.
        /// </summary>
        /// <param name="filePath">Path of the audio file to be loaded.</param>
        public Music(string filePath) : base()
		{
			if (!File.Exists(filePath))
				throw new Exception($"({Path.GetFileNameWithoutExtension(filePath)})File not found");

            if (!ALC.MakeContextCurrent(ContextAudio.Context))
                throw new Exception("The context was not properly initialized.");

            WaveFileInfo waveFileInfo;

			switch(HelperFormat.Get(Path.GetExtension(filePath)))
			{
				case FileTypes.WAV:
                    LoadFiles.WAVE(filePath, out BufferID, out waveFileInfo);
					break;

				case FileTypes.MP3:
					LoadFiles.MP3(filePath, out BufferID, out waveFileInfo);
                    break;

				case FileTypes.WMA:
				case FileTypes.AAC:
				case FileTypes.MP4:
                    LoadFiles.MediaFoundation(filePath, out BufferID, out waveFileInfo);
					break;

				case FileTypes.FLAC:
                    LoadFiles.FLAC(filePath, out BufferID, out waveFileInfo);
                    break;

                case FileTypes.OGG:
                    LoadFiles.OGG(filePath, out BufferID, out waveFileInfo);
                    break;

                default:
					throw new Exception($"({this.Name})Unsupported file type.");
			}

            this.Name           = waveFileInfo.Name;
            this.Encoding       = waveFileInfo.Encoding;
            this.BitsPerSample  = waveFileInfo.BitsPerSample;
            this.SampleRate     = waveFileInfo.SampleRate;
            this.Channels	    = waveFileInfo.Channels;
            this.FileFormat     = waveFileInfo.FileFormat;
            this.TotalTime	    = waveFileInfo.TotalTime;

            AL.Source(this.SourceID, ALSourcei.Buffer, BufferID);
            stopWatch = new Stopwatch();
        }
        public override void Play()
        {
            if (stopWatch.IsRunning)
                stopWatch.Restart();
            else
                stopWatch.Start();

            if(State != ALSourceState.Playing)
                base.Play();

        }
        public override void Pause()
        {
            if (stopWatch.IsRunning)
                stopWatch.Stop();

            base.Pause();
        }
        public override void Stop()
        {
            stopWatch.Reset();
            base.Stop();
        }
        public override void Dispose()
        {
			AL.DeleteBuffer(BufferID);
            base.Dispose();
        }
    }
}