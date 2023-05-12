using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera aimVirtualCamera;
    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private Animator animator;

    [SerializeField] GameObject weapon;

    [SerializeField] int maxBullet = 3;
    public int currentBulletCount;
    bool canShoot = true;
    public float reloadTime = 1f;
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
    }
    
    void Start()
    {
        currentBulletCount = maxBullet;

    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentBulletCount = maxBullet;
        }
        
        AimShoot();
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
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
    }


    void SpawnBullet()
    {
        currentBulletCount--;
        Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        starterAssetsInputs.shoot = false;
    }
    
    public void CanShoot(bool status)
    {
        canShoot = status;
    }
}