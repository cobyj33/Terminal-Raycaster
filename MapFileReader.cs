using System;
using System.IO;
using System.Collections.Generic;

class MapFileReader {
    public static Dictionary<int, Func<Tile>> tileMap = new Dictionary<int, Func<Tile>>() { 
        [0] = () => new EmptyTile(),
        [1] = () => new WallTile()
    };

    // public static Map? FromImageFile(string relativeFilePath) {
    //     System.IO.FileInfo()
    // }

    public static Map? FromFile(string relativeFilePath) {
        string fullFilePath = Path.GetFullPath(relativeFilePath);
        string[] lines = File.ReadAllLines(fullFilePath);
        if (IsValidMapFile(lines)) {
            Dimension mapDimensions = new Dimension(lines.Length, lines[0].Length);
            Tile[,] tiles = new Tile[mapDimensions.rows,mapDimensions.cols];
            
            for (int row = 0; row < mapDimensions.rows; row++) {
                char[] charArray = lines[row].ToCharArray();
                for (int col = 0; col < mapDimensions.cols; col++) {
                    tiles[row,col] = tileMap[ (int)char.GetNumericValue(charArray[col]) ]();
                }
            }

            return new Map(tiles);
        }

        return null;
    }

    public static bool IsValidMapFile(string relativeFilePath) {
        string fullFilePath = Path.GetFullPath(relativeFilePath);
        string[] lines = File.ReadAllLines(fullFilePath);
        return IsValidMapFile(lines);
    }

    public static bool IsValidMapFile(string[] lines) {
        if (lines.Length == 0) {
            return false;
        }

        foreach (string line in lines) {
            char[] characters = line.ToCharArray();
            foreach (char character in characters) {
                if (!char.IsDigit(character)) {
                    return false;
                } else if (!tileMap.ContainsKey((int)char.GetNumericValue(character))) {
                    return false;
                }
            }
        }

        return true;
    } 
}