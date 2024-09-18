using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody myRB;
    Camera playercam;
    Vector2 camRotation;


    [Header("Movement Stats")]
    public bool sprinting = false;
    public float groundDetection = 1.0f;
    public float sprintMult = 1.5f;
    public float speed = 10f;
    public float jumpHeight = 5f;

    [Header("WeaponStats")]
    public Transform weaponSlot;


    [Header("Player Stats")]
    public int health = 5;
    public int maxHealth = 10;
    public int healthPickupAmount;

    [Header("User Settings")]
    public bool sprintToggle = false;
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

        if (!sprinting)
        {
            if (!sprintToggle && Input.GetKey(KeyCode.LeftShift)) 
                sprinting = true;

            
            if (sprintToggle && (Input.GetAxisRaw("Vertical") > 0) && Input.GetKey(KeyCode.LeftShift))
                sprinting = true;

        }
      

        temp.z = Input.GetAxisRaw("Horizontal") * speed;
        temp.x = Input.GetAxisRaw("Vertical") * speed;

        if (sprinting)
            temp.z *= sprintMult;

        if (sprinting && sprintToggle && (Input.GetAxisRaw("Vertical") <= 0))
            sprinting = false;

        if (sprinting && !sprintToggle && Input.GetKeyUp(KeyCode.LeftShift))
            sprinting = false;
    
        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, groundDetection))
            temp.y = jumpHeight;

        myRB.velocity = (transform.forward * temp.x) + (transform.right * temp.z) + (transform.up * temp.y);
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if((collision.gameObject.tag == "HealthPickup") && health < maxHealth)
        {
            if (health + healthPickupAmount > maxHealth)
                health = maxHealth;
            else
                health += healthPickupAmount;

            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Weapon")
        {
            other.transform.position = weaponSlot.position;
            other.transform.rotation = weaponSlot.rotation;

            
            other.transform.SetParent(weaponSlot);
        }
    }
}
