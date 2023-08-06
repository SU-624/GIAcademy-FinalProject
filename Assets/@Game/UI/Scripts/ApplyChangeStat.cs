using System;
using System.Collections;
using System.Collections.Generic;
using StatData.Runtime;
using UnityEngine;

/// <summary>
/// 수업을 다 들은 후 내가 선택한 수업의 체력만큼 선택한 강사의 체력을 내려준다.
/// 후에 학생의 스탯도 여기서 올려준다.
/// 
/// </summary>
public class ApplyChangeStat : MonoBehaviour
{
    private string[] m_ArtProfessorName = new string[2];
    private string[] m_GameDesignerProfessorName = new string[2];
    private string[] m_ProgrammingProfessorName = new string[2];

    private int[] m_ArtClassHealth = new int[2];
    private int[] m_GameDesignerClassHealth = new int[2];
    private int[] m_ProgrammingClassHealth = new int[2];

    [SerializeField] private SelectClass m_ClassData;

    private Professor m_ProfessorData = new Professor();

    public void ApplyProfessorStat()
    {
        BringStat();

        for (int i = 0; i < 2; i++)
        {
            int _index = Professor.Instance.ArtProfessor.IndexOf(SelectClass.m_ArtData[i].m_SelectProfessorDataSave);

            if (Professor.Instance.ArtProfessor[_index].m_ProfessorName == m_ArtProfessorName[i])
            {
                Professor.Instance.ArtProfessor[_index].m_ProfessorHealth -= m_ArtClassHealth[i];
                //Professor.Instance.ArtProfessor[_index].m_ProfessorHealth -= 5;
                Professor.Instance.ArtProfessor[_index].m_ProfessorPassion -= 3;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            int _index = Professor.Instance.GameManagerProfessor.IndexOf(SelectClass.m_GameDesignerData[i].m_SelectProfessorDataSave);

            if (Professor.Instance.GameManagerProfessor[_index].m_ProfessorName == m_GameDesignerProfessorName[i])
            {
                Professor.Instance.GameManagerProfessor[_index].m_ProfessorHealth -= m_GameDesignerClassHealth[i];
                //Professor.Instance.GameManagerProfessor[_index].m_ProfessorHealth -= 5;
                Professor.Instance.GameManagerProfessor[_index].m_ProfessorPassion -= 3;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            int _index = Professor.Instance.ProgrammingProfessor.IndexOf(SelectClass.m_ProgrammingData[i].m_SelectProfessorDataSave);

            if (Professor.Instance.ProgrammingProfessor[_index].m_ProfessorName == m_ProgrammingProfessorName[i])
            {
                Professor.Instance.ProgrammingProfessor[_index].m_ProfessorHealth -= m_ProgrammingClassHealth[i];
                //Professor.Instance.ProgrammingProfessor[_index].m_ProfessorHealth -= 5;
                Professor.Instance.ProgrammingProfessor[_index].m_ProfessorPassion -= 3;
            }
        }
    }

    private void BringStat()
    {
        for (int i = 0; i < 2; i++)
        {
            m_ArtProfessorName[i] = SelectClass.m_ArtData[i].m_SelectProfessorDataSave.m_ProfessorName;
            m_ArtClassHealth[i] = SelectClass.m_ArtData[i].m_SelectClassDataSave.m_Health;
        }

        for (int i = 0; i < 2; i++)
        {
            m_GameDesignerProfessorName[i] = SelectClass.m_GameDesignerData[i].m_SelectProfessorDataSave.m_ProfessorName;
            m_GameDesignerClassHealth[i] = SelectClass.m_GameDesignerData[i].m_SelectClassDataSave.m_Health;
        }

        for (int i = 0; i < 2; i++)
        {
            m_ProgrammingProfessorName[i] = SelectClass.m_ProgrammingData[i].m_SelectProfessorDataSave.m_ProfessorName;
            m_ProgrammingClassHealth[i] = SelectClass.m_ProgrammingData[i].m_SelectClassDataSave.m_Health;
        }
    }

    public void ApplyStudentStat()
    {
        for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
        {
            if (ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType == StudentType.Art)
            {
                for (int j = 0; j < 2; j++)
                {
                    ProfessorStat nowProfessorStat = SelectClass.m_ArtData[j].m_SelectProfessorDataSave;
                    float magnification = m_ProfessorData.m_StatMagnification[nowProfessorStat.m_ProfessorPower];
                    float increaseStat = m_ClassData.SetIncreaseClassFeeOrStat(PlayerInfo.Instance.CurrentRank, true);

                    float _personalityInsight = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 0);
                    float _personalityConcentration = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 1);
                    float _personalitySense = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 2);
                    float _personalityTechnique = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 3);
                    float _personalitywit = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 4);

                    double _insight = Math.Round(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Insight * magnification * _personalityInsight * increaseStat);
                    double _concentration = Math.Round(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Concentration * magnification * _personalityConcentration * increaseStat);
                    double _sense = Math.Round(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Sense * magnification * _personalitySense * increaseStat);
                    double _technique = Math.Round(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Technique * magnification * _personalityTechnique * increaseStat);
                    double _wit = Math.Round(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Wit * magnification * _personalitywit * increaseStat);

                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Insight] += (int)_insight;
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Concentration] += (int)_concentration;
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Sense] += (int)_sense;
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Technique] += (int)_technique;
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Wit] += (int)_wit;
                }
            }
            else if (ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType == StudentType.Programming)
            {
                for (int j = 0; j < 2; j++)
                {
                    ProfessorStat nowProfessorStat = SelectClass.m_ProgrammingData[j].m_SelectProfessorDataSave;
                    float magnification = m_ProfessorData.m_StatMagnification[nowProfessorStat.m_ProfessorPower];

                    float _personalityInsight = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 0);
                    float _personalityConcentration = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 1);
                    float _personalitySense = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 2);
                    float _personalityTechnique = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 3);
                    float _personalitywit = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 4);

                    double _insight = Math.Round(SelectClass.m_ProgrammingData[j].m_SelectClassDataSave.m_Insight * magnification * _personalityInsight);
                    double _concentration = Math.Round(SelectClass.m_ProgrammingData[j].m_SelectClassDataSave.m_Concentration * magnification * _personalityConcentration);
                    double _sense = Math.Round(SelectClass.m_ProgrammingData[j].m_SelectClassDataSave.m_Sense * magnification * _personalitySense);
                    double _technique = Math.Round(SelectClass.m_ProgrammingData[j].m_SelectClassDataSave.m_Technique * magnification * _personalityTechnique);
                    double _wit = Math.Round(SelectClass.m_ProgrammingData[j].m_SelectClassDataSave.m_Wit * magnification * _personalitywit);

                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Insight] += (int)_insight;
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Concentration] += (int)_concentration;
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Sense] += (int)_sense;
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Technique] += (int)_technique;
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Wit] += (int)_wit;
                }
            }
            else if (ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType == StudentType.GameDesigner)
            {
                for (int j = 0; j < 2; j++)
                {
                    ProfessorStat nowProfessorStat = SelectClass.m_GameDesignerData[j].m_SelectProfessorDataSave;
                    float magnification = m_ProfessorData.m_StatMagnification[nowProfessorStat.m_ProfessorPower];

                    float _personalityInsight = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 0);
                    float _personalityConcentration = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 1);
                    float _personalitySense = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 2);
                    float _personalityTechnique = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 3);
                    float _personalitywit = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality, 4);

                    double _insight = Math.Round(SelectClass.m_GameDesignerData[j].m_SelectClassDataSave.m_Insight * magnification * _personalityInsight);
                    double _concentration = Math.Round(SelectClass.m_GameDesignerData[j].m_SelectClassDataSave.m_Concentration * magnification * _personalityConcentration);
                    double _sense = Math.Round(SelectClass.m_GameDesignerData[j].m_SelectClassDataSave.m_Sense * magnification * _personalitySense);
                    double _technique = Math.Round(SelectClass.m_GameDesignerData[j].m_SelectClassDataSave.m_Technique * magnification * _personalityTechnique);
                    double _wit = Math.Round(SelectClass.m_GameDesignerData[j].m_SelectClassDataSave.m_Wit * magnification * _personalitywit);

                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Insight] += (int)_insight;
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Concentration] += (int)_concentration;
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Sense] += (int)_sense;
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Technique] += (int)_technique;
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Wit] += (int)_wit;
                }
            }

            CheckcBasicSkillLevel(i);
            ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Health -= 5;
            ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Passion -= 3;
        }
    }

    // 증가한 스탯을 점검하며 일정 수치가 올라갔으면 레벨 올려주기
    private void CheckcBasicSkillLevel(int _studentIndex)
    {
        for (int i = 0; i < 5; i++)
        {
            ObjectManager.Instance.m_StudentList[_studentIndex].m_StudentStat.m_AbilitySkills[i] = ObjectManager.Instance.CheckStatSkills(ObjectManager.Instance.m_StudentList[_studentIndex].m_StudentStat.m_AbilityAmountArr[i]);
        }
    }
}
