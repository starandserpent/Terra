using System.Collections.Generic;
using System;
using Godot;

public class Terra
{
    // Declare member variables here. Examples:
    private volatile Octree octree;
    private volatile Node parent;
    private volatile Dictionary<string, MeshInstance> meshes;
    public static volatile int traversals = 0;

    public Terra(int sizeX, int sizeY, int sizeZ, Node parent)
    {
        this.parent = parent;
        octree = new Octree(sizeX, sizeY, sizeZ);

        meshes = new Dictionary<string, MeshInstance>();
    }

    public Octree GetOctree()
    {
        return octree;
    }

    public OctreeNode TraverseOctree(float posX, float posY, float posZ, int layer)
    {
        OctreeNode currentNode = octree.mainNode;
        while (currentNode.layer > layer)
        {
            currentNode = currentNode.SelectChild(posX, posY, posZ);
            if (!currentNode.Initialized)
                currentNode.Initialize();
        }
        return currentNode;
    }

    public OctreeNode TraverseOctree(int posX, int posY, int posZ, int layer)
    {
        traversals++;
        if (posX >= 0 && posY >= 0 && posZ >= 0 && layer < octree.layers)
        {
            int currentLayer = octree.layers;
            OctreeNode currentNode = octree.mainNode;
            while (currentLayer > layer)
            {
                int nodePosX = (int) (posX / (currentLayer * 2));
                int nodePosY = (int) (posY / (currentLayer * 2));
                int nodePosZ = (int) (posZ / (currentLayer * 2));

                currentLayer -= 1;
                int nodePos = SelectChildOctant(nodePosX, nodePosY, nodePosZ);
                OctreeNode childNode = currentNode.children[nodePos & 1, (nodePos & 2) >> 1, (nodePos & 4) >> 2];
                if (!childNode.Initialized)
                    childNode.Initialize();

                currentNode = childNode;

               /* string name = "layer: " + currentLayer + " " + nodePosX * 16 * (float) Math.Pow(2, currentLayer) + " " +
                              nodePosY * 16 * (float) Math.Pow(2, currentLayer) +
                              " " + nodePosZ * 16 * (float) Math.Pow(2, currentLayer);
                if (!meshes.ContainsKey(name))
                {
                    MeshInstance instance = DebugMesh();
                    instance.Scale = new Vector3(32 * (float) Math.Pow(2, currentLayer - 2),
                        32 * (float) Math.Pow(2, currentLayer - 2),
                        32 * (float) Math.Pow(2, currentLayer - 2));
                    instance.Name = name;
                    instance.Translation = new Vector3(nodePosX * 16 * (float) Math.Pow(2, currentLayer - 1),
                        nodePosY * 16 * (float) Math.Pow(2, currentLayer - 1),
                        nodePosZ * 16 * (float) Math.Pow(2, currentLayer - 1));
                    parent.CallDeferred("add_child", instance);
                    meshes.Add(name, instance);
                }*/
            }
            if (!currentNode.Initialized)
                currentNode.Initialize();

            if (currentLayer == 0)
            {

                int pos = SelectChildOctant(posX, posY, posZ);
                OctreeNode childNode = currentNode.children[pos & 1, (pos & 2) >> 1, (pos & 4) >> 2];
                

                return childNode;
            }

            return currentNode;
        }

        return null;
    }

    public void PlaceChunk(int posX, int posY, int posZ, Chunk chunk)
    {
        OctreeNode node = TraverseOctree(posX, posY, posZ, 0);
        node.chunk = chunk;
        node.materialID = (int)(chunk.voxels[0]);
    }

    public void ReplaceChunk(int posX, int posY, int posZ, Chunk chunk)
    {
        /*int lolong = (int) Morton3D.encode(posX, posY, posZ);
        OctreeNode node = octree.nodes[0][lolong];
        node.chunk = chunk;
        octree.nodes[0][lolong] = node;*/
    }

    private int SelectChildOctant(int posX, int posY, int posZ)
    {
        /*
        if (posX % 2 == 0 && posY % 2 == 0 && posZ % 2 == 0)
        {
            return 0;
        }
        else if (posX % 2 == 1 && posY % 2 == 0 && posZ % 2 == 0)
        {
            return 1;
        }
        else if (posX % 2 == 0 && posY % 2 == 0 && posZ % 2 == 1)
        {
            return 2;
        }
        else if (posX % 2 == 1 && posY % 2 == 0 && posZ % 2 == 1)
        {
            return 3;
        }
        else if (posX % 2 == 0 && posY % 2 == 1 && posZ % 2 == 0)
        {
            return 4;
        }
        else if (posX % 2 == 1 && posY % 2 == 1 && posZ % 2 == 0)
        {
            return 5;
        }
        else if (posX % 2 == 0 && posY % 2 == 1 && posZ % 2 == 1)
        {
            return 6;
        }
        else
        {
            return 7;
        }
        */
        //return 4 * (posY % 2) + 2 * (posZ % 2) + (posX % 2);
        return 4 * (1 & posZ) + 2 * (1 & posY) + (1 & posX);
    }

    private static MeshInstance DebugMesh()
    {
        SurfaceTool tool = new SurfaceTool();
        tool.Begin(PrimitiveMesh.PrimitiveType.Lines);

        //Front
        tool.AddVertex(new Vector3(0, 0, 0));
        tool.AddVertex(new Vector3(1, 0, 0));
        tool.AddVertex(new Vector3(1, 0, 0));
        tool.AddVertex(new Vector3(1, 1, 0));
        tool.AddVertex(new Vector3(1, 1, 0));
        tool.AddVertex(new Vector3(0, 1, 0));
        tool.AddVertex(new Vector3(0, 1, 0));
        tool.AddVertex(new Vector3(0, 0, 0));

        //Back
        tool.AddVertex(new Vector3(0, 0, 1));
        tool.AddVertex(new Vector3(1, 0, 1));
        tool.AddVertex(new Vector3(1, 0, 1));
        tool.AddVertex(new Vector3(1, 1, 1));
        tool.AddVertex(new Vector3(1, 1, 1));
        tool.AddVertex(new Vector3(0, 1, 1));
        tool.AddVertex(new Vector3(0, 1, 1));
        tool.AddVertex(new Vector3(0, 0, 1));

        //BOTTOM
        tool.AddVertex(new Vector3(0, 0, 0));
        tool.AddVertex(new Vector3(0, 0, 1));
        tool.AddVertex(new Vector3(0, 0, 1));
        tool.AddVertex(new Vector3(1, 0, 1));
        tool.AddVertex(new Vector3(1, 0, 1));
        tool.AddVertex(new Vector3(1, 0, 0));
        tool.AddVertex(new Vector3(1, 0, 0));
        tool.AddVertex(new Vector3(0, 0, 0));

        //TOP
        tool.AddVertex(new Vector3(0, 1, 0));
        tool.AddVertex(new Vector3(0, 1, 1));
        tool.AddVertex(new Vector3(0, 1, 1));
        tool.AddVertex(new Vector3(1, 1, 1));
        tool.AddVertex(new Vector3(1, 1, 1));
        tool.AddVertex(new Vector3(1, 1, 0));
        tool.AddVertex(new Vector3(1, 1, 0));
        tool.AddVertex(new Vector3(0, 1, 0));

        MeshInstance instance = new MeshInstance();
        instance.SetMesh(tool.Commit());
        return instance;
    }
}