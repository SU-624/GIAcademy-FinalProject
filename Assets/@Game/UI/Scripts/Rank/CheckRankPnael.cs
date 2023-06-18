using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckRankPnael : MonoBehaviour
{
    [SerializeField] private PopUpUI m_PopUpCheckRankPanel;
    [SerializeField] private PopOffUI m_PopOffCheckRankPanel;
    [SerializeField] private TextMeshProUGUI m_Contents;

    public void SetActiveCheckRankPanel(bool _flag)
    {
        if (_flag)
            m_PopUpCheckRankPanel.TurnOnUI();
        else
            m_PopOffCheckRankPanel.TurnOffUI();
    }

    public void SetContents(string _contents)
    {
        m_Contents.text = _contents;
    }


}
