using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

public class Camera : IPositionable, IDrawable {

    public char Char { get; } = 'C';

    Vector2Double position = new Vector2Double(0, 0);
    Vector2Double direction = new Vector2Double(0, 1);

    private Map map;
    public Vector2Double Position { get => position; set {
            position = value;
        }
    }

    public Vector2Double Direction { get => direction; set {
            direction = value;
        }
    }

    // public void SetPosition(Vector2Double position) {
    //     this.position = position;
    // }

    private List<double> lineHeightPercentages = new List<double>();
    public List<double> LineHeightPercentages { get => lineHeightPercentages; }
    public int LineCount { get => Console.BufferWidth - 1; }
    public double FOV = 70;
    public int viewDistance = 30;

    public Camera(Map map) { this.map = map; }

    public void ForceLineHeightPercentages(List<double> lines) {
        lineHeightPercentages = new List<double>(lines);
    }

    public void Render() {
        lineHeightPercentages = new List<double>();
        // Console.WriteLine( "Field of view: " + FOV );
        for (double rotation = Vector2Double.ToAngleDegrees(direction) - FOV / 2.0; rotation <= Vector2Double.ToAngleDegrees(direction) + FOV / 2.0; rotation += FOV / LineCount) {
            if (lineHeightPercentages.Count >= LineCount) {
                break;
            }

            Ray ray = new Ray(this.position, Vector2Double.RotateDegrees(this.direction, rotation), (hit) => {
                lineHeightPercentages.Add( 1 - Vector2Double.Distance(hit, this.position) / viewDistance );
                // lineHeightPercentages.Add( Vector2Double.Distance(hit, this.position) / viewDistance );
            });
            // Console.WriteLine(" Direction: " + Vector2Double.RotateDegrees(this.direction, rotation) );
            ray.Cast(viewDistance, map);
        }
    }

    public void Display() {
        Console.Clear();
        int height = Console.BufferHeight;
        int centerHeight = height / 2;
        int width = lineHeightPercentages.Count;
        List<int> lineHeights = lineHeightPercentages.Select(percentage => (int)(percentage * height)).ToList();

        StringBuilder builder = new StringBuilder();
        for (int row = 0; row < height; row++) {
            int distanceFromCenter = Math.Abs(row - centerHeight);
            for (int col = 0; col < width; col++) {
                if (lineHeights[col] / 2 > distanceFromCenter) {
                    builder.Append("■");
                } else {
                    builder.Append(" ");
                }
            }
            builder.AppendLine();
        }

        Console.WriteLine(builder.ToString());
    }

    public override string ToString()
    {
        return $"[Camera] Direction: { direction }, Position: { position } ";
    }
}