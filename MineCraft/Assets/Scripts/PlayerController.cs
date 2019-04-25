using ShiFang.Scripts.Voxel;
using ShiFang.Util.Vector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //视线范围
    public int viewRange = 30;

    void Update()
    {
        for (float x = transform.position.x - Chunk.width * viewRange / 10; x < transform.position.x + Chunk.width * viewRange / 10; x += Chunk.width)
        {
            for (float y = transform.position.y - Chunk.height * viewRange / 10; y < transform.position.y + Chunk.height * viewRange / 10; y += Chunk.height)
            {
                //Y轴上是允许最大16个Chunk，方块高度最大是256
                if (y <= Chunk.height && y > 0)
                {
                    for (float z = transform.position.z - Chunk.width * viewRange / 10; z < transform.position.z + Chunk.width * viewRange / 10; z += Chunk.width)
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
    }
}