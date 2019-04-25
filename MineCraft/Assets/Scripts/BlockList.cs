
using System.Collections.Generic;
using UnityEngine;

namespace ShiFang.Scripts
{
    /// <summary>
    /// 存储所有的Block对象的信息
    /// </summary>
    public class BlockList : MonoBehaviour
    {

        public static Dictionary<byte, Block> blocks = new Dictionary<byte, Block>();

        private void Awake()
        {
            Block dirt = new Block(1, "Dirt", 2, 31);
            blocks.Add(dirt.id, dirt);

            Block grass = new Block(2, "Grass", 3, 31, 0, 31, 2, 31);
            blocks.Add(grass.id, grass);

            Block stone = new Block(3, "Stone", 1, 31);
            blocks.Add(stone.id, stone);
        }

        public static Block GetBlock(byte id)
        {
            return blocks.ContainsKey(id) ? blocks[id] : null;
        }
    }
}
