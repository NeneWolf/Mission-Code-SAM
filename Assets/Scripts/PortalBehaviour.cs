using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalBehaviour : MonoBehaviour
{
    [SerializeField] bool isMissionPortal;
    [SerializeField] String sceneToLoad;

    CanvasManager _canvas;

    bool isAtPortal = false;

    private void Awake()
    {
        _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
    }
    
    void Update()
    {
        if (!isMissionPortal && Input.GetKeyDown(KeyCode.E) && isAtPortal)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isMissionPortal)
        {
            if (other.gameObject.tag == "Player")
            {
                _canvas.TurnEInteraction(true);
                isAtPortal = true;
            }
        }
        else
        {
            if (other.gameObject.tag == "Car")
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _canvas.TurnEInteraction(false);
        isAtPortal = false;
    }
}