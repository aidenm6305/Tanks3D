using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

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
    // Instance fields
    private MazeGenerator mazeGenerator = new MazeGenerator();

    int startX;
    int startZ;

    Vector3 wallScale;
    Vector3 floorScale;

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
                if (cell != MazeCell.Path)
                {

                    GameObject wallToInstantiate; 
                    switch (cell)
                    {
                        case MazeCell.Wall:
                            wallToInstantiate = baseWall;
                            break;
                        case MazeCell.BreakableWall:
                            Debug.Log($"Instantiating breakable wall at: {itterX}, {itterZ}");
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
                    float yOffset = (cell == MazeCell.Border) ? edgeWallYOffset : baseWallYOffset;

                    var tempWall = Instantiate(
                        wallToInstantiate,
                        new Vector3(itterX * wallScale.x - wallScale.x, wallHeight + yOffset, itterZ * wallScale.z - wallScale.z),
                        Quaternion.identity,
                        folder.transform
                    );

                    tempWall.isStatic = true;
                }
                itterX -= 1;
            }

            itterX = startX;
            itterZ -= 1;
        }
    }
}