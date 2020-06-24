public abstract class Marker {

    public TerraVector3 origin { get; set; }
    public TerraBasis basis { get; set; }

    public abstract void ChangePosition (TerraVector3 origin, TerraBasis basis);

    public abstract void Move (TerraVector3 origin);

    public abstract void Rotate (TerraBasis change);

    public bool Equals (TerraVector3 origin, TerraBasis basis) {
        return origin == this.origin &&
            basis.matrix[0] == this.basis.matrix[0] &&
            basis.matrix[1] == this.basis.matrix[1] &&
            basis.matrix[2] == this.basis.matrix[2];
    }

    public Position ToGlobal (Position coords) {
        Position pos = new Position ();
        pos.x = (int) (basis.matrix[0].Dot (coords) + origin.x);
        pos.y = (int) (basis.matrix[1].Dot (coords) + origin.y);
        pos.z = (int) (basis.matrix[2].Dot (coords) + origin.z);
        return pos;
    }

    public TerraVector3 ToGlobal (TerraVector3 coords) {
        TerraVector3 pos = new TerraVector3 ();
        pos.x = (basis.matrix[0].Dot (coords) + origin.x);
        pos.y = (basis.matrix[1].Dot (coords) + origin.y);
        pos.z = (basis.matrix[2].Dot (coords) + origin.z);
        return pos;
    }

    /**
     * The radius which this marker will force the world to be loaded.
     * Squared to avoid sqrt.
     */
    public int loadRadius { get; set; }
}