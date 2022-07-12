using System;
using System.Linq;
using System.Threading;

class Program {
    public static void Main(string[] args) {
        // Tester tester = new Tester();
        // tester.AddTest(new Test("Vector2Double Rotation", () => {
        //     new Vector2Double(0, 0);
        // }) );
        // tester.RunTests();


        Console.WriteLine("Hello World");
        Map map = new Map(new Dimension(30, 30));
        map.PlaceTile(new WallTile(), (int)map.Center.row, (int)map.Center.col + 5);
        Console.WriteLine(map.ToString());

        Camera camera = new Camera(map);
        camera.Position = map.Center;
        camera.Render();
        // camera.LineHeightPercentages.ForEach(percentage => Console.WriteLine(percentage + " ") );
        camera.Display();

        EventSystem eventSystem = EventSystem.instance;
        eventSystem.AddAction(new KeyAction(ConsoleKey.W, () => camera.Position += Vector2Double.normalized(camera.Direction)));
        eventSystem.AddAction(new KeyAction(ConsoleKey.S, () => camera.Position -= Vector2Double.normalized(camera.Direction)));
        eventSystem.AddAction(new KeyAction(ConsoleKey.A, () => camera.Direction = camera.Direction.RotateDegrees(10)));
        eventSystem.AddAction(new KeyAction(ConsoleKey.D, () => camera.Direction = camera.Direction.RotateDegrees(-10)) );
        eventSystem.StartThread();

        while (true) {
            Thread.Sleep(200);
            camera.Render();
            camera.Display();
            // camera.Direction = Vector2Double.RotateDegrees(camera.Direction, 10);
            Console.WriteLine(camera);
            // Console.WriteLine( map.ToString(camera));
        }

        // Vector2Double vector = new Vector2Double(0, 1);
        // Console.WriteLine(Vector2Double.RotateDegrees(vector, 270));

        Console.CancelKeyPress += delegate(object? sender, ConsoleCancelEventArgs e) {
            e.Cancel = true;
            UserThread.EndProgram();
        };
    }
}