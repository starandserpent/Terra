using Godot;

public class TerraObject
{
    public int worldID { get; set; }
    public string fullName { get; set; }
    public string name { get; }
    public SpatialMaterial material { get; }
    public TerraMesh mesh { get; set; }
    public bool IsSurface {get;}

    public TerraObject(string name, SpatialMaterial material, bool isSurface)
    {
        this.name = name;
        this.material = material;
        this.IsSurface = isSurface;
    }
}