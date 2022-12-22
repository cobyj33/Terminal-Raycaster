using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

public class Camera {

    private Vector2Double position = new Vector2Double(0, 0);
    private Vector2Double direction = new Vector2Double(0, 1);

    
    private Map map;
    public char Char { get; } = 'C';
    public event Action? OnPositionChange;
    public event Action? OnDirectionChange;

    public Vector2Double Position { get => position; set {
            if (map.At((int)value.row, (int)value.col).canBeHit() == false && map.InBounds(value)) {
                position = value;
                OnPositionChange?.Invoke();
            }
        }
    }

    public Vector2Double Direction { get => direction; set {
            direction = value;
            OnDirectionChange?.Invoke();
        }
    }

    // public void SetPosition(Vector2Double position) {
    //     this.position = position;
    // }

    private List<CameraLine> cameraLineData = new List<CameraLine>();
    public List<CameraLine> CameraLineData { get => cameraLineData; }
    private int _lineCount;
    public int LineCount { get => _lineCount; }
    public double FOV = 70;
    public int viewDistance = 60;

    public Camera(Map map) {
        this.map = map;
    }

    public string RenderString(int width, int height, int lineCount) {
        char[,] lines = Render(width, height, lineCount);
        StringBuilder builder = new StringBuilder();
        for (int row = 0; row < lines.GetLength(0); row++) {
            for (int col = 0; col < lines.GetLength(1); col++) {
                builder.Append(lines[row,col]);
            }
            builder.AppendLine();
        }
        return builder.ToString();
    }


    public char[,] Render(int width, int height, int lineCount) {
        this.cameraLineData = new List<CameraLine>();
        this._lineCount = lineCount;

        
        Vector2Double startingCameraPlaneLocation = position + direction.RotateDegrees(FOV / 2.0).ToLength(0.1);
        Vector2Double endingCameraPlaneLocation =  position + direction.RotateDegrees(360 - FOV / 2.0).ToLength(0.1);
        Vector2Double middleCameraPlaneLocation = Vector2Double.Midpoint(startingCameraPlaneLocation, endingCameraPlaneLocation);

        Vector2Double perpendicularDirection = endingCameraPlaneLocation - startingCameraPlaneLocation;
        double distanceBetweenStartAndEnd = Vector2Double.Distance(startingCameraPlaneLocation, endingCameraPlaneLocation);
        // double distanceFromPlaneToCamera = Vector2Double.Distance(this.position, middleCameraPlaneLocation);
        Vector2Double currentCameraPlaneLocation = startingCameraPlaneLocation;


        for (int i = 0; i < lineCount; i++) {
            currentCameraPlaneLocation += perpendicularDirection.ToLength(distanceBetweenStartAndEnd / lineCount);

            Vector2Double rayDirection = currentCameraPlaneLocation - position;
            Ray ray = new Ray(this.position, rayDirection, (hit) => { //changed distance from currentCameraPlaneLocation to position. The actual rendering plane now lies perpendicular to the direction of the camera and through the camera's position
                double distanceFromHitToPlane = Vector2Double.Distance(currentCameraPlaneLocation, hit.Position) * Math.Sin( Vector2Double.AngleBetween( perpendicularDirection, rayDirection  ) );
                cameraLineData.Add( new CameraLine(hit, (1.0d / distanceFromHitToPlane ) ) );
            }, () => { cameraLineData.Add( CameraLine.Empty ); }  );
            ray.Cast(viewDistance, map);
        }

        int centerHeight = height / 2;
        List<int> lineHeights = cameraLineData.Select(lineData => (int)(lineData.LineLengthPercentage * height)).ToList();
        char[,] buffer = new char[height,width];

        for (int row = 0; row < height; row++) {
            int distanceFromCenter = Math.Abs(row - centerHeight);
            for (int col = 0; col < width; col++) {
                if (lineHeights[col] / 2 > distanceFromCenter) {
                    CameraLine currentLine = cameraLineData[col];
                    if (currentLine.Hit.HasValue) {
                        buffer[row,col] = CardinalToChar(currentLine.Hit.Value.Side);
                    }
                } else {
                    buffer[row,col] = ' ';
                }
            }
        }

        return buffer;
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

    public override string ToString()
    {
        return $"[Camera] Direction: { direction }, Position: { position } ";
    }
}