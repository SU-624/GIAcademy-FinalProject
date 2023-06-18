using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankScene2 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_GoodsScoreText;
    [SerializeField] private TextMeshProUGUI m_FamousScoreText;
    [SerializeField] private TextMeshProUGUI m_ManagementScoreText;
    [SerializeField] private TextMeshProUGUI m_ActivityScoreText;
    [SerializeField] private TextMeshProUGUI m_TalentDevelopmentScoreText;
    [SerializeField] private TextMeshProUGUI m_TotalScoreText;

    [SerializeField] private GameObject[] m_ScoreRankImgae;
    [SerializeField] private GameObject[] m_ScoreTextObj;
    [SerializeField] private GameObject[] m_RankImage;

    public GameObject[] ScoreRankImgae { get { return m_ScoreRankImgae; } set { m_ScoreRankImgae = value; } }
    public GameObject[] ScoreTextObj { get { return m_ScoreTextObj; } set { m_ScoreTextObj = value; } }
    public GameObject[] RankImage { get { return m_RankImage; } set { m_RankImage = value; } }

    public void SetScoreText(string _goodsScore, string _famousScore, string _managementScore, string _activityScore, string _talentDevelopmentScore, string _totalScore)
    {
        m_GoodsScoreText.text = _goodsScore;
        m_FamousScoreText.text = _famousScore;
        m_ManagementScoreText.text = _managementScore;
        m_ActivityScoreText.text = _activityScore;
        m_TalentDevelopmentScoreText.text = _talentDevelopmentScore;
        m_TotalScoreText.text = _totalScore;
    }

    public void SetScoreImage(Sprite _goods, Sprite _famous, Sprite _management, Sprite _activity, Sprite _talentDevelopment)
    {
        m_ScoreRankImgae[0].GetComponent<Image>().sprite = _goods;
        m_ScoreRankImgae[1].GetComponent<Image>().sprite = _famous;
        m_ScoreRankImgae[2].GetComponent<Image>().sprite = _management;
        m_ScoreRankImgae[3].GetComponent<Image>().sprite = _activity;
        m_ScoreRankImgae[4].GetComponent<Image>().sprite = _talentDevelopment;
    }
}
