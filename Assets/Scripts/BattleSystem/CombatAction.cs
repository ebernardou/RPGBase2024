using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatAction : MonoBehaviour
{
    private GameObject enemy;
    private GameObject hero;

    [SerializeField] private GameObject simpleAttackPrefab;
    [SerializeField] private GameObject tagAttackPrefab;
    [SerializeField] private GameObject useItemPrefab;
    [SerializeField] private Sprite faceIcon;

    private GameObject currentAttack;
    
  

    private void Awake()
    {
        hero = GameObject.FindGameObjectWithTag("Hero");
        enemy = GameObject.FindGameObjectWithTag("Enemy");

    }

    public void SelectCommand(string btn)
    {
        GameObject target = hero;
        if (tag == "Hero")
        {
            target = enemy;
        }
        if (btn.CompareTo("SimpleAttack") == 0)
        {
            simpleAttackPrefab.GetComponent<AttackScript>().Attack(target);
        }
        if (btn.CompareTo("TagAttack") == 0)
        {
            if(target.GetComponent<CharacterStats>().mp <= tagAttackPrefab.GetComponent<AttackScript>().specialCost)
            {
                tagAttackPrefab.GetComponent<AttackScript>().Attack(target);
            }
            else if (tag == "Hero")
            {
                Debug.Log("Te falta mana");
                return;
            }
            else if (tag == "Enemy")
            {
                simpleAttackPrefab.GetComponent<AttackScript>().Attack(target);
            }
        }
        if (btn.CompareTo("UseItem") == 0)
        {
            //useItemPrefab.GetComponent<AttackScript>().Attack(target);
        }
        if (btn.CompareTo("Run") == 0)
        {
            Debug.Log("You ran away!");
        }
    }


}
