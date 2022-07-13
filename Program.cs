using System;
using System.Linq;
using System.Threading;

class Program {
    public static void Main(string[] args) {
        Console.WriteLine("Hello World");
        Map map = MapFileReader.FromFile("map.txt");
        Console.WriteLine(map.ToString());

        Camera camera = new Camera(map);
        camera.Position = map.Center;
        camera.Render();
        camera.Display();

        EventSystem eventSystem = EventSystem.instance;
        eventSystem.AddAction(new KeyAction(ConsoleKey.W, () => camera.Position += camera.Direction.ToLength(0.25) ));
        eventSystem.AddAction(new KeyAction(ConsoleKey.S, () => camera.Position -= camera.Direction.ToLength(0.25) ));
        eventSystem.AddAction(new KeyAction(ConsoleKey.A, () => camera.Direction = camera.Direction.RotateDegrees(-5)));
        eventSystem.AddAction(new KeyAction(ConsoleKey.D, () => camera.Direction = camera.Direction.RotateDegrees(5)) );
        eventSystem.StartThread();

        while (true) {
            Thread.Sleep( (int)((1.0d / 30) * 1000) );
            camera.Render();
            camera.Display();
            Console.WriteLine(camera);
        }

        Console.CancelKeyPress += delegate(object? sender, ConsoleCancelEventArgs e) {
            e.Cancel = true;
            UserThread.EndProgram();
        };
    }
}