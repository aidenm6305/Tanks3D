using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot: MonoBehaviour
{
    public void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("Shoot");
    }

}