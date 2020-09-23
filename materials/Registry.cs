using System.Collections.Generic;
using Godot;

public class Registry : List<TerraObject>
{
    private Dictionary<string, TerraObject> nameToObject;

    public Registry()
    {
        nameToObject = new Dictionary<string, TerraObject>();
        RegisterDefaultObjects();
    }

    private void RegisterDefaultObjects()
    {
        TerraObject air = new TerraObject("air", null, false);
        RegisterObject(air);
    }

    public void RegisterObject(TerraObject terraObject)
    {
        this.Add(terraObject);
        int worldID = this.IndexOf(terraObject);

        terraObject.worldID = worldID;

        string fullName = terraObject.name;
        terraObject.fullName = fullName;
        nameToObject.Add(fullName, terraObject);

        GD.Print(terraObject.fullName);
    }

    public TerraObject SelectByName(string fullName)
    {
        return nameToObject[fullName];
    }

    public TerraObject SelectByID(int id)
    {
        return this[id];
    }
}