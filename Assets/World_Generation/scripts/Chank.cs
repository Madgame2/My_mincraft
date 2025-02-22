using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;


public enum FaceType { Front, Back, Up, Down, Right, Left };
public class Chank
{
    static int? widht;
    static int? height;
    static int? depth;


    private int[] blocks;
    public Vector2Int postiont { get; private set; }

    public Mesh mesh { get; private set; }

    private List<Vector3> vertices;
    private List<Vector2> uvs;
    private List<int> triangles;

    private static readonly Dictionary<FaceType, Vector3[]> faceVertexOffsets = new Dictionary<FaceType, Vector3[]>()
    {
        { FaceType.Front, new Vector3[] { new Vector3(0,0,1), new Vector3(1,0,1), new Vector3(1,1,1), new Vector3(0,1,1) } },
        { FaceType.Back,  new Vector3[] { new Vector3(1,0,0), new Vector3(0,0,0), new Vector3(0,1,0), new Vector3(1,1,0) } },
        { FaceType.Up,    new Vector3[] { new Vector3(0,1,1), new Vector3(1,1,1), new Vector3(1,1,0), new Vector3(0,1,0) } },
        { FaceType.Down,  new Vector3[] { new Vector3(0,0,0), new Vector3(1,0,0), new Vector3(1,0,1), new Vector3(0,0,1) } },
        { FaceType.Right, new Vector3[] { new Vector3(1,0,1), new Vector3(1,0,0), new Vector3(1,1,0), new Vector3(1,1,1) } },
        { FaceType.Left,  new Vector3[] { new Vector3(0,0,0), new Vector3(0,0,1), new Vector3(0,1,1), new Vector3(0,1,0) } }
    };

    // Шаблон треугольников для грани (индексы относительно 4 вершин)
    private static readonly int[] faceTriangles = new int[] { 0, 2, 1, 0, 3, 2 };

    // Стандартные UV-координаты для грани
    private static readonly Vector2[] faceUVs = new Vector2[]
    {
        new Vector2(0,0), new Vector2(1,0), new Vector2(1,1), new Vector2(0,1)
    };

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

    public void set_position(Vector2Int position) {

        this.postiont = position;
    }


    private bool IsFaceExposed(Vector3 blockPos, Vector3 faceDir)
    {
        int nx = (int)blockPos.x + (int)faceDir.x;
        int ny = (int)blockPos.y + (int)faceDir.y;
        int nz = (int)blockPos.z + (int)faceDir.z;

        if (nx < 0 || nx >= widht || ny < 0 || ny >= height || nz < 0 || nz >= depth)
            return true;

        return get_index(nx, ny, nz) == 0;
    }

    //private void addFace(Vector3 blockPos,FaceType faceType)
    //{
    //    int vCount = vertices.Count;
    //    Vector3[] offsets = faceVertexOffsets[faceType];

    //    for (int i = 0; i < offsets.Length; i++)
    //    {
    //        vertices.Add(blockPos+offsets[i]);
    //    }

    //    for(int i =0;i< faceTriangles.Length; i++)
    //    {
    //        triangles.Add(vCount + faceTriangles[i]);
    //    }

    //    uvs.AddRange(faceUVs);
    //}
}
