using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public int mapWidthInTiles, mapDepthInTiles, heightAboveBaseTiles;

    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    public GameObject spawnPointPrefab;

    [SerializeField]
    public GameObject endPointPrefab;

    [SerializeField]
    private GameObject m_tiles;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject pathfinder;

    private Vector3 tileCenter;
    
    [HideInInspector]
    public Vector3[] m_tile_positions;

    [HideInInspector]
    public GameObject spawnEnd;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    void GenerateMap() 
    {
        // initialize array holding M tiles positions.

        m_tile_positions = new Vector3[mapWidthInTiles + 1];

        // get tile dimensions from the tile Prefab
        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        int tileWidth = (int)tileSize.x;
        int tileDepth = (int)tileSize.z;

        //Helper variables.
        Vector3 mPosition = Vector3.zero;
        int mTracker = 0;
        int mSpawnValue = -1;
        bool mCanSpawn = true;

        for(int xTileIndex = 0; xTileIndex < mapWidthInTiles; xTileIndex++) {

            //Spawn one M tile for every row.
            if(mTracker == xTileIndex) { mSpawnValue = Random.Range(0, mapDepthInTiles); }  
            mTracker++;

            for(int zTileIndex = 0; zTileIndex < mapDepthInTiles; zTileIndex++) {
                mCanSpawn = true;
                // calculate the tile position based on the X and Z indices
                Vector3 tilePosition = new Vector3
                (
                    this.gameObject.transform.position.x + xTileIndex * tileWidth,
                    this.gameObject.transform.position.y,
                    this.gameObject.transform.position.z + zTileIndex * tileDepth
                );

                // instantiate a new tile 
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;

                // instantiate spawn at random position on 1st tile -- don't allow an M tile to spawn here.
                if(xTileIndex == 0 && zTileIndex == 0) {
                    Vector3 spawnPosition = new Vector3
                    (
                        Random.Range(-tileWidth / 2, tileWidth / 2),
                        this.gameObject.transform.position.y + heightAboveBaseTiles,
                        Random.Range(-tileDepth / 2, tileDepth / 2)
                    );

                    GameObject spawnPoint = Instantiate(spawnPointPrefab, spawnPosition, Quaternion.identity) as GameObject;
                    spawnPoint.GetComponent<Renderer>().material.SetColor("_Color", Color.green);

                    //GameObject spawnPlayer = Instantiate(player, spawnPosition, Quaternion.identity) as GameObject;
                    GameObject spawnPathfinder = Instantiate(pathfinder, spawnPosition, Quaternion.identity) as GameObject;

                    mCanSpawn = false;
                }
                
                // instantiate end at random position on the nth tile -- don't allow M tiles to spawn here.
                if(xTileIndex == mapWidthInTiles - 1 && zTileIndex == mapDepthInTiles - 1) {
                    Vector3 endPosition = new Vector3
                    (
                      tile.transform.position.x + Random.Range(-tileWidth / 2, tileWidth / 2),
                      this.gameObject.transform.position.y + heightAboveBaseTiles,
                      tile.transform.position.z + Random.Range(-tileDepth / 2, tileDepth / 2)  
                    );

                    spawnEnd = Instantiate(endPointPrefab, endPosition, Quaternion.identity) as GameObject;
                    spawnEnd.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    mCanSpawn = false;

                    //add the final tile at the nth position within the m tiles array.
                    m_tile_positions[xTileIndex + 1] = endPosition;
                }

                // Spawn M tile if we are not on first or nth tile, no other tile has been spawned on this row.
                if(mCanSpawn && mSpawnValue == zTileIndex) 
                {
                    mPosition = new Vector3
                    (
                      tile.transform.position.x + Random.Range(-tileWidth / 2, tileWidth / 2),
                      this.gameObject.transform.position.y + heightAboveBaseTiles,
                      tile.transform.position.z + Random.Range(-tileDepth / 2, tileDepth / 2)  
                    );

                    GameObject spawnMTiles = Instantiate(m_tiles, mPosition, Quaternion.identity) as GameObject;
                    spawnMTiles.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);

                    //Store array of M tile positions to feed into pathfinder. 
                    m_tile_positions[xTileIndex] = mPosition;

                }

            }
        }
    }
}
