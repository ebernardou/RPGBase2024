using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class MakeButton : MonoBehaviour
{
    [SerializeField]
    private bool physical;
    private GameObject hero;
    private GameObject enemy;
    private GameObject targetSelectParent;
    private string temp;
    //private bool targetSelected = false;

    void Start()
    {
        temp = gameObject.name;
        gameObject.GetComponent<Button>().onClick.AddListener(() => AttachCallback());
        hero = GameObject.FindGameObjectWithTag("Hero");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        targetSelectParent = GameObject.FindGameObjectWithTag("TargetSelect");
    }

    private void AttachCallback()
    {
        StartCoroutine("SelectTarget");
        targetSelectParent.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        targetSelectParent.GetComponentInChildren<Button>().onClick.AddListener(() => TargetSelected(temp));
    }

    IEnumerator SelectTarget()
    {
        Vector2 enemyPosition = enemy.gameObject.transform.position;
        targetSelectParent.gameObject.SetActive(true);
        targetSelectParent.GetComponent<RectTransform>().position = new Vector2(enemyPosition.x, enemyPosition.y + 3);
        targetSelectParent.GetComponentInChildren<Button>().Select();
        yield return null;
    }

    private void TargetSelected(string btn)
    {
        targetSelectParent.gameObject.SetActive(false);

        if (btn.CompareTo("SimpleAttackButton") == 0)
        {
            hero.GetComponent<CombatAction>().SelectCommand("SimpleAttack");
        }
        else if (btn.CompareTo("TagAttackButton") == 0)
        {
            hero.GetComponent<CombatAction>().SelectCommand("TagAttack");
        }
        else if (btn.CompareTo("ItemButton") == 0)
        {
            hero.GetComponent<CombatAction>().SelectCommand("UseItem");
        }
        else if (btn.CompareTo("RunButton") == 0)
        {
            hero.GetComponent<CombatAction>().SelectCommand("Run");
        }
    }

}
