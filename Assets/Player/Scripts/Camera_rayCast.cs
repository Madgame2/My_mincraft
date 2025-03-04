using UnityEngine;

public class Camera_rayCast : MonoBehaviour
{
    [SerializeField] private Camera _Camera;
    private Ray ray_camera_postitoin;
    Ray ray_player_position;

    [SerializeField] private LineRenderer lineRendererCameraRay;  // Линия от камеры
    [SerializeField] private LineRenderer lineRendererPlayerRay;   // Линия от игрока

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
    }

    void Update()
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
        }
        else
        {
            ray_player_position = new Ray(transform.position, ray_camera_postitoin.direction);

            lineRendererPlayerRay.SetPosition(0, ray_player_position.origin); // Начало луча
            lineRendererPlayerRay.SetPosition(1, ray_player_position.origin + ray_player_position.direction * 100);
        }
    }
}
