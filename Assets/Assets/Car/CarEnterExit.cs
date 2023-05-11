using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityStandardAssets.CrossPlatformInput;
using StarterAssets;

public class CarEnterExit : MonoBehaviour
{
    public GameObject Car;
    public GameObject carSeat;
    public GameObject player;
    public GameObject cameraRoot;

    public GameObject CameraPlayer;
    public GameObject CameraCar;

    bool canDrive;
    

    private void Awake()
    {
        Car = GameObject.FindGameObjectWithTag("Car");
        player = GameObject.FindGameObjectWithTag("Player");
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
            // Set player to seat position and disable the player controls
            player.transform.position = carSeat.transform.position;
            player.transform.rotation = carSeat.transform.rotation;
            player.transform.SetParent(carSeat.transform);
            player.GetComponent<ThirdPersonController>().enabled = false;
            player.GetComponent<CharacterController>().enabled = false;

            player.GetComponent<Animator>().SetBool("isDriving", true);


            Car.GetComponent<CarUserControl>().canDrive = true;


            //switch cameras
            CameraPlayer.SetActive(false);
            CameraCar.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canDrive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canDrive = false;
        }
    }
}
