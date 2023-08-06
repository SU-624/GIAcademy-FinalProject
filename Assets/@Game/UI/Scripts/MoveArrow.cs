using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    private float m_MoveAmount;
    private float m_PrevTime;
    private const float m_MaxMove = 20;
    private const float m_MoveSpeed = 3;

    void OnEnable()
    {
        m_MoveAmount = 0;
        StartCoroutine(MoveUp());
    }

    IEnumerator MoveUp()
    {
        while (m_MoveAmount < m_MaxMove)
        {
            m_MoveAmount += m_MoveSpeed;
            gameObject.transform.Translate(0, m_MoveSpeed, 0);

            yield return new WaitForSecondsRealtime(0.01f);
        }

        m_MoveAmount = 0;
        StartCoroutine(MoveDown());
    }

    IEnumerator MoveDown()
    {
        while (m_MoveAmount < m_MaxMove)
        {
            m_MoveAmount += m_MoveSpeed;
            gameObject.transform.Translate(0, -m_MoveSpeed, 0);

            yield return new WaitForSecondsRealtime(0.01f);
        }

        m_MoveAmount = 0;
        StartCoroutine(MoveUp());
    }
}
