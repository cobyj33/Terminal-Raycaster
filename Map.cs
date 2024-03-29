using System;
using System.Text;

public class Map {
    private Tile[,] tiles;

    public Tile[,] Tiles { get => tiles; }

    public Map(Dimension dimension) {
        tiles = new Tile[dimension.rows,dimension.cols];
        for (int row = 0; row < dimension.rows; row++) {
            for (int col = 0; col < dimension.cols; col++) {
                tiles[row,col] = new EmptyTile();
            }
        }
    }

    public Map(Dimension dimension, IMapGenerator generator) {
        this.tiles = generator.Generate(new Dimension(dimension.rows, dimension.cols)).Tiles;
    }

    public Map(Tile[,] tiles) {
        this.tiles = tiles;
    }

    public void PlaceTile(Tile tile, int row, int col) {
        tiles[row,col] = tile;
    }

    public void PlaceTile(Tile tile, Vector2Int position) {
        tiles[position.row,position.col] = tile;
    }

    public Vector2Double Center { get => new Vector2Double(tiles.GetLength(0) / 2, tiles.GetLength(1) / 2); }
    public bool InBounds(Vector2Double doubleVector) {
        return InBounds(doubleVector.Int());
    }

    public bool InBounds(Vector2Int vector) {
        return vector.row >= 0 && vector.row < tiles.GetLength(0) && vector.col >= 0 && vector.col < tiles.GetLength(1);
    }

    public Tile At(int row, int col) {
        return tiles[row,col];
    }

    public Tile At(Vector2Int vector2Int) {
        return tiles[vector2Int.row,vector2Int.col];
    }

    public void FillEdges() {
        for (int row = 0; row < tiles.GetLength(0); row++) {
            for (int col = 0; col < tiles.GetLength(1); col++) {
                if (row == 0 || row == tiles.GetLength(0) - 1 || col == 0 || col == tiles.GetLength(1) - 1) {
                    tiles[row,col] = new WallTile();  
                }
            }
        }
    }

    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        for (int row = 0; row < tiles.GetLength(0); row++) {
            for (int col = 0; col < tiles.GetLength(1); col++) {
                builder.Append(tiles[row,col].ToString());
            }
            builder.AppendLine();
        }
        return builder.ToString();
    }

    public string ToString(Camera camera) {
        StringBuilder builder = new StringBuilder();
        for (int row = 0; row < tiles.GetLength(0); row++) {
            for (int col = 0; col < tiles.GetLength(1); col++) {

                if ((int)camera.Position.row == row && (int)camera.Position.col == col) {
                    builder.Append(camera.Char);
                } else {
                    builder.Append(tiles[row,col].ToString());
                }
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }
}