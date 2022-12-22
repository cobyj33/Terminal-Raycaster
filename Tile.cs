using System;

public interface ITile {
    public bool canBeHit();
    public char getChar();
}

public class Tile : ITile {
    public char displaychar;
    private bool hittable;
    public bool canBeHit() {
        return hittable;
    }
    public char getChar() {
        return displaychar;
    }
    public Tile(char character, bool hittable) {
        this.displaychar = character;
        this.hittable = hittable;
    }

    public override string ToString()
    {
       return displaychar.ToString();
    }
}

public class EmptyTile : Tile {
    public EmptyTile() : base(' ', false) {}
}

public class WallTile : Tile {
    public WallTile() : base('*', true) {}
}