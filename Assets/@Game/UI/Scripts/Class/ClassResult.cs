using System;
using System.Collections;
using System.Collections.Generic;
using StatData.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClassResult : MonoBehaviour
{
    [SerializeField] private ClassResultPanel m_ClassResultPanel;
    [SerializeField] private SelectClass m_ClassData;
    [SerializeField] private GameObject m_ResultClassStudentPrefab;
    [SerializeField] private Sprite[] m_IncreasesArrows;

    private int[] m_Stats = new int[5];
    private int[] m_IncreasesStats = new int[5];
    private string[] m_GameDesignerSkillName = new string[5];
    private string[] m_ArtSkillName = new string[5];
    private string[] m_ProgrammingSkillName = new string[5];

    private void Start()
    {
        InitSkillName();
    }

    // 스킬의 레벨이 올랐을 때 각 파트에 맞는 이름을 띄워주기 위해 초기화해두었다.
    private void InitSkillName()
    {
        m_GameDesignerSkillName[0] = "▶사업력";
        m_GameDesignerSkillName[1] = "▶이해력";
        m_GameDesignerSkillName[2] = "▶창의력";
        m_GameDesignerSkillName[3] = "▶소통력";
        m_GameDesignerSkillName[4] = "▶분석력";

        m_ArtSkillName[0] = "▶공간감";
        m_ArtSkillName[1] = "▶상상력";
        m_ArtSkillName[2] = "▶표현력";
        m_ArtSkillName[3] = "▶테크닉";
        m_ArtSkillName[4] = "▶색감각";

        m_ProgrammingSkillName[0] = "▶광기";
        m_ProgrammingSkillName[1] = "▶구현력";
        m_ProgrammingSkillName[2] = "▶활용력";
        m_ProgrammingSkillName[3] = "▶탐구력";
        m_ProgrammingSkillName[4] = "▶논리력";
    }

    /// 수업이 끝났을 때 미니게임으로 인해 터치카운트가 증가하여 팝업창이 꺼지는 일을 막기위해 하나의 큰 버튼을 만든다.
    public void ClickResultButton()
    {
        m_ClassResultPanel.SetPanel(true);
        SetResultPanel(StudentType.GameDesigner);
    }

    // 처음에는 기획을 만들고 다음 페이지를 보는 버튼을 눌렀을 때 차례대로 아트, 플밍 순서대로 만들어준다.
    private void SetResultPanel(StudentType _studentType)
    {
        if (_studentType == StudentType.GameDesigner)
        {
            m_ClassResultPanel.SetPartName("기획");
        }
        else if (_studentType == StudentType.Art)
        {
            m_ClassResultPanel.SetPartName("아트");
        }
        else
        {
            m_ClassResultPanel.SetPartName("플밍");
        }

        for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
        {
            if (ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType == _studentType)
            {
                for (int j = 0; j < 5; j++)
                {
                    m_Stats[j] = 0;
                    m_IncreasesStats[j] = 0;
                }

                if (_studentType == StudentType.GameDesigner)
                {
                    List<SaveSelectClassInfoData> _classData = new List<SaveSelectClassInfoData>(SelectClass.m_GameDesignerData);
                    MakeStudentPrefab(i, _classData);
                }
                else if (_studentType == StudentType.Art)
                {
                    List<SaveSelectClassInfoData> _classData = new List<SaveSelectClassInfoData>(SelectClass.m_ArtData);
                    MakeStudentPrefab(i, _classData);
                }
                else
                {
                    List<SaveSelectClassInfoData> _classData = new List<SaveSelectClassInfoData>(SelectClass.m_ProgrammingData);
                    MakeStudentPrefab(i, _classData);
                }
            }
        }
    }

    // 학생들의 증가한 스탯 정보를 넣은 프리팹을 만들어준다.
    private void MakeStudentPrefab(int _studentListIndex, List<SaveSelectClassInfoData> _classData)
    {
        GameObject _resultStudent = Instantiate(m_ResultClassStudentPrefab, m_ClassResultPanel.ResultPanelContent);

        for (int i = 0; i < 2; i++)
        {
            ProfessorStat nowProfessorStat = _classData[i].m_SelectProfessorDataSave;

            float magnification = Professor.Instance.m_StatMagnification[nowProfessorStat.m_ProfessorPower];
            float increaseStat = m_ClassData.SetIncreaseClassFeeOrStat(PlayerInfo.Instance.CurrentRank, true);

            float _personalityInsight = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_Personality, 0);
            float _personalityConcentration = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_Personality, 1);
            float _personalitySense = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_Personality, 2);
            float _personalityTechnique = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_Personality, 3);
            float _personalitywit = ObjectManager.Instance.CheckIncreaseStat(ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_Personality, 4);

            double _insight = Math.Round(_classData[i].m_SelectClassDataSave.m_Insight * magnification * _personalityInsight * increaseStat);
            double _concentration = Math.Round(_classData[i].m_SelectClassDataSave.m_Concentration * magnification * _personalityConcentration * increaseStat);
            double _sense = Math.Round(_classData[i].m_SelectClassDataSave.m_Sense * magnification * _personalitySense * increaseStat);
            double _technique = Math.Round(_classData[i].m_SelectClassDataSave.m_Technique * magnification * _personalityTechnique * increaseStat);
            double _wit = Math.Round(_classData[i].m_SelectClassDataSave.m_Wit * magnification * _personalitywit * increaseStat);

            m_IncreasesStats[0] += (int)(_insight);

            m_IncreasesStats[1] += (int)(_concentration);

            m_IncreasesStats[2] += (int)(_sense);

            m_IncreasesStats[3] += (int)(_technique);

            m_IncreasesStats[4] += (int)(_wit);
        }

        m_Stats[0] = ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Insight];
        m_Stats[1] = ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Concentration];
        m_Stats[2] = ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Sense];
        m_Stats[3] = ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Technique];
        m_Stats[4] = ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Wit];

        if (ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_UserSettingName != "")
        {
            _resultStudent.GetComponent<ClassResultStudentPrefab>().m_StudnetName.text = ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_UserSettingName;
        }
        else
        {
            _resultStudent.GetComponent<ClassResultStudentPrefab>().m_StudnetName.text = ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_StudentName;
        }
        _resultStudent.GetComponent<ClassResultStudentPrefab>().m_StudentImgae.sprite = ObjectManager.Instance.m_StudentList[_studentListIndex].StudentProfileImg;

        for (int i = 0; i < 5; i++)
        {
            if (m_Stats[i] <= 0)
            {
                m_Stats[i] = 1;
            }

            _resultStudent.GetComponent<ClassResultStudentPrefab>().m_Stat[i].text = m_Stats[i].ToString();
            _resultStudent.GetComponent<ClassResultStudentPrefab>().m_IncreasesStat[i].text = m_IncreasesStats[i].ToString();

            if (m_IncreasesStats[i] > 0)
            {
                _resultStudent.GetComponent<ClassResultStudentPrefab>().m_IncreasesArrow[i].sprite = m_IncreasesArrows[0];
            }
            else if (m_IncreasesStats[i] == 0)
            {
                _resultStudent.GetComponent<ClassResultStudentPrefab>().m_IncreasesArrow[i].gameObject.SetActive(false);
                _resultStudent.GetComponent<ClassResultStudentPrefab>().m_IncreasesStat[i].gameObject.SetActive(false);
            }
            else
            {
                _resultStudent.GetComponent<ClassResultStudentPrefab>().m_IncreasesArrow[i].sprite = m_IncreasesArrows[2];
            }

            // 레벨이 상승했을 때만 해준다.
            if (ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_AbilitySkills[i] < ObjectManager.Instance.CheckStatSkills(m_Stats[i]))
            {
                _resultStudent.GetComponent<ClassResultStudentPrefab>().m_StatSkills[i].SetActive(true);

                if (ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_StudentType == StudentType.GameDesigner)
                {
                    _resultStudent.GetComponent<ClassResultStudentPrefab>().m_StatSkillsText[i].text = m_GameDesignerSkillName[i] + " 레벨 상승!";
                }
                else if (ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_StudentType == StudentType.Art)
                {
                    _resultStudent.GetComponent<ClassResultStudentPrefab>().m_StatSkillsText[i].text = m_ArtSkillName[i] + " 레벨 상승!";
                }
                else
                {
                    _resultStudent.GetComponent<ClassResultStudentPrefab>().m_StatSkillsText[i].text = m_ProgrammingSkillName[i] + " 레벨 상승!";
                }
            }
        }
    }

    public void ClickPrevOrNextButton()
    {
        GameObject _currentClickObj = EventSystem.current.currentSelectedGameObject;

        // 기획반
        if (m_ClassResultPanel.m_NextButton.activeSelf && !m_ClassResultPanel.m_PrevButton.activeSelf)
        {
            m_ClassResultPanel.m_PrevButton.SetActive(true);
            m_ClassResultPanel.DestroyObject();
            SetResultPanel(StudentType.Art);
        }
        // 아트반
        else if (m_ClassResultPanel.m_NextButton.activeSelf && m_ClassResultPanel.m_PrevButton.activeSelf)
        {
            // 이번 버튼을 누르면 기획으로 만들기
            if (_currentClickObj.name == "PrevButton")
            {
                m_ClassResultPanel.m_PrevButton.SetActive(false);

                m_ClassResultPanel.DestroyObject();
                SetResultPanel(StudentType.GameDesigner);
            }
            // 다음 버튼을 누르면 플밍으로 만들기
            else if (_currentClickObj.name == "NextButton")
            {
                m_ClassResultPanel.m_NextButton.SetActive(false);

                m_ClassResultPanel.DestroyObject();
                SetResultPanel(StudentType.Programming);
            }
        }
        // 플밍반
        else if (!m_ClassResultPanel.m_NextButton.activeSelf && m_ClassResultPanel.m_PrevButton.activeSelf)
        {
            m_ClassResultPanel.m_NextButton.SetActive(true);
            m_ClassResultPanel.DestroyObject();
            SetResultPanel(StudentType.Art);
        }

        m_ClassResultPanel.ContentRect.verticalNormalizedPosition = 1f;
    }
}
