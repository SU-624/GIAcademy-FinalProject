using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// 메일 시스템을 테스트할 용의 스크립트
/// 
/// 
/// </summary>
public class MailSystem : MonoBehaviour
{
    public UnityEvent m_SendMail;
    public bool m_IsTurnOnIcon = false;

    void Update()
    {
        if (m_IsTurnOnIcon == true && GameTime.Instance.isChangeWeek == true)
        {
            m_IsTurnOnIcon = false;
        }

        if ((GameTime.Instance.FlowTime.NowWeek == 1 || GameTime.Instance.FlowTime.NowWeek == 2) && GameTime.Instance.FlowTime.NowWeek == 2 && m_IsTurnOnIcon == false)
        {
            SatisfyRequirement();
        }
    }

    private void SatisfyRequirement()
    {
        m_SendMail.Invoke();
        m_IsTurnOnIcon = true;
    }
}
