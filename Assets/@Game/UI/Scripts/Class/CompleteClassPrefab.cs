using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CompleteClassPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_ClassName;
    [SerializeField] private TextMeshProUGUI m_ProfessorName;
    [SerializeField] private Image m_ProfessorImage;

    [SerializeField] private GameObject[] m_Stat;
    [SerializeField] private GameObject m_AllStat;

    [SerializeField] private TextMeshProUGUI[] m_StatText;
    [SerializeField] private TextMeshProUGUI m_AllStatText;

    [SerializeField] private Image[] m_StatImage;

    [SerializeField] private TextMeshProUGUI m_MoneyText;
    [SerializeField] private TextMeshProUGUI m_HealthText;

    [SerializeField] private Button m_ModifiyButton;

    public TextMeshProUGUI ClassName { get { return m_ClassName; } set { m_ClassName = value; } }
    public TextMeshProUGUI ProfessorName { get { return m_ProfessorName; } set { m_ProfessorName = value; } }
    public Image ProfessorImage { get { return m_ProfessorImage; } set { m_ProfessorImage = value; } }
    public GameObject[] Stat { get { return m_Stat; } set { m_Stat = value; } }
    public GameObject AllStat { get { return m_AllStat; } set { m_AllStat = value; } }
    public TextMeshProUGUI[] StatText { get { return m_StatText; } set { m_StatText = value; } }
    public TextMeshProUGUI AllStatText { get { return m_AllStatText; } set { m_AllStatText = value; } }
    public Image[] StatImage { get { return m_StatImage; } set { m_StatImage = value; } }
    public TextMeshProUGUI MoneyText { get { return m_MoneyText; } set { m_MoneyText = value; } }
    public TextMeshProUGUI HealthText { get { return m_HealthText; } set { m_HealthText = value; } }
    public Button ModifiyButton { get { return m_ModifiyButton; } set { m_ModifiyButton = value; } }
}
