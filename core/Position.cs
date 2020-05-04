using System;
public struct Position {
    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; }

    public int GetMax(){
        return Math.Max(x, Math.Max(y, z));
    }
}