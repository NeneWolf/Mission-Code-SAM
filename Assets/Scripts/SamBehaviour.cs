using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SamBehaviour : MonoBehaviour
{
    [SerializeField] int health = 100;
    public int currentHealth;
    
    public NavMeshAgent friend;
    public Transform player;

    public bool followPlayer;
    bool isHelpingSam =false;

    public BoxCollider interactBoxCollider;
    private CapsuleCollider capsuleCollider;
    
    private CanvasManager _canvas;
    
    Animator _anim;
    // animation IDs
    private int _animSpeed;
    private float _animationBlend;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
        _anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();


    }
    // Start is called before the first frame update
    void Start()
    {
        _animSpeed = Animator.StringToHash("Speed");
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
            GoingDown();
       
                
        if (Input.GetKeyDown(KeyCode.E) && isHelpingSam)
        {
            _canvas.GetComponent<CanvasManager>().TurnSamInteraction(false);
            if (currentHealth <= 0)
            {
                currentHealth = 30;
                followPlayer = true;
                interactBoxCollider.enabled = false;
            }
            else if(currentHealth == health)
            {
                followPlayer = true;
                interactBoxCollider.enabled = false;
            }
        }

        FollowPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _canvas.GetComponent<CanvasManager>().TurnSamInteraction(true);
            isHelpingSam = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isHelpingSam = false;
        _canvas.GetComponent<CanvasManager>().TurnSamInteraction(false);
    }

    void FollowPlayer()
    {
        if (followPlayer)
        {
            friend.enabled = true;
            float velocity = friend.velocity.magnitude / friend.speed;
            capsuleCollider.enabled = true;
            _anim.SetFloat(_animSpeed, velocity);
            _anim.SetBool("isDown", false);
            friend.SetDestination(player.position);
            this.gameObject.AddComponent<Rigidbody>();
        }
    }

    void GoingDown()
    {
        capsuleCollider.enabled = false;
        transform.position = new Vector3(transform.position.x, -0.805f, transform.position.z);
        followPlayer = false;
        interactBoxCollider.enabled = true;
        _anim.SetBool("isDown", true);
        friend.enabled = false;
        Destroy(this.GetComponent<Rigidbody>());
    }
    
    public void DisableSamMovement()
    {
        followPlayer = false;
        friend.enabled = false;
        _anim.SetBool("isSitting", true);
        interactBoxCollider.enabled = false;
        Destroy(this.GetComponent<Rigidbody>());
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
