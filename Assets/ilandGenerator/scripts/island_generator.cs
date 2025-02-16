using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;

public class island_generator //: MonoBehaviour
{

    [Space(10)]
    [Header("generation parmas")]
    [SerializeField] private int map_scale = 10;
    [SerializeField] private GameObject root;

    [Space(10)]
    [Header("prefabs and debugs")]
    [SerializeField] private GameObject block;
    [SerializeField] private Texture2D debug_texture;

    private void generate_base_island(float[,] map)
    {


        for (int x =0; x < map.GetLength(0); x++)
        {
            for(int y =0; y < map.GetLength(1); y++)
            {
                int height = (int)(map[x, y] * map_scale);

                for(int i = 0; i < height; i++)
                {
                    //GameObject cube = Instantiate(block);
                    //cube.transform.parent = isalnd;


                    //cube.transform.position = new Vector3(x, i, y);
                }
            }
        }
    }

    private void combine_meshs()
    {
        GameObject core_object = root.transform.Find("island").gameObject;

        List<MeshFilter> childs_meshs = new List<MeshFilter>(core_object.GetComponentsInChildren<MeshFilter>());

        MeshFilter parent_mash_filter = core_object.GetComponent<MeshFilter>();
        if(parent_mash_filter == null)
        {
            parent_mash_filter = core_object.AddComponent<MeshFilter>();// = gameObject.AddComponent<MeshFilter>();
        }

        CombineInstance[] combine = new CombineInstance[childs_meshs.Count];


        int index = 0;

        foreach(MeshFilter child in childs_meshs)
        {
            combine[index].mesh = child.sharedMesh;
            combine[index].transform = child.transform.localToWorldMatrix;

            child.gameObject.SetActive(false);
            index++;
        }

        Mesh combineMash = new Mesh();

        combineMash.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        combineMash.CombineMeshes(combine,mergeSubMeshes:true,useMatrices:true);


        parent_mash_filter.mesh = combineMash;


        MeshRenderer renderer = root.GetComponent<MeshRenderer>();
        if(renderer == null)
        {
            renderer = core_object.AddComponent<MeshRenderer>();
        }

        renderer.material = block.GetComponent<Renderer>().sharedMaterial;
    }


    public IEnumerable<(int,int, Chank)> generate_chanks(float[,] map)
    {
        Vector3Int chankSize = Chank.getChankSize();

        int x_iterations = map.GetLength(0)/chankSize.x;
        if (map.GetLength(0) % chankSize.x != 0)
        {
            x_iterations++;
        }

        int z_iterations = map.GetLength(1) / chankSize.z;
        if (map.GetLength(1) % chankSize.z != 0)
        {
            z_iterations++;
        }

        for(int i = 0; i < x_iterations; i++)
        {
            for(int j =0;j< z_iterations; j++)
            {
                Vector2Int startPoint = new Vector2Int(i*chankSize.x, j*chankSize.z);

                yield return (i,j,buildChank(map, startPoint));
            }
        }

        yield break;
    }


    private Chank buildChank(float[,] map, Vector2Int start_point) 
    {
        Vector3Int chankSize = Chank.getChankSize(); 

        Chank newChank = new Chank();
        for(int local_x = 0; local_x < chankSize.x; local_x++)
        {
            for(int local_z = 0;local_z < chankSize.z; local_z++)
            {
                Vector2Int globalIndex = new Vector2Int(local_x+start_point.x,local_z+start_point.y);
                int height = (int)(map[globalIndex.x, globalIndex.y] * map_scale);


                for(int local_y = 0; local_y < height; local_y++)
                {
                    newChank[local_x, local_y, local_z] = 1;
                }
            }
        }

        return newChank;
    }
}
