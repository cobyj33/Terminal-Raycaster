using System;
// using System.Linq;
// using System.Threading;

class Program {
    public enum RaycasterView {
        BIRD, PERSPECTIVE
    }

    public static int viewIndex;

    public static RaycasterView view = RaycasterView.PERSPECTIVE;
    // public static void Main(string[] args) {
    //     Console.WriteLine("Hello World");
    //     // Map map = new Map(new Dimension(10, 10));
    //     Map map = MapFileReader.FromFile("boxes.txt");
    //     // Map map = new Map(new Dimension(50, 50), new Prim());
    //     map.FillEdges();
    //     Console.WriteLine(map.ToString());

    //     Camera camera = new Camera(map);
    //     camera.Position = map.Center;
    //     camera.Render();
    //     camera.Display();

    //     EventSystem eventSystem = EventSystem.instance;
    //     eventSystem.AddAction(new KeyAction(ConsoleKey.W, () => camera.Position += camera.Direction.ToLength(0.25) ));
    //     eventSystem.AddAction(new KeyAction(ConsoleKey.S, () => camera.Position -= camera.Direction.ToLength(0.25) ));
    //     eventSystem.AddAction(new KeyAction(ConsoleKey.A, () => camera.Direction = camera.Direction.RotateDegrees(5)));
    //     eventSystem.AddAction(new KeyAction(ConsoleKey.D, () => camera.Direction = camera.Direction.RotateDegrees(-5)) );
    //     eventSystem.AddAction(new KeyAction(ConsoleKey.LeftArrow, () =>{ 
    //         viewIndex--;
    //         if (viewIndex < 0) { viewIndex =  Enum.GetValues(typeof(View)).Length - 1; };
    //         view = (View)((View[])Enum.GetValues(typeof(View))).GetValue(viewIndex); 
    //         }) );
    //     eventSystem.AddAction(new KeyAction(ConsoleKey.RightArrow, () =>{ 
            // viewIndex++;
            // viewIndex %= Enum.GetValues(typeof(View)).Length;
            // view = (View)((View[])Enum.GetValues(typeof(View))).GetValue(viewIndex); 
    //         }) );
    //     eventSystem.StartThread();

    //     while (true) {
    //         Thread.Sleep( (int)((1.0d / 30) * 1000) );
    //         switch (view) {
    //             case View.BIRD: {Console.WriteLine(map.ToString(camera)); break;}
    //             case View.PERSPECTIVE: {
    //                 camera.Render();
    //                 camera.Display();
    //                 break;
    //             }
    //         }
    //     }

    //     Console.CancelKeyPress += delegate(object? sender, ConsoleCancelEventArgs e) {
    //         e.Cancel = true;
    //         UserThread.EndProgram();
    //     };
    // }

    public static void RenderCamera(Camera camera, Terminal.Gui.View view) {
        int width, height;
        view.GetCurrentWidth(out width);
        view.GetCurrentHeight(out height);
        
        string output = camera.RenderString(width, height, width);
        view.Text = output;
    }

    public static void Main(string[] args) {
        Terminal.Gui.Application.Init ();
        var top = Terminal.Gui.Application.Top;
        string fileToLoad = "maps/defaults/boxes.txt";
        Map? map = MapFileReader.FromFile(fileToLoad);

        // var menu = new Terminal.Gui.MenuBar (new Terminal.Gui.MenuBarItem [] {
        //     new Terminal.Gui.MenuBarItem ("_File", new Terminal.Gui.MenuItem [] {
        //         new Terminal.Gui.MenuItem ("_New", "Creates new file", () => {}),
        //         new Terminal.Gui.MenuItem ("_Open", "", () => { 
        //             Terminal.Gui.OpenDialog dialog = new Terminal.Gui.OpenDialog("Select Map File", "Select a map file", new List<string>() { ".txt" }, Terminal.Gui.OpenDialog.OpenMode.File);
        //             string path = dialog.FilePath.ToString();
        //             map = MapFileReader.FromFile(path);
                    
        //      }),
        //         new Terminal.Gui.MenuItem ("_Close", "", () => {}),
        //         new Terminal.Gui.MenuItem ("_Quit", "", () => { Terminal.Gui.Application.RequestStop(top); })

        //     }),
        //     new Terminal.Gui.MenuBarItem ("_Edit", new Terminal.Gui.MenuItem [] {
        //         new Terminal.Gui.MenuItem ("_Copy", "", null),
        //         new Terminal.Gui.MenuItem ("C_ut", "", null),
        //         new Terminal.Gui.MenuItem ("_Paste", "", null)
        //     })
        // });

        var win = new Terminal.Gui.Window ("Terminal Raycaster") {
            X = 0,
            Y = 1,
            Width = Terminal.Gui.Dim.Fill (),
            Height = Terminal.Gui.Dim.Fill () - 1
        };  

        var textArea = new Terminal.Gui.TextView() {
            X = Terminal.Gui.Pos.Center(),
            Y = Terminal.Gui.Pos.Center(),
            Width = Terminal.Gui.Dim.Percent(100),
            Height = Terminal.Gui.Dim.Percent(100)
        };  

        if (map != null) {
            map.FillEdges();
            Camera camera = new Camera(map);
            camera.Position = map.Center;
            RenderCamera(camera, textArea);
            Terminal.Gui.Application.Top.KeyDown += (key) => {
                char ch = (char)(key.KeyEvent.Key);
                switch (ch) {
                    case 'w': camera.Position += camera.Direction.ToLength(0.25); break;
                    case 'a': camera.Direction = camera.Direction.RotateDegrees(5); break;
                    case 's': camera.Position -= camera.Direction.ToLength(0.25); break;
                    case 'd': camera.Direction = camera.Direction.RotateDegrees(-5); break;
                    case ' ': {
                        viewIndex++;
                        viewIndex %= Enum.GetValues(typeof(RaycasterView)).Length;
                        view = (RaycasterView)((RaycasterView[])Enum.GetValues(typeof(RaycasterView))).GetValue(viewIndex); 
                    }; break;
                }

                switch (view) {
                    case RaycasterView.PERSPECTIVE: RenderCamera(camera, textArea); break;
                    case RaycasterView.BIRD: textArea.Text = map.ToString(camera); break; 
                }
            };
        } else {
            throw new InvalidDataException("File could not be found");
        }

        
        // Add both menu and win in a single call
        // top.Add(menu, win);
        top.Add(win);
        win.Add(textArea);
        Terminal.Gui.Application.Run();
        Terminal.Gui.Application.Shutdown ();
    }
}