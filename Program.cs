using System;
// using System.Linq;
// using System.Threading;
using Terminal.Gui;

class Program {

    public enum RaycasterView {
        BIRD, PERSPECTIVE
    }

    public static int viewIndex;

    public static RaycasterView view;
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

    public static void RenderCamera(Camera camera, View view) {
        int width, height;
        view.GetCurrentWidth(out width);
        view.GetCurrentHeight(out height);
        
        string output = camera.RenderString(width, height, width);
        view.Text = output;
    }

    public static void Main(string[] args) {
        Application.Init ();
        var top = Application.Top;
        Map? map = MapFileReader.FromFile("maps/boxes.txt");



        var menu = new MenuBar (new MenuBarItem [] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_New", "Creates new file", () => {}),
                new MenuItem ("_Open", "", () => { 
                    OpenDialog dialog = new OpenDialog("Select Map File", "Select a map file", new List<string>() { ".txt" }, OpenDialog.OpenMode.File);
                    string path = dialog.FilePath.ToString();
                    map = MapFileReader.FromFile(path);
                    
             }),
                new MenuItem ("_Close", "", () => {}),
                new MenuItem ("_Quit", "", () => { Application.RequestStop(top); })

            }),
            new MenuBarItem ("_Edit", new MenuItem [] {
                new MenuItem ("_Copy", "", null),
                new MenuItem ("C_ut", "", null),
                new MenuItem ("_Paste", "", null)
            })
        });

        var win = new Window ("Hello") {
            X = 0,
            Y = 1,
            Width = Dim.Fill (),
            Height = Dim.Fill () - 1
        };  

        var textArea = new TextView() {
            X = Pos.Center(),
            Y = Pos.Center(),
            Width = Dim.Percent(100),
            Height = Dim.Percent(100)
        };  

        if (map != null) {
            map.FillEdges();
            Camera camera = new Camera(map);
            camera.Position = map.Center;
            RenderCamera(camera, textArea);
            Application.Top.KeyDown += (key) => {
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
        top.Add(menu, win);
        win.Add(textArea);
        Application.Run ();
        Application.Shutdown ();
    }
}