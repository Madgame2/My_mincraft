using System.IO;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Vector3 direction;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    private void FixedUpdate()
    {
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveZ = Input.GetAxisRaw("Vertical");

        direction = Vector3.Normalize(new Vector3(MoveX,0,MoveZ));

        transform.Translate(direction * speed * Time.deltaTime,Space.Self);

        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 direction = new Vector3(0, 1, 0);

            GetComponent<Rigidbody>().AddForce(direction * jumpForce);
        }
    }

}
