using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityStandardAssets.CrossPlatformInput;
using StarterAssets;
using UnityEngine.InputSystem;

public class CarEnterExit : MonoBehaviour
{
    public GameObject Car;
    public GameObject carSeat;
    public GameObject player;

    public GameObject playerPositionOutsideCar;

    public GameObject CameraPlayer;
    public GameObject CameraCar;

    bool canDrive;
    bool playerInside;
    bool firstTimeDriving = true;

    private CanvasManager _canvas;

    private void Awake()
    {
        Car = GameObject.FindGameObjectWithTag("Car");
        player = GameObject.FindGameObjectWithTag("Player");
        _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        Car.GetComponent<CarUserControl>().canDrive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canDrive && Input.GetKeyDown(KeyCode.E))
        {
            player.GetComponent<ThirdPersonShooterController>().CanShoot(false);
            StartDriving();
        }
        else if (playerInside && Input.GetKeyDown(KeyCode.E) && canDrive== false)
        {
            player.GetComponent<ThirdPersonShooterController>().CanShoot(true);
            ExitCar();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canDrive = true;
            _canvas.TurnCarInteraction(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canDrive = false;
            _canvas.TurnCarInteraction(false);
        }
    }
    
    void StartDriving()
    {
        if (firstTimeDriving)
            _canvas.TurnOnCarInstruction();
        _canvas.TurnCarInteraction(false);
        // Set player to seat position and disable the player controls
        player.transform.position = carSeat.transform.position;
        player.transform.rotation = carSeat.transform.rotation;
        player.transform.SetParent(carSeat.transform);
        player.GetComponent<ThirdPersonController>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;

        player.GetComponent<Animator>().SetBool("isDriving", true);

        //Turn on car controls
        Car.GetComponent<CarUserControl>().canDrive = true;


        //switch cameras
        CameraPlayer.SetActive(false);
        CameraCar.SetActive(true);

        //Checkpoints to see if the player is inside
        playerInside = true;
        canDrive = false;
        firstTimeDriving = false;
    }

    void ExitCar()
    {
        playerInside = false;
        player.transform.SetParent(null);
        player.transform.position = playerPositionOutsideCar.transform.position;
        player.GetComponent<ThirdPersonController>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<Animator>().SetBool("isDriving", false);
        Car.GetComponent<CarUserControl>().canDrive = false;

        CameraPlayer.SetActive(true);
        CameraCar.SetActive(false);
    }
}
