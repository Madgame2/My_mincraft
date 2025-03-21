using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System;

[ExecuteInEditMode]
public class Blocks_manager : MonoBehaviour
{

    [SerializeField] List<Block> generalBlocks;
    [SerializeField] List<Block> special_objects;

    private static Block[] blocks_array;
    private int general_end;

    private void Start()
    {
        blocks_array = new Block[generalBlocks.Count + special_objects.Count];

        int index = 0;
        foreach (Block block in generalBlocks) {
            blocks_array[index] = block;
            index++;
        }
        general_end = index;

        foreach (Block block in special_objects) {
            blocks_array[index] = block;
            index++;
        }
    }

    private void OnValidate()
    {
        blocks_array = new Block[generalBlocks.Count + special_objects.Count];

        int index = 0;
        foreach (Block block in generalBlocks)
        {
            blocks_array[index] = block;
            index++;
        }
        general_end = index;

        foreach (Block block in special_objects)
        {
            blocks_array[index] = block;
            index++;
        }
    }

    public static Block getBlock(int index){
        if (blocks_array[index] == null) throw new Exception($"Blocks array doesnt exist this index {index}");

        return blocks_array[index];
    }
}
