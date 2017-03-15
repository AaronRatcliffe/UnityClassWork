using UnityEngine;
using System.Collections;

public class Ground_Tile {
    //A* values
    public int G;
    public int H;
    public int F;
    //special tile cases
    public bool pathable { get; set; }
    public bool start { get; set; }
    public bool end { get; set; }
    //parent of this tile
    public Ground_Tile parent { get; set; }
    //world cordonents of tiles location
    public Vector3 tileLocation { get; set; }

    //tiles location in the board
    public int tileX { get; set; }
    public int tileZ { get; set; }

    public Ground_Tile(Vector3 cordonents, int boardX, int boardZ) {
        tileX = boardX;
        tileZ = boardZ;
        H = 0;
        G = 0;
        parent = null;
        pathable = true;
        end = false;
        tileLocation = cordonents;
        F = G + H;
    }
    public void updateValues(int h, int g)
    {
        H = h;
        G = g;
        F = G + H;
    }
    public void updateValues(int g)
    {
        G = g;
        F = G + H;
    }
    public bool equals(Ground_Tile other)
    {
        return this.tileLocation.Equals(other.tileLocation);
    }

    public int CompareTo(Ground_Tile other)
    {
        int result = 1;
        if (this.F == other.F)
        {
            result = 0;
        }
        else if(this.F < other.F)
        {
            result = -1;
        }
        return result;
    }
}
