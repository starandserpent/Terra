using System;
public class Octree {
    public Position center {get; set;}
    public int layers { get; set; }
    public int size { get; set; }
    public OctreeNode mainNode { get; set; }

    public Octree (Position center, int size) {
        this.center = center;
        this.size = size;
        // Calculate number of layers needed to represent the level
        layers = (int) Math.Log (Math.Pow (size, 3), 2);

        // Generate all the OctreeNodes
        mainNode = new OctreeNode (center, size, layers);
        mainNode.Initialize ();
    }
}