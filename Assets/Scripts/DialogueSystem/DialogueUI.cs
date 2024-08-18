using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Cinemachine;
using JetBrains.Annotations;
using Cinemachine.Utility;
using UnityEngine.EventSystems;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private TMP_Text npcName;
    [SerializeField] private GameObject nextDialogueAnim;
    [SerializeField] private GameObject endDialogueAnim;
    [SerializeField] private GameObject interactSprite;
    [SerializeField] private CinemachineVirtualCamera npcCamera;
    private GameObject npcGameObject;
    private CinemachineSwitcher cinemachineSwitcher;


    public bool IsOpen { get; private set; }
    
    private ResponseHandler responseHandler;
    private TypewriterEffect typewriterEffect;

    private RectTransform dialogueBoxPosition;
    private Vector2 npcPosition;
    private Camera cam;
    private GameObject dialogueTail;
    private GameObject player;

    private int i;
    public bool dialogueBoxRelocation = false;
    private GameObject targetNPC;
    private DialogueObject localDialogueObject;
    public bool waitForDialogueDone = false;

    private void Update()
    {
        if (dialogueBoxRelocation == true)
        {
            RelocateDialogueBox();
        }
        else
        {
            return;
        }
        if (compareNPCandCameraPos() && waitForDialogueDone == false)
        {
            ShowDialogue();
        }
    }


    private void Start()
    {
        cinemachineSwitcher = FindObjectOfType<CinemachineSwitcher>();
        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();
        dialogueTail = GameObject.FindGameObjectWithTag("DialogueTail");
        player = GameObject.FindGameObjectWithTag("Player");
        dialogueBoxPosition = dialogueBox.GetComponent<RectTransform>();
        CloseDialogueBox();
    }

    public void SetNPCCamera()
    {
        npcCamera.Follow = targetNPC.transform;
    }

    public void WaitForDialogue(DialogueObject dialogueObject)
    {
        localDialogueObject = dialogueObject;
    }

    public void ShowDialogue()
    {
        waitForDialogueDone = true;
        IsOpen = true;
        interactSprite.SetActive(false);
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(localDialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {

        for (i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];

            nextDialogueAnim.SetActive(false);
            endDialogueAnim.SetActive(false);
            yield return RunTypingEffect(dialogue, dialogueObject);

            textLabel.text = dialogue;

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        if (dialogueObject.HasResponses)
        {
            endDialogueAnim.SetActive(false);
            nextDialogueAnim.SetActive(false);
            responseHandler.ShowResponses(dialogueObject.Responses);
        }


        else
        {
            CloseDialogueBox();
            cinemachineSwitcher.SwitchState();
            interactSprite.SetActive(true);
        }

    }

    private IEnumerator RunTypingEffect(string dialogue, DialogueObject dialogueObject)
    {
        typewriterEffect.Run(dialogue, textLabel);

        while (typewriterEffect.IsRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                typewriterEffect.Stop();
                if (i < dialogueObject.Dialogue.Length && dialogueObject.Dialogue.Length != 1)
                {
                    nextDialogueAnim.SetActive(true);
                }
                else
                {
                    endDialogueAnim.SetActive(true);
                }

            }
        }
        if (!typewriterEffect.IsRunning)
        {
            if (i < dialogueObject.Dialogue.Length && dialogueObject.Dialogue.Length != 1)
            {
                nextDialogueAnim.SetActive(true);
            }
            else
            {
                endDialogueAnim.SetActive(true);
            }
        }
    }

    public void CloseDialogueBox()
    {
        dialogueBoxRelocation = false;
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }

    public void SetNPCName(string name)
    {
        npcName.text = name;
    }

    public void RelocateDialogueBox()
    {
        Vector2 npcPosition = targetNPC.transform.position;
        cam = FindObjectOfType<Camera>();
        dialogueBoxPosition.transform.position = RectTransformUtility.WorldToScreenPoint(cam, npcPosition);
        dialogueBoxPosition.transform.position = new Vector2(dialogueBoxPosition.transform.position.x, dialogueBoxPosition.transform.position.y + 30);
        dialogueTail.transform.position = new Vector2(dialogueBoxPosition.transform.position.x-2, dialogueTail.transform.position.y);
    }

    public void SetNPC(GameObject npc)
    {
        targetNPC = npc;
    }

    public bool compareNPCandCameraPos()
    {
        cam = FindObjectOfType<Camera>();
        Vector2 camPos = new Vector2 (cam.transform.position.x, cam.transform.position.y);
        Vector2 npcPos = new Vector2(targetNPC.transform.position.x, targetNPC.transform.position.y);
        if (npcPos == camPos) {
            return true;
        }
        else
        {
            return false;
        }
    }

}
