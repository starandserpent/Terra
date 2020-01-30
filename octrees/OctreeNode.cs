using System.Numerics;
using System;
public class OctreeNode
{
    const int DIM_LEN = 2;
    public Vector3 center { get; private set; }
    public Vector3 size { get; private set; }
    public bool Initialized { get; private set; }
    public OctreeNode[,,] children { get; private set; }
    public Chunk chunk { get; set; }
    public int materialID { get; set; } = -1;
    public static int numNodes = 0;
    public static int numNodesInit = 0;
    public int layer {get; private set;}

    public OctreeNode(Vector3 size, Vector3 center, int layer)
    {
        this.size = size;
        this.center = center;
        this.layer = layer;
        this.Initialized = false;
        numNodes++;
    }

    public void Initialize()
    {
        children = new OctreeNode[2,2,2];
        for (int i=0; i < DIM_LEN; i++)
            for (int j=0; j < DIM_LEN; j++)
                for (int k=0; k < DIM_LEN; k++)
                {
                    children[i,j,k] = new OctreeNode(size/2, CenterCalc(i, j, k), layer - 1);
                }
        Initialized = true;
        numNodesInit++;
    }

    Vector3 CenterCalc(int x, int y, int z)
    {
        Vector3 centerOffset = new Vector3(x-0.5f, y-0.5f, z-0.5f);
        return center + centerOffset;
    }

    public OctreeNode SelectChild(float x, float y, float z)
    {
        return children[Convert.ToInt32(x > center.X), Convert.ToInt32(y > center.Y), Convert.ToInt32(z > center.Z)];
    }
}