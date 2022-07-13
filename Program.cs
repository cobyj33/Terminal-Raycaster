using System;
using System.Linq;
using System.Threading;

class Program {

    public enum View {
        BIRD, PERSPECTIVE
    }

    public static int viewIndex;

    public static View view;
    public static void Main(string[] args) {
        Console.WriteLine("Hello World");
        // Map map = new Map(new Dimension(10, 10));
        Map map = MapFileReader.FromFile("map.txt");
        // Map map = new Map(new Dimension(50, 50), new Prim());
        map.FillEdges();
        Console.WriteLine(map.ToString());

        Camera camera = new Camera(map);
        camera.Position = map.Center;
        camera.Render();
        camera.Display();

        EventSystem eventSystem = EventSystem.instance;
        eventSystem.AddAction(new KeyAction(ConsoleKey.W, () => camera.Position += camera.Direction.ToLength(0.25) ));
        eventSystem.AddAction(new KeyAction(ConsoleKey.S, () => camera.Position -= camera.Direction.ToLength(0.25) ));
        eventSystem.AddAction(new KeyAction(ConsoleKey.A, () => camera.Direction = camera.Direction.RotateDegrees(5)));
        eventSystem.AddAction(new KeyAction(ConsoleKey.D, () => camera.Direction = camera.Direction.RotateDegrees(-5)) );
        eventSystem.AddAction(new KeyAction(ConsoleKey.LeftArrow, () =>{ 
            viewIndex--;
            viewIndex %= Enum.GetValues(typeof(View)).Length;
            view = (View)((View[])Enum.GetValues(typeof(View))).GetValue(viewIndex); 
            }) );
        eventSystem.AddAction(new KeyAction(ConsoleKey.RightArrow, () =>{ 
            viewIndex++;
            viewIndex %= Enum.GetValues(typeof(View)).Length;
            view = (View)((View[])Enum.GetValues(typeof(View))).GetValue(viewIndex); 
            }) );
        eventSystem.StartThread();

        while (true) {
            Thread.Sleep( (int)((1.0d / 30) * 1000) );
            switch (view) {
                case View.BIRD: Console.WriteLine(map); break;
                case View.PERSPECTIVE: {
                    camera.Render();
                    camera.Display();
                    break;
                }
            }
            Console.WriteLine(camera);
        }

        Console.CancelKeyPress += delegate(object? sender, ConsoleCancelEventArgs e) {
            e.Cancel = true;
            UserThread.EndProgram();
        };
    }
}