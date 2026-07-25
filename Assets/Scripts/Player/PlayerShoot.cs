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

    private bool isOnCooldown;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponentInParent<PlayerHealth>();
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