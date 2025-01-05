using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class island_noise_generator 
{
    static public float [,] generate_noise_map(int widht, int height, float noiseScale, float center_scale, int seed)
    {
        float [,] noise_map = new float[widht, height];

        System.Random rand = new System.Random(seed);
        float offsetX = rand.Next(-100000, 100000);
        float offsetY = rand.Next(-100000, 100000);

        for(int x = 0; x < widht; x++)
        {
            for(int y = 0; y < height; y++)
            {
                float distanse = Vector2.Distance(new Vector2(x,y), new Vector2(widht/2,height/2))* center_scale;


                float xCord = (float)x / widht * noiseScale + offsetX;
                float yCord = (float)y / height * noiseScale + offsetY;

                noise_map[x, y] = Mathf.PerlinNoise(xCord, yCord);///(Mathf.Pow( distanse,0.2f));

                //noise_map[x, y] = 1f - Mathf.PerlinNoise(xCord, yCord) * Mathf.Pow(distanse, 2);
            }
        }

        return noise_map;
    }
}
