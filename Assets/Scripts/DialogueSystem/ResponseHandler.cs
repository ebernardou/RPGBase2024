using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;

    private DialogueUI dialogueUI;
    private ResponseEvent[] responseEvents;
    private GameObject responseButton;
    private CinemachineSwitcher cinemachineSwitcher;
    private GameObject activeNPC;


    List<GameObject> tempResponseButtons = new List<GameObject>();

    private void Start()
    {
        cinemachineSwitcher = FindObjectOfType<CinemachineSwitcher>();
        dialogueUI = GetComponent<DialogueUI>();
        responseBox.gameObject.SetActive(false);
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        this.responseEvents = responseEvents;
    }

    public void ShowResponses(Response[] responses)
    {
        float responseBoxWidth = 0;

        for (int i = 0; i < responses.Length; i++)
        {
            Response response = responses[i];
            int responseIndex = i;

            responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponentInChildren<Button>().onClick.AddListener(()=> OnPickedResponse(response, responseIndex));
            
            tempResponseButtons.Add(responseButton);

            responseBoxWidth += responseButtonTemplate.sizeDelta.x;
        }
        responseBox.sizeDelta = new Vector2(responseBoxWidth, responseBox.sizeDelta.y);
        responseBox.gameObject.SetActive(true);
        responseBox.GetComponentInChildren<Button>().Select();
    }

    private void OnPickedResponse(Response response, int responseIndex)
    {
        responseBox.gameObject.SetActive(false);

        foreach (GameObject button in tempResponseButtons)
        {
            Destroy(button);
        }
        tempResponseButtons.Clear();
        if (responseEvents != null && responseIndex <= responseEvents.Length)
        {
            responseEvents[responseIndex].OnPickedResponse?.Invoke();
        }

        responseEvents = null;

        if (response.DialogueObject)
        {
            activeNPC = GameObject.FindGameObjectWithTag("ActiveNPC");
            dialogueUI.dialogueBoxRelocation = true;
            dialogueUI.waitForDialogueDone = false;
            dialogueUI.WaitForDialogue(response.DialogueObject);
        }
        else
        {
            dialogueUI.CloseDialogueBox();
        }
    }
}
