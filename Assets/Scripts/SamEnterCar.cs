using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamEnterCar : MonoBehaviour
{
    CanvasManager _canvas;
    GameManager _gameManager;

    [SerializeField] private Transform samSeatPosition;

    private void Awake()
    {
        _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sam")
        {

            other.gameObject.transform.SetParent(samSeatPosition);
            other.gameObject.transform.position = samSeatPosition.position;
            other.gameObject.GetComponent<SamBehaviour>().DisableSamMovement();

            _canvas.UpdateMissionText("Return To the Bar");
            _gameManager.TurnOnFinalMissionPortal();
            Destroy(this.gameObject);
        }
    }
}