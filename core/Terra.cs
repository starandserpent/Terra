using System;
using Threading = System.Threading;
using System.Collections.Generic;
using Godot;

public class Terra {
    // Declare member variables here. Examples:
    private volatile Octree octree;
    private volatile Node parent;
    private volatile Dictionary<string, MeshInstance> meshes;

    public Position boundries{get; private set;}

    public Terra (Position position, Position boundries, Node parent) {
        this.parent = parent;

        this.boundries = boundries;

        octree = new Octree (position, boundries.GetMax());

        meshes = new Dictionary<string, MeshInstance> ();
    }

    public Octree GetOctree () {
        return octree;
    }

    public OctreeNode TraverseOctree (int posX, int posY, int posZ, int layer) {
        if (layer < octree.layers) {
                int currentLayer = octree.layers;
                OctreeNode currentNode = octree.mainNode;

                Position pos = new Position(posX, posY, posZ);
                int size = octree.size;

                while (currentLayer > layer) {

                    currentNode = currentNode.SelectChild(Convert.ToInt32(posX > currentNode.center.x), Convert.ToInt32(posY > currentNode.center.y), Convert.ToInt32(posZ > currentNode.center.z));

                    if (!currentNode.Initialized)
                        currentNode.Initialize ();

                    if(currentNode == null)
                    {
                        return null;
                    }

                    currentLayer -= 1;                                        
                }

                if (!currentNode.Initialized)
                    currentNode.Initialize ();

                if (currentLayer == 0) {
                    currentNode = currentNode.SelectChild(Convert.ToInt32(posX > 0), Convert.ToInt32(posY > 0), Convert.ToInt32(posZ > 0));
                }

                return currentNode;
        }
        return null;
    }

    public void PlaceChunk (int posX, int posY, int posZ, Chunk chunk) {
        OctreeNode node = TraverseOctree (posX, posY, posZ, 0);
        node.chunk = chunk;
     //   node.materialID = (int) (chunk.voxels[0].value);
    }

    public void ReplaceChunk (int posX, int posY, int posZ, Chunk chunk) {
        /*int lolong = (int) Morton3D.encode(posX, posY, posZ);
        OctreeNode node = octree.nodes[0][lolong];
        node.chunk = chunk;
        octree.nodes[0][lolong] = node;*/
    }

    private static MeshInstance DebugMesh () {
        SurfaceTool tool = new SurfaceTool ();
        tool.Begin (PrimitiveMesh.PrimitiveType.Lines);

        //Front
        tool.AddVertex (new Vector3 (0, 0, 0));
        tool.AddVertex (new Vector3 (1, 0, 0));
        tool.AddVertex (new Vector3 (1, 0, 0));
        tool.AddVertex (new Vector3 (1, 1, 0));
        tool.AddVertex (new Vector3 (1, 1, 0));
        tool.AddVertex (new Vector3 (0, 1, 0));
        tool.AddVertex (new Vector3 (0, 1, 0));
        tool.AddVertex (new Vector3 (0, 0, 0));

        //Back
        tool.AddVertex (new Vector3 (0, 0, 1));
        tool.AddVertex (new Vector3 (1, 0, 1));
        tool.AddVertex (new Vector3 (1, 0, 1));
        tool.AddVertex (new Vector3 (1, 1, 1));
        tool.AddVertex (new Vector3 (1, 1, 1));
        tool.AddVertex (new Vector3 (0, 1, 1));
        tool.AddVertex (new Vector3 (0, 1, 1));
        tool.AddVertex (new Vector3 (0, 0, 1));

        //BOTTOM
        tool.AddVertex (new Vector3 (0, 0, 0));
        tool.AddVertex (new Vector3 (0, 0, 1));
        tool.AddVertex (new Vector3 (0, 0, 1));
        tool.AddVertex (new Vector3 (1, 0, 1));
        tool.AddVertex (new Vector3 (1, 0, 1));
        tool.AddVertex (new Vector3 (1, 0, 0));
        tool.AddVertex (new Vector3 (1, 0, 0));
        tool.AddVertex (new Vector3 (0, 0, 0));

        //TOP
        tool.AddVertex (new Vector3 (0, 1, 0));
        tool.AddVertex (new Vector3 (0, 1, 1));
        tool.AddVertex (new Vector3 (0, 1, 1));
        tool.AddVertex (new Vector3 (1, 1, 1));
        tool.AddVertex (new Vector3 (1, 1, 1));
        tool.AddVertex (new Vector3 (1, 1, 0));
        tool.AddVertex (new Vector3 (1, 1, 0));
        tool.AddVertex (new Vector3 (0, 1, 0));

        MeshInstance instance = new MeshInstance ();
        instance.Mesh = tool.Commit ();
        return instance;
    }
}