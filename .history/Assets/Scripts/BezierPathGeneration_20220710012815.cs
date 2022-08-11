using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[RequireComponent(typeof(PathCreator))]
public class BezierPathGeneration : MonoBehaviour
{
    private BezierPath bezierPath;
    private Vector3[] bezierPoints;
    public bool closedLoop = false;

    [HideInInspector]
    public int flag = 0;
    [HideInInspector]
    public Vector3[] pathfinderPoints;

    [SerializeField]
    public LevelGeneration levelGenerationScript;

    [SerializeField]
    public PathfinderScript pathfinderScript;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject sceneController = GameObject.Find("SceneController");
        // LevelGeneration levelGenerationScript = sceneController.GetComponent<LevelGeneration>();
        // PathfinderScript pathfinderScript = sceneController.GetComponent<PathfinderScript>();
        bezierPoints = levelGenerationScript.m_tile_positions;
        pathfinderPoints = pathfinderScript.pathfinderPosition;

        //foreach(var x in bezierPoints) Debug.Log(x.ToString());
 
    }

    void Update()
    {
        flag = pathfinderScript.flag;
        
        if(flag == 1) {
            BezierPath bezierPath = new BezierPath(pathfinderPoints, closedLoop, PathSpace.xyz);
            GetComponent<PathCreator>().bezierPath = bezierPath;
            flag = 0;
        }
    }
}
