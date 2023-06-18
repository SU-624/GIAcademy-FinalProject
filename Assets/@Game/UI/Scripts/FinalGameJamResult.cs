using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalGameJamResult : MonoBehaviour
{
    [SerializeField] private GameObject m_FinalGameJamResultPanel;
    [SerializeField] private TextMeshProUGUI m_AcademyName;
    [SerializeField] private TextMeshProUGUI m_GameName;
    [SerializeField] private TextMeshProUGUI m_ResultTitle;

    [SerializeField] private TextMeshProUGUI m_CurrnetMoney;
    [SerializeField] private TextMeshProUGUI m_CurrnetSpecialPoints;

    [Space(5f)]
    [Header("원래 학원 점수")]
    [SerializeField] private TextMeshProUGUI m_PreAwareness;
    [SerializeField] private TextMeshProUGUI m_PreManagement;
    [SerializeField] private TextMeshProUGUI m_PrePracticalTalent;

    [Space(5f)]
    [Header("변동이 될 점수")]
    [SerializeField] private TextMeshProUGUI m_ChangeAwareness;
    [SerializeField] private TextMeshProUGUI m_ChangeManagement;
    [SerializeField] private TextMeshProUGUI m_ChangePracticalTalent;
    [SerializeField] private Image m_AwarnessArrow;
    [SerializeField] private Image m_ManagementArrow;
    [SerializeField] private Image m_PracticalTalentArrow;

    [Space(5f)]
    [Header("변동이 된 최종 점수")]
    [SerializeField] private TextMeshProUGUI m_FinalAwareness;
    [SerializeField] private TextMeshProUGUI m_FinalManagement;
    [SerializeField] private TextMeshProUGUI m_FinalPracticalTalent;
    [SerializeField] private Image m_FinalAwarnessArrow;
    [SerializeField] private Image m_FinalManagementArrow;
    [SerializeField] private Image m_FinalPracticalTalentArrow;

    [SerializeField] private TextMeshProUGUI m_RewardMoney;

    [Space(5f)]
    [Header("변동된 점수를 보여줄 슬라이더")]
    [SerializeField] private Slider m_AwarenessSlider;
    [SerializeField] private Slider m_ManagementSlider;
    [SerializeField] private Slider m_PracticalTalentSlider;

    public PopUpUI _turnOnPanel;
    public PopOffUI _turnOffPanel;

    // 게임결과 최종확인 버튼누르면 패널 켜줄 함수
    public void ClickResultButton()
    {
        _turnOnPanel.TurnOnUI();
        SetMoneyAndSpecialPoints();
        //m_FinalGameJamResultPanel.SetActive(true);
    }

    public void SetMoneyAndSpecialPoints()
    {
        m_CurrnetMoney.text = PlayerInfo.Instance.m_MyMoney.ToString();
        m_CurrnetSpecialPoints.text = PlayerInfo.Instance.m_SpecialPoint.ToString();
    }

    public void ClickQuitButton()
    {
        _turnOffPanel.TurnOffUI();
        //m_FinalGameJamResultPanel.SetActive(false);
    }

    public void SetResultPanel(string _academyName, string _gameJamName, string _resultTitle, string _rewardMoney)
    {
        m_AcademyName.text = _academyName;
        m_GameName.text = _gameJamName;
        m_ResultTitle.text = _resultTitle;
        m_RewardMoney.text = _rewardMoney;
    }

    public void SetPreInfo(string _preAwareness, string _prePracticalTalent, string _preManagement)
    {
        m_PreAwareness.text = _preAwareness;
        m_PrePracticalTalent.text = _prePracticalTalent;
        m_PreManagement.text = _preManagement;
    }

    public void SetFinalInfo(string _finalAwareness, string _finalPracticalTalent, string _finalManagement)
    {
        m_FinalAwareness.text = _finalAwareness;
        m_FinalPracticalTalent.text = _finalPracticalTalent;
        m_FinalManagement.text = _finalManagement;
    }

    public void SetFinalInfoArrowImage(Sprite _awarness, Sprite _practicalTalent, Sprite _management)
    {
        m_FinalAwarnessArrow.sprite = _awarness;
        m_FinalPracticalTalentArrow.sprite = _practicalTalent;
        m_FinalManagementArrow.sprite = _management;
    }

    public void SetChangeInfo(string _changeAwareness, string _changePracticalTalent, string _management)
    {
        m_ChangeAwareness.text = _changeAwareness;
        m_ChangePracticalTalent.text = _management;
        m_ChangeManagement.text = _changePracticalTalent;

    }

    public void SetChangeInfoArrowImage(Sprite _awarness, Sprite _practicalTalent, Sprite _management)
    {
        m_AwarnessArrow.sprite = _awarness;
        m_PracticalTalentArrow.sprite = _practicalTalent;
        m_ManagementArrow.sprite = _management;
    }

    public void SetSlider(int _awareness, int _practicalTalent, int _management)
    {
        m_AwarenessSlider.value = _awareness;
        m_PracticalTalentSlider.value = _practicalTalent;
        m_ManagementSlider.value = _management;
    }
}
