using UnityEngine;
using System.Collections.Generic;
public class Spawner : MonoBehaviour
{
    private List<Vector3> paths;
    private List<int> pickupIndexes = new List<int>();
    private float squareSize;
    public Spawner(List<Vector3> paths, float squareSize)
    {
        this.paths = paths;
        this.squareSize = squareSize;

        for (int i = 0; i < paths.Count; i++)
        {
            pickupIndexes.Add(i);
        }
    }
    public void placePickup(Pickup obj, bool animate = false)
    {
        if (pickupIndexes.Count == 0)
        {
            return;
        }
        int tile = Random.Range(0, pickupIndexes.Count);

        Vector3 randomUnitCircle = Random.insideUnitCircle * squareSize * 0.5f;
        Vector3 spawnPosition = paths[pickupIndexes[tile]] + randomUnitCircle;

        spawnPosition.y = 0.0f;

        Pickup item = Instantiate(obj, spawnPosition, Quaternion.identity);
        item.SetSpawnIndexValue(pickupIndexes[tile], this);
        pickupIndexes.RemoveAt(tile);

    }
    public void addBackIndex(int index)
    {
        if (!pickupIndexes.Contains(index))
        {
            pickupIndexes.Add(index);
        }
    }


}