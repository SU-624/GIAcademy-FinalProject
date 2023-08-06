using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBall : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_Firework1;
    [SerializeField] private ParticleSystem m_Firework2;

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "GoalTrigger1")
        {
            ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.SportsRoom, 1);
            m_Firework1.Play();
        }
        else if (other.name == "GoalTrigger2")
        {
            ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.SportsRoom, 1);
            m_Firework2.Play();
        }
    }
}
