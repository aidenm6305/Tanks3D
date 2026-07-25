using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    ParticleSystem breakParticleSystem;
    public void Start()
    {
        breakParticleSystem = GetComponent<ParticleSystem>();
    }
    public void BreakWall()
    {
        Debug.Log($"Breakable wall at {transform.position} has been broken.");
        breakParticleSystem.Play();
        Destroy(gameObject, breakParticleSystem.main.duration);
    }

}