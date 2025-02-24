using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class Blocks_manager : MonoBehaviour
{

    [SerializeField] List<Block> generalBlocks;
    [SerializeField] List<Block> special_objects;

    private Block[] blocks_array;
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

        Debug.Log(blocks_array);
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

        Debug.Log(this[0]);
    }

    public Block this[int index]{
        get => blocks_array[index];    
    }
}
