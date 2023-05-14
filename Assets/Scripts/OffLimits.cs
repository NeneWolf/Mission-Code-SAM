using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffLimits : MonoBehaviour
{
    CanvasManager canvasManager;

    private void Awake()
    {
        canvasManager = FindObjectOfType<CanvasManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Car")
        {
            canvasManager.TurnOffLimits(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Car")
        {
            canvasManager.TurnOffLimits(false);
        }
    }
}