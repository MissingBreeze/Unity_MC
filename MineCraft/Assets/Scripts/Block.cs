
using UnityEngine;
/// <summary>
/// 方块的方向枚举
/// </summary>
public enum BlockDirection : byte
{
    Front = 0,
    Back = 1,
    Left = 2,
    Right = 3,
    Top = 4,
    Bottom = 5
}

public class Block
{
    /// <summary>
    /// 方块id
    /// </summary>
    public byte id;

    /// <summary>
    /// 方块名字
    /// </summary>
    public string name;

    /// <summary>
    /// 方块图标
    /// </summary>
    public Texture icon;

    /// <summary>
    /// 方向（指前面所面朝的方向）
    /// </summary>
    public BlockDirection direction = BlockDirection.Front;

    // 前面贴图的坐标
    public byte textureFrontX;
    public byte textureFrontY;

    // 后面贴图的坐标
    public byte textureBackX;
    public byte textureBackY;

    // 右面贴图的坐标
    public byte textureRightX;
    public byte textureRightY;

    // 左面贴图的坐标
    public byte textureLeftX;
    public byte textureLeftY;

    // 上面贴图的坐标
    public byte textureTopX;
    public byte textureTopY;

    // 下面贴图的坐标
    public byte textureBottomX;
    public byte textureBottomY;

    /// <summary>
    /// 都是A面的方块
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="textureX"></param>
    /// <param name="textureY"></param>
    public Block(byte id, string name, byte textureX, byte textureY) : this(id, name, textureX, textureY, textureX, textureY, textureX, textureY, textureX, textureY)
    {
    }

    /// <summary>
    /// 上面是A，其他面是B的方块
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="textureX"></param>
    /// <param name="textureY"></param>
    /// <param name="textureTopX"></param>
    /// <param name="textureTopY"></param>
    public Block(byte id, string name, byte textureX, byte textureY,byte textureTopX,byte textureTopY):this(id,name,textureX,textureY,textureX,textureY, textureX, textureY, textureX, textureY, textureTopX, textureTopY, textureX, textureY)
    {
    }

    /// <summary>
    /// 上面是A，下面是B，其他面是C的方块
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="textureX"></param>
    /// <param name="textureY"></param>
    /// <param name="textureTopX"></param>
    /// <param name="textureTopY"></param>
    /// <param name="textureBottomX"></param>
    /// <param name="textureBottomY"></param>
    public Block(byte id, string name, byte textureX, byte textureY, byte textureTopX, byte textureTopY, byte textureBottomX, byte textureBottomY)
        : this(id, name, textureX, textureY, textureX, textureY, textureX, textureY, textureX, textureY, textureTopX, textureTopY, textureBottomX, textureBottomY)
    {
    }

    /// <summary>
    /// 上面是A，下面是B，前面是C，其他面是D的方块
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="textureFrontX"></param>
    /// <param name="textureFrontY"></param>
    /// <param name="textureX"></param>
    /// <param name="textureY"></param>
    /// <param name="textureTopX"></param>
    /// <param name="textureTopY"></param>
    /// <param name="textureBottomX"></param>
    /// <param name="textureBottomY"></param>
    public Block(byte id, string name, byte textureFrontX, byte textureFrontY, byte textureX, byte textureY, byte textureTopX, byte textureTopY, byte textureBottomX, byte textureBottomY)
        : this(id, name, textureFrontX, textureFrontY, textureX, textureY, textureX, textureY, textureX, textureY, textureTopX, textureTopY, textureBottomX, textureBottomY)
    {
    }

    /// <summary>
    /// 上下左右前后面都不一样的方块
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="textureFrontX"></param>
    /// <param name="textureFrontY"></param>
    /// <param name="textureBackX"></param>
    /// <param name="textureBackY"></param>
    /// <param name="textureRightX"></param>
    /// <param name="textureRightY"></param>
    /// <param name="textureLeftX"></param>
    /// <param name="textureLeftY"></param>
    /// <param name="textureTopX"></param>
    /// <param name="textureTopY"></param>
    /// <param name="textureBottomX"></param>
    /// <param name="textureBottomY"></param>
    public Block(byte id, string name, byte textureFrontX, byte textureFrontY, byte textureBackX, byte textureBackY, byte textureRightX, byte textureRightY,
        byte textureLeftX, byte textureLeftY, byte textureTopX, byte textureTopY, byte textureBottomX, byte textureBottomY)
    {
        this.id = id;
        this.name = name;
        this.textureFrontX = textureFrontX;
        this.textureFrontY = textureFrontY;

        this.textureBackX = textureBackX;
        this.textureBackY = textureBackY;

        this.textureRightX = textureRightX;
        this.textureRightY = textureRightY;

        this.textureLeftX = textureLeftX;
        this.textureLeftY = textureLeftY;

        this.textureTopX = textureTopX;
        this.textureTopY = textureTopY;

        this.textureBottomX = textureBottomX;
        this.textureBottomY = textureBottomY;
    }

}
