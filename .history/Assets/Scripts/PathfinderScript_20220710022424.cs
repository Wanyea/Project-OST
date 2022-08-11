using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[RequireComponent(typeof(PathCreator))]
public class PathfinderScript : MonoBehaviour
{

    [SerializeField]
    GameObject pathfinder;

    [SerializeField]
    float movementSpeed = 10f;
    [SerializeField]
    float rotationalDamp = .5f;

    [SerializeField]
    float rayCastOffset = 1f;

    [SerializeField]
    float detectionDistance = 20f; 

    [HideInInspector]
    public int flag = 0;
    private Vector3 target;
    private GameObject endTile;
    private int mTracker = 0;
    private Vector3[] m_tiles;
    private int numOfM_Tiles;
    public Vector3[] pathfinderPosition;
    private int pathfinderPositionIndex = 0;

    public bool closedLoop = false;



    void Start() 
    {
        LevelGeneration levelGenerationScript = GetComponent<LevelGeneration>();
        numOfM_Tiles = levelGenerationScript.mapDepthInTiles;
        m_tiles = levelGenerationScript.m_tile_positions;
        endTile = levelGenerationScript.spawnEnd;
        pathfinderPosition = new Vector3[1000]; //size can be changed to width * depth of total tiles in the future.

        Instantiate(pathfinder, levelGenerationScript.spawnPointPrefab.transform.position , Quaternion.identity);

        if(pathfinder != null) 
            InvokeRepeating("StorePathfinderPosition", 0.0f, 0.3f);
        
    }


    void Update()
    {
        Move();
        AvoidCollision();

    }

    void Turn() 
    {
        target = m_tiles[mTracker];
        
        Vector3 relativePosition = target - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);
        UpdateTarget();
    }

    void Move() 
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    void AvoidCollision() 
    {
        RaycastHit hit;
        Vector3 offset = Vector3.zero;

        Vector3 left = transform.position - transform.right * rayCastOffset;
        Vector3 right = transform.position + transform.right * rayCastOffset;
        Vector3 up = transform.position + transform.up * rayCastOffset;
        Vector3 down = transform.position - transform.up * rayCastOffset;

        Debug.DrawRay(left, transform.forward * detectionDistance, Color.red);
        Debug.DrawRay(right, transform.forward * detectionDistance, Color.red);
        Debug.DrawRay(up, transform.forward * detectionDistance, Color.red);
        Debug.DrawRay(down, transform.forward * detectionDistance, Color.red);

        //Handle raycast collisions on left and right and move out of way accordingly.
        if(Physics.Raycast(left, transform.forward, out hit, detectionDistance)) 
            offset += Vector3.right;    
        else if(Physics.Raycast(right, transform.forward, out hit, detectionDistance))
            offset -= Vector3.left;  

        //Handle raycast collisions above and below and move out of way accordingly.
        if(Physics.Raycast(up, transform.forward, out hit, detectionDistance)) 
            offset -= Vector3.up;    
        else if(Physics.Raycast(down, transform.forward, out hit, detectionDistance))
            offset += Vector3.up;  
        
        if(offset != Vector3.zero) {
            transform.Rotate(offset * 5f * Time.deltaTime);
        } else {
            Turn();
        }
    }

    void UpdateTarget() 
    {
        //Update target for 0 ---> n - 1 tiles. 
        if(target != endTile.transform.position) {
            if(Vector3Int.RoundToInt(transform.position) == m_tiles[mTracker]) 
                this.mTracker++;
        }

        //Handle n (final state) case.
        if(Vector3Int.RoundToInt(transform.position) == endTile.transform.position) {
            endTile.GetComponent<Renderer>().material.SetColor("_Color", Color.green);  
            Destroy(this.gameObject);  
            GenerateBezierPath();    
        }

    }

    void StorePathfinderPosition() {
       pathfinderPosition[this.pathfinderPositionIndex] = pathfinder.transform.position;
       Debug.Log(this.pathfinderPosition[pathfinderPositionIndex]);
       this.pathfinderPositionIndex++;
    }

    void GenerateBezierPath() {
           BezierPath bezierPath = new BezierPath(pathfinderPosition, closedLoop,  PathSpace.xyz);
        }
}
