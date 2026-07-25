using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public void BreakWall()
    {
        Debug.Log($"Breakable wall at {transform.position} has been broken.");
        Destroy(gameObject);
    }

}