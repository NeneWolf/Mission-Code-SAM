using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SamBehaviour : MonoBehaviour
{
    [SerializeField] int health = 100;
    public int currentHealth;

    [SerializeField] int maxAmountOfDeath;
    public int currentAmountOfDeath;
    
    public NavMeshAgent friend;
    public Transform player;

    public bool followPlayer;
    bool isHelpingSam =false;

    [SerializeField] GameObject body;
    [SerializeField] Transform downTrans;
    [SerializeField] Transform upTrans;

    public BoxCollider interactBoxCollider;
    private CapsuleCollider capsuleCollider;
    
    private CanvasManager _canvas;

    Rigidbody rigidbody;
    [SerializeField] Rigidbody rigidBody;
    
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
        currentAmountOfDeath = maxAmountOfDeath;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
        if (rigidbody != null)
            rigidbody = rigidBody;
        
        if (currentHealth <= 0)
            GoingDown();
       
                
        if (Input.GetKeyDown(KeyCode.E) && isHelpingSam)
        {
            _canvas.GetComponent<CanvasManager>().TurnSamInteraction(false);
            body.transform.position = upTrans.position;


            if (currentHealth <= 0)
            {
                currentHealth = health/2;
                followPlayer = true;
                interactBoxCollider.enabled = false;
            }
            else if(currentHealth == health)
            {
                followPlayer = true;
                interactBoxCollider.enabled = true;
            }
        }

        FollowPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !followPlayer)
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

            if(rigidbody == null)
                this.gameObject.AddComponent<Rigidbody>();
        }
    }

    void GoingDown()
    {

        _anim.SetBool("isDown", true);
        capsuleCollider.enabled = false;
        //transform.position = new Vector3(transform.position.x, transform.position.y+ 0.2f, transform.position.z);
        body.transform.position = downTrans.position;
        followPlayer = false;

        if(currentAmountOfDeath <= 0)
        {
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.Restart(true);

        }else
            interactBoxCollider.enabled = true;
        
        friend.enabled = false;
        Destroy(this.GetComponent<Rigidbody>());

    }
    
    public void DisableSamMovement()
    {
        followPlayer = false;
        friend.enabled = false;
        _anim.SetBool("isSitting", true);
        interactBoxCollider.enabled = false;
        Destroy(rigidBody);
    }

    public void TakeDamage(int damage)
    {
        if(currentHealth - damage <= 0)
        {
            currentAmountOfDeath--;
            currentHealth = 0;
        }
        else if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
        }
    }
}
