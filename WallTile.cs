using System;

public class WallTile : Tile, IHittable {
    public WallTile() : base() {
        DisplayChar = '■';
    }
}