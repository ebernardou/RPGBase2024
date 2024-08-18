using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IIinteractable
{
    [SerializeField] private SpriteRenderer _interactSprite;
    private GameObject player;
    private Vector2 triggerPosition;
    private bool isDialogueOpen;
    

    private const float INTERACT_DISTANCE = 0.5f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        triggerPosition = new Vector2
            ((transform.position.x + this.gameObject.GetComponent<CircleCollider2D>().offset.x),
            (transform.position.y + this.gameObject.GetComponent<CircleCollider2D>().offset.y));

    }

    private void Update()
    {
        isDialogueOpen = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogueUI>().IsOpen;
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {

        }

        if (isDialogueOpen)
        {
            _interactSprite.gameObject.SetActive(false);

        }
        else
        {
            if (_interactSprite.gameObject.activeSelf && !IsWithinInteractDistance())
            {
                _interactSprite.gameObject.SetActive(false);
            }

            else if (!_interactSprite.gameObject.activeSelf && IsWithinInteractDistance())
            {
                _interactSprite.gameObject.SetActive(true);
            }
        }


    }

    public abstract void Interact(PlayerController player);

    private bool IsWithinInteractDistance()
    {
        if(Vector2.Distance(player.transform.position, triggerPosition) < INTERACT_DISTANCE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
