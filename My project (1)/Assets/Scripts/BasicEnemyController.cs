using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BasicEnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public PlayerController player;

    [Header("EnemyStats")]
    public int health = 3;
    public int maxHealth = 5;
    public int damageGiven = 1;
    public int damageReceived = 1;
    public float pushBackForce = 5;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
       
        target = player.transform;
        agent.destination = target.position;
        if (health <= 0)
            Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            health -= damageReceived;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "player")
        {
            if (!player.takenDamage)
            {
                collision.gameObject.GetComponent<PlayerController>().health -= damageGiven;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * pushBackForce);
                collision.gameObject.GetComponent<PlayerController>().takenDamage = true;
            }
        }
    }

}
