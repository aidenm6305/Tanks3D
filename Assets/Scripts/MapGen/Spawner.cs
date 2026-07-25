using UnityEngine;
using System.Collections.Generic;
public class Spawner : MonoBehaviour
{
    private List<Vector3> paths;
    private List<int> pickupIndexes = new List<int>();
    private float squareSize;
    List<Bullet> pickupBullets = new List<Bullet>();
    public Spawner(List<Vector3> paths,List<Bullet> pickupBullets, float squareSize)
    {

        this.paths = paths;
        this.pickupBullets = pickupBullets;
        this.squareSize = squareSize;

        for (int i = 0; i < paths.Count; i++)
        {
            pickupIndexes.Add(i);
        }
    }
    public void placePickup(BulletPickup obj, bool animate = false)
    {
        if (pickupIndexes.Count == 0)
        {
            return;
        }
        int tile = Random.Range(0, pickupIndexes.Count);

        Vector3 randomUnitCircle = Random.insideUnitCircle * squareSize * 0.5f;
        Vector3 spawnPosition = paths[pickupIndexes[tile]] + randomUnitCircle;

        spawnPosition.y = obj.transform.position.y; 

        BulletPickup item = Instantiate(obj, spawnPosition, Quaternion.identity);
        item.SetSpawnIndexValue(pickupIndexes[tile], pickupBullets, this);
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