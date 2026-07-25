using UnityEngine;
using UnityEngine.InputSystem;
    
public class PlayerShoot: MonoBehaviour
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform muzzleTransform;

    public AudioSource myTankAudioSource;
    public void SetBullet(Bullet newBullet)
    {
        bullet = newBullet;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        var tempBullet = Instantiate(
                    bullet,
                    muzzleTransform.position,
                    muzzleTransform.rotation
                    );
        tempBullet.SetPlayerWhoShot(gameObject);
        AudioManager.Instance.PlayShoot(myTankAudioSource);
    }
}