using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using NaughtyAttributes;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public class Chank_manager : MonoBehaviour
{
    [SerializeField] private Dictionary<Vector2,Chank> chanks = new Dictionary<Vector2,Chank>();

    [Header("Chanks params")]
    [SerializeField] private int chank_width;
    [SerializeField] private int chank_Height;
    [SerializeField] private int chank_depth;

    private island_generator islandGen = new island_generator();

    [Space(10)]
    [Header("island params")]
    [SerializeField] private int Widht;
    [SerializeField] private int Depth;
    [SerializeField] private float scale;
    [SerializeField] private float radius;
    [SerializeField] private float edgeFalloff;
    [SerializeField] private int seed;

    [Space(10)]
    [Header("prefabs and debugs")]
    [SerializeField] private GameObject block;
    [SerializeField] private Material debug_texture;
    [SerializeField] private GameObject islandRoot;

    [Button("Generate")]
    private void debug_start()
    {
        chanks.Clear();
        Chank.set_chank_size(chank_width, chank_Height, chank_depth);


        island_info island = new island_info();
        island.width = Widht;
        island.height = Depth;
        island.scale = scale;
        island.islandRadius = radius;
        island.edgeFalloff = edgeFalloff;
        island.seed = seed;

        float[,] island_noise = island_noise_generatin_v_2.generate_noise(island);

        ApplyTexture(debug_texture, island_noise);

        generateBaseStruct();

        foreach (var (x,y, curent) in islandGen.generate_chanks(island_noise))
        {
            Vector2Int position = new Vector2Int(x,y);
            chanks.Add(position, curent);

            Generate_chank(chanks[position], position);
        }

    }


    [Button("Clear_All")]
    private void clear_ALL()
    {
        chanks.Clear();
        DestroyImmediate(islandRoot);
    }

    private void Start()
    {
        Chank.set_chank_size(chank_width,chank_Height,chank_depth);


    }

    private void generateBaseStruct()
    {
        if (islandRoot != null)
        {
            DestroyImmediate(islandRoot);
        }
        islandRoot = new GameObject("islands");
    }


    private void Generate_chank(Chank chank, Vector2Int postion)
    {
        GameObject ChankRoot = new GameObject("Chank");

        
        Vector3Int chankSize = Chank.getChankSize();
        for(int x = 0; x < chankSize.x; x++)
        {
            for(int z =0;z<chankSize.z; z++)
            {
                for(int y =0;y<chankSize.y; y++)
                {
                    Vector3Int curent = new Vector3Int(x,y,z);
                    int curentBclok = chank[curent];


                    if (curentBclok != 0)
                    {
                        GameObject newBlcok = Instantiate(block);
                        newBlcok.transform.parent = ChankRoot.transform;
                        newBlcok.transform.position = curent;
                    }
                }
            }
        }

        ChankRoot.transform.position = new Vector3(postion.x*Chank.getChankSize().x,0,postion.y*Chank.getChankSize().z);
        ChankRoot.transform.parent = islandRoot.transform;
    }


    public static void ApplyTexture(Material material, float[,] data)
    {
        if (material == null)
        {
            Debug.LogError("Материал не указан!");
            return;
        }

        int width = data.GetLength(0);
        int height = data.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point; // Чёткие пиксели без размытия
        texture.wrapMode = TextureWrapMode.Clamp; // Без повторения краёв

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float value = Mathf.Clamp01(data[x, y]); // Обрезаем значение от 0 до 1
                Color color = new Color(value, value, value, 1); // Градации серого
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply(); // Применяем изменения
        material.mainTexture = texture; // Назначаем текстуру на материал
    }
}
