using ShiFang.Scripts.Voxel;
using ShiFang.Util.Vector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //视线范围
    public int viewRange = 30;

    void Update()
    {
        for (float x = transform.position.x - Chunk.width * 3; x < transform.position.x + Chunk.width * 3; x += Chunk.width)
        {
            for (float z = transform.position.z - Chunk.width * 3; z < transform.position.z + Chunk.width * 3; z += Chunk.width)
            {
                int xx = Chunk.width * Mathf.FloorToInt(x / Chunk.width);
                int zz = Chunk.width * Mathf.FloorToInt(z / Chunk.width);
                if (!Map.instance.ChunkExists(xx, 0, zz))
                {
                    Map.instance.CreateChunk(new Vector3i(xx, 0, zz));
                }
            }
        }
    }
}