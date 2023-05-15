using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private int health = 100;
    public int currentHealth;
    AudioManager audioManager;
    
    [SerializeField]
    private CinemachineVirtualCamera aimVirtualCamera;
    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private Animator animator;

    private CanvasManager _canvas;
    //Scene scene;

    [SerializeField] GameObject weapon;

    [SerializeField] int maxBullet = 3;
    public int currentBulletCount;
    bool canShoot = true;
    public float reloadTime = 1f;
    float currentTime;
    bool isReloading = false;

    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;

    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();

    Vector3 mouseWorldPosition;
    Vector3 worldAimTarget;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
        _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();

    }
    
    void Start()
    {
        currentBulletCount = maxBullet;
        //scene = SceneManager.GetActiveScene();
        currentHealth = health;
        currentTime = reloadTime;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isReloading == false)
        {
            audioManager.PlaySound("Reload");
            isReloading = true;
        }
        
        Reloading();

        if (SceneManager.GetActiveScene().name == "BarLevel")
        {
            canShoot = false;
        }
        
        if(!GameObject.FindObjectOfType<CanvasManager>().GetComponent<CanvasManager>().ReturnGameStatus())
            AimShoot();
    }

    void Reloading()
    {
        if (isReloading)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime = reloadTime;
                isReloading = false;
                currentBulletCount = maxBullet;
            }
        }
    }

    void AimShoot()
    {
        mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }


        if (canShoot && starterAssetsInputs.aim)
        {
            weapon.SetActive(true);
            _canvas.TurnAiming(true);

            //Set aiming target to mouse position|| Turn off character rotation to rotate with the aiming direction
            thirdPersonController.SetRotateOnMove(false);
            aimVirtualCamera.gameObject.SetActive(true);
            worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            //Activate second animation layer
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            //Pressing Shooting
            if (currentBulletCount > 0 && Input.GetMouseButtonDown(0))
            {
                SpawnBullet();
            }
        }
        else
        {
            weapon.SetActive(false);
            _canvas.TurnAiming(false);
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
    }

    void SpawnBullet()
    {
        currentBulletCount--;
        audioManager.PlaySound("Shooting");
        Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        starterAssetsInputs.shoot = false;
    }
    
    public void CanShoot(bool status)
    {
        canShoot = status;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth - damage <= 0)
        {
            currentHealth = 0;
        }
        else if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
        }
    }

    public int ReturnCurrentHealth()
    {
        return currentHealth;
    }
}