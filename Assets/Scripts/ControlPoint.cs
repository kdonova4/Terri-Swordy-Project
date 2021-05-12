using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPoint : MonoBehaviour
{
    public Image controlBar;
    public Canvas canvas;
    private int occupying = 0;
    private Camera cam;
    private float controlDelay = 1f;
    private float currentControlDelay;
    private Renderer mat;
    public List<Transform> spawnPoints = new List<Transform>();
    private bool captured = false;
    private bool spawned = false;
    public GameObject Troop;
    private void Awake()
    {
        cam = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponentInParent<Renderer>();
        canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        canvas.transform.LookAt(cam.transform);
        currentControlDelay += Time.deltaTime;
        if(controlBar.fillAmount == 1)
        {
            mat.material.SetColor("_Color", Color.blue);
            gameObject.tag = "Capped Flag";
            captured = true;
        }
        if(captured)
        {
            SpawnTroops();
            
        }

    }


    public void SpawnTroops()
    {
        if(spawned == false)
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                Instantiate(Troop, spawnPoints[i].position, Quaternion.identity);
            }
            
            spawned = true;
        }
            
    }    

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player Unit" )
        {
            
            ControlBar(true);
            occupying += 1;

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(occupying > 0 && other.gameObject.tag != "EnemyUnit")
        {
            if(currentControlDelay > controlDelay)
            {
                controlBar.fillAmount += .1f;
                currentControlDelay = 0;
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player Unit")
        {
            
            
            occupying -= 1;
        }
        if(occupying == 0)
        {
            ControlBar(false);
        }
    }

    private void ControlBar(bool b)
    {
        if(b)
        {
            canvas.gameObject.SetActive(true);
        }
        else
        {
            canvas.gameObject.SetActive(false);
        }
    }

    
}
