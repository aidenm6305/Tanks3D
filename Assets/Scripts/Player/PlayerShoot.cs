using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShoot: MonoBehaviour
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform muzzleTransform;

    public AudioSource myTankAudioSource;
    public AudioSource myTankAudioSource2;


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

    [SerializeField]
    private TextMeshProUGUI bulletTypeText;

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
        bulletTypeText.text = newBullet.bulletName;
    }
    private System.Collections.IEnumerator HandleCooldown()
    {
        isOnCooldown = true;
        float timer = fireCooldownTime;
        bool playedReloadSound = false;

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
                if (1f - (timer / fireCooldownTime) >= .5f && !playedReloadSound)
                {
                    playedReloadSound = true;
                    AudioManager.Instance.PlayReload(myTankAudioSource2);
                }
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
        bulletTypeText.text = normalBullet.bulletName;
    }
    public void Shoot(InputAction.CallbackContext context)
    {
        // Prevent shooting if the game hasn't started yet
        if (GameManager.Instance != null && !GameManager.Instance.isGameActive) return;

        if (playerHealth != null && playerHealth.isDead) return;

        if (!context.performed)
            return;

        if (isOnCooldown)
        {
            return;
        }

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