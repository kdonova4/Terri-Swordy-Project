using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float camSpeed = 5f;
    public Vector3 offset;
    public bool smoothFollow = true;


    // Update is called once per frame
    void Update()
    {

        


        Vector3 newpos = transform.position;
        newpos.x = target.position.x;
        newpos.z = target.position.z - offset.z;
        newpos.y = target.position.y - offset.y;

        transform.position = Vector3.Lerp(transform.position, newpos, camSpeed * Time.deltaTime);

        
        
    }
}
