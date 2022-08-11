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
    float rayCastOffset = 1f;

    [SerializeField]
    float detectionDistance = 20f;

    void Update()
    {
        Pathfinding();
    }

    void Turn() 
    {
        Vector3 relativePosition = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);
    }

    void Move() 
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    void Pathfinding() 
    {
        RaycastHit hit;

        Vector3 left = transform.position - transform.right * rayCastOffset;
        Vector3 right = transform.position + transform.right * rayCastOffset;
        Vector3 up = transform.position + transform.up * rayCastOffset;
        Vector3 down = transform.position - transform.up * rayCastOffset;

        Debug.DrawRay(left, transform.forward * detectionDistance, Color.red);
        Debug.DrawRay(right, transform.forward * detectionDistance, Color.red);
        Debug.DrawRay(up, transform.forward * detectionDistance, Color.red);
        Debug.DrawRay(down, transform.forward * detectionDistance, Color.red);

        if(Physics.Raycast(left, transform.forward, out hit, detectionDistance)) 
        {

        }

        if(Physics.Raycast(right, transform.forward, out hit, detectionDistance)) 
        {
            
        }

    }
}
