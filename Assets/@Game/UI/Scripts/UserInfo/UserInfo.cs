using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어의 이름과 아카데미 이름, 현재까지 받은 학원 점수들을 확인하는 패널의
/// 데이터를 넣어주는 스크립트
/// 
/// 2023.07.19 Ocean
/// </summary>
public class UserInfo : MonoBehaviour
{
    [SerializeField] private UserInfoPanel m_InfoPanel;
    [SerializeField] private Sprite[] m_Rank;

    public void SetInfoPanel()
    {
        string _userName = PlayerInfo.Instance.PrincipalName;
        string _academyName = PlayerInfo.Instance.AcademyName;

        string _GoodsScore = PlayerInfo.Instance.Goods.ToString();
        string _famousScore = PlayerInfo.Instance.Famous.ToString();
        string _activityScore = PlayerInfo.Instance.Activity.ToString();
        string _ManagmentScore = PlayerInfo.Instance.Management.ToString();
        string _talentDevelopmentScore = PlayerInfo.Instance.TalentDevelopment.ToString();

        int _studentCount = ObjectManager.Instance.m_StudentList.Count;
        int _professorCount = Professor.Instance.ArtProfessor.Count + Professor.Instance.GameManagerProfessor.Count + Professor.Instance.ProgrammingProfessor.Count;

        Rank _rank = PlayerInfo.Instance.CurrentRank;
        Sprite _rankSprite = SetRankSprite(_rank);
        m_InfoPanel.SetAcademyAndUserName(_userName, _academyName);
        m_InfoPanel.SetAcademyScore(_GoodsScore, _famousScore, _activityScore, _ManagmentScore, _talentDevelopmentScore);
        m_InfoPanel.SetStudentAndProfessorCount(_studentCount.ToString(), _professorCount.ToString());

        m_InfoPanel.PlayTime.text = GameTime.Instance.FlowTime.NowYear.ToString() + "년차";
        m_InfoPanel.RankImage.sprite = _rankSprite;
    }

    private Sprite SetRankSprite(Rank _rank)
    {
        switch (_rank)
        {
            case Rank.SSS:
            return m_Rank[(int)Rank.SSS];

            case Rank.SS:
            return m_Rank[(int)Rank.SS];

            case Rank.S:
            return m_Rank[(int)Rank.S];

            case Rank.A:
            return m_Rank[(int)Rank.A];

            case Rank.B:
            return m_Rank[(int)Rank.B];

            case Rank.D:
            return m_Rank[(int)Rank.D];

            case Rank.E:
            return m_Rank[(int)Rank.E];

            case Rank.F:
            return m_Rank[(int)Rank.F];
        }

        return null;
    }
}
