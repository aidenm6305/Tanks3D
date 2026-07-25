using UnityEngine;


public class FastBullet : Bullet
{
    public float speed = 50f; 

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}