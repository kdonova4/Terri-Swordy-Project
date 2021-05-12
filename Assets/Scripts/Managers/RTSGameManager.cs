using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RTSGameManager : MonoBehaviour
{
    private int enemies;
    private int troops;
    private int flagsCapped;
    private int flagsUncapped;
    public static void UnitTakeDamage(UnitController attackingController, UnitController attackedController)
    {
        var damage = attackingController.unitStats.attackDamage;

        attackedController.TakeDamage(attackedController, damage);
    }

    private void Start()
    {
        
        flagsUncapped = GameObject.FindGameObjectsWithTag("Uncapped Flag").Length;
        
    }
    private void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("EnemyUnit").Length;
        troops = GameObject.FindGameObjectsWithTag("Player Unit").Length;
        flagsCapped = GameObject.FindGameObjectsWithTag("Capped Flag").Length;

        if (enemies == 0 && flagsCapped.Equals(flagsUncapped))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else if (troops == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
    
    
}
