using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UnitController : MonoBehaviour
{
    public UnitStats unitStats;
    private NavMeshAgent navAgent;
    public PlayerManager refScript;
    private Transform currentTarget;
    private float attackCooldown;
    private float currentHealth;
    private Canvas canvas;
    private Camera cam;
    
    [SerializeField]
    private Image healthBar;
    
    [SerializeField]
    public float healthShowTime = 4f;
    

    private void Awake()
    {
        cam = Camera.main;
    }
    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);
        attackCooldown = unitStats.attackSpeed;
        currentHealth = unitStats.health;
    }

    private void Update()
    {
        canvas.transform.LookAt(cam.transform);
        attackCooldown += Time.deltaTime;

        if (currentTarget != null)
        {
            navAgent.destination = currentTarget.position;


            var distance = (transform.position - currentTarget.position).magnitude;

            if (distance <= unitStats.attackRange)
            {
                Attack();
            }
        }
        
    }

    public void MoveUnit(Vector3 dest)
    {
        currentTarget = null;
        
          
        navAgent.destination = dest;
        
        
    }

    public void MoveAllUnits(Vector3 dest)
    {
        currentTarget = null;
        Vector3 randomDirection = dest + Random.insideUnitSphere * 10f;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10, 1))
        {
            finalPosition = hit.position;
        }
        navAgent.destination = finalPosition;
    }

    public void SetSelected(bool isSelected)
    {
        transform.Find("Highlight").gameObject.SetActive(isSelected);
    }

    Vector3 RandomRoamPosition(float radius, Vector3 dest)
    {
        Vector3 randomDirection = dest + Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public void SetNewTarget(Transform enemy)
    {
        currentTarget = enemy;
    }

    public void Attack()
    {
        if(attackCooldown >= unitStats.attackSpeed)
        {
            RTSGameManager.UnitTakeDamage(this, currentTarget.GetComponent<UnitController>());
            attackCooldown = 0;
        }
    }

    public void TakeDamage(UnitController enemy, float damage)
    {
        
        
        currentHealth -= damage;
        
        //Debug.Log(enemy.transform.GetChild(0).GetComponentInParent<Renderer>().material.color);
        StartCoroutine(Flasher(enemy, enemy.transform.GetChild(0).GetComponent<Renderer>().material.color));
        if(currentHealth <= .1)
        {
            refScript.selectedUnits.Remove(enemy);
            Destroy(gameObject);
            
        }
        healthBar.fillAmount = currentHealth;
         
        StopCoroutine(ShowWhenDamaged());
        StartCoroutine(ShowWhenDamaged());
        
        
        

    } 

    IEnumerator ShowWhenDamaged()
    {
        ShowHealthBar();
        yield return new WaitForSeconds(6f);
        HideHealthBar();
    }

    IEnumerator Flasher(UnitController enemy, Color defaultColor)
    {
        var renderer = enemy.transform.GetChild(0).GetComponent<Renderer>();
        for (int i = 0; i < 2; i++)
        {
            renderer.material.color = Color.gray;
            yield return new WaitForSeconds(0.05f);
            renderer.material.color = defaultColor;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void ShowHealthBar()
    {
        canvas.gameObject.SetActive(true);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player Unit" && this.gameObject.tag != "Player Unit")
        {
            currentTarget = other.transform;
            navAgent.destination = other.transform.position;
        }
        if (other.gameObject.tag == "EnemyUnit" && this.gameObject.tag != "EnemyUnit")
        {
            currentTarget = other.transform;
            navAgent.destination = other.transform.position;
        }
    }
    public void HideHealthBar()
    {
        canvas.gameObject.SetActive(false);
    }



    
}
