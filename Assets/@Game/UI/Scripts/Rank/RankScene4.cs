using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankScene4 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_1stAcademyName;
    [SerializeField] private TextMeshProUGUI m_2ndAcademyName;
    [SerializeField] private TextMeshProUGUI m_3rdAcademyName;

    [SerializeField] private TextMeshProUGUI m_1stAcademyScore;
    [SerializeField] private TextMeshProUGUI m_2ndAcademyScore;
    [SerializeField] private TextMeshProUGUI m_3rdAcademyScore;

    [SerializeField] private TextMeshProUGUI m_OtherAcademy1Score;
    [SerializeField] private TextMeshProUGUI m_MyAcademyScore;
    [SerializeField] private TextMeshProUGUI m_OtherAcademy2Score;

    [SerializeField] private TextMeshProUGUI m_OtherAcademy1Name;
    [SerializeField] private TextMeshProUGUI m_MyAcademyName;
    [SerializeField] private TextMeshProUGUI m_OtherAcademy2Name;

    [SerializeField] private TextMeshProUGUI m_OtherAcademy1Rank;
    [SerializeField] private TextMeshProUGUI m_MyAcademyRank;
    [SerializeField] private TextMeshProUGUI m_OtherAcademy2Rank;

    [SerializeField] private TextMeshProUGUI m_OtherAcademy1Increase;
    [SerializeField] private TextMeshProUGUI m_MyAcademyIncrease;
    [SerializeField] private TextMeshProUGUI m_OtherAcademy2Increase;

    [SerializeField] private GameObject m_MyAcademy;
    [SerializeField] private Button m_BackButton;

    public GameObject MyAcademy { get { return m_MyAcademy; } set { m_MyAcademy = value; } }
    public Button BackButton { get { return m_BackButton; } set { m_BackButton = value; } }

    public void SetRankGraph(string _1stAcademyName, string _2ndAcademyName, string _3rdAcademyName,
        string _1stAcademyScore, string _2ndAcademyScore, string _3rdAcademyScore)
    {
        m_1stAcademyName.text = _1stAcademyName;
        m_2ndAcademyName.text = _2ndAcademyName;
        m_3rdAcademyName.text = _3rdAcademyName;

        m_1stAcademyScore.text = _1stAcademyScore;
        m_2ndAcademyScore.text = _2ndAcademyScore;
        m_3rdAcademyScore.text = _3rdAcademyScore;
    }

    public void SetOtherAcademy(string _otherAcademy1Name, string _otherAcademy1Score, string _otherAcademy2Name, string _otherAcademy2Score,
        string _otherAcademy1Rank, string _otherAcademy2Rank)
    {
        m_OtherAcademy1Name.text = _otherAcademy1Name;
        m_OtherAcademy2Name.text = _otherAcademy2Name;

        m_OtherAcademy1Score.text = _otherAcademy1Score;
        m_OtherAcademy2Score.text = _otherAcademy2Score;

        m_OtherAcademy1Rank.text = _otherAcademy1Rank;
        m_OtherAcademy2Rank.text = _otherAcademy2Rank;
    }

    public void SetMyAcademy(string _myAcademyName, string _myAcademyScore, string _myAcademyRank)
    {
        m_MyAcademyName.text = _myAcademyName;
        m_MyAcademyScore.text = _myAcademyScore;
        m_MyAcademyRank.text = _myAcademyRank;
    }

    public void IncreaseAndDecrease(string _otherAcademy1, string _myAcademy, string _otherAcademy2)
    {
        m_OtherAcademy1Increase.text = _otherAcademy1;
        m_OtherAcademy2Increase.text = _otherAcademy2;
        m_MyAcademyIncrease.text = _myAcademy;

    }
}
