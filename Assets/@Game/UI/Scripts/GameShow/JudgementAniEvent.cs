using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementAniEvent : MonoBehaviour
{
    [SerializeField] private GameShow m_GameShowAnim;

    public void ShowFunAssessment()
    {
        m_GameShowAnim.ShowFunAssessment();
    }

    public void ShowFunScore()
    {
        m_GameShowAnim.ShowFunScore();
    }

    public void ShowGraphicAssessment()
    {
        m_GameShowAnim.ShowGraphicAssessment();
    }

    public void ShowGraphicScore()
    {
        m_GameShowAnim.ShowGraphicScore();
    }

    public void ShowPerfectionAssessment()
    {
        m_GameShowAnim.ShowPerfectionAssessment();
    }

    public void ShowPerfectionScore()
    {
        m_GameShowAnim.ShowPerfectionScore();
    }

    public void ShowGenreAssessment()
    {
        m_GameShowAnim.ShowGenreAssessment();
    }

    public void ShowGenreScore()
    {
        m_GameShowAnim.ShowGenreScore();
    }

    public void ShowConceptAssessment()
    {
        m_GameShowAnim.ShowConceptAssessment();
    }

    public void ShowConceptScore()
    {
        m_GameShowAnim.ShowConceptScore();
    }
}
