using System;
public struct Vector2Double {
    public static Vector2Double zero = new Vector2Double(0, 0);
    public static Vector2Double left = new Vector2Double(0, -1);
    public static Vector2Double right = new Vector2Double(0, 1);

    public static Vector2Double up = new Vector2Double(-1, 0);

    public static Vector2Double down = new Vector2Double(1, 0);


    public double Row { get => row; }
    public double Col { get => col; }
    public double row;
    public double col;

    public Vector2Double(double row, double col) {
        this.row = row; 
        this.col = col;
    }

    public Vector2Int Int() {
        return new Vector2Int((int)this.row, (int)this.col);
    }

    public Vector2Double Rotate(double angleInRadians) {
        return Vector2Double.Rotate(this, angleInRadians);
    }

    public Vector2Double RotateDegrees(double angleInDegrees) {
        return Vector2Double.RotateDegrees(this, angleInDegrees);
    }

    public static Vector2Double RotateDegrees(Vector2Double vector, double angleInDegrees) {
        return Rotate(vector, angleInDegrees * (Math.PI / 180.0));
    }   
    public static Vector2Double Rotate(Vector2Double vector, double angleInRadians) {  
        double newCol = vector.col * Math.Cos(angleInRadians) + vector.row * Math.Sin(angleInRadians);
        double newRow = -vector.col * Math.Sin(angleInRadians) + vector.row * Math.Cos(angleInRadians);
        return new Vector2Double(Math.Round(newRow, 3), Math.Round(newCol, 3));
    }

    public static Vector2Double FromAngleDegrees(double angleInDegrees) {
        return Vector2Double.FromAngle(angleInDegrees * (Math.PI / 180.0));
    }

    public static Vector2Double FromAngle(double angleInRadians) {
        return new Vector2Double( Math.Round(-Math.Sin(angleInRadians), 3), Math.Round( Math.Cos(angleInRadians), 3) );
    }

    public static double ToAngle(Vector2Double vector) {
        // return Math.Acos( vector.col / Vector2Double.Length(vector) );
        return Math.Atan2(-vector.row, vector.col);
    }

    public static double ToAngleDegrees(Vector2Double vector) {
        return Math.Round( Vector2Double.ToAngle(vector) * ( 180.0 / Math.PI), 3);
    }


    public static Vector2Double AlterToCol(Vector2Double vector, double col) {
        double factor = col / vector.col;
        return vector * factor;
    }   

    public static Vector2Double AlterToRow(Vector2Double vector, double row) {
        double factor = row / vector.row;
        return vector * factor;
    }

    public static double Length(Vector2Double vector2) {
        return Math.Sqrt(Math.Pow(vector2.row, 2) + Math.Pow(vector2.col, 2));
    }

    public static double Distance(Vector2Double first, Vector2Double second) {
        return Math.Sqrt(Math.Pow(second.Row - first.Row, 2) + Math.Pow(second.Col - first.Col, 2));
    }

    public static Vector2Double normalized(Vector2Double vector2) {
        double length = Vector2Double.Length(vector2);
        return new Vector2Double((vector2.Row / length), (vector2.Col / length));
    }

    public static Vector2Double operator +(Vector2Double a, Vector2Double b) => new Vector2Double(a.Row + b.Row, a.Col + b.Col);
    public static Vector2Double operator -(Vector2Double a, Vector2Double b) => new Vector2Double(a.Row - b.Row, a.Col - b.Col);
    // public static Vector2Double operator *(Vector2Double a, Vector2Double b) => new Vector2Double(a.x - b.x, a.y - b.y);
    public static Vector2Double operator *(Vector2Double a, double b) => new Vector2Double((a.Row * b), (a.Col * b));

    public override string ToString()
    {
        return $"Vector2Double: [ Row: {this.row}, Col: {this.col} ]";
    }
}