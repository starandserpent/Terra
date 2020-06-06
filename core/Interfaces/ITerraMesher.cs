using System.Buffers;
public interface ITerraMesher{
    void MeshChunk (Chunk chunk, ArrayPool<Position> pool);
    void SetRegistry (Registry reg);
}