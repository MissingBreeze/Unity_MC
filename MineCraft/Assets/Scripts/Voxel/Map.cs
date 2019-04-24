using System.Collections;
using System.Collections.Generic;
using ShiFang.Util.Vector;
using UnityEngine;

namespace ShiFang.Scripts.Voxel
{
    public class Map : MonoBehaviour
    {
        public static Map instance;

        public static GameObject chunkPrefab;

        public Dictionary<Vector3i, GameObject> chunks = new Dictionary<Vector3i, GameObject>();

        private bool spawningChunk = false;


        private void Awake()
        {
            instance = this;
            chunkPrefab = Resources.Load("Prefab/Chunk") as GameObject;
        }

        
        public void CreateChunk(Vector3i pos)
        {
            if (spawningChunk)
                return;
            StartCoroutine(SpawningChunk(pos));
        }


        public IEnumerator SpawningChunk(Vector3i pos)
        {
            spawningChunk = true;
            Instantiate(chunkPrefab, pos, Quaternion.identity);
            yield return null;
            spawningChunk = false;
        }

        /// <summary>
        /// 通过坐标判断chunk是否存在
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public bool ChunkExists(Vector3i worldPosition)
        {
            return ChunkExists(worldPosition.x, worldPosition.y, worldPosition.z);
        }

        public bool ChunkExists(int X, int Y, int Z)
        {
            return chunks.ContainsKey(new Vector3i(X, Y, Z));
        }

    }
}
