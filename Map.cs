using System;
using System.Text;

public class Map {
    
    private Tile[,] tiles;

    public Map(Dimension dimension) {
        tiles = new Tile[dimension.rows,dimension.cols];
        for (int row = 0; row < dimension.rows; row++) {
            for (int col = 0; col < dimension.cols; col++) {
                tiles[row,col] = new EmptyTile();
            }
        }
        FillEdges();
    }

    public Map(Tile[,] tiles) {
        this.tiles = tiles;
    }

    public void PlaceTile(Tile tile, int row, int col) {
        tiles[row,col] = tile;
    }

    public Vector2Double Center { get => new Vector2Double(tiles.GetLength(0) / 2, tiles.GetLength(1) / 2); }

    public bool InBounds(Vector2Double vector) {
        return vector.row >= 0 && vector.row < tiles.GetLength(0) && vector.col >= 0 && vector.col < tiles.GetLength(1);
    }

    public Tile At(int row, int col) {
        return tiles[row,col];
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

    public string ToString<T>(T element) where T: IPositionable, IDrawable {
        StringBuilder builder = new StringBuilder();
        for (int row = 0; row < tiles.GetLength(0); row++) {
            for (int col = 0; col < tiles.GetLength(1); col++) {

                if ((int)element.Position.row == row && (int)element.Position.col == col) {
                    builder.Append(element.Char);
                } else {
                    builder.Append(tiles[row,col].ToString());
                }
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }
}