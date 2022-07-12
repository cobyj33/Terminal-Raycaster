using System;
public struct Vector2Int {
    public static Vector2Int zero = new Vector2Int(0, 0);
    public static Vector2Int left = new Vector2Int(0, -1);
    public static Vector2Int right = new Vector2Int(0, 1);

    public static Vector2Int up = new Vector2Int(-1, 0);

    public static Vector2Int down = new Vector2Int(1, 0);


    public int Row { get => row; }
    public int Col { get => col; }
    public int row;
    public int col;

    public Vector2Int(int row, int col) {
        this.row = row; 
        this.col = col;
    }

    public Vector2Double Double() {
        return new Vector2Double(this.row, this.col);
    }

    public static Vector2Int RotateCCW(Vector2Int vector) {
        return new Vector2Int(vector.col, -vector.row);
    }   

    public static Vector2Int RotateCW(Vector2Int vector, double angleInRadians) {
        return new Vector2Int(-vector.col, vector.row);
    }

    public static double Length(Vector2Int vector2) {
        return Math.Sqrt(Math.Pow(vector2.row, 2) + Math.Pow(vector2.col, 2));
    }

    public static double Distance(Vector2Int first, Vector2Int second) {
        return Math.Sqrt(Math.Pow(second.Row - first.Row, 2) + Math.Pow(second.Col - first.Col, 2));
    }

    public static Vector2Int normalized(Vector2Int vector2) {
        double length = Vector2Int.Length(vector2);
        return new Vector2Int((int)(vector2.Row / length), (int)(vector2.Col / length));
    }

    public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new Vector2Int(a.Row + b.Row, a.Col + b.Col);
    public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new Vector2Int(a.Row - b.Row, a.Col - b.Col);
    // public static Vector2Int operator *(Vector2Int a, Vector2Int b) => new Vector2Int(a.x - b.x, a.y - b.y);
    public static Vector2Int operator *(Vector2Int a, double b) => new Vector2Int((int)(a.Row * b), (int)(a.Col * b));
}