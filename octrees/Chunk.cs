using System.Collections.Generic;
public class Chunk
{
    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; }
    public int Materials { get; set; }
    public List<Run> Voxels { get; set; }
    public bool IsEmpty { get; set; }
    public bool IsSurface { get; set; }
    public bool IsGenerated { get; set; }
    public bool IsFilled{ get; set; }

    public bool IsSolid { get; set; }

    public bool[][] Borders{get;}

    public Chunk ()
    {
        Borders = new bool[6][];
        for(int i = 0; i < 6; i ++)
        {
            Borders[i] = new bool[Constants.CHUNK_SIZE2D];
        }

        IsFilled = false;

        Voxels = new List<Run>();
    }
}