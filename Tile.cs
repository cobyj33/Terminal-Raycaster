using System;

public abstract class Tile {
    public char DisplayChar { get; set; } = ' ';
    public Tile() { }

    public override string ToString()
    {
       return DisplayChar.ToString();
    }
}