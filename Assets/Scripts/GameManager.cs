using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject LimtsMission;
    [SerializeField] float missionTimer = 720.0f;
    [SerializeField]
    GameObject endMissionPortal;


    float timer;
    public bool hasActivatedMission = false;
    UnityEngine.SceneManagement.Scene currentScene;

    ThirdPersonShooterController player;

    bool stopTimer =  false;

    CanvasManager canvasManager;

    public bool restartGame = false;
    
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
            DontDestroyOnLoad(this.gameObject);

    }

    private void Start()
    {
        timer = missionTimer;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonShooterController>();
        
        if (hasActivatedMission)
            currentScene = SceneManager.GetActiveScene();
        
        if (hasActivatedMission && !stopTimer)
        {
            canvasManager = FindObjectOfType<CanvasManager>();
            canvasManager.TurnOnMissionUI(true);
            canvasManager.StartMissionTimer(timer);
            StartCountDown();

            PortalManager portalManager = FindObjectOfType<PortalManager>();
            if(portalManager != null)
            {
                GameObject barPortal = portalManager.ReturnBarPortal();

                if (barPortal.activeInHierarchy)
                {
                    barPortal.SetActive(false);
                }

                
                if(LimtsMission == null)
                    LimtsMission = GameObject.FindGameObjectWithTag("MissionLimit").gameObject;

                if (LimtsMission != null && LimtsMission.activeInHierarchy == true)
                {
                    LimtsMission.SetActive(false);
                }
            }
        }

        //Conditions to Restart Mission
        if (restartGame || timer <= 0 || player.ReturnCurrentHealth() <= 0)
            RestartMission();
    }

    public void StartCountDown()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0;
        }

        canvasManager.StartMissionTimer(timer);
    }

    public void HasFinishNPCMissionTalk()
    {
        hasActivatedMission = true;
    }

    public void TurnOnFinalMissionPortal()
    {
        PortalManager portalManager = GameObject.FindObjectOfType<PortalManager>();
        endMissionPortal = portalManager.ReturnEndPortal();

        endMissionPortal.SetActive(true);
    }
    
    public void StopTimer()
    {
        stopTimer = true;
    }

    void RestartMission()
    {
        canvasManager = GameObject.FindObjectOfType<CanvasManager>();
        canvasManager.GameOverScreen(true);
        Time.timeScale = 0;

        if (Input.GetKeyDown(KeyCode.R))
        {
            timer = missionTimer;
            Time.timeScale = 1;
            restartGame = false;
            SceneManager.LoadScene("MissionLevel");
        }
    }

    public void Restart(bool status)
    {
        restartGame = status;
    }
}
