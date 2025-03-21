using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UIElements;
using System;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;

public class Builder : MonoBehaviour
{
    [SerializeField] private Chank active_chunk;
    [SerializeField] private Vector3Int localBlockPos;
    [SerializeField] private Vector3 Normal;
    [SerializeField] GameObject selecteble_area_prefab;

    [Space(10)]
    [Header("Debug")]
    [SerializeField] private GameObject debugObject;
    [SerializeField] private Vector3 debugGlobalPos;


    private GameObject rednered_prefab;
    public void SelectBlock(Vector3 block_globalPos,Vector3 normal)
    {
        if (rednered_prefab)
        {
            Destroy(rednered_prefab);
        }

        this.Normal = normal;
        
        Vector3 chankSize = Chank.getChankSize();

        Vector2Int chankPos = new Vector2Int((int)(block_globalPos.x / chankSize.x), (int)(block_globalPos.z / chankSize.z));


        active_chunk = Chank_manager.get_chank(chankPos);
        localBlockPos = getLockalPost(block_globalPos);




        rednered_prefab = Instantiate(selecteble_area_prefab);

        rednered_prefab.transform.position = getAreaPosition(block_globalPos, normal);
        rotate_Area(normal, rednered_prefab.transform);

        //Debug.Log(block_globalPos);

        debugGlobalPos = block_globalPos;
    }

    public void build()
    {

        if (active_chunk == null) return;


        Vector3Int pos = new Vector3Int((int)(localBlockPos.x + Normal.x),
                                        (int)(localBlockPos.y + Normal.y),
                                        (int)(localBlockPos.z + Normal.z));

        active_chunk.addBlock(pos, 1);

        //GameObject newObject = Instantiate(debugObject);

        //newObject.transform.position = debugGlobalPos+Normal;
    }

    public void undoSelection()
    {
        if (rednered_prefab)
        {
            Destroy(rednered_prefab);
        }
        active_chunk = null;
        localBlockPos = default;
    }

    private Vector3Int getLockalPost(Vector3 global)
    {
        Vector3 chankSize = Chank.getChankSize();
        Vector3Int pos = new Vector3Int((int)(global.x % chankSize.x), (int)(global.y % chankSize.y), (int)(global.z % chankSize.z));

        return pos;
    }
    private Vector3 getAreaPosition(Vector3 blockPoss, Vector3 Normal) {

        Vector3 poss = new Vector3((int)blockPoss.x, (int)blockPoss.y, (int)blockPoss.z);
        Vector3 offset = Normal *0.501f;

        return poss+ offset;
    }

    private void rotate_Area(Vector3 normal, Transform transform)
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
    }
}
