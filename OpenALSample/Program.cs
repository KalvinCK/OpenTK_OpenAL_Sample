using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace OpenALSample
{
    class Program
    {
        public static GameWindow window = new GameWindow(GameWindowSettings.Default,
        new NativeWindowSettings
        {
            Size = new Vector2i(1600, 900),
            Flags = ContextFlags.ForwardCompatible,
            Title = "OpenAL Example",
            APIVersion = Version.Parse("4.6"),
            API = ContextAPI.OpenGL,
            Location = new Vector2i(50, 50),
            Vsync = VSyncMode.On
        });

        private static ImGuiConfig? imGuiConfig;
        public static Vector2 Size { get; private set; } = Vector2.Zero;
        private static Launcher? launcher;
        static void Main(string[] args)
        {
            window.Load += delegate
            {
                GL.ClearColor(new Color4(0.1f, 0.1f, 0.1f, 1.0f));
                imGuiConfig = new ImGuiConfig(Size);

                launcher = new Launcher();
            };
            window.RenderFrame += delegate (FrameEventArgs frameEventArgs)
            {
                imGuiConfig!.Update(window, (float)frameEventArgs.Time);

                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                launcher!.RenderFrame();


                imGuiConfig.Render();
                imGuiConfig.CheckGLError("End of frame");


                window.SwapBuffers();
            };
            window.UpdateFrame += delegate (FrameEventArgs eventArgs)
            {
                TimerGL.TimerUpdateFrame(eventArgs);

                if (window.IsKeyPressed(Keys.Escape))
                    window.Close();

            };
            window.Resize += delegate (ResizeEventArgs resizeEvent)
            {
                Size = resizeEvent.Size;
                GL.Viewport(0, 0, resizeEvent.Size.X, resizeEvent.Size.Y);

                imGuiConfig!.WindowResized(resizeEvent.Size);
            };
            window.TextInput += delegate (TextInputEventArgs textInputEvent)
            {
                imGuiConfig!.PressChar((char)textInputEvent.Unicode);
            };
            window.Unload += delegate
            {
                imGuiConfig!.Dispose();
                launcher!.Dispose();
            };
            window.Run();
        }
    }
}
