using System.IO;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Vector3 direction;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float gravityMultiplier = 3f;

    private Transform camera_transsform;
    private bool isGrouded = true;

    Rigidbody rb;
    Collider player_collider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player_collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveZ = Input.GetAxisRaw("Vertical");

        Quaternion rotationY = Quaternion.Euler(0, camera_transsform.eulerAngles.y, 0);

        direction = rotationY * (new Vector3(MoveX, 0, MoveZ)).normalized;

        Vector3 offset = direction * speed*Time.deltaTime;


        Vector3 offsetX = new Vector3(offset.x, 0, 0);
        Vector3 offsetZ = new Vector3(0, 0, offset.z);

        bool blockedX = WillCollide(offsetX);
        bool blockedZ = WillCollide(offsetZ);

        if (blockedX && blockedZ)
        {
            offset = Vector3.zero; 
        }
        else
        {
            if (blockedX) offset.x = 0;
            if (blockedZ) offset.z = 0;
        }

        rb.MovePosition(transform.position + offset);

        if (Input.GetKey(KeyCode.Space))
        {

            if (isGrouded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            }
        }

        if (!isGrouded)
        {
            rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
        }
    }


    public void set_camera_rotated(Transform camera)
    {
        this.camera_transsform = camera;
    }

    private bool WillCollide(Vector3 offset)
    {
        float skinWidth = 0.05f; // Маленькое смещение
        return Physics.BoxCast(transform.position, player_collider.bounds.size * 0.5f - Vector3.one * skinWidth,
                               offset.normalized, out RaycastHit hit, Quaternion.identity, offset.magnitude);
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach(ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                isGrouded = true;
                return;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrouded = false;
    }
}
