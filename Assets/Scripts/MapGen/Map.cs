using UnityEngine;
using System.Collections.Generic;
using System;

public class Map : MonoBehaviour
{

    [Header("Prefabs")]
    [Tooltip("The base wall prefab used for constructing the maze interior")]
    [SerializeField] private GameObject baseWall;
    [Tooltip("The edge wall prefab used for the maze boundaries")]
    [SerializeField] private GameObject edgeWall;
    [Tooltip("The main floor prefab for the maze")]
    [SerializeField] private GameObject mainFloor;


    [Range(0.001f, 1.0f)]
    [SerializeField] float density = 0.5f;
    [Header("Folders")]
    [Tooltip("The folder to place the base walls in")]
    [SerializeField] private GameObject wallFolder;
    [Tooltip("The folder to place the edge walls in")]
    [SerializeField] private GameObject edgeWallFolder;

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
        Debug.Log($"Density: {density}");

        // MazePath takes the interior size; the generator adds the outer border itself.
        List<List<MazeCell>> maze = mazeGenerator.MazePath(cellCountX - 2, cellCountZ - 2, seed: UnityEngine.Random.Range(1, 10000000), density: density);
        float wallHeight = wallScale.y / 2f;

        foreach (List<MazeCell> row in maze)
        {
            foreach (MazeCell cell in row)
            {
                if (cell == MazeCell.Wall || cell == MazeCell.Border)
                {

                    GameObject wallToInstantiate = (cell == MazeCell.Border) ? edgeWall : baseWall;
                    GameObject folder = (cell == MazeCell.Border) ? edgeWallFolder : wallFolder;
                    var tempWall = Instantiate(
                        wallToInstantiate,
                        new Vector3(itterX * wallScale.x - wallScale.x, wallHeight, itterZ * wallScale.z - wallScale.z),
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