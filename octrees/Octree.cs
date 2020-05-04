using System;
using System.Numerics;
public class Octree
{
    public int layers { get; set; }
    Vector3 size;
    public int sizeX { get => (int)size.X; }
    public int sizeY { get => (int)size.Y; }
    public int sizeZ { get => (int)size.Z; }
    public OctreeNode mainNode { get; set; }

    public Octree(int sizeX, int sizeY, int sizeZ)
    {
        size.X = sizeX;
        size.Y = sizeY;
        size.Z = sizeZ;
        // Calculate number of layers needed to represent the level
        layers = (int)Math.Log(sizeX * sizeY * sizeZ, 2);

        // Generate all the OctreeNodes
        mainNode = new OctreeNode(size, size/2, layers);
        mainNode.Initialize();
    }
}