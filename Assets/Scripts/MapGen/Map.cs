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
    void Start()
    {

        wallScale = baseWall.transform.localScale;
        floorScale = mainFloor.transform.localScale;

        startX = (int)MathF.Ceiling(floorScale.x);
        startZ = (int)MathF.Ceiling(floorScale.z);

        int itterX = startX;
        int itterZ = startZ;

        Debug.Log($"Floor Scale: {floorScale.x} x {floorScale.z}");
        Debug.Log($"Start Position: {startX}, {startZ}");
        Debug.Log($"Density: {density}");

        // * 2 as each path is 2 cells wide - 3 for the 4 edges (0th row is a edge so - 3)
        List<List<MazeCell>> maze = mazeGenerator.MazePath(startX * 2 - 3, startZ * 2 - 3, seed: UnityEngine.Random.Range(1, 10000000), density: density);
        float wallHeight = wallScale.y / 2;

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

                }
                itterX -= 1;
            }

            itterX = startX;
            itterZ -= 1;
        }
    }
}