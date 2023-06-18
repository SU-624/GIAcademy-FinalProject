using System.Collections;
using System.Collections.Generic;
using StatData.Runtime;
using UnityEngine;

/// <summary>
/// ������ �� ���� �� ���� ������ ������ ü�¸�ŭ ������ ������ ü���� �����ش�.
/// �Ŀ� �л��� ���ȵ� ���⼭ �÷��ش�.
/// 
/// </summary>
public class ApplyChangeStat : MonoBehaviour
{
    [SerializeField] private SelectClass m_SelectClassData;

    private string[] m_ArtProfessorName = new string[2];
    private string[] m_GMProfessorName = new string[2];
    private string[] m_ProgrammingProfessorName = new string[2];

    private int[] m_ArtClassHealth = new int[2];
    private int[] m_GMClassHealth = new int[2];
    private int[] m_ProgrammingClassHealth = new int[2];

    private Professor m_ProfessorData = new Professor();

    public void ApplyProfessorStat()
    {
        BringStat();

        for (int i = 0; i < 2; i++)
        {
            int _index = m_SelectClassData.m_NowPlayerProfessor.ArtProfessor.IndexOf(SelectClass.m_ArtData[i].m_SelectProfessorDataSave);

            if (m_SelectClassData.m_NowPlayerProfessor.ArtProfessor[_index].m_ProfessorNameValue == m_ArtProfessorName[i])
            {
                m_SelectClassData.m_NowPlayerProfessor.ArtProfessor[_index].m_ProfessorHealth -= m_ArtClassHealth[i];
                //i++;
                //m_SelectClassData.m_NowPlayerProfessor.ArtProfessor[_index].m_ProfessorHealth -= 5;
                m_SelectClassData.m_NowPlayerProfessor.ArtProfessor[_index].m_ProfessorPassion -= 3;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            int _index = m_SelectClassData.m_NowPlayerProfessor.GameManagerProfessor.IndexOf(SelectClass.m_GameDesignerData[i].m_SelectProfessorDataSave);

            if (m_SelectClassData.m_NowPlayerProfessor.GameManagerProfessor[_index].m_ProfessorNameValue == m_GMProfessorName[i])
            {
                m_SelectClassData.m_NowPlayerProfessor.GameManagerProfessor[_index].m_ProfessorHealth -= m_GMClassHealth[i];
                //i++;
                //m_SelectClassData.m_NowPlayerProfessor.GameManagerProfessor[_index].m_ProfessorHealth -= 5;
                m_SelectClassData.m_NowPlayerProfessor.GameManagerProfessor[_index].m_ProfessorPassion -= 3;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            int _index = m_SelectClassData.m_NowPlayerProfessor.ProgrammingProfessor.IndexOf(SelectClass.m_ProgrammingData[i].m_SelectProfessorDataSave);

            if (m_SelectClassData.m_NowPlayerProfessor.ProgrammingProfessor[_index].m_ProfessorNameValue == m_ProgrammingProfessorName[i])
            {
                m_SelectClassData.m_NowPlayerProfessor.ProgrammingProfessor[_index].m_ProfessorHealth -= m_ProgrammingClassHealth[i];
                //i++;
                //m_SelectClassData.m_NowPlayerProfessor.ProgrammingProfessor[_index].m_ProfessorHealth -= 5;
                m_SelectClassData.m_NowPlayerProfessor.ProgrammingProfessor[_index].m_ProfessorPassion -= 3;
            }
        }
    }

    private void BringStat()
    {
        for (int i = 0; i < 2; i++)
        {
            m_ArtProfessorName[i] = SelectClass.m_ArtData[i].m_SelectProfessorDataSave.m_ProfessorNameValue;
            m_ArtClassHealth[i] = SelectClass.m_ArtData[i].m_SelectClassDataSave.m_Health;
        }

        for (int i = 0; i < 2; i++)
        {
            m_GMProfessorName[i] = SelectClass.m_GameDesignerData[i].m_SelectProfessorDataSave.m_ProfessorNameValue;
            m_GMClassHealth[i] = SelectClass.m_GameDesignerData[i].m_SelectClassDataSave.m_Health;
        }

        for (int i = 0; i < 2; i++)
        {
            m_ProgrammingProfessorName[i] = SelectClass.m_ProgrammingData[i].m_SelectProfessorDataSave.m_ProfessorNameValue;
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

                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Insight] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Insight * magnification);
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Concentration] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Concentration * magnification);
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Sense] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Sense * magnification);
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Technique] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Technique * magnification);
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Wit] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Wit * magnification);
                }
            }
            else if (ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType == StudentType.Programming)
            {
                for (int j = 0; j < 2; j++)
                {
                    ProfessorStat nowProfessorStat = SelectClass.m_ProgrammingData[j].m_SelectProfessorDataSave;
                    float magnification = m_ProfessorData.m_StatMagnification[nowProfessorStat.m_ProfessorPower];

                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Insight] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Insight * magnification);
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Concentration] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Concentration * magnification);
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Sense] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Sense * magnification);
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Technique] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Technique * magnification);
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Wit] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Wit * magnification);
                }
            }
            else if (ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType == StudentType.GameDesigner)
            {
                for (int j = 0; j < 2; j++)
                {
                    ProfessorStat nowProfessorStat = SelectClass.m_GameDesignerData[j].m_SelectProfessorDataSave;
                    float magnification = m_ProfessorData.m_StatMagnification[nowProfessorStat.m_ProfessorPower];

                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Insight] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Insight * magnification);
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Concentration] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Concentration * magnification);
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Sense] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Sense * magnification);
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Technique] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Technique * magnification);
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat
                            .m_AbilityAmountList[(int)AbilityType.Wit] +=
                        (int)(SelectClass.m_ArtData[j].m_SelectClassDataSave.m_Wit * magnification);
                }
            }

            CheckcBasicSkillLevel(i);
            ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Health -= 5;
            ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Passion -= 3;
        }
    }

    // ������ ������ �����ϸ� ���� ��ġ�� �ö����� ���� �÷��ֱ�
    private void CheckcBasicSkillLevel(int _studentIndex)
    {
        for (int i = 0; i < 5; i++)
        {
            ObjectManager.Instance.m_StudentList[_studentIndex].m_StudentStat.m_AbilitySkills[i] = ObjectManager.Instance.CheckStatSkills(ObjectManager.Instance.m_StudentList[_studentIndex].m_StudentStat.m_AbilityAmountList[i]);
        }
    }
}
