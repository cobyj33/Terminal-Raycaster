using System;
using System.Collections.Generic;

public class Prim : IMapGenerator {

    public Prim() { }

    public Map Generate(Dimension dimensions) {
        Tile[,] tiles = new Tile[dimensions.rows, dimensions.cols];
        Map map = new Map(tiles);
        List<Vector2Int> walls = new List<Vector2Int>();
        HashSet<Vector2Int> paths = new HashSet<Vector2Int>(new Vector2IntComparer());
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>(new Vector2IntComparer());


        for (int row = 0; row < tiles.GetLength(0); row++) {
            for (int col = 0; col < tiles.GetLength(1); col++) {
                tiles[row, col] = new WallTile();
            }
        }

        Vector2Int start = map.Center.Int();
        Random random = new Random();
        map.PlaceTile(new EmptyTile(), start);
        walls = new List<Vector2Int>(GetAdjacent(start));
        paths.Add(start);

        while (walls.Count > 0) {
            int nextWallIndex = random.Next(0, walls.Count);
            Vector2Int current = walls[nextWallIndex];
            walls.RemoveAt(nextWallIndex);
            visited.Add(current);
            Vector2Int[] adjacent = GetAdjacent(current);
            int numOfDrawnNeighbors = 0;

            foreach (Vector2Int neighbor in adjacent) {
                if (map.InBounds(neighbor)) {
                    if (map.At(neighbor) is WallTile && !visited.Contains(neighbor)) {
                        walls.Add(neighbor);
                    }

                    if (paths.Contains(neighbor)) {
                        numOfDrawnNeighbors++;
                    }
                }
            }
            
            if (numOfDrawnNeighbors == 1) {
                map.PlaceTile(new EmptyTile(), current);
                paths.Add(current);
            }
        }

        return map;
    }

    private Vector2Int[] GetAdjacent(Vector2Int vector) {
        return new Vector2Int[]{vector + Vector2Int.up, vector + Vector2Int.down, vector + Vector2Int.right, vector + Vector2Int.left};
    }
}