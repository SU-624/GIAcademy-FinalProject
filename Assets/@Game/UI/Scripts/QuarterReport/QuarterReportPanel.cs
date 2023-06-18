using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuarterReportPanel : MonoBehaviour
{
    [SerializeField] private PopUpUI m_PopUpQuarterReportPanel;
    [SerializeField] private PopUpUI m_PopUpQuarterSepcificationPanel;
    [SerializeField] private PopOffUI m_PopOffQuarterReportPanel;
    [SerializeField] private TextMeshProUGUI m_ReportText;

    // 분기보고 팝업이 뜨면 화면 아무대나 터치를 해서 다음 popup으로 넘어가야한다.
    public void ClickPanel()
    {
        m_PopOffQuarterReportPanel.TurnOffUI();
        m_PopUpQuarterSepcificationPanel.TurnOnUI();
    }

    public void PopUpReportPanel()
    {
        m_PopUpQuarterReportPanel.TurnOnUI();
    }

    public void ChangeQuarterName(string _reportContent)
    {
        m_ReportText.text = _reportContent;

    }
}
