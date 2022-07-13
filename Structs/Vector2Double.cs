using System;
public struct Vector2Double {
    public double Row { get => row; }
    public double Col { get => col; }
    public double Length { get => Math.Sqrt(Math.Pow(this.row, 2) + Math.Pow(this.col, 2)); }
    public double row;
    public double col;

    public Vector2Double(double row, double col) {
        this.row = row; 
        this.col = col;
    }

    public Vector2Int Int() {
        return new Vector2Int((int)this.row, (int)this.col);
    }

    // public Vector2Double Rotate(double angleInRadians) {
    //     return Vector2Double.Rotate(this, angleInRadians);
    // }

    // public Vector2Double RotateDegrees(double angleInDegrees) {
    //     return Vector2Double.RotateDegrees(this, angleInDegrees);
    // }

    public Vector2Double RotateDegrees(double angleInDegrees) {
        return Rotate(angleInDegrees * (Math.PI / 180.0));
    }   
    public Vector2Double Rotate(double angleInRadians) {  
        double newCol = this.col * Math.Cos(angleInRadians) + this.row * Math.Sin(angleInRadians);
        double newRow = -this.col * Math.Sin(angleInRadians) + this.row * Math.Cos(angleInRadians);
        return new Vector2Double(Math.Round(newRow, 3), Math.Round(newCol, 3));
    }

    public static Vector2Double DirectionFromTo(Vector2Double source, Vector2Double target) {
        return target - source;
    }


    public Vector2Double ToLength(double length) {
        return this * ( length / this.Length );
    }

    public double ToAngle() {
        return Math.Atan2(-row, col);
    }

    public double ToAngleDegrees() {
        return Math.Round( this.ToAngle() * ( 180.0 / Math.PI), 3);
    }


    public Vector2Double AlterToCol(double col) {
        double factor = col / this.col;
        return this * factor;
    }   

    public Vector2Double AlterToRow(double row) {
        double factor = row / this.row;
        return this * factor;
    }

    public Vector2Double normalized() {
        return new Vector2Double((this.Row / this.Length), (this.Col / this.Length));
    }

    public static double AngleBetween(Vector2Double start, Vector2Double end) {
        return end.ToAngle() - start.ToAngle();
    }

    public override string ToString()
    {
        return $"Vector2Double: [ Row: {this.row}, Col: {this.col} Angle: { this.ToAngleDegrees() }]";
    }
    public static Vector2Double operator +(Vector2Double a, Vector2Double b) => new Vector2Double(a.Row + b.Row, a.Col + b.Col);
    public static Vector2Double operator -(Vector2Double a, Vector2Double b) => new Vector2Double(a.Row - b.Row, a.Col - b.Col);
    // public static Vector2Double operator *(Vector2Double a, Vector2Double b) => new Vector2Double(a.x - b.x, a.y - b.y);
    public static Vector2Double operator *(Vector2Double a, double b) => new Vector2Double((a.Row * b), (a.Col * b));


    public static Vector2Double FromAngleDegrees(double angleInDegrees) {
        return Vector2Double.FromAngle(angleInDegrees * (Math.PI / 180.0));
    }

    public static Vector2Double FromAngle(double angleInRadians) {
        return new Vector2Double( Math.Round(-Math.Sin(angleInRadians), 3), Math.Round( Math.Cos(angleInRadians), 3) );
    }

    public static double Distance(Vector2Double first, Vector2Double second) {
        return Math.Sqrt(Math.Pow(second.Row - first.Row, 2) + Math.Pow(second.Col - first.Col, 2));
    }
}