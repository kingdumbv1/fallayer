using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody myRB;
    Camera playercam;
    Vector2 camRotation;
    Transform cameraHolder;

    [Header("Movement Stats")]
    // accessmodifier "public", datatype "bool", name "sprinting"
    public bool sprinting = false;
    public float groundDetection = 1.5f;
    public float sprintMult = 2.0f;
    public float speed = 10f;
    public float jumpHeight = 5f;

    [Header("WeaponStats")]
    public Transform weaponSlot;
    public GameObject shot;
    public float shotVel = 0;
    public int weaponID = -1;
    public int fireMode = 0;
    public float fireRate = 0;
    public float currentClip = 0;
    public float clipSize = 0;
    public float maxAmmo = 0;
    public float currentAmmo = 0;
    public float reloadAmt = 0;
    public float bulletLifeSpan = 0;
    public bool canFire = true;

    [Header("Player Stats")]
    public float damageCooldownTimer = .5f;
    public bool takenDamage = false;
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
        playercam = Camera.main;

        cameraHolder = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (takenDamage)
        {

        }
        Vector3 temp = myRB.velocity;

        Quaternion mouseLook = playercam.transform.rotation;

        Quaternion verticalRotation = Quaternion.AngleAxis(camRotation.y, Vector3.left);


        camRotation.x += Input.GetAxisRaw("Mouse X") * mousesensx;
        camRotation.y += Input.GetAxisRaw("Mouse Y") * mousesensy;

        camRotation.y = Mathf.Clamp(camRotation.y, -90, 90);
        playercam.transform.position = cameraHolder.position;
        playercam.transform.rotation = Quaternion.Euler(-camRotation.y, camRotation.x, 0);
        transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);

        if (Input.GetMouseButton(0) && canFire && currentClip > 0 && weaponID >= 0)
        {
            if (weaponID == 0)
            {
                Fire(6);
            }
            else
            {
                Fire(1);
            }
        }

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
            temp.x *= sprintMult;
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
        if((collision.gameObject.tag == "ammoPickup") && currentAmmo < maxAmmo)
        {
            if (currentAmmo + reloadAmt > maxAmmo)
                currentAmmo = maxAmmo;
            else
                currentAmmo += reloadAmt;

            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Weapon")
        {
            other.transform.SetParent(weaponSlot);
            other.transform.SetPositionAndRotation(weaponSlot.position, weaponSlot.rotation); 
            switch(other.gameObject.name)
            {
                case "Weapon1":
                    weaponID = 0;
                    shotVel = 10000;
                    fireMode = 0;
                    fireRate = 2f;
                    currentClip = 20;
                    clipSize = 20;
                    maxAmmo = 400;
                    currentAmmo = 200;
                    reloadAmt = 20;
                    bulletLifeSpan = 0.5f;
                    break;

                default:
                    break;
            }
        }
    }

    public void reloadClip()
    {
        if (currentClip >= clipSize)
            return;
        else
        {
            float reloadCount = clipSize - currentClip;
            if (currentAmmo < reloadCount)
            {
                currentClip += currentAmmo;
                currentAmmo = 0;
                return;
            }
            else
            {
                currentClip += reloadCount;
                currentAmmo -= reloadCount;
                return;
            }
        }
    }

    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    IEnumerator cooldownDamage()
    {
        yield return new WaitForSeconds(damageCooldownTimer);
        takenDamage = true;
    }

    void Fire(int bullets = 1)
    {
        for (int i = 0; i < bullets; i++)
        {
            GameObject s = Instantiate(shot, weaponSlot.position, weaponSlot.rotation);
            s.transform.localRotation = playercam.transform.rotation;
            s.transform.localRotation = Quaternion.Euler(Random.Range(-5f, 5f) + s.transform.localRotation.eulerAngles.x, Random.Range(-5f, 5f) + s.transform.localRotation.eulerAngles.y, s.transform.localRotation.eulerAngles.z);
            s.GetComponent<Rigidbody>().AddForce(s.transform.forward * shotVel);
            Destroy(s, bulletLifeSpan);
        }

        canFire = false;
        currentClip--;
        StartCoroutine("cooldown");
    }
}
