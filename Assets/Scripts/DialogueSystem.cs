using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogueSystem: MonoBehaviour {

    private GameManager gameManager;
    
    public Text nameText;
    public Text dialogueText;

    public GameObject npcInteractUI;
    public Transform dialogueBoxGUI;

    public float letterDelay = 0.1f;
    public float letterMultiplier = 0.5f;

    //public KeyCode DialogueInput = KeyCode.F;

    public string Names;

    public string[] dialogueLines;

    public bool letterIsMultiplied = false;
    public bool dialogueActive = false;
    public bool dialogueEnded = false;
    public bool outOfRange = true;

    bool spokenTo = false;


    //public AudioClip audioClip;
    //AudioSource audioSource;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        dialogueText.text = "";
    }

    void Update()
    {
        if (spokenTo)
        {
            gameManager.HasFinishNPCMissionTalk();
        }
    }

    public void NPCInteractUI(bool status)
    {
        npcInteractUI.SetActive(status);
    }

    public void NPCName(string name, string[] sentences)
    {
        NPCInteractUI(false);
        dialogueBoxGUI.gameObject.SetActive(true);
        
        Names = name;
        dialogueLines = sentences;
        
        outOfRange = false;

        nameText.text = Names;

        if (!dialogueActive)
        {
            dialogueActive = true;
            StartCoroutine(StartDialogue());
        }

        StartDialogue();
    }



    

    private IEnumerator StartDialogue()
    {
        if (outOfRange == false)
        {
            int dialogueLength = dialogueLines.Length;
            int currentDialogueIndex = 0;

            while (currentDialogueIndex < dialogueLength || !letterIsMultiplied)
            {
                if (!letterIsMultiplied)
                {
                    letterIsMultiplied = true;
                    StartCoroutine(DisplayString(dialogueLines[currentDialogueIndex++]));

                    if (currentDialogueIndex >= dialogueLength)
                    {
                        dialogueEnded = true;
                    }
                }
                yield return 0;
            }

            while (true)
            {
                if (Input.GetKeyDown(KeyCode.E) && dialogueEnded == false)
                {
                    break;
                }
                yield return 0;
            }
            dialogueEnded = false;
            dialogueActive = false;
            DropDialogue();
            spokenTo = true;
        }
    }

    private IEnumerator DisplayString(string stringToDisplay)
    {
        if (outOfRange == false)
        {
            int stringLength = stringToDisplay.Length;
            int currentCharacterIndex = 0;

            dialogueText.text = "";

            while (currentCharacterIndex < stringLength)
            {
                dialogueText.text += stringToDisplay[currentCharacterIndex];
                currentCharacterIndex++;

                if (currentCharacterIndex < stringLength)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        yield return new WaitForSeconds(letterDelay * letterMultiplier);

                        //if (audioClip) audioSource.PlayOneShot(audioClip, 0.5F);
                    }
                    else
                    {
                        yield return new WaitForSeconds(letterDelay);

                        //if (audioClip) audioSource.PlayOneShot(audioClip, 0.5F);
                    }
                }
                else
                {
                    dialogueEnded = false;
                    break;
                }
            }
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    break;
                }
                yield return 0;
            }
            dialogueEnded = false;
            letterIsMultiplied = false;
            dialogueText.text = "";
        }
    }

    public void DropDialogue()
    {
        NPCInteractUI(false);
        dialogueBoxGUI.gameObject.SetActive(false);
    }

    public void OutOfRange()
    {
        outOfRange = true;
        if (outOfRange == true)
        {
            letterIsMultiplied = false;
            dialogueActive = false;
            StopAllCoroutines();
            NPCInteractUI(false);
            dialogueBoxGUI.gameObject.SetActive(false);
        }
    }
}
