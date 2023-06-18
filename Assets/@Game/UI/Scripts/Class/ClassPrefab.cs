using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClassPrefab : MonoBehaviour
{
    [SerializeField] private Image m_PartImage;
    [SerializeField] private TextMeshProUGUI m_ClassName;
    [SerializeField] private GameObject[] m_Stat;
    [SerializeField] private GameObject m_AllStat;
    [SerializeField] private Image[] m_StatImage;
    [SerializeField] private TextMeshProUGUI[] m_StatText;
    [SerializeField] private TextMeshProUGUI m_AllStatText;
    [SerializeField] private TextMeshProUGUI m_MoneyText;
    [SerializeField] private TextMeshProUGUI m_HealthText;

    public Image PartImage { get { return m_PartImage; } set { m_PartImage = value; } }
    public TextMeshProUGUI ClassName { get { return m_ClassName; } set { m_ClassName = value; } }
    public GameObject[] Stat { get { return m_Stat; } set {m_Stat = value; } }
    public GameObject AllStat { get { return m_AllStat; } set { m_AllStat = value; } }
    public Image[] StatImage { get { return m_StatImage; } set { m_StatImage = value; } }
    public TextMeshProUGUI[] StatText { get { return m_StatText; } set { m_StatText = value; } }
    public TextMeshProUGUI AllStatText { get { return m_AllStatText; } set { m_AllStatText = value; } }
    public TextMeshProUGUI MoneyText { get { return m_MoneyText; } set { m_MoneyText = value; } }
    public TextMeshProUGUI HealthText { get { return m_HealthText; } set { m_HealthText = value; } }
}
