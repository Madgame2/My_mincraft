using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;

public class island_generator : MonoBehaviour
{
    [Header("island params")]
    [SerializeField] private int Widht;
    [SerializeField] private int Depth;
    [SerializeField] private float scale;
    [SerializeField] private float radius;
    [SerializeField] private float edgeFalloff;
    [SerializeField] private int seed;

    [Space(10)]
    [Header("generation parmas")]
    [SerializeField] private int map_scale = 10;
    [SerializeField] private GameObject root;

    [Space(10)]
    [Header("prefabs and debugs")]
    [SerializeField] private GameObject block;
    public Material blockMaterial;
    [SerializeField] private Texture2D debug_texture;
    [SerializeField] private Material Debug_materila;

    [Button("Generaate")]
    public void generate_isalnd()
    {
        island_info island = new island_info();
        island.width = Widht;
        island.height = Depth;
        island.scale = scale;
        island.islandRadius = radius;
        island.edgeFalloff = edgeFalloff;
        island.seed = seed;

        float[,] island_map = island_noise_generatin_v_2.generate_noise(island); //island_noise_generator.generate_noise_map(Widht, Depth, scale, center_scale, 42);

        debug_texture = generateTexture(island_map);
        Debug_materila.mainTexture = debug_texture;

        Renderer render = GetComponent<Renderer>();
        if (render != null)
        {
            render.material.mainTexture = debug_texture;
        }

        generate_base_struct();
        generate_base_island(island_map);
    }

    private void generate_base_struct()
    {
        if (root != null)
        {
            DestroyImmediate(root);
        }

        root = new GameObject("map");
        GameObject island = new GameObject("island");

        island.transform.parent = root.transform;
    }
    private void generate_base_island(float[,] map)
    {
        Transform isalnd = root.transform.Find("island");


        for (int x =0; x < map.GetLength(0); x++)
        {
            for(int y =0; y < map.GetLength(1); y++)
            {
                int height = (int)(map[x, y] * map_scale);

                for(int i = 0; i < height; i++)
                {
                    GameObject cube = Instantiate(block);
                    cube.transform.parent = isalnd;


                    cube.transform.position = new Vector3(x, i, y);
                }
            }
        }

        combine_meshs();
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

    private void Start()
    {
        generate_isalnd();
    }

    private Texture2D generateTexture(float[,] noise_map)
    {
        int widht = noise_map.GetLength(0);
        int height = noise_map.GetLength(1);
        Texture2D debug_texture = new Texture2D(widht, height);

        for(int x = 0; x < widht; x++)
        {
            for(int y = 0; y < height; y++)
            {
                float value = noise_map[x, y];
                Color color = new Color(value, value, value);
                debug_texture.SetPixel(x, y, color);
            }
        }

        debug_texture.Apply();
        return debug_texture;
    }


    private void addCubeMesh(Vector3 position, List<Vector3> vertices, List<int> triangles, ref int vertexIndex)
    {

    }
}
