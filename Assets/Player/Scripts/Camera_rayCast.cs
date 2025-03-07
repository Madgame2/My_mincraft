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

    private Vector3Int CurentPosition;
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

            if (Physics.Raycast(ray_camera_postitoin, out RaycastHit hit))
            {


                // Отображаем линию от игрока до точки пересечения луча
                Vector3 direction = hit.point - transform.position;
                ray_player_position = new Ray(transform.position, direction);

                lineRendererPlayerRay.SetPosition(0, transform.position);
                lineRendererPlayerRay.SetPosition(1, hit.point);

                if (direction.magnitude<= arm_leght)
                {
                    Vector3 buffer = new Vector3Int((int)hit.point.x, (int)hit.point.y, (int)hit.point.z);
                    if (CurentPosition != buffer)
                    {
                        CurentPosition = new Vector3Int((int)hit.point.x, (int)hit.point.y, (int)hit.point.z);
                        Debug.Log($"selected this position: {hit.point}");
                        selectBlock?.Invoke(hit.point, hit.normal);
                    }
                }
            }
            else
            {
                ray_player_position = new Ray(transform.position, ray_camera_postitoin.direction);

                lineRendererPlayerRay.SetPosition(0, ray_player_position.origin); // Начало луча
                lineRendererPlayerRay.SetPosition(1, ray_player_position.origin + ray_player_position.direction * 100);
            }
            //Debug.Log(is_active);
        }
    }

    public void setActiveMode(bool active)
    {
        //Debug.Log($"setActiveMode вызван с {active}, is_active теперь {active}. Вызван из: " + new System.Diagnostics.StackTrace());
        is_active = !active;
    }
}
