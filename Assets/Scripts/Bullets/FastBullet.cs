using UnityEngine;


public class FastBullet : Bullet
{
    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}