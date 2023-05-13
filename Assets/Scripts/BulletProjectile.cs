using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    [SerializeField] private float speed = 10f;

    public bool isEnemyBullet = false;
    public int damage;
    
    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.tag);
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyAI>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<ThirdPersonShooterController>().TakeDamage(damage);
        }
        else if (other.gameObject.tag == "Sam" )
        {
            other.gameObject.GetComponent<SamBehaviour>().TakeDamage(damage);
        }
        
        Destroy(gameObject);
    }

}
