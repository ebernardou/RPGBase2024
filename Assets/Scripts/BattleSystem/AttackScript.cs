using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AttackScript : MonoBehaviour
{
    public GameObject owner;

    [SerializeField]
    private bool specialAttack;

    [SerializeField]
    private string animationName;

    [SerializeField]
    public float specialCost;

    [SerializeField]
    private float minAttackMultiplier;

    [SerializeField]
    private float maxAttackMultiplier;

    [SerializeField]
    private float minDefenseMultiplier;

    [SerializeField]
    private float maxDefenseMultiplier;



    private CharacterStats attackerStats;
    private CharacterStats targetStats;
    private float damage = 0.0f;
    private GameObject battleMenu;
    private bool movementBegin = false;
    public bool movementBack = false;
    private GameObject targetPos;
    public Vector2 ownerOriginalPosition;
    public GameObject turnManager;
    public float movOffset = 0;
    private AnimatorClipInfo[] animInfo;
    private float animDuration;
    private Animator animator;


    private void Start()
    {
        animator = owner.GetComponent<Animator>();
        turnManager = GameObject.FindGameObjectWithTag("TurnManager");
        ownerOriginalPosition = owner.transform.position;
        battleMenu = GameObject.FindGameObjectWithTag("BattleMenu");
        GetAnimDuration();
    }

    private void Update()
    {

        float movOffset = 0;
        if(owner.tag == "Hero")
        {
            movOffset = -2f;
        }
        if (owner.tag == "Enemy")
        {
            movOffset = 2f;
        }
        if (movementBegin == true)
        {
            Vector2 ownerPosition = owner.transform.position;
            Vector2 targetPosition = new Vector2(targetPos.transform.position.x + movOffset, targetPos.transform.position.y);
            owner.transform.position = Vector2.MoveTowards(ownerPosition, targetPosition, Time.deltaTime * 15);
            animator.SetBool("IsMoving", true);
            if (Vector2.Distance(owner.transform.position, targetPosition) == 0 && movementBegin == true)
            {
                movementBegin = false;
                animator.SetBool("IsMoving", false);
                animator.Play(animationName);
                Invoke("DamageAnimation", animDuration - 0.5f);
            }
        }
        if (movementBack == true)
        {

            Vector2 ownerPosition = owner.transform.position;
            owner.transform.position = Vector2.MoveTowards(ownerPosition, ownerOriginalPosition, Time.deltaTime * 15);
            animator.SetBool("IsMoving", true);
            if (owner.transform.position.x == ownerOriginalPosition.x && owner.transform.position.y == ownerOriginalPosition.y)
            {
                movementBegin = false;
                movementBack = false;
                animator.SetBool("IsMoving", false);
                turnManager.GetComponent<TurnManager>().NextTurn();
            }
        }
    }

    public void Attack(GameObject target)
    {
        turnManager.GetComponent<TurnManager>().animDone = false;
        targetPos = target;
        attackerStats = owner.GetComponent<CharacterStats>();
        targetStats = target.GetComponent<CharacterStats>();
        if (attackerStats.mp >= specialCost)
        {
            battleMenu.SetActive(false);
            float multiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);
            damage = multiplier * attackerStats.meleeAttack;
            if (specialAttack)
            {
                damage = multiplier * attackerStats.specialAttack;
            }
            else
            {
                float defenseMultiplier = Random.Range(minDefenseMultiplier, maxDefenseMultiplier);
                damage = Mathf.Max(0, damage - (defenseMultiplier * targetStats.defense));
            }
            movementBegin = true;
            Debug.Log("ataquinhooo");
            attackerStats.updateMPFill(specialCost);
        }
        else
        {
            return;
        }
    }

    void DamageTextFade()
    {
        targetPos.gameObject.GetComponent<CharacterStats>().damageText.gameObject.SetActive(false);
    }

    private void DamageAnimation()
    {
        targetStats.ReceiveDamage(Mathf.CeilToInt(damage));
        Invoke("WaitForMovementBack", 1);
    }

    private void WaitForMovementBack()
    {
        movementBack = true;
    }

    private void GetAnimDuration()
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == animationName)
            {
                animDuration = ac.animationClips[i].length;
            }
        }
    }
    
}

