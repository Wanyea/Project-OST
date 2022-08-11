using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderScript : MonoBehaviour
{

    [SerializeField]
    Transform target;

    [SerializeField]
    float movementSpeed = 10f;
    [SerializeField]
    float rotationalDamp = .5f;

    [SerializeField]
    float rayCastOffset = 2.5f;

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
        Vector3 relativePosition = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePosition);
        transform.rotation = Quaternion.Slerp(transform.position, rotation, rotationalDamp * Time.deltaTime);
    }

    void Move() 
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    void Pathfinding() 
    {
        Vector3 left = transform.position - transform.right * rayCastOffset;
    }
}
