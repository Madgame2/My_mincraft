using UnityEngine;

public class test_camera_script : MonoBehaviour
{
    [SerializeField] Transform player_pos;
    [SerializeField] Transform camera_pos;
    [SerializeField] float radius;

    [SerializeField] Material material;

    // Update is called once per frame
    void Update()
    {
        material.SetVector("_Player_position", player_pos.position);
        material.SetVector("_Camera_position", camera_pos.position);
        material.SetFloat("_Radius", radius);
    }
}
