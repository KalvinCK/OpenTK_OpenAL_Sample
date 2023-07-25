
namespace OpenAL.Utils
{
    public enum FileTypes
    {
        WAV,
        MP3,
        MP4,
        WMA,
        AAC,
        OGG,
        FLAC,
        UNDEFINED,
    }

    public static class HelperFormat
    {
        public static FileTypes Get(string fileFormat)
        {
            FileTypes format;

            switch (fileFormat.ToLower())
            {
                case ".wave":
                case ".wav":
                    format = FileTypes.WAV;
                    break;

                case ".mp3":
                    format = FileTypes.MP3;
                    break;

                case ".mp4":
                    format = FileTypes.MP4;
                    break;

                case ".wma":
                    format = FileTypes.WMA;
                    break;

                case ".aac":
                    format = FileTypes.AAC;
                    break;

                case ".ogg":
                    format = FileTypes.OGG;
                    break;

                case ".flac":
                    format = FileTypes.FLAC;
                    break;
               
                default:
                    format = FileTypes.UNDEFINED;
                    break;
            }

            return format;
        }
    }
}
