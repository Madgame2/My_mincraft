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
    private Dictionary<Mesh,Material> Materials = new Dictionary<Mesh,Material>();

    public event Action<Vector2Int,Mesh> meshsUpdated;

    public Dictionary<Mesh, Material> get_Materials => Materials;
    public Chank(int x, int y)
    {
        if (widht == null || height == null || depth == null)
        {
            throw new InvalidDataException("chaks demension is not defined");
        }

        blocks = new int[widht.Value * height.Value * depth.Value];


        postiont=  new Vector2Int(x,y);
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
        {

            if (ny < 0 || ny >= height)
            {
                return false;
            }

            Vector2Int neighboring_chank_position = new Vector2Int(postiont.x,postiont.y);
            Vector3Int Block_pos = new Vector3Int(nx,ny,nz);

            if (nx < 0) { 
                neighboring_chank_position.x = postiont.x-1;
                Block_pos.x = widht.Value - 1;
            }else if (nx>=widht)
            {
                neighboring_chank_position.x = postiont.x + 1;
                Block_pos.x = 0;
            }

            if (nz < 0)
            {
                neighboring_chank_position.y = postiont.y - 1;
                Block_pos.z = depth.Value - 1;
            }
            else if (nz >= depth)
            {
                neighboring_chank_position.y = postiont.y + 1;
                Block_pos.z = 0;
            }

            Chank neighbor_chank = Chank_manager.get_chank(neighboring_chank_position);


            if (neighbor_chank == null)
            {
                return true;   
            }

            //Debug.Log(Block_pos);

            int curentBlock = neighbor_chank[Block_pos];

            return curentBlock==0;

        }

        Vector3Int curent = new Vector3Int(nx, ny, nz);
        int curentBclok = this[curent];

        return curentBclok == 0;
    }

    public void generateMesh()
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
                            newCombine.mesh = Blocks_manager.getBlock(0).Forward;
                            newCombine.transform = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                            combineInstances.Add(newCombine);
                        }

                        if (IsFaceExposed(pos, Vector3.back))
                        {
                            CombineInstance newCombine = new CombineInstance();
                            newCombine.mesh = Blocks_manager.getBlock(0).Backward;
                            newCombine.transform = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                            combineInstances.Add(newCombine);
                        }

                        if (IsFaceExposed(pos, Vector3.left))
                        {
                            CombineInstance newCombine = new CombineInstance();
                            newCombine.mesh = Blocks_manager.getBlock(0).Left;
                            newCombine.transform = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                            combineInstances.Add(newCombine);
                        }

                        if (IsFaceExposed(pos, Vector3.right))
                        {
                            CombineInstance newCombine = new CombineInstance();
                            newCombine.mesh = Blocks_manager.getBlock(0).Right;
                            newCombine.transform = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                            combineInstances.Add(newCombine);
                        }

                        if (IsFaceExposed(pos, Vector3.up))
                        {
                            CombineInstance newCombine = new CombineInstance();
                            newCombine.mesh = Blocks_manager.getBlock(0).Up;
                            newCombine.transform = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                            combineInstances.Add(newCombine);
                        }

                        if (IsFaceExposed(pos, Vector3.down))
                        {
                            CombineInstance newCombine = new CombineInstance();
                            newCombine.mesh = Blocks_manager.getBlock(0).Down;
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

        Materials[mesh] = Blocks_manager.getBlock(0).Material;
    }


    public void addBlock(Vector3Int pos, int blockID)
    {
        if (this[pos] != 0)
        {
            throw new Exception($"Block already exist: chank{this}, pos: {pos}, block_id: {blockID}");
        }

        this[pos] = blockID;

        generateMesh();


        meshsUpdated?.Invoke(postiont, mesh);
    }
}