using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UIElements;

public class Builder : MonoBehaviour
{
    [SerializeField] private Chank active_chunk;
    [SerializeField] private Vector3Int localBlockPos;
    [SerializeField] private Vector3 Normal;
    [SerializeField] GameObject selecteble_area_prefab;


    private GameObject rednered_prefab;
    public void SelectBlock(Vector3 block_globalPos,Vector3 normal)
    {


        this.Normal = normal;
        
        Vector3 chankSize = Chank.getChankSize();

        Vector2Int chankPos = new Vector2Int((int)(block_globalPos.x / chankSize.x), (int)(block_globalPos.z / chankSize.z));


        active_chunk = Chank_manager.get_chank(chankPos);
        localBlockPos = getLockalPost(block_globalPos);




        //rednered_prefab = Instantiate(selecteble_area_prefab);

        //rednered_prefab.transform.position = getAreaPosition(block_globalPos, normal);

        Debug.Log(localBlockPos);
    }

    private Vector3Int getLockalPost(Vector3 global)
    {
        Vector3 chankSize = Chank.getChankSize();
        Vector3Int pos = new Vector3Int((int)(global.x % chankSize.x), (int)(global.y % chankSize.y), (int)(global.z % chankSize.z));

        return pos;
    }
    private Vector3 getAreaPosition(Vector3 blockPoss, Vector3 Normal) {
        Vector3 poss = new Vector3((int)blockPoss.x, (int)blockPoss.y, (int)blockPoss.z);
        Vector3 offset = Normal*0.51f;

        return poss + offset;
    } 
}
