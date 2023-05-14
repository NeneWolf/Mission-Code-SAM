using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using StarterAssets;

public class CanvasManager : MonoBehaviour
{
    GameManager gameManager;

    [Header("General UI InformationUI")]
    [SerializeField] private GameObject eInteractUI;
    [SerializeField] private GameObject carInstructionsUI;
    [SerializeField] private GameObject aimingUI;
    [SerializeField] private GameObject samInteractionUI;
    [SerializeField] private GameObject MissionUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject controlsInstructionsUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject OffLimits;

    bool isPaused;

    [Header("Player InformationUI")]
    [SerializeField] Slider playerHealth;
    [SerializeField] Slider samHealth;
    [SerializeField] GameObject samHealthUI;

    [Header("Timer InformationUI")]
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private TextMeshProUGUI countdownTimerUI;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssetsInputs>().cursorLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();

        if (!gameManager.hasActivatedMission)
        {
            StartCoroutine(TurnOnControlsInstructionsUI());
        }
    }

    private void Update()
    {
        playerHealth.value = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonShooterController>().currentHealth;
        
        if (gameManager.hasActivatedMission && GameObject.FindGameObjectWithTag("Sam") != null)
        {
            samHealthUI.SetActive(true);
            samHealth.value = GameObject.FindGameObjectWithTag("Sam").GetComponent<SamBehaviour>().currentHealth;
        }
        else
            samHealthUI.SetActive(false);

        PauseGame();
    }
    
    private void PauseGame()
    {
        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && pauseMenuUI.activeInHierarchy == false)
        {
            //mouse
            GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssetsInputs>().cursorLocked = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            isPaused = true;
            Time.timeScale = 0f;

            PauseMenu(true);
        }
        else if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && pauseMenuUI.activeInHierarchy == true)
        {
            //mouse
            GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssetsInputs>().cursorLocked = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            isPaused = false;
            Time.timeScale = 1f;

            PauseMenu(false);
        }
    }

    public void TurnEInteraction(bool status)
    {
        eInteractUI.SetActive(status);
    }

    public void TurnAiming(bool status)
    {
        aimingUI.SetActive(status);
    }

    public void TurnOffLimits(bool status)
    {
        OffLimits.SetActive(status);
    }

    public void TurnSamInteraction(bool status)
    {
        samInteractionUI.SetActive(status);
    }

    public void TurnOnCarInstruction()
    {
        if (controlsInstructionsUI.activeInHierarchy)
            controlsInstructionsUI.SetActive(false);
        
        StartCoroutine("TurnOnCarInstructionsUI");
    }

    public void TurnOnMissionUI(bool status)
    {
        MissionUI.SetActive(status);
    }
    
    public void StartMissionTimer(float timer)
    {
        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");
            
       
       countdownTimerUI.text = string.Format("{0}:{1}", minutes,seconds);
    }

    public void UpdateMissionText(string text)
    {
        missionText.text = text;
    }

    public void GameOverScreen(bool status)
    {
        gameOverUI.SetActive(true);
    }

    public bool ReturnGameStatus()
    {
        return isPaused;
    }

    IEnumerator TurnOnCarInstructionsUI()
    {
        carInstructionsUI.SetActive(true);
        yield return new WaitForSeconds(10f);
        carInstructionsUI.SetActive(false);
    }

    IEnumerator TurnOnControlsInstructionsUI()
    {
        controlsInstructionsUI.SetActive(true);
        yield return new WaitForSeconds(10f);
        controlsInstructionsUI.SetActive(false);
    }

    public void PauseMenu(bool status)
    {
        pauseMenuUI.SetActive(status);
    }

    public void MainMenuB()
    {
        SceneManager.LoadScene(0);
        Destroy(FindObjectOfType<GameManager>().gameObject);
    }

    public void QuitGameB()
    {
        Application.Quit();
    }

}
