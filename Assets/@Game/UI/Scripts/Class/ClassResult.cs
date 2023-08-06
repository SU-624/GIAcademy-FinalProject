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

    // ��ų�� ������ �ö��� �� �� ��Ʈ�� �´� �̸��� ����ֱ� ���� �ʱ�ȭ�صξ���.
    private void InitSkillName()
    {
        m_GameDesignerSkillName[0] = "�������";
        m_GameDesignerSkillName[1] = "�����ط�";
        m_GameDesignerSkillName[2] = "��â�Ƿ�";
        m_GameDesignerSkillName[3] = "�������";
        m_GameDesignerSkillName[4] = "���м���";

        m_ArtSkillName[0] = "��������";
        m_ArtSkillName[1] = "������";
        m_ArtSkillName[2] = "��ǥ����";
        m_ArtSkillName[3] = "����ũ��";
        m_ArtSkillName[4] = "��������";

        m_ProgrammingSkillName[0] = "������";
        m_ProgrammingSkillName[1] = "��������";
        m_ProgrammingSkillName[2] = "��Ȱ���";
        m_ProgrammingSkillName[3] = "��Ž����";
        m_ProgrammingSkillName[4] = "������";
    }

    /// ������ ������ �� �̴ϰ������� ���� ��ġī��Ʈ�� �����Ͽ� �˾�â�� ������ ���� �������� �ϳ��� ū ��ư�� �����.
    public void ClickResultButton()
    {
        m_ClassResultPanel.SetPanel(true);
        SetResultPanel(StudentType.GameDesigner);
    }

    // ó������ ��ȹ�� ����� ���� �������� ���� ��ư�� ������ �� ���ʴ�� ��Ʈ, �ù� ������� ������ش�.
    private void SetResultPanel(StudentType _studentType)
    {
        if (_studentType == StudentType.GameDesigner)
        {
            m_ClassResultPanel.SetPartName("��ȹ");
        }
        else if (_studentType == StudentType.Art)
        {
            m_ClassResultPanel.SetPartName("��Ʈ");
        }
        else
        {
            m_ClassResultPanel.SetPartName("�ù�");
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

    // �л����� ������ ���� ������ ���� �������� ������ش�.
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

            // ������ ������� ���� ���ش�.
            if (ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_AbilitySkills[i] < ObjectManager.Instance.CheckStatSkills(m_Stats[i]))
            {
                _resultStudent.GetComponent<ClassResultStudentPrefab>().m_StatSkills[i].SetActive(true);

                if (ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_StudentType == StudentType.GameDesigner)
                {
                    _resultStudent.GetComponent<ClassResultStudentPrefab>().m_StatSkillsText[i].text = m_GameDesignerSkillName[i] + " ���� ���!";
                }
                else if (ObjectManager.Instance.m_StudentList[_studentListIndex].m_StudentStat.m_StudentType == StudentType.Art)
                {
                    _resultStudent.GetComponent<ClassResultStudentPrefab>().m_StatSkillsText[i].text = m_ArtSkillName[i] + " ���� ���!";
                }
                else
                {
                    _resultStudent.GetComponent<ClassResultStudentPrefab>().m_StatSkillsText[i].text = m_ProgrammingSkillName[i] + " ���� ���!";
                }
            }
        }
    }

    public void ClickPrevOrNextButton()
    {
        GameObject _currentClickObj = EventSystem.current.currentSelectedGameObject;

        // ��ȹ��
        if (m_ClassResultPanel.m_NextButton.activeSelf && !m_ClassResultPanel.m_PrevButton.activeSelf)
        {
            m_ClassResultPanel.m_PrevButton.SetActive(true);
            m_ClassResultPanel.DestroyObject();
            SetResultPanel(StudentType.Art);
        }
        // ��Ʈ��
        else if (m_ClassResultPanel.m_NextButton.activeSelf && m_ClassResultPanel.m_PrevButton.activeSelf)
        {
            // �̹� ��ư�� ������ ��ȹ���� �����
            if (_currentClickObj.name == "PrevButton")
            {
                m_ClassResultPanel.m_PrevButton.SetActive(false);

                m_ClassResultPanel.DestroyObject();
                SetResultPanel(StudentType.GameDesigner);
            }
            // ���� ��ư�� ������ �ù����� �����
            else if (_currentClickObj.name == "NextButton")
            {
                m_ClassResultPanel.m_NextButton.SetActive(false);

                m_ClassResultPanel.DestroyObject();
                SetResultPanel(StudentType.Programming);
            }
        }
        // �ùֹ�
        else if (!m_ClassResultPanel.m_NextButton.activeSelf && m_ClassResultPanel.m_PrevButton.activeSelf)
        {
            m_ClassResultPanel.m_NextButton.SetActive(true);
            m_ClassResultPanel.DestroyObject();
            SetResultPanel(StudentType.Art);
        }

        m_ClassResultPanel.ContentRect.verticalNormalizedPosition = 1f;
    }
}
