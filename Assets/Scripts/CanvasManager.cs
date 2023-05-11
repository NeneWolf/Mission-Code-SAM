using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject carInteractUI;
    [SerializeField] private GameObject carInstructionsUI;

    public void TurnCarInteraction(bool status)
    {
        carInteractUI.SetActive(status);
    }

    public void TurnOnCarInstruction()
    {
        StartCoroutine("TurnOnCarInstructionsUI");
    }

    IEnumerator TurnOnCarInstructionsUI()
    {
        carInstructionsUI.SetActive(true);
        yield return new WaitForSeconds(10f);
        carInstructionsUI.SetActive(false);
    }
}
