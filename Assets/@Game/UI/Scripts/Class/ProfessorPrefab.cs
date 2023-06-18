using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfessorPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_professorName;
    [SerializeField] private Image m_professorPosition;
    [SerializeField] private TextMeshProUGUI m_CurrentPassion;
    [SerializeField] private TextMeshProUGUI m_CurrentHealth;
    [SerializeField] private TextMeshProUGUI m_SkillName;
    [SerializeField] private TextMeshProUGUI m_SkillInfo;
    //[SerializeField] private Image[] m_ProfessorSkillImage;
    //[SerializeField] private TextMeshProUGUI[] m_ProfessorSkillName;
    //[SerializeField] private GameObject[] m_ProfessorSkillUnLockImage;
    [SerializeField] private Image m_ProfessorProfileImg;

    public TextMeshProUGUI professorName { get { return m_professorName; } set { m_professorName = value; } }
    public Image professorPosition { get { return m_professorPosition; } set { m_professorPosition = value; } }
    public TextMeshProUGUI CurrentPassion { get { return m_CurrentPassion; } set { m_CurrentPassion = value; } }
    public TextMeshProUGUI CurrentHealth { get { return m_CurrentHealth; } set { m_CurrentHealth = value; } }
    public TextMeshProUGUI ProfessorSkillName { get { return m_SkillName; } set { m_SkillName = value; } }
    public TextMeshProUGUI ProfessorSkillInfo { get { return m_SkillInfo; } set { m_SkillInfo = value; } }
    //public Image[] ProfessorSkillImage { get { return m_ProfessorSkillImage; } set { m_ProfessorSkillImage = value; } }
    //public TextMeshProUGUI[] ProfessorSkillName { get { return m_ProfessorSkillName; } set { m_ProfessorSkillName = value; } }
    //public GameObject[] ProfessorSkillUnLockImage { get { return m_ProfessorSkillUnLockImage; } set { m_ProfessorSkillUnLockImage = value; } }
    public Image ProfessorProfileImg { get { return m_ProfessorProfileImg; }set { m_ProfessorProfileImg = value; } }
}
