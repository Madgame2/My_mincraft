using UnityEngine;
using System;
using System.IO;

public class Chank
{
    static int? widht;
    static int? height;
    static int? depth;


    private int[] blocks;
    public Chank()
    {
        if (widht == null || height == null || depth == null) {
            throw new InvalidDataException("chaks demension is not defined");
        }

        blocks = new int[widht.Value * height.Value * depth.Value];

    }

    public ref int this[int x, int y, int z]
    {
        get { 
            return ref blocks[get_index(x, y, z)];
        }    
     }

    public ref int this[Vector3Int block_position]
    {
        get
        {
            return ref blocks[get_index(block_position)];
        }
    }

    public static void set_chank_size(int x, int y,int z)
    {
        widht=x; height=y; depth=z;
    }
    public static void set_chank_size(Vector3Int size)
    {
        widht = size.x;
        height = size.y;
        depth = size.z;
    }

    private int get_index(int x,int y,int z)
    {
        return x + widht.Value * (z + depth.Value * y);
    }
    private int get_index(Vector3Int position)
    {
        return position.x + widht.Value * (position.z + depth.Value * position.y);
    }


    public static Vector3Int getChankSize()
    {
        return new Vector3Int(widht.Value, height.Value, depth.Value);
    }
}
