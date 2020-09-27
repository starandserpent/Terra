using System.Collections.Concurrent;
using System;
using System.Numerics;
public class OctreeNode {
    public Position center { get; private set; }
    public int size { get; private set; }
    public bool Initialized { get; private set; }
    private ConcurrentDictionary<ValueTuple<int, int, int>, OctreeNode> children;
    public Chunk chunk { get; set; }
    public int layer { get; private set; }

    public OctreeNode (Position center, int size, int layer) {
        this.center = center;
        this.size = size;
        this.layer = layer;
        this.Initialized = false;
        children = new ConcurrentDictionary<(int, int, int), OctreeNode>();
    }

    public void Initialize () {
        int size = this.size/2;
        children.TryAdd(new ValueTuple<int, int, int>(1, 1 , 1), new OctreeNode (new Position(center.x + size, center.y + size, center.z + size), size, layer - 1));
        children.TryAdd(new ValueTuple<int, int, int>(0, 1 , 1), new OctreeNode (new Position(center.x - size, center.y + size, center.z + size), size, layer - 1));
        children.TryAdd(new ValueTuple<int, int, int>(1, 0 , 1), new OctreeNode (new Position(center.x + size, center.y - size, center.z + size), size, layer - 1));
        children.TryAdd(new ValueTuple<int, int, int>(0, 0, 1), new OctreeNode (new Position(center.x - size, center.y - size, center.z + size), size,layer - 1));

        children.TryAdd(new ValueTuple<int, int, int>(1, 1 , 0), new OctreeNode (new Position(center.x + size, center.y + size, center.z - size), size, layer - 1));
        children.TryAdd(new ValueTuple<int, int, int>(0, 1 , 0), new OctreeNode (new Position(center.x - size, center.y + size, center.z - size), size, layer - 1));
        children.TryAdd(new ValueTuple<int, int, int>(1, 0, 0), new OctreeNode (new Position(center.x + size, center.y - size, center.z - size), size, layer - 1));
        children.TryAdd(new ValueTuple<int, int, int>(0, 0, 0), new OctreeNode (new Position(center.x - size, center.y - size, center.z - size), size, layer - 1));

        Initialized = true;
    }

    public OctreeNode SelectChild (int x, int y, int z) {
        OctreeNode node = null;
        children.TryGetValue( new ValueTuple<int, int, int>(x, y, z), out node);
        return node;
    }
}