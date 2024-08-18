using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;
    public IIinteractable Interactable { get; set; }
    public float moveSpeed;
    private bool isMoving;
    private Vector2 input;
    private Animator animator;
    public LayerMask solidObjectsLayer;
    private Rigidbody2D playerRigidBody;
    private bool isInventoryEnabled;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isInventoryEnabled = GetComponent<InventoryController>().inventoryUI.isInventoryEnabled;
        if (Input.GetKeyDown(KeyCode.Z) && dialogueUI.IsOpen == false)
        {
            if (isInventoryEnabled)
            {
                return;
            }
            else
            {
                if (Interactable != null)
                {
                    Interactable.Interact(this);
                }
            }
        }

    }

    private void FixedUpdate()
    {
        if (dialogueUI.IsOpen)
        {
            return;
        }
        if (isInventoryEnabled)
        {

            return;
        }

        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                Vector2 targetPos = playerRigidBody.velocity;
                targetPos.x += input.x * moveSpeed * Time.deltaTime;
                targetPos.y += input.y * moveSpeed * Time.deltaTime;
                if ((input.x != 0) && (input.y != 0))
                {
                    targetPos.y = targetPos.y*0.75f;
                    targetPos.x = targetPos.x*0.75f;
                }
                StartCoroutine(Move(targetPos));
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

     
    IEnumerator Move(Vector2 targetPos)
    {
        isMoving = true;
        while ((targetPos - playerRigidBody.velocity).sqrMagnitude > Mathf.Epsilon)
        {
            playerRigidBody.velocity = targetPos;
            yield return null;
        }
        playerRigidBody.velocity = Vector2.zero;

        isMoving = false;
    }
}