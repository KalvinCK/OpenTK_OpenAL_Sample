using OpenTK.Audio.OpenAL;

namespace OpenAL.Utils
{
    public struct WaveFileInfo
    {
        public required string       Name            { get; set; }
        public required string       Encoding        { get; set; }
        public required int          BitsPerSample   { get; set; }
        public required int          SampleRate      { get; set; }
        public required ALFormat     Channels        { get; set; }
        public required FileTypes    FileFormat        { get; set; }
        public required TimeSpan     TotalTime       { get; set; }
        public override string ToString()
        {
            return $"Name: {Name} | Format: {FileFormat} | Encoding: {Encoding} | {BitsPerSample}-Bit | Sample Rate: {SampleRate}Hz | {Channels} Channels";
        }
    }
}