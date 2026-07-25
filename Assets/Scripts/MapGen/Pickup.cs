using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;

    private Spawner spawner;
    private int spawnIndex;
    public void SetSpawnIndexValue(int index, Spawner spawner)
    {
        spawnIndex = index;
        this.spawner = spawner;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawner.addBackIndex(spawnIndex);
            Destroy(gameObject); 
        }
    }
}