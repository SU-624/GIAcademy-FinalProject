using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankScene3 : MonoBehaviour
{
    [SerializeField] private GameObject m_Title;
    [SerializeField] private GameObject[] m_ThrophyImage;
    [SerializeField] private GameObject[] m_ThrophyText;
    [SerializeField] private GameObject m_AcademyGrade;
    [SerializeField] private Image m_Throphy;

    [SerializeField] private TextMeshProUGUI m_TitleText;
    [SerializeField] private TextMeshProUGUI m_AcademyName;
    [SerializeField] private TextMeshProUGUI m_AcademyScore;

    public GameObject AcademyGrade { get { return m_AcademyGrade; } set { m_AcademyGrade = value; } }
    public GameObject Title { get { return m_Title; } set { m_Title = value; } }

    public void SetTitleText(string _title)
    {
        m_TitleText.text = _title;
    }

    public void SetThrophyImage(int _index,bool _flag)
    {
        m_ThrophyImage[_index].SetActive(_flag);
        m_ThrophyText[_index].SetActive(_flag);
    }

    public void SetAcademyGrade(string _name, string _score, Sprite _throphy)
    {
        m_Throphy.sprite = _throphy;
        m_AcademyName.text = _name;
        m_AcademyScore.text = _score;
    }
}
