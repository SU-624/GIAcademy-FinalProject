using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInfoPanel : MonoBehaviour
{
    [SerializeField] private Image m_Rank;
    [SerializeField] private Button m_Close;
    [SerializeField] private TextMeshProUGUI m_UserName;
    [SerializeField] private TextMeshProUGUI m_AcademyName;

    [Space(5f)]
    [Header("학원 점수")]
    [SerializeField] private TextMeshProUGUI m_GoodsScore;
    [SerializeField] private TextMeshProUGUI m_FamousScore;
    [SerializeField] private TextMeshProUGUI m_ActivityScore;
    [SerializeField] private TextMeshProUGUI m_ManagementScore;
    [SerializeField] private TextMeshProUGUI m_TalentDevelopmentScore;

    [Space(5f)]
    [Header("학생,교사 수")]
    [SerializeField] private TextMeshProUGUI m_StudentCount;
    [SerializeField] private TextMeshProUGUI m_ProfessorCount;
    [SerializeField] private TextMeshProUGUI m_PlayTime;

    public TextMeshProUGUI PlayTime { get { return m_PlayTime; } set { m_PlayTime = value; } }
    public Image RankImage { get { return m_Rank; } set { RankImage = value; } }

    public void SetAcademyAndUserName(string _userName, string _academyName)
    {
        m_UserName.text = _userName;
        m_AcademyName.text = _academyName;
    }

    public void SetAcademyScore(string _goods, string _famous, string _activity, string _management, string _talentdevelopment)
    {
        m_GoodsScore.text = _goods;
        m_FamousScore.text = _famous;
        m_ActivityScore.text = _activity;
        m_ManagementScore.text = _management;
        m_TalentDevelopmentScore.text = _talentdevelopment;
    }

    public void SetStudentAndProfessorCount(string _studentCoutn, string _professorCount)
    {
        m_StudentCount.text = _studentCoutn;
        m_ProfessorCount.text = _professorCount;
    }
}
