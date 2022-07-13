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
            if (!map.InBounds(value)) {
                Console.WriteLine("Cannot Move Position, Out of Bounds");
            } else if (map.At((int)value.row, (int)value.col) is IHittable) {
                Console.WriteLine($"Cannot move into hittable object: {(int)value.row} {(int)value.col}");
            } else {
                position = value;
            }
        }
    }

    public Vector2Double Direction { get => direction; set {
            direction = value;
        }
    }

    // public void SetPosition(Vector2Double position) {
    //     this.position = position;
    // }

    private List<CameraLine> cameraLineData = new List<CameraLine>();
    public List<CameraLine> CameraLineData { get => cameraLineData; }
    public int LineCount { get => Console.BufferWidth - 1; }
    public double FOV = 70;
    public int viewDistance = 60;
    public int castDistance = 50;

    public Camera(Map map) { this.map = map; }

    // public void ForceLineHeightPercentages(List<double> lines) {
    //     lineHeightPercentages = new List<double>(lines);
    // }

    public void Render() {
        cameraLineData = new List<CameraLine>();

        
        Vector2Double startingCameraPlaneLocation = position + direction.RotateDegrees(FOV / 2.0).ToLength(0.1);
        Vector2Double endingCameraPlaneLocation =  position + direction.RotateDegrees(360 - FOV / 2.0).ToLength(0.1);
        Vector2Double middleCameraPlaneLocation = Vector2Double.Midpoint(startingCameraPlaneLocation, endingCameraPlaneLocation);

        Vector2Double perpendicularDirection = endingCameraPlaneLocation - startingCameraPlaneLocation;
        double distanceBetweenStartAndEnd = Vector2Double.Distance(startingCameraPlaneLocation, endingCameraPlaneLocation);
        double distanceFromPlaneToCamera = Vector2Double.Distance(this.position, middleCameraPlaneLocation);
        Vector2Double currentCameraPlaneLocation = startingCameraPlaneLocation;


        for (int i = 0; i < LineCount; i++) {
            currentCameraPlaneLocation += perpendicularDirection.ToLength(distanceBetweenStartAndEnd / LineCount);
            if (cameraLineData.Count >= LineCount) {
                break;
            }

            Vector2Double rayDirection = currentCameraPlaneLocation - position;
            Ray ray = new Ray(this.position, rayDirection, (hit) => { //changed distance from currentCameraPlaneLocation to position. The actual rendering plane now lies perpendicular to the direction of the camera and through the camera's position
                double distanceFromHitToPlane = Vector2Double.Distance(currentCameraPlaneLocation, hit.Position) * Math.Sin( Vector2Double.AngleBetween( perpendicularDirection, rayDirection  ) );
                cameraLineData.Add( new CameraLine(hit, (1.0d / distanceFromHitToPlane ) ) );
            }, () => { cameraLineData.Add( CameraLine.Empty ); }  );
            ray.Cast(viewDistance, map);
        }
    }

    public char CardinalToChar(Cardinal cardinal) {
        switch (cardinal) {
            case Cardinal.NORTH: return '$';
            case Cardinal.EAST: return '@';
            case Cardinal.SOUTH: return '-';
            case Cardinal.WEST: return '*';
            default: return '%';
        }
    }

    public void Display() {
        Console.Clear();
        int height = Console.BufferHeight;
        int centerHeight = height / 2;
        int width = cameraLineData.Count;
        List<int> lineHeights = cameraLineData.Select(lineData => (int)(lineData.LineLengthPercentage * height)).ToList();

        StringBuilder builder = new StringBuilder();
        for (int row = 0; row < height; row++) {
            int distanceFromCenter = Math.Abs(row - centerHeight);
            for (int col = 0; col < width; col++) {
                if (lineHeights[col] / 2 > distanceFromCenter) {
                    CameraLine currentLine = cameraLineData[col];
                    if (currentLine.Hit.HasValue) {
                        builder.Append(CardinalToChar(currentLine.Hit.Value.Side));
                    }
                } else {
                    builder.Append(' ');
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