using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Item : MonoBehaviour, IIinteractable
{

    [SerializeField]
    private SpriteRenderer _interactSprite;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float duraton = 0.3f;

    private GameObject player;
    private Vector2 triggerPosition;
    private bool isDialogueOpen;

    [HideInInspector]
    public ItemSO aInventoryItem;

    [HideInInspector]
    public int aQuantity;

    private const float INTERACT_DISTANCE = 1f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        triggerPosition = new Vector2
            ((transform.position.x + this.gameObject.GetComponent<CircleCollider2D>().offset.x),
            (transform.position.y + this.gameObject.GetComponent<CircleCollider2D>().offset.y));
        /*player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(player.name);*/
    }

    internal void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickup());
    }

    private IEnumerator AnimateItemPickup()
    {
        //audioSource.Play();
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duraton)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duraton);
            yield return null;
        }
        //if (!isDialogueOpen)
            Destroy(gameObject);
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
        if (Vector2.Distance(player.transform.position, triggerPosition) < INTERACT_DISTANCE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

