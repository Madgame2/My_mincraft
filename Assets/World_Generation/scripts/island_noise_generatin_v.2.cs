using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class island_info
{
    public int width = 256;  // Ширина карты
    public int height = 256; // Высота карты
    public float scale = 20f; // Масштаб шума
    public float islandRadius = 0.5f; // Радиус острова
    public float edgeFalloff = 3f; // Резкость края острова
    public int seed = 42; // Сид для генерации шума
}

public class island_noise_generatin_v_2 : MonoBehaviour
{
    static island_info island;

    static float offSetX;
    static float offSetY;

    static public float[,] generate_noise(island_info info)
    {
        island = info;
        System.Random rand = new System.Random(info.seed);

        float[,] result = new float[info.width, info.height];
        offSetX = (float)rand.NextDouble() * 1000;
        offSetY = (float)rand.NextDouble() * 1000;

        for (int x=0; x<info.width; x++)
        {
            for(int y=0; y<info.height; y++)
            {
                float xCoord = (float)x / info.width -0.5f;
                float yCoord = (float)y / info.height - 0.5f;

                //float distance = Mathf.Sqrt(xCoord * xCoord + yCoord * yCoord);
                //float mask = calculate_mask(xCoord, yCoord);

                float noiseValue = Mathf.PerlinNoise((xCoord * info.scale) + offSetX, (yCoord * info.scale) + offSetY);

                float finalyValue = noiseValue;

                result[x, y] = finalyValue;

            }
        }

        return add_ditales(result);
    }


    static float calculate_mask(float xCord, float yCord)
    {
        float distance = Mathf.Sqrt(xCord * xCord + yCord * yCord);

        // Дополнительный шум для радиального искажения
        float radialNoise = Mathf.PerlinNoise(
            (xCord + offSetX) * island.scale * 0.5f,
            (yCord + offSetY) * island.scale * 0.5f
        );

        // Маска с учетом радиального шума
        float mask = Mathf.Clamp01(1 - Mathf.Pow(distance / (island.islandRadius * (0.8f + radialNoise * 0.6f)), island.edgeFalloff));

        return mask;
    }

    static float[,] add_ditales(float[,] baseMap)
    {
        int width = island.width;
        int height = island.height;

        float ditaleScale1 = island.scale * 2f;
        float ditaleScale2 = island.scale * 4f;

        float[,] new_map = new float[width, height];

        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width-0.5f;
                float yCoord = (float)y / height-0.5f;

                float baise_noise = baseMap[x, y];

                float mask = calculate_mask(xCoord, yCoord);

                float DetaileLayer1 = Mathf.PerlinNoise(
                    (xCoord * ditaleScale1) + offSetX,
                    (yCoord * ditaleScale1) + offSetY
                    ) * 0.2f;

                float DetaileLayer2 = Mathf.PerlinNoise(
                        (xCoord * ditaleScale2) + offSetX,
                        (yCoord * ditaleScale2) + offSetY
                    ) * 0.1f;

                new_map[x, y] = (baise_noise + DetaileLayer1+DetaileLayer2)*mask;
            }
        }

        return new_map;
    }
}
