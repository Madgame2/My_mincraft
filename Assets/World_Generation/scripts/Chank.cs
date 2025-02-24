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

    public Chank()
    {
        if (widht == null || height == null || depth == null)
        {
            throw new InvalidDataException("chaks demension is not defined");
        }

        blocks = new int[widht.Value * height.Value * depth.Value];

    }

    public ref int this[int x, int y, int z]
    {
        get
        {
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

    public static void set_chank_size(int x, int y, int z)
    {
        widht = x; height = y; depth = z;
    }
    public static void set_chank_size(Vector3Int size)
    {
        widht = size.x;
        height = size.y;
        depth = size.z;
    }

    private int get_index(int x, int y, int z)
    {
        return x + widht.Value * (y + height.Value * z);
    }

    private int get_index(Vector3Int position)
    {
        return position.x + widht.Value * (position.y + height.Value * position.z);
    }


    public static Vector3Int getChankSize()
    {
        return new Vector3Int(widht.Value, height.Value, depth.Value);
    }

    public void set_position(Vector2Int position)
    {

        this.postiont = position;
    }


    private bool IsFaceExposed(Vector3 blockPos, Vector3 faceDir)
    {
        int nx = (int)blockPos.x + (int)faceDir.x;
        int ny = (int)blockPos.y + (int)faceDir.y;
        int nz = (int)blockPos.z + (int)faceDir.z;

        if (nx < 0 || nx >= widht || ny < 0 || ny >= height || nz < 0 || nz >= depth)
            return true;

        Vector3Int curent = new Vector3Int(nx, ny, nz);
        int curentBclok = this[curent];

        return curentBclok == 0;
    }

    public void generateMesh(Blocks_manager bloks)
    {

        List<CombineInstance> combineInstances = new List<CombineInstance>();



        for (int x = 0; x < widht.Value; x++)
        {
            for (int y = 0; y < height.Value; y++)
            {
                for (int z = 0; z < depth.Value; z++)
                {
                    Vector3Int curent = new Vector3Int(x, y, z);
                    int curentBclok = this[curent];


                    if (curentBclok != 0)
                    {

                        Vector3 pos = new Vector3(x, y, z);

                        if (IsFaceExposed(pos, Vector3.forward))
                        {
                            CombineInstance newCombine = new CombineInstance();
                            newCombine.mesh = bloks[0].Forward;
                            newCombine.transform = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                            combineInstances.Add(newCombine);
                        }

                        if (IsFaceExposed(pos, Vector3.back))
                        {
                            CombineInstance newCombine = new CombineInstance();
                            newCombine.mesh = bloks[0].Backward;
                            newCombine.transform = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                            combineInstances.Add(newCombine);
                        }

                        if (IsFaceExposed(pos, Vector3.left))
                        {
                            CombineInstance newCombine = new CombineInstance();
                            newCombine.mesh = bloks[0].Left;
                            newCombine.transform = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                            combineInstances.Add(newCombine);
                        }

                        if (IsFaceExposed(pos, Vector3.right))
                        {
                            CombineInstance newCombine = new CombineInstance();
                            newCombine.mesh = bloks[0].Right;
                            newCombine.transform = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                            combineInstances.Add(newCombine);
                        }

                        if (IsFaceExposed(pos, Vector3.up))
                        {
                            CombineInstance newCombine = new CombineInstance();
                            newCombine.mesh = bloks[0].Up;
                            newCombine.transform = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                            combineInstances.Add(newCombine);
                        }

                        if (IsFaceExposed(pos, Vector3.down))
                        {
                            CombineInstance newCombine = new CombineInstance();
                            newCombine.mesh = bloks[0].Down;
                            newCombine.transform = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                            combineInstances.Add(newCombine);
                        }
                    }
                }
            }
        }

        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.CombineMeshes(combineInstances.ToArray());

    }
}