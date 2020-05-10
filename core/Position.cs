using System;
public struct Position {
    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; }

    public int id { get; set; }

    public Position(int x, int y, int z){
        this.x = x;
        this.y = y;
        this.z = z;
        this.id = 0;
    }

    public int GetMax(){
        return Math.Max(x, Math.Max(y, z));
    }
}