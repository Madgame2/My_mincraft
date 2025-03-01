using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;

public class island_generator //: MonoBehaviour
{

    

    public IEnumerable<(int,int, Chank)> generate_chanks(float[,] map,int map_scale)
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
                Chank new_chank = new Chank(i,j);

                yield return (i,j,buildChank(map, startPoint, map_scale, new_chank));
            }
        }

        yield break;
    }


    private Chank buildChank(float[,] map, Vector2Int start_point, int map_scale, Chank newChank) 
    {
        Vector3Int chankSize = Chank.getChankSize(); 

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
