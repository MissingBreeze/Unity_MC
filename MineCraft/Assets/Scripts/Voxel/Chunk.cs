using System;
using System.Collections;
using System.Collections.Generic;
using ShiFang.Util.Vector;
using UnityEngine;

namespace ShiFang.Scripts.Voxel
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        public static int width = 16;
        public static int height = 16;

        public byte[,,] blocks;
        public Vector3i position;
        private Mesh mesh;
        /// <summary>
        /// 面需要的点
        /// </summary>
        private List<Vector3> vertices = new List<Vector3>();

        /// <summary>
        /// 生成三角面时用到的vertices的index
        /// </summary>
        private List<int> triangles = new List<int>();

        /// <summary>
        /// 所有的UV信息
        /// </summary>
        private List<Vector2> uv = new List<Vector2>();
        /// <summary>
        /// uv贴图每行每列的宽度(0~1)，这里我的贴图是32×32的，所以是1/32
        /// </summary>
        public static float textureOffset = 1 / 32f;
        /// <summary>
        /// 让UV稍微缩小一点，避免出现它旁边的贴图
        /// </summary>
        public static float shrinkSize = 0.001f;

        /// <summary>
        /// 当前chunk是否正在生成
        /// </summary>
        private bool isWorking = false;

        private void Start()
        {
            position = new Vector3i(this.transform.position);
            if (Map.instance.chunks.ContainsKey(position))
            {
                Destroy(this);
            }
            else
            {
                Map.instance.chunks.Add(position, gameObject);
                this.name = string.Format("({0},{1},{2})", position.x, position.y, position.z);
                StartFunction();
            }
        }

        private void StartFunction()
        {
            mesh = new Mesh();
            mesh.name = "Chunk";
            StartCoroutine(CreateMap());
        }

        IEnumerator CreateMap()
        {
            while (isWorking)
            {
                yield return null;
            }
            isWorking = true;
            blocks = new byte[width, height, width];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < width; z++)
                    {
                        if(y == height - 1)
                        {
                            if(UnityEngine.Random.Range(1,5) == 1)
                            {
                                blocks[x, y, z] = 2;
                            }
                            else
                            {
                                blocks[x, y, z] = 1;
                            }
                        }
                        else
                        {
                            blocks[x, y, z] = 1;
                        } 
                    }
                }
            }
            StartCoroutine(CreateMesh());
        }

        IEnumerator CreateMesh()
        {
            vertices.Clear();
            triangles.Clear();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < width; z++)
                    {
                        Block block = BlockList.GetBlock(blocks[x, y, z]);
                        if (block == null) continue;

                        if (IsBlockTransparent(x + 1, y, z))
                        {
                            AddFrontFace(x, y, z, block);
                        }
                        if (IsBlockTransparent(x - 1, y, z))
                        {
                            AddBackFace(x, y, z, block);
                        }
                        if (IsBlockTransparent(x, y, z + 1))
                        {
                            AddRightFace(x, y, z, block);
                        }
                        if (IsBlockTransparent(x, y, z - 1))
                        {
                            AddLeftFace(x, y, z, block);
                        }
                        if (IsBlockTransparent(x, y + 1, z))
                        {
                            AddTopFace(x, y, z, block);
                        }
                        if (IsBlockTransparent(x, y - 1, z))
                        {
                            AddBottomFace(x, y, z, block);
                        }
                    }
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uv.ToArray();

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;

            yield return null;
            isWorking = false;
        }

        public static bool IsBlockTransparent(int x,int y,int z)
        {
            if (x >= width || y >= height || z >= width || x < 0 || y < 0 || z < 0)
            {
                return true;
            }
            return false;
        }

        //前面
        void AddFrontFace(int x, int y, int z, Block block)
        {
            //第一个三角面
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);

            //第二个三角面
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);


            //添加4个点
            vertices.Add(new Vector3(0 + x, 0 + y, 0 + z));
            vertices.Add(new Vector3(0 + x, 0 + y, 1 + z));
            vertices.Add(new Vector3(0 + x, 1 + y, 1 + z));
            vertices.Add(new Vector3(0 + x, 1 + y, 0 + z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.textureFrontX * textureOffset, block.textureFrontY * textureOffset) + new Vector2(shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureFrontX * textureOffset + textureOffset, block.textureFrontY * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureFrontX * textureOffset + textureOffset, block.textureFrontY * textureOffset + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureFrontX * textureOffset, block.textureFrontY * textureOffset + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
        }

        //背面
        void AddBackFace(int x, int y, int z, Block block)
        {
            //第一个三角面
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);

            //第二个三角面
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);


            //添加4个点
            vertices.Add(new Vector3(-1 + x, 0 + y, 1 + z));
            vertices.Add(new Vector3(-1 + x, 0 + y, 0 + z));
            vertices.Add(new Vector3(-1 + x, 1 + y, 0 + z));
            vertices.Add(new Vector3(-1 + x, 1 + y, 1 + z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            if(block == null)
            {
                Debug.LogError("");
            }
            uv.Add(new Vector2(block.textureBackX * textureOffset, block.textureBackY * textureOffset) + new Vector2(shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureBackX * textureOffset + textureOffset, block.textureBackY * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureBackX * textureOffset + textureOffset, block.textureBackY * textureOffset + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureBackX * textureOffset, block.textureBackY * textureOffset + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
        }

        //右面
        void AddRightFace(int x, int y, int z, Block block)
        {
            //第一个三角面
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);

            //第二个三角面
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);


            //添加4个点
            vertices.Add(new Vector3(0 + x, 0 + y, 1 + z));
            vertices.Add(new Vector3(-1 + x, 0 + y, 1 + z));
            vertices.Add(new Vector3(-1 + x, 1 + y, 1 + z));
            vertices.Add(new Vector3(0 + x, 1 + y, 1 + z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.textureRightX * textureOffset, block.textureRightY * textureOffset) + new Vector2(shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureRightX * textureOffset + textureOffset, block.textureRightY * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureRightX * textureOffset + textureOffset, block.textureRightY * textureOffset + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureRightX * textureOffset, block.textureRightY * textureOffset + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
        }

        //左面
        void AddLeftFace(int x, int y, int z, Block block)
        {
            //第一个三角面
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);

            //第二个三角面
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);


            //添加4个点
            vertices.Add(new Vector3(-1 + x, 0 + y, 0 + z));
            vertices.Add(new Vector3(0 + x, 0 + y, 0 + z));
            vertices.Add(new Vector3(0 + x, 1 + y, 0 + z));
            vertices.Add(new Vector3(-1 + x, 1 + y, 0 + z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.textureLeftX * textureOffset, block.textureLeftY * textureOffset) + new Vector2(shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureLeftX * textureOffset + textureOffset, block.textureLeftY * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureLeftX * textureOffset + textureOffset, block.textureLeftY * textureOffset + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureLeftX * textureOffset, block.textureLeftY * textureOffset + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
        }

        //上面
        void AddTopFace(int x, int y, int z, Block block)
        {
            //第一个三角面
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);

            //第二个三角面
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);


            //添加4个点
            vertices.Add(new Vector3(0 + x, 1 + y, 0 + z));
            vertices.Add(new Vector3(0 + x, 1 + y, 1 + z));
            vertices.Add(new Vector3(-1 + x, 1 + y, 1 + z));
            vertices.Add(new Vector3(-1 + x, 1 + y, 0 + z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.textureTopX * textureOffset, block.textureTopY * textureOffset) + new Vector2(shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureTopX * textureOffset + textureOffset, block.textureTopY * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureTopX * textureOffset + textureOffset, block.textureTopY * textureOffset + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureTopX * textureOffset, block.textureTopY * textureOffset + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
        }

        //下面
        void AddBottomFace(int x, int y, int z, Block block)
        {
            //第一个三角面
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);

            //第二个三角面
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);


            //添加4个点
            vertices.Add(new Vector3(-1 + x, 0 + y, 0 + z));
            vertices.Add(new Vector3(-1 + x, 0 + y, 1 + z));
            vertices.Add(new Vector3(0 + x, 0 + y, 1 + z));
            vertices.Add(new Vector3(0 + x, 0 + y, 0 + z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.textureBottomX * textureOffset, block.textureBottomY * textureOffset) + new Vector2(shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureBottomX * textureOffset + textureOffset, block.textureBottomY * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureBottomX * textureOffset + textureOffset, block.textureBottomY * textureOffset + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureBottomX * textureOffset, block.textureBottomY * textureOffset + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
        }

    }
}
