using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public int health = 100;
    public GameObject gun;
    public float shootingRange = 10f;  // Shooting distance
    public float shootCooldown = 1f;  // Time to wait after each shot (seconds)
    private float shootTimer = 0f;  // shot timer
    public GameObject bulletPrefab;  // References to bullet prefabs
    public Transform gunBarrel;  // A reference to the muzzle of a pistol, used to determine the initial position and orientation of the bullet
    public int bulletDamage; // Damage of the shoots
    public float bulletSpeed = 20f;  // bullet speed
    //public Transform gunMuzzle; // Transform of pistol muzzle
    public float bulletLifeTime = 3f;  // Time the bullet exists (seconds)


    private NavMeshAgent agent;
    private Animator animator;
    private float wanderTimer;
    private float wanderCooldown = 5f;
    private float wanderRange = 20f;
    private bool isWalking = false;
    private bool isShooting = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        shootTimer = shootCooldown;
    }

    void Update()
    {
        // update shot timer
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= shootingRange)
        {
            agent.SetDestination(transform.position); // Stop the agent
            
            if (!isShooting)
            {
                animator.SetBool("isShooting", true);
                isShooting = true;
                gun.SetActive(true);
            }

            shootTimer -= Time.deltaTime; // Reduce the shoot timer
            if (shootTimer <= 0f)
            {
                Shoot(); // Only shoot when the timer reaches 0
                shootTimer = shootCooldown; // Reset the timer after shooting
            }

            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        else if (distance <= detectionRange)  // Player is within detection range but not shooting range
        {

            // If the player is detected but not in shooting range, move towards the player
            if (isShooting)
            {
                animator.SetBool("isShooting", false);
                isShooting = false;
                gun.SetActive(false);
            }
            agent.SetDestination(player.position); // Move towards the player
        }
        else  // Player is not within detection range
        {
            // If the player is not detected, wander around
            if (isShooting)
            {
                animator.SetBool("isShooting", false);
                isShooting = false;
                gun.SetActive(false);
            }
            if (wanderTimer <= 0f)
            {
                isWalking = !isWalking;
                animator.SetBool("isWalking", isWalking);
                if (isWalking)
                {
                    Vector3 randomDirection = Random.insideUnitSphere * wanderRange;
                    randomDirection += transform.position;
                    NavMeshHit hit;
                    // If SamplePosition returns false, it means that the randomDirection pointed to cannot find a valid position on the NavMesh
                    while (!NavMesh.SamplePosition(randomDirection, out hit, wanderRange, 1))
                    {
                        // reselect a random direction
                        randomDirection = Random.insideUnitSphere * wanderRange;
                        randomDirection += transform.position;
                    }
                    agent.SetDestination(hit.position);
                }
                wanderTimer = wanderCooldown;
            }
            else
            {
                wanderTimer -= Time.deltaTime;
            }
        }

    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);
        bullet.GetComponent<BulletProjectile>().isEnemyBullet = true;
        bullet.GetComponent<BulletProjectile>().damage = bulletDamage;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = gunBarrel.forward * bulletSpeed;

        // Destroy the bullet after a certain time
        Destroy(bullet, bulletLifeTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag =="Car")
        {
            TakeDamage(100);
        }
    }
}