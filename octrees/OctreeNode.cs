using System.Collections.Concurrent;
using System;
using System.Numerics;
public class OctreeNode {
    const int DIM_LEN = 1;
    public Vector3 center { get; private set; }
    public int size { get; private set; }
    public bool Initialized { get; private set; }
    private ConcurrentDictionary<ValueTuple<int, int, int>, OctreeNode> children;
    public Chunk chunk { get; set; }
    public static int numNodes = 0;
    public static int numNodesInit = 0;
    public int layer { get; private set; }

    public OctreeNode (int size, int layer) {
        this.size = size;
        this.center = center;
        this.layer = layer;
        this.Initialized = false;
        numNodes++;
    }

    public void Initialize () {
        children = new ConcurrentDictionary<(int, int, int), OctreeNode>();
        for (int i = 0; i < DIM_LEN; i ++)
            for (int j = 0; j < DIM_LEN; j ++)
                for (int k = 0; k < DIM_LEN; k ++) {
                    children.TryAdd(new ValueTuple<int, int, int>(-i, -j , -k), new OctreeNode (size / 2, layer - 1));
                }

        Initialized = true;
        numNodesInit++;
    }

    public OctreeNode SelectChild (int x, int y, int z) {
        OctreeNode node = null;
        children.TryGetValue( new ValueTuple<int, int, int>(x, y, z), out node);
        return node;
    }
}