using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusSkillPrefab : MonoBehaviour
{
    [SerializeField] private Image StudentBonusSkillPrefab;
    [SerializeField] private TextMeshProUGUI StudentBonusSkillNameText;

    public Image GetStudentBonusSkillPrefab
    {
        get { return StudentBonusSkillPrefab; }
        set { StudentBonusSkillPrefab = value; }
    }

    public TextMeshProUGUI GetStudentBonusSkillNameText
    {
        get { return StudentBonusSkillNameText; }
        set { StudentBonusSkillNameText = value; }
    }
}