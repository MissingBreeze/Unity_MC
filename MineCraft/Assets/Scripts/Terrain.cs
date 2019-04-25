using LibNoise.Generator;
using ShiFang.Util.Vector;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    /// <summary>
    /// 通过方块的世界坐标获取它的方块类型
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public static byte GetTerrainBlock(Vector3i worldPosition)
    {
        // 噪音对象
        Perlin noise = new Perlin(1f, 1f, 1f, 8, GameManager.rangomSeed, LibNoise.QualityMode.High);
        // 为随机数指定种子，这样子每次随机的都是同样的值
        Random.InitState(GameManager.rangomSeed);
        // 柏林噪声在（0,0）点上下左右对称，设置一个很远的地方作为（0,0）点
        Vector3 offset = new Vector3(Random.value * 100000, Random.value * 100000, Random.value * 100000);

        float noiseX = Mathf.Abs((worldPosition.x + offset.x) / 20);
        float noiseY = Mathf.Abs((worldPosition.y + offset.y) / 20);
        float noiseZ = Mathf.Abs((worldPosition.z + offset.z) / 20);
        double noiseValue = noise.GetValue(noiseX, noiseY, noiseZ);

        noiseValue += (20 - worldPosition.y) / 15f;
        noiseValue /= worldPosition.y / 5f;

        if(noiseValue > 0.5f)
        {
            return 1;
        }
        return 0;
    }

}

