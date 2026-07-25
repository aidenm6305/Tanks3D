using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Map : MonoBehaviour
{

    [Header("Prefabs")]
    [Tooltip("The base wall prefabs used for constructing the maze interior")]
    [SerializeField] private GameObject baseWall;
    [SerializeField] private GameObject breakableWall;
    [Tooltip("The edge wall prefab used for the maze boundaries")]
    [SerializeField] private GameObject edgeWall;
    [Tooltip("The main floor prefab for the maze")]
    [SerializeField] private GameObject mainFloor;


    [Header("Maze Generation Settings (Wall Density)")]
    [Range(0.001f, 1.0f)]
    [SerializeField] float wallDensity = 0.5f;
    [Range(0.001f, 1.0f)]
    [SerializeField] float breakableWallDensity = 0.5f;
    [Header("Folders")]
    [Tooltip("The folder to place the base walls in")]
    [SerializeField] private GameObject wallFolder;
    [Tooltip("The folder to place the edge walls in")]
    [SerializeField] private GameObject edgeWallFolder;
    [Tooltip("The offset for the wall's Y position")]
    [SerializeField] private float baseWallYOffset = 0; 
    [SerializeField] private float edgeWallYOffset = 0; 
    

    [Header("Bullet Spawns")]
    [SerializeField] private float minSpawnTime = 1f;

    [SerializeField] private float maxSpawnTime = 1f;

    [SerializeField] private float startSpawnWaitTime = 1f;
    [SerializeField] private BulletPickup bulletPickupPrefab;
    
    [SerializeField] private List<Bullet> pickupBullets;
    // Instance fields
    private MazeGenerator mazeGenerator = new MazeGenerator();

    int startX;
    int startZ;

    Vector3 wallScale;
    Vector3 floorScale;
    private Spawner spawner;
    List<Vector3> pathLocations = new List<Vector3>();
    private static Vector3 GetWorldSize(GameObject target)
    {
        var meshFilter = target.GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            return Vector3.Scale(meshFilter.sharedMesh.bounds.size, target.transform.lossyScale);
        }

        var renderer = target.GetComponent<Renderer>();
        return renderer != null ? renderer.bounds.size : target.transform.lossyScale;
    }

    void Start()
    {

        wallScale = GetWorldSize(baseWall);
        floorScale = GetWorldSize(mainFloor);

        int cellCountX = Mathf.Max(1, Mathf.RoundToInt(floorScale.x / wallScale.x));
        int cellCountZ = Mathf.Max(1, Mathf.RoundToInt(floorScale.z / wallScale.z));

        startX = Mathf.CeilToInt(cellCountX / 2f);
        startZ = Mathf.CeilToInt(cellCountZ / 2f);

        int itterX = startX;
        int itterZ = startZ;

        Debug.Log($"Floor Scale: {floorScale.x} x {floorScale.z}");
        Debug.Log($"Start Position: {startX}, {startZ}");

        // MazePath takes the interior size; the generator adds the outer border itself.
        List<List<MazeCell>> maze = mazeGenerator.MazePath(
            cellCountX - 2,
            cellCountZ - 2,
            seed: UnityEngine.Random.Range(1, 10000000),
            wallDensity: wallDensity,
            breakableWallDensity: breakableWallDensity
        );

        float wallHeight = wallScale.y / 2f;

        foreach (List<MazeCell> row in maze)
        {
            foreach (MazeCell cell in row)
            {
                float yOffset = (cell == MazeCell.Border) ? edgeWallYOffset : baseWallYOffset;
                if (cell != MazeCell.Path)
                {

                    GameObject wallToInstantiate; 
                    switch (cell)
                    {
                        case MazeCell.Wall:
                            wallToInstantiate = baseWall;
                            break;
                        case MazeCell.BreakableWall:
                            wallToInstantiate = breakableWall;
                            break;
                        case MazeCell.Border:
                            wallToInstantiate = edgeWall;
                            break;
                        default:
                            wallToInstantiate = baseWall;
                            break;
                    }

                    GameObject folder = (cell == MazeCell.Border) ? edgeWallFolder : wallFolder;

                    var tempWall = Instantiate(
                        wallToInstantiate,
                        new Vector3(itterX * wallScale.x - wallScale.x, wallHeight + yOffset, itterZ * wallScale.z - wallScale.z),
                        Quaternion.identity,
                        folder.transform
                    );

                    tempWall.isStatic = true;
                }
                else
                {
                    Vector3 newLocation = new Vector3(itterX * wallScale.x - wallScale.x, wallHeight + yOffset, itterZ * wallScale.z - wallScale.z);
                    Debug.Log($"Instantiating path at: {itterX}, {itterZ}");
                    pathLocations.Add(newLocation);
                }
                itterX -= 1;
            }

            itterX = startX;
            itterZ -= 1;
        }
        StartCoroutine(SpawnPickups());
    }



    private IEnumerator SpawnPickups()
    {
        spawner = new Spawner(pathLocations, pickupBullets, Mathf.Min(wallScale.x, wallScale.z));
        Debug.Log($"Spawn area size: {Mathf.Min(wallScale.x, wallScale.z)}");
        yield return new WaitForSeconds(startSpawnWaitTime);
        while (true)
        {
            spawner.placePickup(bulletPickupPrefab, true);
            float spawnTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);

        }
    }


}
