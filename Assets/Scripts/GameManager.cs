using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    float timer = 10.0f;
    public bool hasActivatedMission = false;
    UnityEngine.SceneManagement.Scene currentScene;

    CanvasManager canvasManager;


    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        
        DontDestroyOnLoad(this.gameObject);
        canvasManager = GameManager.FindObjectOfType<CanvasManager>();
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(hasActivatedMission)
            currentScene = SceneManager.GetActiveScene();
        
        if (hasActivatedMission && currentScene.name == "Playground")
        {
            canvasManager.TurnOnMissionUI(true);
            canvasManager.MissionNameTime(timer);
            StartCountDown();
        }

        print(currentScene.name);
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

        canvasManager.MissionNameTime(timer);
    }

    public void HasFinishNPCMissionTalk()
    {
        hasActivatedMission = true;
    }
}
