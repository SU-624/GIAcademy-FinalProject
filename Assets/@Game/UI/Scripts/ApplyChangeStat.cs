using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 수업을 다 들은 후 내가 선택한 수업의 체력만큼 선택한 강사의 체력을 내려준다.
/// 후에 학생의 스탯도 여기서 올려준다.
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

    public void ApplyProfessorStat()
    {
        BringStat();

        for (int i = 0; i < 2;)
        {
            int _index = m_SelectClassData.m_NowPlayerProfessor.ArtProcessor.IndexOf(m_SelectClassData.m_ArtData[i].m_SelectProfessorDataSave);

            if (m_SelectClassData.m_NowPlayerProfessor.ArtProcessor[_index].m_ProfessorNameValue == m_ArtProfessorName[i])
            {
                m_SelectClassData.m_NowPlayerProfessor.ArtProcessor[_index].m_ProfessorHealth -= m_ArtClassHealth[i];
                i++;
            }
        }

        for (int i = 0; i < 2;)
        {
            int _index = m_SelectClassData.m_NowPlayerProfessor.GameManagerProcessor.IndexOf(m_SelectClassData.m_GameDesignerData[i].m_SelectProfessorDataSave);

            if (m_SelectClassData.m_NowPlayerProfessor.GameManagerProcessor[_index].m_ProfessorNameValue == m_GMProfessorName[i])
            {                                          
                m_SelectClassData.m_NowPlayerProfessor.GameManagerProcessor[_index].m_ProfessorHealth -= m_GMClassHealth[i];
                i++;
            }
        }

        for (int i = 0; i <2;)
        {
            int _index = m_SelectClassData.m_NowPlayerProfessor.ProgrammingProfessor.IndexOf(m_SelectClassData.m_ProgrammingData[i].m_SelectProfessorDataSave);

            if (m_SelectClassData.m_NowPlayerProfessor.ProgrammingProfessor[_index].m_ProfessorNameValue == m_ProgrammingProfessorName[i])
            {                                        
                m_SelectClassData.m_NowPlayerProfessor.ProgrammingProfessor[_index].m_ProfessorHealth -= m_ProgrammingClassHealth[i];
                i++;
            }
        }
    }

    private void BringStat()
    {
        for (int i = 0; i < 2; i++)
        {
            m_ArtProfessorName[i] = m_SelectClassData.m_ArtData[i].m_SelectProfessorDataSave.m_ProfessorNameValue;
            m_ArtClassHealth[i] = m_SelectClassData.m_ArtData[i].m_SelectClassDataSave.m_Health;
        }

        for (int i = 0; i < 2; i++)
        {
            m_GMProfessorName[i] = m_SelectClassData.m_GameDesignerData[i].m_SelectProfessorDataSave.m_ProfessorNameValue;
            m_GMClassHealth[i] = m_SelectClassData.m_GameDesignerData[i].m_SelectClassDataSave.m_Health;
        }

        for (int i = 0; i < 2; i++)
        {
            m_ProgrammingProfessorName[i] = m_SelectClassData.m_ProgrammingData[i].m_SelectProfessorDataSave.m_ProfessorNameValue;
            m_ProgrammingClassHealth[i] = m_SelectClassData.m_ProgrammingData[i].m_SelectClassDataSave.m_Health;
        }
    }
}
