using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject eInteractUI;
    [SerializeField] private GameObject carInstructionsUI;
    [SerializeField] private GameObject aimingUI;
    [SerializeField] private GameObject samInteractionUI;
    [SerializeField] private GameObject MissionUI;
    [SerializeField] private TextMeshProUGUI countdownTimerUI;


    public void TurnEInteraction(bool status)
    {
        eInteractUI.SetActive(status);
    }

    public void TurnAiming(bool status)
    {
        aimingUI.SetActive(status);
    }

    public void TurnSamInteraction(bool status)
    {
        samInteractionUI.SetActive(status);
    }

    public void TurnOnCarInstruction()
    {
        StartCoroutine("TurnOnCarInstructionsUI");
    }

    public void TurnOnMissionUI(bool status)
    {
        MissionUI.SetActive(status);
    }
    
    public void MissionNameTime(float timer)
    {
        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");
            
       
       countdownTimerUI.text = string.Format("{0}:{1}", minutes,seconds);
    }

    IEnumerator TurnOnCarInstructionsUI()
    {
        carInstructionsUI.SetActive(true);
        yield return new WaitForSeconds(10f);
        carInstructionsUI.SetActive(false);
    }
}
