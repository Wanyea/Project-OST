using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Turn() 
    {
        Vector3 position = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(position);
    }
}
