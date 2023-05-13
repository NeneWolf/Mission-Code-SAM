using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class NPC : MonoBehaviour {

    public Transform ChatBackGround;
    public Transform NPCCharacter;

    public GameObject player;

    private DialogueSystem dialogueSystem;

    public string Name;

    bool isColliding = false;

    [TextArea(5, 10)]
    public string[] sentences;

    void Start () {
        dialogueSystem = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogueSystem>();
    }
	
	void Update () {

        if (isColliding)
        {
            if (Input.GetKeyDown(KeyCode.F) )
            {
                dialogueSystem.NPCName(Name, sentences);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            player.GetComponent<ThirdPersonShooterController>().CanShoot(false);
            isColliding = true;
            dialogueSystem.NPCInteractUI(true);
        }
            
    }

    public void OnTriggerExit()
    {
        player.GetComponent<ThirdPersonShooterController>().CanShoot(true);
        isColliding = false;
        dialogueSystem.NPCInteractUI(false);
        dialogueSystem.OutOfRange();
    }
}

