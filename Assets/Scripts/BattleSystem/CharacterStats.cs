using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.Processors;
using System;
using TMPro;
using Unity.VisualScripting;

public class CharacterStats : MonoBehaviour, IComparable
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject hpFill;

    [SerializeField]
    private GameObject mpFill;

    [SerializeField]
    public TextMeshProUGUI damageText;

    [SerializeField]
    public GameObject damageExplosion;

    [Header("Stats")]
    public float hp;
    public float mp;
    public float meleeAttack;
    public float specialAttack;
    public float defense;
    public int initiative;
    public float experience;

    private float startHP;
    private float startMP;

    [HideInInspector]
    public int nextActTurn;

    private bool dead = false;

    //Resize Health and Special Bar
    private Transform hpTransform;
    private Transform mpTransform;

    private Vector2 hpScale;
    private Vector2 mpScale;

    private float xNewHPScale;
    private float xNewMPScale;


    void Awake()
    {

        hpTransform = hpFill.GetComponent<RectTransform>();
        hpScale = hpFill.transform.localScale;

        mpTransform = mpFill.GetComponent<RectTransform>();
        mpScale = mpFill.transform.localScale;

        startHP = hp;
        startMP = mp;
    }

    public void ReceiveDamage(float damage)
    {
        hp = hp - damage;
        //Set damage text

        if (hp <= 0)
        {
            dead = true;
            gameObject.tag = "Dead";
            Destroy(hpFill);
            Destroy(mpFill);
            Destroy(this.gameObject);
        }
        else if (damage > 0)
        {
            xNewHPScale = hpScale.x * (hp / startHP);
            hpFill.transform.localScale = new Vector2(xNewHPScale, hpScale.y);
            animator.SetBool("IsHurt", true);
            damageText.text = damage.ToString();
            damageText.gameObject.SetActive(true);
            damageExplosion.gameObject.SetActive(true);
            Invoke("ResetHurtAnimation", 0.5f);
        }
        else if (damage < 0)
        {
            Debug.Log("Miss");
            damageText.text = "Miss";
            damageText.gameObject.SetActive(true);
        }
        Invoke("DamageTextFade", 1.5f);
    }



    void ResetHurtAnimation()
    {
        animator.SetBool("IsHurt", false);
    }

    public void updateMPFill(float cost)
    {
        if (cost  > 0)
        {
            mp -= cost;
            xNewMPScale = mpScale.x * (mp / startMP);
            mpFill.transform.localScale = new Vector2(xNewMPScale, mpScale.y);
        }
        
    }

    public void ContinueGame()
    {
            Debug.Log("pasa turno");
            GameObject.Find("TurnManager").GetComponent<TurnManager>().NextTurn();
    }

    public bool GetDead()
    {
        return dead;
    }

    public void CalculateNextTurn(int currentTurn)
    {
        nextActTurn = currentTurn + initiative;
    }

    public int CompareTo(object otherStats)
    {
        int nex = nextActTurn.CompareTo(((CharacterStats)otherStats).nextActTurn);
        return nex;
    }

    public void DamageTextFade()
    {
        damageText.gameObject.SetActive(false);
        damageExplosion.gameObject.SetActive(false);
    }
}
