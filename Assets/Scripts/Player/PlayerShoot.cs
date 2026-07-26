using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShoot: MonoBehaviour
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform muzzleTransform;

    public AudioSource myTankAudioSource;

    [Header("Cooldown Settings")]
    [SerializeField] private float fireCooldownTime = 1f;
    [SerializeField] private Image cooldownImage;

    [Header("Fancy Bullet Settings")]
    [SerializeField] private float bulletResetTime = 5f;
    [SerializeField] private Image bulletResetImage;
    private bool isOnCooldown;
    private bool isOnFancyBullet;
    private float fancyBulletTimer;
    private PlayerHealth playerHealth;
    private Bullet normalBullet; 

    private void Awake()
    {
        playerHealth = GetComponentInParent<PlayerHealth>();
        normalBullet = bullet;
    }

    public void SetNewBullet(Bullet newBullet)
    {
        fancyBulletTimer = bulletResetTime;
        StartCoroutine(HandleBulletReset());
        bullet = newBullet;
    }
    private System.Collections.IEnumerator HandleCooldown()
    {
        isOnCooldown = true;
        float timer = fireCooldownTime;

        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 0f;
        }

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (cooldownImage != null)
            {
                cooldownImage.fillAmount = 1f - (timer / fireCooldownTime);
            }

            yield return null;
        }

        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 1f;
        }

        isOnCooldown = false;
    }
    private System.Collections.IEnumerator HandleBulletReset()
    {

        if (bulletResetImage != null)
        {
            bulletResetImage.fillAmount = 0f;
        }

        while (fancyBulletTimer > 0f)
        {
            fancyBulletTimer -= Time.deltaTime;

            if (bulletResetImage != null)
            {
                bulletResetImage.fillAmount = 1f - (fancyBulletTimer / bulletResetTime);
            }

            yield return null;
        }

        if (bulletResetImage != null)
        {
            bulletResetImage.fillAmount = 1f;
        }

        bullet = normalBullet;

    }
    public void Shoot(InputAction.CallbackContext context)
    {
        if (playerHealth != null && playerHealth.isDead) return;

        if (!context.performed)
            return;

        if (isOnCooldown)
            return;

        StartCoroutine(HandleCooldown());

        var tempBullet = Instantiate(
                    bullet,
                    muzzleTransform.position,
                    muzzleTransform.rotation
                    );
        tempBullet.SetPlayerWhoShot(gameObject);
        AudioManager.Instance.PlayShoot(myTankAudioSource);
    }
}