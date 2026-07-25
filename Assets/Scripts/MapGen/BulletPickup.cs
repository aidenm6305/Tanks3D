using UnityEngine;
using System.Collections.Generic;
public class BulletPickup : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    private Spawner spawner;
    private List<Bullet> pickupBullets = new List<Bullet>();
    private int spawnIndex;
    private Bullet bullet; 
    private void SetRandomPickup()
    {
        if (pickupBullets.Count == 0)
        {
            Debug.LogWarning("No pickup objects available.");
        }

        int randomIndex = Random.Range(0, pickupBullets.Count);
        bullet = pickupBullets[randomIndex];
        Color bulletColor = bullet.bulletColor;
        var meshRender = GetComponent<MeshRenderer>();
        var mesh = meshRender.materials[0];
        Material clonedTexture = Instantiate(mesh);
        clonedTexture.color = bulletColor;
        meshRender.material = clonedTexture;
    } 
    

    public void SetSpawnIndexValue(int index, List<Bullet> pickupBullets, Spawner spawner)
    {
        spawnIndex = index;
        this.pickupBullets = pickupBullets;
        this.spawner = spawner;
        SetRandomPickup();
    
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShoot playerShoot = other.GetComponentInParent<PlayerShoot>();
            if (playerShoot != null && bullet != null)
            {
                playerShoot.SetNewBullet(bullet);
            }

            spawner.addBackIndex(spawnIndex);
            Destroy(gameObject); 
        }
    }
}