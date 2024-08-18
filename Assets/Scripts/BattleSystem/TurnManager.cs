using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    private List<CharacterStats> characterStats;

    [SerializeField]
    private GameObject battleMenu;

    public bool animDone;

    void Start()
    {
        animDone = true;
        characterStats = new List<CharacterStats>();
        GameObject hero = GameObject.FindGameObjectWithTag("Hero");
        CharacterStats currentCharacterStats = hero.GetComponent<CharacterStats>();
        currentCharacterStats.CalculateNextTurn(0);
        characterStats.Add(currentCharacterStats);

        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        CharacterStats currentEnemyStats = enemy.GetComponent<CharacterStats>();
        currentEnemyStats.CalculateNextTurn(0);
        characterStats.Add(currentEnemyStats);
        this.battleMenu.SetActive(false);
        NextTurn();
    }

    public void NextTurn()
    {
            int i = 0;
            CharacterStats currentCharacterStats = characterStats[i];
            characterStats.Remove(currentCharacterStats);
            if (!currentCharacterStats.GetDead())
            {
                GameObject currentUnit = currentCharacterStats.gameObject;
                currentCharacterStats.CalculateNextTurn(currentCharacterStats.nextActTurn);
                characterStats.Add(currentCharacterStats);
                if (currentUnit.tag == "Hero")
                {
                    this.battleMenu.SetActive(true);
                    battleMenu.GetComponentInChildren<Button>().Select();
                    i = 1;
                }
                if (currentUnit.tag == "Enemy")
                {
                    string attackType = Random.Range(0, 2) == 1 ? "SimpleAttack" : "TagAttack";
                    currentUnit.GetComponent<CombatAction>().SelectCommand(attackType);
                    i = 0;
                }
            }
            else
            {
                    Debug.Log("Mothafukaz ded");
                    NextTurn();
            }
    }

}




