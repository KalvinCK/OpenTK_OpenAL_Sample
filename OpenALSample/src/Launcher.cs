using ImGuiNET;
using NAudio.Wave;
using OpenAL;

namespace OpenALSample
{
    public class Launcher
    {
        private ContextAudio contextAudio;
        private Music music;
        private AudioBuffer audioBuffer;

        public Launcher()
        {
            contextAudio = new ContextAudio();
            music = new Music("../../../resources/Where Is My Mind.ogg");
            audioBuffer = new AudioBuffer("../../../resources/media/32_bit_float.WAV");
           
        }
        private float volume = 40;
        public void RenderFrame()
        {
            
            ImGui.Begin("OpenAL Sampler");
            ImGui.Text(contextAudio.Vendor + contextAudio.Version);

            ImGui.SliderFloat("Volume", ref volume, 0f, 100f);
            ImGui.NewLine();

            music.Volume = volume;
            audioBuffer.Volume = volume;

            int indexInput = 0, indexOutput = 0;
            ImGui.ListBox("in", ref indexInput, contextAudio.AllInputDevices.ToArray(), contextAudio.AllInputDevices.Count);
            ImGui.NewLine();

            ImGui.ListBox($"Out", ref indexOutput, contextAudio.AllOutputDevices.ToArray(), contextAudio.AllOutputDevices.Count);
            ImGui.NewLine();

                    
            ImGui.Text("Music");
            ImGui.Text($"Music Name: {music.Name}");
            ImGui.Text($"Music Format: {music.FileFormat}");
            ImGui.Text($"Music Time: {music.TotalTimeString}");
            ImGui.Text($"Music State: {music.State}");
            ImGui.Text($"Music Looping {music.Looping}");
            ImGui.Text($"Music Channel: {music.Channels}");
            ImGui.Text($"Music Rate: {music.SampleRate}");
            ImGui.Text($"Music Current time: {music.CurrentTimeStrig}");

            if (ImGui.Button("Music " + (music.Looping == true ? "OFF" : "ON")))
                    music.Looping = !music.Looping;

            
            if (ImGui.Button("Music Play"))
            {
                music.Play();
            }
            if (ImGui.Button("Music Pause"))
                music.Pause();
            if (ImGui.Button("Music Stop"))
                music.Stop();

            ImGui.NewLine();

            ImGui.Text($"Buffer Sound Name: {audioBuffer.Name}");
            ImGui.Text($"Buffer Sound Format: {audioBuffer.FileFormat}");
            ImGui.Text($"Buffer Sound Time: {audioBuffer.TotalTimeString}");
            ImGui.Text($"Buffer Sound State: {audioBuffer.State}");
            ImGui.Text($"Buffer Sound Channel: {audioBuffer.Channels}");
            ImGui.Text($"Buffer Sound Rate: {audioBuffer.SampleRate}");

            if (ImGui.Button("Buffer Sound Repeat" + (audioBuffer.Looping == true ? "OFF" : "ON")))
                audioBuffer.Looping = !audioBuffer.Looping;

            if (ImGui.Button("Buffer Sound Play"))
                audioBuffer.Play();
            if (ImGui.Button("Buffer Sound Pause"))
                audioBuffer.Pause();
            if (ImGui.Button("Buffer Sound Stop"))
                audioBuffer.Stop();

            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.End();

        }
        public void PlayNAduio(string pathFile)
        {
            using (var waveOut = new WaveOutEvent())
            {
                // Cria uma instância de AudioFileReader com o arquivo WAV
                using (var audioFile = new WaveFileReader(pathFile))
                {
                    var floatProvider = new Wave24ToFloatProvider(audioFile);
                    var waveProvider16 = new WaveFloatTo16Provider(floatProvider);


                    // Define o dispositivo de saída padrão para reprodução do som
                    waveOut.DeviceNumber = -1; // -1 representa o dispositivo de saída padrão

                    // Define o arquivo WAV como fonte para reprodução
                    waveOut.Init(floatProvider);

                    // Inicia a reprodução
                    waveOut.Play();

                    // Aguarda até que a reprodução termine
                    while (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }
        public void Dispose()
        {
            contextAudio.Dispose();
            music.Dispose();
            audioBuffer.Dispose();
            
        }
    }
}