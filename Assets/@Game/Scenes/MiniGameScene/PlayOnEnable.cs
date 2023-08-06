using UnityEngine;

public class PlayOnEnable : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_Particle;

    private void OnEnable()
    {
        m_Particle.Play(true);
    }
}