using Cinemachine;
using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, IIinteractable
{

    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private string npcName;
    private GameObject npc;
    private DialogueUI dialogueUI;

    private CinemachineSwitcher cinemachineSwitcher;
    


    private void Start()
    {
        cinemachineSwitcher = FindObjectOfType<CinemachineSwitcher>();
        dialogueUI = GetComponent<DialogueUI>();
        npc = this.gameObject;
    }

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController player))
        {
            player.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController player))
        {
            if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                player.Interactable = null;
            }
        }
    }

    public void Interact(PlayerController player)
    {
        foreach (DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if (responseEvents.DialogueObject == dialogueObject)
            {
                player.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }
        this.gameObject.tag = "ActiveNPC";
        player.GetComponent<Animator>().SetBool("isMoving", false);
        cinemachineSwitcher.SwitchState();
        player.DialogueUI.SetNPC(npc);
        player.DialogueUI.SetNPCName(npcName);
        player.DialogueUI.SetNPCCamera();
        player.DialogueUI.dialogueBoxRelocation = true;
        player.DialogueUI.waitForDialogueDone = false;
        player.DialogueUI.WaitForDialogue(dialogueObject);
    }
}


