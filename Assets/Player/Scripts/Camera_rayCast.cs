using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Camera_rayCast : MonoBehaviour
{
    [SerializeField] private Camera _Camera;
    [SerializeField] private float arm_leght;
    private Ray ray_camera_postitoin;
    Ray ray_player_position;

    [SerializeField] private LineRenderer lineRendererCameraRay;  // Линия от камеры
    [SerializeField] private LineRenderer lineRendererPlayerRay;   // Линия от игрока
    [SerializeField] private UnityEvent<Vector3,Vector3> selectBlock;
    [SerializeField] private UnityEvent undoSelection;
    [SerializeField] private LayerMask ignoreLayer;

    private Vector3Int CurrentPosition;
    private Vector3 CurrentNoraml;
    private bool is_active;
    void Start()
    {
        lineRendererCameraRay.positionCount = 2;
        lineRendererCameraRay.startWidth = 0.1f;
        lineRendererCameraRay.endWidth = 0.1f;
        lineRendererCameraRay.startColor = Color.blue;
        lineRendererCameraRay.endColor = Color.blue;

        lineRendererPlayerRay.positionCount = 2;
        lineRendererPlayerRay.startWidth = 0.1f;
        lineRendererPlayerRay.endWidth = 0.1f;
        lineRendererPlayerRay.startColor = Color.red;
        lineRendererPlayerRay.endColor = Color.red;

        is_active = true;
    }

    void Update()
    {
        if (is_active)
        {
            ray_camera_postitoin = _Camera.ScreenPointToRay(Input.mousePosition);

            lineRendererCameraRay.SetPosition(0, _Camera.transform.position);
            lineRendererCameraRay.SetPosition(1, _Camera.transform.position + ray_camera_postitoin.direction * 100);

            if (Physics.Raycast(ray_camera_postitoin, out RaycastHit hit, Mathf.Infinity, ~ignoreLayer))
            {

                Vector3 direction = hit.point - transform.position;
                ray_player_position = new Ray(transform.position, direction);

                lineRendererPlayerRay.SetPosition(0, transform.position);
                lineRendererPlayerRay.SetPosition(1, hit.point);

                if (direction.magnitude <= arm_leght)
                {
                    Vector3 hitPoint = hit.point;


                    if (hit.normal.y > 0.5f) hitPoint.y -= 0.001f;
                    if (hit.normal.y < -0.5f) hitPoint.y += 0.001f;

                    if (hit.normal.x > 0.5f) hitPoint.x -= 0.001f;
                    if (hit.normal.x < -0.5f) hitPoint.x += 0.001f;

                    if (hit.normal.z > 0.5f) hitPoint.z -= 0.001f;
                    if (hit.normal.z < -0.5f) hitPoint.z += 0.001f;

                    Vector3Int buffer = new Vector3Int(
                        Mathf.FloorToInt(hitPoint.x + 0.5f),
                        Mathf.RoundToInt(hitPoint.y),
                        Mathf.FloorToInt(hitPoint.z + 0.5f)
                    );

                   // Debug.Log(hit.point + $"buffer: {buffer} Normal {hit.normal}");

                    if (CurrentPosition != buffer|| hit.normal!= CurrentNoraml)
                    {
                        CurrentPosition = buffer;
                        CurrentNoraml = hit.normal;
                        selectBlock?.Invoke(CurrentPosition, hit.normal);
                    }
                }
                else
                {
                    undoSelection?.Invoke();
                }
            }
            else
            {
                if (CurrentPosition != default) {
                    CurrentPosition = default;
                    CurrentNoraml = default;
                    undoSelection?.Invoke();
                }

                ray_player_position = new Ray(transform.position, ray_camera_postitoin.direction);

                lineRendererPlayerRay.SetPosition(0, ray_player_position.origin); 
                lineRendererPlayerRay.SetPosition(1, ray_player_position.origin + ray_player_position.direction * 100);
            }
        }
    }

    public void setActiveMode(bool active)
    {
        is_active = !active;
    }
}
