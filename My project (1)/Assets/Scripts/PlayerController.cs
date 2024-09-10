using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody myRB;
    Camera playercam;
    Vector2 camRotation;

    public float speed = 10f;
    public float jumpHeight = 5f;
    public float mousesensitivity = 2.0f;
    public float mousesensx = 2.0f;
    public float mousesensy = 2.0f;

    public float camlimit = 90f;
    // Start is called before the first frame update
    void Start()
    {
        camRotation = Vector2.zero;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        myRB = GetComponent<Rigidbody>();
        playercam = transform.GetChild(0).GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = myRB.velocity;

        Quaternion mouseLook = playercam.transform.rotation;

        Quaternion verticalRotation = Quaternion.AngleAxis(camRotation.y, Vector3.left);


        camRotation.x += Input.GetAxisRaw("Mouse X") * mousesensx;
        camRotation.y += Input.GetAxisRaw("Mouse Y") * mousesensy;

        camRotation.y = Mathf.Clamp(camRotation.y, -90, 90);
        playercam.transform.localRotation = Quaternion.AngleAxis(camRotation.y, Vector3.left);
        transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);

        temp.z = Input.GetAxisRaw("Horizontal") * speed;
        temp.x = Input.GetAxisRaw("Vertical") * speed;

        if (Input.GetKeyDown(KeyCode.Space))
            temp.y = jumpHeight;

        myRB.velocity = (transform.forward * temp.x) + (transform.right * temp.z) + (transform.up * temp.y);
    }
}
