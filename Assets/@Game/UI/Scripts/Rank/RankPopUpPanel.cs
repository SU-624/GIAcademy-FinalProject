using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class RankPopUpPanel : MonoBehaviour
{
    [SerializeField] private PopUpUI m_RankPopUpPanel;
    [SerializeField] private PopOffUI m_RankPopOffPanel;
    [SerializeField] private TextMeshProUGUI m_RankPopUpText;
    [SerializeField] private RankPanel m_RankPanel;

    public void ChangeRankPopUpText(string _text)
    {
        m_RankPopUpText.text = _text;
    }

    public void RankPopUpAndOff(bool _flag)
    {
        if (_flag)
        {
            m_RankPopUpPanel.TurnOnUI();
        }
        else
        {
            m_RankPopOffPanel.TurnOffUI();
        }
    }
}
