using OpenTK.Audio.OpenAL;

namespace OpenAL.Utils
{
    public class SourceControll
    {
        public readonly int SourceID;

		/// <summary>
		/// The name of the sound sample.
		/// </summary>
		public string Name { get; protected set; } = string.Empty;
		public string Encoding { get; protected set; } = string.Empty;
		public int BitsPerSample { get; protected set; }
		public int SampleRate { get; protected set; }

        public ALFormat Channels { get; protected set; }

        /// <summary>
        /// The specific format of the sound.
        /// </summary>
        public FileTypes FileFormat { get; protected set; }

        /// <summary>
        /// Represents the audio time.
        /// </summary>
        public TimeSpan TotalTime;

		public string TotalTimeString
			=> $"{TotalTime.Minutes:D2}:{TotalTime.Seconds:D2}:{TotalTime.Milliseconds / 10:D2}";

        public SourceControll()
        {
            SourceID = AL.GenSource();
        }
        /// <summary>
		/// Delete audio buffers from memory.
		/// </summary>
        public virtual void Dispose()
			=> AL.DeleteSource(SourceID);
        /// <summary>
        /// Start the audio.
        /// </summary>
        public virtual void Play()
			=> AL.SourcePlay(SourceID); 

        /// <summary>
        /// Stop audio.
        /// </summary>
        public virtual void Pause()
			=> AL.SourcePause(SourceID);

		/// <summary>
		/// Stop and rewind the audio.
		/// </summary>
		public virtual void Stop()
			=> AL.SourceStop(SourceID);

        /// <summary>
        /// Sets the state of the audio, It does the same process as the Play(), Pause() e Stop() methods;
        /// </summary>
        /// <value>ALSourceState.Initial value is ignored</value>
        public virtual ALSourceState State
			=> AL.GetSourceState(SourceID);
		//AL.SourcePlay(SourceID);


		private float _vol = 100f;
		/// <summary>
		/// Controls the sound volume.
		/// </summary>
		/// <value>To a value between 0 and 100</value>
		public virtual float Volume 
		{
			get => _vol;
			set
			{
				_vol = value;

				if(_vol >= 100f) { _vol = 100f; }
				else if(_vol < 0.001f) { _vol = 0.0f; }

				AL.Source(SourceID, ALSourcef.Gain, _vol * 0.01f);
			}
		}
		private bool _loop = false;
		/// <summary>
		/// Determines whether to loop playback.
		/// </summary>
		/// <value></value>
		public virtual bool Looping
		{
			get => _loop;
			set
			{
				_loop = value;
				AL.Source(SourceID, ALSourceb.Looping, _loop);
			}
		}
    }
}