using System.Collections;
using System.Collections.Generic;
using StatData.Runtime;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;
using BehaviorDesigner.Runtime.Tasks.Unity.Timeline;
using Coffee.UIExtensions;

public struct SaveSelectClassInfoData
{
    public Class m_SelectClassDataSave;
    public ProfessorStat m_SelectProfessorDataSave;
    public string m_Week;
}

// ������ ���� �� �̹��� ��ü�� ���� enumŬ����
enum PartName
{
    GM,
    Art,
    Programming,
    Commonm,
    Special
}

/// <summary>
/// ���� �ٲ� ���� ���� �˾�â�� ���� �������� ���� ���� ��...
/// ������ ������ ������ �� �ְ� ���� ��(UI���� �������� ���� ���߿� �� �۾��� ���� ����,,!)
/// 
/// 23.02.13 Ocean / 23.04.20 Ocean
/// </summary>
public class SelectClass : MonoBehaviour
{
    [SerializeField] private GameObject m_ClassPrefab;
    [SerializeField] private GameObject m_ProfessorPrefab;
    [SerializeField] private GameObject m_CheckClassPrefab;

    [SerializeField] private Button m_OpenClassButton;

    [SerializeField] private ClassController m_SelectClass; // ���ϴ� ������ ���γ����� �����ֱ� ����
    [SerializeField] private ProfessorController m_SelectProfessor;

    [SerializeField] private TextMeshProUGUI m_InGameUICurrentMoney;

    [SerializeField] private Sprite[] m_ClassPartImage;
    [SerializeField] private Sprite[] m_StatImage;
    [SerializeField] private Sprite[] m_ClassPanelPartImage;
    [SerializeField] private Sprite[] m_WeekButtonSprite;
    [SerializeField] private Sprite m_FullTimeProfessor;
    [SerializeField] private Sprite m_AdjunctProfessor;

    [SerializeField] private GameObject m_TutorialPanel;
    [SerializeField] private Unmask m_Unmask;
    [SerializeField] private Image m_TutorialTextImage;
    [SerializeField] private Image m_TutorialArrowImage;
    [SerializeField] private TextMeshProUGUI m_TutorialText;
    [SerializeField] private GameObject m_BlackScreen;
    [SerializeField] private GameObject m_PDAlarm;
    [SerializeField] private TextMeshProUGUI m_AlarmText;


    public SetClassPanel m_SetClassPanel;
    public CheckSelecteClass m_CheckClassPanel;

    public List<string> m_SelectClassButtonName = new List<string>(); // ���� Ŭ���� ��ư�� ���������� ������ ������ �־��ֱ� ���� ���� ����Ʈ

    public SaveSelectClassInfoData m_SaveData = new SaveSelectClassInfoData();
    public SaveSelectClassInfoData m_TempSaveData = new SaveSelectClassInfoData();
    public static List<SaveSelectClassInfoData> m_GameDesignerData = new List<SaveSelectClassInfoData>();
    public static List<SaveSelectClassInfoData> m_ArtData = new List<SaveSelectClassInfoData>();
    public static List<SaveSelectClassInfoData> m_ProgrammingData = new List<SaveSelectClassInfoData>();

    public EachClass m_NowClass = new EachClass(); // ���� ���� ������ �ִ� ������
    public Professor m_NowPlayerProfessor = new Professor(); // ���� ���� ����ϰ� �ִ� �����

    private ProfessorStat _ClickProfessorData;
    private Class _ClickClassData;
    private GameObject _prevClass;
    private GameObject _prevProfessor;
    private Color m_HighLightColor;

    public int m_WeekClassIndex = 0; // 2�� �Ǹ� 1,2���� ���� ��� �����ѰŴ� �Ϸ�â���� �Ѱ��ֱ�
    public int m_WeekProfessorIndex = 0; // 2�� �Ǹ� 1,2���� ���� ��� �����ѰŴ� �Ϸ�â���� �Ѱ��ֱ�
    public int m_SaveDataIndex = 0; // ������ �����͵��� �ε����� �ٲ��ֱ� ���� ����. 0�� 1�� ����Ѵ�.
    public int m_TotalMoney = 0; // SelectPanel���� ���� ������ �������� �� �����Ḧ ������ֱ� ���� ����
    public int m_TotalHealth = 0;
    public float magnification;

    private int m_ClassStack = 0; // 0~2������ 1���� ��ȹ,��Ʈ, �ù� 3~5������ 2���� ��ȹ,��Ʈ,�ù� ����, ���� ����
    private int[] m_StatNum = new int[5];
    private string[] m_StatName = new string[5];
    private int[] m_AllInfoStatNum = new int[5];
    private string[] m_AllInfoStatName = new string[5];
    private string m_Week; // ���� ������ ������ �� ���� ���� ������ �� ���� �������� ����ִ� ����

    private string
        m_ModifyProfessorString; // ������ �� ������ �� ���� ��ư�� ������ �ش� �а��� ������ �����Ϸ��µ� ������ ���� �� �� �ϳ��� ������ �� ������ ������ ������ ���ܵα� ���� ����ó�� ����

    private string
        m_ModifyClassString; // ������ �� ������ �� ���� ��ư�� ������ �ش� �а��� ������ �����Ϸ��µ� ������ ���� �� �� �ϳ��� ������ �� ������ ������ ������ ���ܵα� ���� ����ó�� ����

    private int m_CurrentMoney = 0; // SelectPanel���� ���� ���� �����ϰ� �ִ� ��ȭ�� ����ֱ� ���� ����
    private int[] m_ClassMoney = new int[6]; // Ŭ���� �������� �����Ḧ �迭�� �����Ͽ� �迭�� ��ҵ��� ��� �����ִ� ����
    private int[] m_ClassHealth = new int[6];
    private bool _isOpenyearTrue = false;
    private bool _isOpenMonthTrue = false;
    private bool m_IsChangeWeekend = false; // 1,2 ���� ���������� ��� �ϰ� �ϷḦ ���� �� �ٽ� false�� �ٲ��ֱ�

    private int m_TutorialCount;
    private int m_ScriptCount;

    #region _����ü ������ ����Ʈ �ε����� �ٲٱ� ���� �Լ�

    public void ChangeListIndex(List<SaveSelectClassInfoData> _tempList, int _index, SaveSelectClassInfoData _saveData)
    {
        SaveSelectClassInfoData _temp = _tempList[_index];
        _temp = _saveData;
        _tempList[_index] = _temp;
    }

    #endregion

    private void Update()
    {
        m_CurrentMoney = int.Parse(m_InGameUICurrentMoney.text);

        if (m_SetClassPanel.CurrentMoney != null)
        {
            m_SetClassPanel.CurrentMoney.text = m_CurrentMoney.ToString();
            m_CheckClassPanel.SetCurrentMoney(m_CurrentMoney.ToString());
        }

        if (PlayerInfo.Instance.IsFirstAcademySetting && PlayerInfo.Instance.IsFirstClassSetting &&
            PlayerInfo.Instance.IsFirstClassSettingPDEnd)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (m_TutorialCount == 0)
                {
                    m_TutorialPanel.SetActive(true);
                    m_PDAlarm.SetActive(false);
                    m_BlackScreen.SetActive(true);
                    m_Unmask.gameObject.SetActive(true);
                    m_Unmask.fitTarget = m_SetClassPanel.PartWeek.GetComponent<RectTransform>();
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(400, 0, 0);
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 1)
                {
                    m_Unmask.fitTarget = m_SetClassPanel.CurrentMoney.gameObject.GetComponent<RectTransform>();
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-600, 0, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 2)
                {
                    m_SetClassPanel.ProfessorScrollView.vertical = false;
                    m_SetClassPanel.ProfessorPrefabParent.GetChild(0).GetComponent<Button>().interactable = false;
                    m_SetClassPanel.ProfessorPrefabParent.GetChild(1).GetComponent<Button>().interactable = false;
                    m_Unmask.fitTarget = m_SetClassPanel.ProfessorScrollView.GetComponent<RectTransform>();
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(500, 0, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 3)
                {
                    m_Unmask.fitTarget =
                        m_SetClassPanel.ProfessorPrefabParent.GetChild(0).GetComponent<RectTransform>();
                    m_SetClassPanel.ProfessorPrefabParent.GetChild(0).GetComponent<Button>().interactable = true;
                    m_SetClassPanel.ProfessorPrefabParent.GetChild(1).GetComponent<Button>().interactable = true;
                    m_TutorialArrowImage.gameObject.SetActive(true);
                    m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
                    m_TutorialTextImage.gameObject.SetActive(false);
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 5)
                {
                    m_SetClassPanel.ClassScrollView.vertical = false;
                    m_Unmask.fitTarget = m_SetClassPanel.ClassScrollView.GetComponent<RectTransform>();
                    m_SetClassPanel.ClassPrefabParent.GetChild(0).GetComponent<Button>().interactable = false;
                    m_SetClassPanel.ClassPrefabParent.GetChild(1).GetComponent<Button>().interactable = false;
                    m_SetClassPanel.ClassPrefabParent.GetChild(2).GetComponent<Button>().interactable = false;
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-700, 0, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 6)
                {
                    m_Unmask.fitTarget = m_SetClassPanel.ClassPrefabParent.GetChild(0).GetComponent<RectTransform>();
                    m_SetClassPanel.ClassPrefabParent.GetChild(0).GetComponent<Button>().interactable = true;
                    m_SetClassPanel.ClassPrefabParent.GetChild(1).GetComponent<Button>().interactable = true;
                    m_SetClassPanel.ClassPrefabParent.GetChild(2).GetComponent<Button>().interactable = true;
                    m_TutorialArrowImage.gameObject.SetActive(true);
                    m_TutorialTextImage.gameObject.SetActive(false);
                    m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 150, 0);
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 8)
                {
                    m_Unmask.fitTarget = m_SetClassPanel.CompleteButton.gameObject.GetComponent<RectTransform>();
                    m_SetClassPanel.CompleteButton.enabled = true;
                    m_TutorialArrowImage.gameObject.SetActive(true);
                    m_TutorialTextImage.gameObject.SetActive(false);
                    m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-200, 150, 0);
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 10)
                {
                    m_TutorialPanel.SetActive(false);
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 13)
                {
                    m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 14)
                {
                    m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 15)
                {
                    m_Unmask.gameObject.SetActive(false);
                    m_BlackScreen.gameObject.SetActive(false);
                    m_TutorialTextImage.gameObject.SetActive(false);
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialPanel.SetActive(false);
                    m_TutorialCount++;
                    PlayerInfo.Instance.IsFirstClassSetting = false;
                    Time.timeScale = 1;
                }
            }
#elif UNITY_ANDROID
            if (Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject())
            {
                Touch touch = Input.GetTouch(0); 
                if (touch.phase == TouchPhase.Ended)
                {
                    if (m_TutorialCount == 0)
                    {
                        m_TutorialPanel.SetActive(true);
                        m_PDAlarm.SetActive(false);
                        m_BlackScreen.SetActive(true);
                        m_Unmask.gameObject.SetActive(true);
                        m_Unmask.fitTarget = m_SetClassPanel.PartWeek.GetComponent<RectTransform>();
                        m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(400, 0, 0);
                        m_TutorialTextImage.gameObject.SetActive(true);
                        m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                        m_ScriptCount++;
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 1)
                    {
                        m_Unmask.fitTarget = m_SetClassPanel.CurrentMoney.gameObject.GetComponent<RectTransform>();
                        m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                        m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-600, 0, 0);
                        m_ScriptCount++;
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 2)
                    {
                        m_SetClassPanel.ProfessorScrollView.vertical = false;
                        m_SetClassPanel.ProfessorPrefabParent.GetChild(0).GetComponent<Button>().interactable = false;
                        m_SetClassPanel.ProfessorPrefabParent.GetChild(1).GetComponent<Button>().interactable = false;
                        m_Unmask.fitTarget = m_SetClassPanel.ProfessorScrollView.GetComponent<RectTransform>();
                        m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                        m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(500, 0, 0);
                        m_ScriptCount++;
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 3)
                    {
                        m_Unmask.fitTarget =
 m_SetClassPanel.ProfessorPrefabParent.GetChild(0).GetComponent<RectTransform>();
                        m_SetClassPanel.ProfessorPrefabParent.GetChild(0).GetComponent<Button>().interactable = true;
                        m_SetClassPanel.ProfessorPrefabParent.GetChild(1).GetComponent<Button>().interactable = true;
                        m_TutorialArrowImage.gameObject.SetActive(true);
                        m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
                        m_TutorialTextImage.gameObject.SetActive(false);
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 5)
                    {
                        m_SetClassPanel.ClassScrollView.vertical = false;
                        m_Unmask.fitTarget = m_SetClassPanel.ClassScrollView.GetComponent<RectTransform>();
                        m_SetClassPanel.ClassPrefabParent.GetChild(0).GetComponent<Button>().interactable = false;
                        m_SetClassPanel.ClassPrefabParent.GetChild(1).GetComponent<Button>().interactable = false;
                        m_SetClassPanel.ClassPrefabParent.GetChild(2).GetComponent<Button>().interactable = false;
                        m_TutorialArrowImage.gameObject.SetActive(false);
                        m_TutorialTextImage.gameObject.SetActive(true);
                        m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                        m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-700, 0, 0);
                        m_ScriptCount++;
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 6)
                    {
                        m_Unmask.fitTarget =
 m_SetClassPanel.ClassPrefabParent.GetChild(0).GetComponent<RectTransform>();
                        m_SetClassPanel.ClassPrefabParent.GetChild(0).GetComponent<Button>().interactable = true;
                        m_SetClassPanel.ClassPrefabParent.GetChild(1).GetComponent<Button>().interactable = true;
                        m_SetClassPanel.ClassPrefabParent.GetChild(2).GetComponent<Button>().interactable = true;
                        m_TutorialArrowImage.gameObject.SetActive(true);
                        m_TutorialTextImage.gameObject.SetActive(false);
                        m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 150, 0);
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 8)
                    {
                        m_Unmask.fitTarget = m_SetClassPanel.CompleteButton.gameObject.GetComponent<RectTransform>();
                        m_SetClassPanel.CompleteButton.enabled = true;
                        m_TutorialArrowImage.gameObject.SetActive(true);
                        m_TutorialTextImage.gameObject.SetActive(false);
                        m_TutorialArrowImage.transform.position =
 m_Unmask.fitTarget.position + new Vector3(-200, 150, 0);
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 10)
                    {
                        m_TutorialPanel.SetActive(false);
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 13)
                    {
                        m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                        m_ScriptCount++;
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 14)
                    {
                        m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                        m_ScriptCount++;
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 15)
                    {
                        m_Unmask.gameObject.SetActive(false);
                        m_BlackScreen.gameObject.SetActive(false);
                        m_TutorialTextImage.gameObject.SetActive(false);
                        m_TutorialArrowImage.gameObject.SetActive(false);
                        m_TutorialPanel.SetActive(false);
                        m_TutorialCount++;
                        PlayerInfo.Instance.IsFirstClassSetting = false;
                        Time.timeScale = 1;
                    }
                }
            }

#endif
        }
    }

    private void Start()
    {
        m_TutorialCount = 0;
        m_ScriptCount = 4;
        m_GameDesignerData.Capacity = 2;
        m_ArtData.Capacity = 2;
        m_ProgrammingData.Capacity = 2;
        m_HighLightColor = Color.gray;
        InitClass();
        InitProfessor();
        //InitData();
        m_SetClassPanel.TotalMoney(Color.white, "0");
        m_SetClassPanel.TotalHealth("0");
        ObjectManager.Instance.LinkInstructorDataToObject(m_NowPlayerProfessor); // �����͸� �ִ´�
    }

    // ���� ���� �г��� ������ ���� ����� �������� �� ����Ʈ�� �־��ش�.
    private void InitClass()
    {
        for (int i = 0; i < m_SelectClass.classData.Count; i++)
        {
            if (m_SelectClass.classData.ElementAt(i).Value.m_ClassType == StudentType.Art &&
                m_SelectClass.classData.ElementAt(i).Value.m_UnLockClass)
            {
                m_NowClass.ArtClass.Add(m_SelectClass.classData.ElementAt(i).Value);
            }

            if (m_SelectClass.classData.ElementAt(i).Value.m_ClassType == StudentType.GameDesigner &&
                m_SelectClass.classData.ElementAt(i).Value.m_UnLockClass)
            {
                m_NowClass.GameDesignerClass.Add(m_SelectClass.classData.ElementAt(i).Value);
            }

            if (m_SelectClass.classData.ElementAt(i).Value.m_ClassType == StudentType.Programming &&
                m_SelectClass.classData.ElementAt(i).Value.m_UnLockClass)
            {
                m_NowClass.ProgrammingClass.Add(m_SelectClass.classData.ElementAt(i).Value);
            }

            if (m_SelectClass.classData.ElementAt(i).Value.m_ClassType == StudentType.None &&
                m_SelectClass.classData.ElementAt(i).Value.m_UnLockClass)
            {
                m_NowClass.NoneClass.Add(m_SelectClass.classData.ElementAt(i).Value);
            }
        }
    }

    // ���� ��밡���� ���縦 ã�Ƽ� ����Ʈ�� �־��ִ� �Լ� -> �̺�Ʈ �ر� �ڵ忡�� ����
    public void InitProfessor()
    {
        for (int i = 0; i < m_SelectProfessor.professorData.Count; i++)
        {
            if (m_SelectProfessor.professorData.ElementAt(i).Value.m_ProfessorType == StudentType.Art &&
                m_SelectProfessor.professorData.ElementAt(i).Value.IsUnLockProfessor == true)
            {
                if (!m_NowPlayerProfessor.ArtProfessor.Contains(m_SelectProfessor.professorData.ElementAt(i).Value))
                {
                    m_NowPlayerProfessor.ArtProfessor.Add(m_SelectProfessor.professorData.ElementAt(i).Value);
                }
            }

            if (m_SelectProfessor.professorData.ElementAt(i).Value.m_ProfessorType == StudentType.GameDesigner &&
                m_SelectProfessor.professorData.ElementAt(i).Value.IsUnLockProfessor == true)
            {
                if (!m_NowPlayerProfessor.GameManagerProfessor.Contains(m_SelectProfessor.professorData.ElementAt(i)
                        .Value))
                {
                    m_NowPlayerProfessor.GameManagerProfessor.Add(m_SelectProfessor.professorData.ElementAt(i).Value);
                }
            }

            if (m_SelectProfessor.professorData.ElementAt(i).Value.m_ProfessorType == StudentType.Programming &&
                m_SelectProfessor.professorData.ElementAt(i).Value.IsUnLockProfessor == true)
            {
                if (!m_NowPlayerProfessor.ProgrammingProfessor.Contains(m_SelectProfessor.professorData.ElementAt(i)
                        .Value))
                {
                    m_NowPlayerProfessor.ProgrammingProfessor.Add(m_SelectProfessor.professorData.ElementAt(i).Value);
                }
            }

            // ���⿡ ��� ������ ����
            // m_NowPlayerProfessor.AllProfessor.Add(m_SelectProfessor.professorData.ElementAt(i).Value);
        }
    }

    public void InitData()
    {
        m_GameDesignerData.Clear();
        m_ArtData.Clear();
        m_ProgrammingData.Clear();
        m_InGameUICurrentMoney.text = (int.Parse(m_InGameUICurrentMoney.text) - m_TotalMoney).ToString();

        for (int i = 0; i < 2; i++)
        {
            m_GameDesignerData.Add(new SaveSelectClassInfoData());
            m_ArtData.Add(new SaveSelectClassInfoData());
            m_ProgrammingData.Add(new SaveSelectClassInfoData());
        }
    }

    private Sprite SetStatImage(string _statName)
    {
        Sprite _stat = null;

        switch (_statName)
        {
            case "����":
            {
                _stat = m_StatImage[(int)AbilityType.Insight];
            }
                break;

            case "����":
            {
                _stat = m_StatImage[(int)AbilityType.Concentration];
            }
                break;

            case "����":
            {
                _stat = m_StatImage[(int)AbilityType.Sense];
            }
                break;

            case "���":
            {
                _stat = m_StatImage[(int)AbilityType.Technique];
            }
                break;

            case "��ġ":
            {
                _stat = m_StatImage[(int)AbilityType.Wit];
            }
                break;
        }

        return _stat;
    }

    // ������ �� �������� ���� ���� �������� ���ȵ��� �������ִ� �Լ�
    private void SetClassStats(int _statCount, GameObject _classPrefab)
    {
        if (_statCount == 5)
        {
            _classPrefab.GetComponent<ClassPrefab>().AllStat.SetActive(true);
            _classPrefab.GetComponent<ClassPrefab>().AllStatText.text = m_StatNum[0].ToString();
        }
        else
        {
            for (int i = 0; i < _statCount; i++)
            {
                _classPrefab.GetComponent<ClassPrefab>().Stat[i].SetActive(true);
                _classPrefab.GetComponent<ClassPrefab>().StatText[i].text = m_StatNum[i].ToString();
                Sprite _statSprite = SetStatImage(m_StatName[i]);
                _classPrefab.GetComponent<ClassPrefab>().StatImage[i].sprite = _statSprite;
            }
        }
    }

    // ������ �� �������� ���� ������ �Ϸ�� ���� �������� ���ȵ��� �������ִ� �Լ�
    private void SetComplelteClassStats(int _statCount, GameObject _completePrefab)
    {
        if (_statCount == 5)
        {
            _completePrefab.GetComponent<CompleteClassPrefab>().AllStat.SetActive(true);
            _completePrefab.GetComponent<CompleteClassPrefab>().AllStatText.text = m_StatNum[0].ToString();
        }
        else
        {
            for (int i = 0; i < _statCount; i++)
            {
                _completePrefab.GetComponent<CompleteClassPrefab>().Stat[i].SetActive(true);
                _completePrefab.GetComponent<CompleteClassPrefab>().StatText[i].text = m_StatNum[i].ToString();
                Sprite _statSprite = SetStatImage(m_StatName[i]);
                _completePrefab.GetComponent<CompleteClassPrefab>().StatImage[i].sprite = _statSprite;
            }
        }
    }

    // ������ ���� ������ �Ǻ��ϴ� �Լ�
    private void DiscriminateClassOpenDate(List<Class> _class, int _index)
    {
        _isOpenyearTrue = false;
        _isOpenMonthTrue = false;

        if (_class[_index].m_OpenYear.Contains(",") && _class[_index].m_OpenMonth.Contains(","))
        {
            string[] _openYear = _class[_index].m_OpenYear.Split(",");
            string[] _openMonth = _class[_index].m_OpenMonth.Split(",");

            for (int x = 0; x < _openYear.Length; x++)
            {
                if (_openYear[x] == GameTime.Instance.FlowTime.NowYear.ToString())
                {
                    _isOpenyearTrue = true;
                    break;
                }
            }

            for (int x = 0; x < _openMonth.Length; x++)
            {
                if (_openMonth[x] == GameTime.Instance.FlowTime.NowMonth.ToString())
                {
                    _isOpenMonthTrue = true;
                    break;
                }
            }
        }
        else if (_class[_index].m_OpenYear.Contains(",") && !_class[_index].m_OpenMonth.Contains(","))
        {
            string[] _openYear = _class[_index].m_OpenYear.Split(",");

            for (int x = 0; x < _openYear.Length; x++)
            {
                if (_openYear[x] == GameTime.Instance.FlowTime.NowYear.ToString())
                {
                    _isOpenyearTrue = true;
                    break;
                }
            }

            if (_class[_index].m_OpenMonth == "0" ||
                _class[_index].m_OpenMonth == GameTime.Instance.FlowTime.NowMonth.ToString())
            {
                _isOpenMonthTrue = true;
            }
        }
        else
        {
            if (_class[_index].m_OpenMonth.Contains(","))
            {
                string[] _openMonth = _class[_index].m_OpenMonth.Split(",");

                for (int x = 0; x < _openMonth.Length; x++)
                {
                    if (_openMonth[x] == GameTime.Instance.FlowTime.NowMonth.ToString())
                    {
                        _isOpenMonthTrue = true;
                        break;
                    }
                }
            }
            else if (_class[_index].m_OpenMonth == "0" ||
                     _class[_index].m_OpenMonth == GameTime.Instance.FlowTime.NowMonth.ToString())
            {
                _isOpenMonthTrue = true;
            }

            if (_class[_index].m_OpenYear.Contains(","))
            {
                string[] _openYear = _class[_index].m_OpenYear.Split(",");

                for (int x = 0; x < _openYear.Length; x++)
                {
                    if (_openYear[x] == GameTime.Instance.FlowTime.NowYear.ToString())
                    {
                        _isOpenyearTrue = true;
                        break;
                    }
                }
            }
            else if (_class[_index].m_OpenYear == "0" ||
                     _class[_index].m_OpenYear == GameTime.Instance.FlowTime.NowYear.ToString())
            {
                _isOpenyearTrue = true;
            }
        }
    }

    private void InitStatArr()
    {
        for (int i = 0; i < 5; i++)
        {
            m_StatNum[i] = 0;
            m_StatName[i] = "";
            m_AllInfoStatNum[i] = 0;
            m_AllInfoStatName[i] = "";
        }
    }

    // ���� ������ �׻� ��� �а��� ������ش�
    private void MakeCommonClass()
    {
        for (int i = 0; i < m_NowClass.NoneClass.Count; i++)
        {
            DiscriminateClassOpenDate(m_NowClass.NoneClass, i);

            if (_isOpenyearTrue && _isOpenMonthTrue)
            {
                GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_SetClassPanel.ClassPrefabParent);

                _classPrefab.name = m_NowClass.NoneClass[i].m_ClassName;
                _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

                // �� �κ��� ����� �а� Ư���� �����ؼ� �ٲ��ֱ�
                if (m_NowClass.NoneClass[i].m_ClassStatType == ClassType.Commonm)
                {
                    _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.Commonm];
                }
                else if (m_NowClass.NoneClass[i].m_ClassStatType == ClassType.Special)
                {
                    _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.Special];
                }

                _classPrefab.GetComponent<ClassPrefab>().ClassName.text = m_NowClass.NoneClass[i].m_ClassName;

                InitStatArr();
                FindStat(m_NowClass.NoneClass, i);

                int StatCount = 0;

                for (int j = 0; j < 5; j++)
                {
                    if (m_StatNum[j] != 0 && m_StatName[j] != "")
                    {
                        StatCount += 1;
                    }
                }

                SetClassStats(StatCount, _classPrefab);

                _classPrefab.GetComponent<ClassPrefab>().MoneyText.text = m_NowClass.NoneClass[i].m_Money.ToString();
                _classPrefab.GetComponent<ClassPrefab>().HealthText.text = m_NowClass.NoneClass[i].m_Health.ToString();
            }
        }
    }

    // ���� ���� �г��� ������� 1���� ��ȹ ��Ʈ �ù�, 2���� ��ȹ ��Ʈ �ù����� ���´�. �� �Լ��� ó�� ȭ���� ���� �Լ��̴�
    public void MakeClass()
    {
        m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.GameDesigner]);

        for (int i = 0; i < m_NowClass.GameDesignerClass.Count; i++)
        {
            DiscriminateClassOpenDate(m_NowClass.GameDesignerClass, i);

            if (_isOpenMonthTrue && _isOpenyearTrue)
            {
                GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_SetClassPanel.ClassPrefabParent);

                _classPrefab.name = m_NowClass.GameDesignerClass[i].m_ClassName;
                _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

                if (m_NowClass.GameDesignerClass[i].m_ClassStatType == ClassType.Class)
                {
                    _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.GM];
                }
                else if (m_NowClass.GameDesignerClass[i].m_ClassStatType == ClassType.Special)
                {
                    _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.Special];
                }

                _classPrefab.GetComponent<ClassPrefab>().ClassName.text = m_NowClass.GameDesignerClass[i].m_ClassName;

                InitStatArr();
                FindStat(m_NowClass.GameDesignerClass, i);

                int StatCount = 0;

                for (int j = 0; j < 5; j++)
                {
                    if (m_StatNum[j] != 0 && m_StatName[j] != "")
                    {
                        StatCount += 1;
                    }
                }

                SetClassStats(StatCount, _classPrefab);

                _classPrefab.GetComponent<ClassPrefab>().MoneyText.text =
                    m_NowClass.GameDesignerClass[i].m_Money.ToString();
                _classPrefab.GetComponent<ClassPrefab>().HealthText.text =
                    m_NowClass.GameDesignerClass[i].m_Health.ToString();
            }
        }

        MakeCommonClass();
    }

    public void MakeProfessor()
    {
        for (int i = 0; i < m_NowPlayerProfessor.GameManagerProfessor.Count; i++)
        {
            GameObject _professorPrefab = Instantiate(m_ProfessorPrefab, m_SetClassPanel.ProfessorPrefabParent);

            _professorPrefab.name = m_NowPlayerProfessor.GameManagerProfessor[i].m_ProfessorNameValue;
            _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickProfessor);

            _professorPrefab.GetComponent<ProfessorPrefab>().professorName.text =
                m_NowPlayerProfessor.GameManagerProfessor[i].m_ProfessorNameValue;

            if (m_NowPlayerProfessor.GameManagerProfessor[i].m_ProfessorSet == "����")
            {
                _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_FullTimeProfessor;
            }
            else
            {
                _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_AdjunctProfessor;
            }

            _professorPrefab.GetComponent<ProfessorPrefab>().CurrentHealth.text =
                m_NowPlayerProfessor.GameManagerProfessor[i].m_ProfessorHealth.ToString();
            _professorPrefab.GetComponent<ProfessorPrefab>().CurrentPassion.text =
                m_NowPlayerProfessor.GameManagerProfessor[i].m_ProfessorPassion.ToString();

            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillName.text =
                m_NowPlayerProfessor.GameManagerProfessor[i].m_ProfessorSkills[0];
            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillInfo.text =
                m_NowPlayerProfessor.GameManagerProfessor[i].m_ProfessorSkills[1];
            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorProfileImg.sprite =
                m_NowPlayerProfessor.GameManagerProfessor[i].m_TeacherProfileImg;
        }

        //if (PlayerInfo.Instance.isFirstClassSetting)
        //{
        //    m_TutorialPanel.SetActive(true);
        //    m_Unmask.fitTarget = m_SetClassPanel.ProfessorPrefabParent.GetChild(0).GetComponent<RectTransform>();
        //    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[0];
        //}
    }

    public void MakeOtherClass()
    {
        // ��ȹ���� ���õư� ��Ʈ���� ������ �ȵ��� �� 
        if (m_GameDesignerData[m_WeekClassIndex].m_SelectClassDataSave != null &&
            m_ArtData[m_WeekClassIndex].m_SelectClassDataSave == null)
        {
            m_SetClassPanel.SetBackButtonActive(true);


            m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.Art]);

            for (int i = 0; i < m_NowClass.ArtClass.Count; i++)
            {
                DiscriminateClassOpenDate(m_NowClass.ArtClass, i);

                if (_isOpenMonthTrue && _isOpenyearTrue)
                {
                    GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_SetClassPanel.ClassPrefabParent);

                    _classPrefab.name = m_NowClass.ArtClass[i].m_ClassName;
                    _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

                    if (m_NowClass.ArtClass[i].m_ClassStatType == ClassType.Class)
                    {
                        _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.Art];
                    }
                    else if (m_NowClass.ArtClass[i].m_ClassStatType == ClassType.Special)
                    {
                        _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite =
                            m_ClassPartImage[(int)PartName.Special];
                    }

                    _classPrefab.GetComponent<ClassPrefab>().ClassName.text = m_NowClass.ArtClass[i].m_ClassName;

                    InitStatArr();
                    FindStat(m_NowClass.ArtClass, i);

                    int StatCount = 0;

                    for (int j = 0; j < 5; j++)
                    {
                        if (m_StatNum[j] != 0 && m_StatName[j] != "")
                        {
                            StatCount += 1;
                        }
                    }

                    SetClassStats(StatCount, _classPrefab);

                    _classPrefab.GetComponent<ClassPrefab>().MoneyText.text = m_NowClass.ArtClass[i].m_Money.ToString();
                    _classPrefab.GetComponent<ClassPrefab>().HealthText.text =
                        m_NowClass.ArtClass[i].m_Health.ToString();
                }
            }

            MakeCommonClass();
        }
        // ��ȹ�ݰ� ��Ʈ�� ��� ���õƴµ� �ù��� ������ �ȵ��� ��
        else if (m_GameDesignerData[m_WeekClassIndex].m_SelectClassDataSave != null &&
                 m_ArtData[m_WeekClassIndex].m_SelectClassDataSave != null &&
                 m_ProgrammingData[m_WeekClassIndex].m_SelectClassDataSave == null)
        {
            m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.Programming]);

            for (int i = 0; i < m_NowClass.ProgrammingClass.Count; i++)
            {
                DiscriminateClassOpenDate(m_NowClass.ProgrammingClass, i);

                if (_isOpenMonthTrue && _isOpenyearTrue)
                {
                    GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_SetClassPanel.ClassPrefabParent);

                    _classPrefab.name = m_NowClass.ProgrammingClass[i].m_ClassName;
                    _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

                    if (m_NowClass.ProgrammingClass[i].m_ClassStatType == ClassType.Class)
                    {
                        _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite =
                            m_ClassPartImage[(int)PartName.Programming];
                    }
                    else if (m_NowClass.ProgrammingClass[i].m_ClassStatType == ClassType.Special)
                    {
                        _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite =
                            m_ClassPartImage[(int)PartName.Special];
                    }

                    _classPrefab.GetComponent<ClassPrefab>().ClassName.text =
                        m_NowClass.ProgrammingClass[i].m_ClassName;
                    InitStatArr();
                    FindStat(m_NowClass.ProgrammingClass, i);

                    int StatCount = 0;

                    for (int j = 0; j < 5; j++)
                    {
                        if (m_StatNum[j] != 0 && m_StatName[j] != "")
                        {
                            StatCount += 1;
                        }
                    }

                    SetClassStats(StatCount, _classPrefab);

                    _classPrefab.GetComponent<ClassPrefab>().MoneyText.text =
                        m_NowClass.ProgrammingClass[i].m_Money.ToString();
                    _classPrefab.GetComponent<ClassPrefab>().HealthText.text =
                        m_NowClass.ProgrammingClass[i].m_Health.ToString();
                }
            }

            MakeCommonClass();
        }
        // 1���� ��ȹ ��Ʈ �ù� ��� �������� ��
        else if (m_GameDesignerData[m_WeekClassIndex].m_SelectClassDataSave != null &&
                 m_ArtData[m_WeekClassIndex].m_SelectClassDataSave != null &&
                 m_ProgrammingData[m_WeekClassIndex].m_SelectClassDataSave != null)
        {
            MakeClass();
            m_WeekClassIndex++;
        }
    }

    public void MakeOtherProfessor()
    {
        if (m_GameDesignerData[m_WeekProfessorIndex].m_SelectClassDataSave != null &&
            m_ArtData[m_WeekProfessorIndex].m_SelectClassDataSave == null)
        {
            for (int i = 0; i < m_NowPlayerProfessor.ArtProfessor.Count; i++)
            {
                GameObject _professorPrefab = Instantiate(m_ProfessorPrefab, m_SetClassPanel.ProfessorPrefabParent);

                _professorPrefab.name = m_NowPlayerProfessor.ArtProfessor[i].m_ProfessorNameValue;
                _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickProfessor);

                _professorPrefab.GetComponent<ProfessorPrefab>().professorName.text =
                    m_NowPlayerProfessor.ArtProfessor[i].m_ProfessorNameValue;

                if (m_NowPlayerProfessor.ArtProfessor[i].m_ProfessorSet == "����")
                {
                    _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_FullTimeProfessor;
                }
                else
                {
                    _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_AdjunctProfessor;
                }

                _professorPrefab.GetComponent<ProfessorPrefab>().CurrentHealth.text =
                    m_NowPlayerProfessor.ArtProfessor[i].m_ProfessorHealth.ToString();
                _professorPrefab.GetComponent<ProfessorPrefab>().CurrentPassion.text =
                    m_NowPlayerProfessor.ArtProfessor[i].m_ProfessorPassion.ToString();

                _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillName.text =
                    m_NowPlayerProfessor.ArtProfessor[i].m_ProfessorSkills[0];
                _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillInfo.text =
                    m_NowPlayerProfessor.ArtProfessor[i].m_ProfessorSkills[1];
                _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorProfileImg.sprite =
                    m_NowPlayerProfessor.ArtProfessor[i].m_TeacherProfileImg;
            }
        }
        else if (m_GameDesignerData[m_WeekProfessorIndex].m_SelectClassDataSave != null &&
                 m_ArtData[m_WeekProfessorIndex].m_SelectClassDataSave != null &&
                 m_ProgrammingData[m_WeekProfessorIndex].m_SelectClassDataSave != null)
        {
            MakeProfessor();
            m_WeekProfessorIndex++;
        }
        else if (m_GameDesignerData[m_WeekProfessorIndex].m_SelectClassDataSave != null &&
                 m_ArtData[m_WeekProfessorIndex].m_SelectClassDataSave != null)
        {
            for (int i = 0; i < m_NowPlayerProfessor.ProgrammingProfessor.Count; i++)
            {
                GameObject _professorPrefab = Instantiate(m_ProfessorPrefab, m_SetClassPanel.ProfessorPrefabParent);

                _professorPrefab.name = m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorNameValue;
                _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickProfessor);

                _professorPrefab.GetComponent<ProfessorPrefab>().professorName.text =
                    m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorNameValue;

                if (m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorSet == "����")
                {
                    _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_FullTimeProfessor;
                }
                else
                {
                    _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_AdjunctProfessor;
                }

                _professorPrefab.GetComponent<ProfessorPrefab>().CurrentHealth.text =
                    m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorHealth.ToString();
                _professorPrefab.GetComponent<ProfessorPrefab>().CurrentPassion.text =
                    m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorPassion.ToString();

                _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillName.text =
                    m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorSkills[0];
                _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillInfo.text =
                    m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorSkills[1];
                _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorProfileImg.sprite =
                    m_NowPlayerProfessor.ProgrammingProfessor[i].m_TeacherProfileImg;
            }
        }
    }

    // �����ϱ� ��ư�� ������ �� �ش��ϴ� �г��� ������ ��������ֱ� ���� �Լ�
    private void SetStatAllInfo(List<SaveSelectClassInfoData> _tempList, int _index)
    {
        InitStatArr();

        m_SetClassPanel.SetBackButtonActive(false);
        //if (m_ClassStack <= 1)
        //{
        //}

        m_SetClassPanel.SetClassName(_tempList[_index].m_SelectClassDataSave.m_ClassName);
        m_SetClassPanel.SetProfessorInfo(_tempList[_index].m_SelectProfessorDataSave.m_ProfessorNameValue,
            _tempList[_index].m_SelectProfessorDataSave.m_TeacherProfileImg);

        int _tempindex = SelectedClassDataStatIndex(_tempList, _index);

        for (int j = 0; j < _tempindex;)
        {
            if (_tempList[_index].m_SelectClassDataSave.m_Insight > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Insight < 0)
            {
                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Insight +
                               (int)m_NowPlayerProfessor.m_StatMagnification[
                                   _tempList[_index].m_SelectProfessorDataSave.professorPower];
                m_StatName[j] = "����";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Concentration > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Concentration < 0)
            {
                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Concentration +
                               (int)m_NowPlayerProfessor.m_StatMagnification[
                                   _tempList[_index].m_SelectProfessorDataSave.professorPower];
                m_StatName[j] = "����";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Sense > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Sense < 0)
            {
                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Sense +
                               (int)m_NowPlayerProfessor.m_StatMagnification[
                                   _tempList[_index].m_SelectProfessorDataSave.professorPower];
                m_StatName[j] = "����";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Technique > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Technique < 0)
            {
                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Technique +
                               (int)m_NowPlayerProfessor.m_StatMagnification[
                                   _tempList[_index].m_SelectProfessorDataSave.professorPower];
                m_StatName[j] = "���";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Wit > 0 || _tempList[_index].m_SelectClassDataSave.m_Wit < 0)
            {
                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Wit +
                               (int)m_NowPlayerProfessor.m_StatMagnification[
                                   _tempList[_index].m_SelectProfessorDataSave.professorPower];
                m_StatName[j] = "��ġ";
                j++;
            }
        }

        int StatCount = 0;

        for (int i = 0; i < 5; i++)
        {
            if (m_StatNum[i] != 0 && m_StatName[i] != "")
            {
                StatCount += 1;
            }
        }

        if (StatCount == 5)
        {
            foreach (GameObject _stat in m_SetClassPanel.AllInfoPanelClassStat)
            {
                _stat.SetActive(false);
            }

            m_SetClassPanel.SetStatsActive(true);
            m_SetClassPanel.SetAllStatText(m_StatNum[0].ToString());
        }
        else
        {
            m_SetClassPanel.SetStatsActive(false);

            for (int i = 0; i < StatCount; i++)
            {
                m_SetClassPanel.AllInfoPanelClassStat[i].SetActive(true);
                m_SetClassPanel.AllInfoPanelClassStatText[i].text = m_StatNum[i].ToString();
                Sprite _statSprite = SetStatImage(m_StatName[i]);
                m_SetClassPanel.AllInfoPanelClassStatImage[i].sprite = _statSprite;
            }
        }

        m_SetClassPanel.SetClassMoney(Color.white, _tempList[_index].m_SelectClassDataSave.m_Money.ToString());
        m_SetClassPanel.SetClassUseHealth(_tempList[_index].m_SelectClassDataSave.m_Health.ToString());
    }

    // checkPanel�� ���� ������ �������� ������ ����ֱ� ���� CompleteClassPrefab�� �������ִ� �Լ�
    private void SetStatCheckClassPrefabInfo(GameObject _obj, List<SaveSelectClassInfoData> _tempList, int _index)
    {
        // CompleteClassPrefab�� ���� �̸��� ���� �̸� ���� �κ�.
        _obj.GetComponent<CompleteClassPrefab>().ClassName.text = _tempList[_index].m_SelectClassDataSave.m_ClassName;
        _obj.GetComponent<CompleteClassPrefab>().ProfessorName.text =
            _tempList[_index].m_SelectProfessorDataSave.m_ProfessorNameValue;
        _obj.GetComponent<CompleteClassPrefab>().ProfessorImage.sprite =
            _tempList[_index].m_SelectProfessorDataSave.m_TeacherProfileImg;

        int _tempindex = SelectedClassDataStatIndex(_tempList, _index);
        for (int j = 0; j < _tempindex;)
        {
            if (_tempList[_index].m_SelectClassDataSave.m_Insight > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Insight < 0)
            {
                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Insight +
                               (int)m_NowPlayerProfessor.m_StatMagnification[
                                   _tempList[_index].m_SelectProfessorDataSave.professorPower];
                m_StatName[j] = "����";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Concentration > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Concentration < 0)
            {
                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Concentration +
                               (int)m_NowPlayerProfessor.m_StatMagnification[
                                   _tempList[_index].m_SelectProfessorDataSave.professorPower];
                m_StatName[j] = "����";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Sense > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Sense < 0)
            {
                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Sense +
                               (int)m_NowPlayerProfessor.m_StatMagnification[
                                   _tempList[_index].m_SelectProfessorDataSave.professorPower];
                m_StatName[j] = "����";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Technique > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Technique < 0)
            {
                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Technique +
                               (int)m_NowPlayerProfessor.m_StatMagnification[
                                   _tempList[_index].m_SelectProfessorDataSave.professorPower];
                m_StatName[j] = "���";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Wit > 0 || _tempList[_index].m_SelectClassDataSave.m_Wit < 0)
            {
                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Wit +
                               (int)m_NowPlayerProfessor.m_StatMagnification[
                                   _tempList[_index].m_SelectProfessorDataSave.professorPower];
                m_StatName[j] = "��ġ";
                j++;
            }
        }

        int StatCount = 0;

        for (int j = 0; j < 5; j++)
        {
            if (m_StatNum[j] != 0 && m_StatName[j] != "")
            {
                StatCount += 1;
            }
        }

        SetComplelteClassStats(StatCount, _obj);

        _obj.GetComponent<CompleteClassPrefab>().MoneyText.text =
            _tempList[_index].m_SelectClassDataSave.m_Money.ToString();
        _obj.GetComponent<CompleteClassPrefab>().HealthText.text =
            _tempList[_index].m_SelectClassDataSave.m_Health.ToString();
        //_obj.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = _tempList[_index].m_SelectClassDataSave.m_Money.ToString();
        //_obj.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().text = _tempList[_index].m_SelectClassDataSave.m_Health.ToString();
    }

    private int SelectedClassDataStatIndex(List<SaveSelectClassInfoData> _tempList, int _listIndex)
    {
        int _index = 0;

        if (_tempList[_listIndex].m_SelectClassDataSave.m_Insight != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_SelectClassDataSave.m_Concentration != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_SelectClassDataSave.m_Sense != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_SelectClassDataSave.m_Technique != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_SelectClassDataSave.m_Wit != 0)
        {
            _index++;
        }

        return _index;
    }

    private int AllInfoDataStatIndex(SaveSelectClassInfoData _saveData)
    {
        int _index = 0;

        if (_saveData.m_SelectClassDataSave.m_Insight != 0)
        {
            _index++;
        }

        if (_saveData.m_SelectClassDataSave.m_Concentration != 0)
        {
            _index++;
        }

        if (_saveData.m_SelectClassDataSave.m_Sense != 0)
        {
            _index++;
        }

        if (_saveData.m_SelectClassDataSave.m_Technique != 0)
        {
            _index++;
        }

        if (_saveData.m_SelectClassDataSave.m_Wit != 0)
        {
            _index++;
        }

        return _index;
    }

    private int UnSelectedClassDataStatIndex(List<Class> _tempList, int _listIndex)
    {
        int _index = 0;

        if (_tempList[_listIndex].m_Insight != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_Concentration != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_Sense != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_Technique != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_Wit != 0)
        {
            _index++;
        }

        return _index;
    }

    // 1�ִ� 0, 2�ִ� 1 �̰� ��ȹ���� 1, ��Ʈ���� 2, �ùֹ��� 3�̴�.
    private void MakeTotalInfo(List<SaveSelectClassInfoData> _tempList, int _partIndex)
    {
        string _1WeekClassName = _tempList[0].m_SelectClassDataSave.m_ClassName;
        string _2WeekClassName = _tempList[1].m_SelectClassDataSave.m_ClassName;
        string _1WeekClassMoney = _tempList[0].m_SelectClassDataSave.m_Money.ToString();
        string _2WeekClassMoney = _tempList[1].m_SelectClassDataSave.m_Money.ToString();

        switch (_partIndex)
        {
            case 1:
            {
                m_CheckClassPanel.SetGMData(_1WeekClassName, _2WeekClassName, _1WeekClassMoney, _2WeekClassMoney);
            }
                break;

            case 2:
            {
                m_CheckClassPanel.SetArtData(_1WeekClassName, _2WeekClassName, _1WeekClassMoney, _2WeekClassMoney);
            }
                break;

            case 3:
            {
                m_CheckClassPanel.SetProgrammingData(_1WeekClassName, _2WeekClassName, _1WeekClassMoney,
                    _2WeekClassMoney);
            }
                break;
        }
    }

    private int CalculateTotalMoneyCheckPanel()
    {
        int _totalMoney;

        _totalMoney = m_ArtData[0].m_SelectClassDataSave.m_Money + m_ArtData[1].m_SelectClassDataSave.m_Money +
                      m_ProgrammingData[0].m_SelectClassDataSave.m_Money +
                      m_ProgrammingData[1].m_SelectClassDataSave.m_Money
                      + m_GameDesignerData[0].m_SelectClassDataSave.m_Money +
                      m_GameDesignerData[1].m_SelectClassDataSave.m_Money;

        return _totalMoney;
    }

    private void ModifyButton()
    {
        m_SetClassPanel.m_ClassPrefab.anchoredPosition = new Vector2(7f, 0f);
        m_SetClassPanel.m_ProfessorPrefab.anchoredPosition = new Vector2(7f, 0f);

        m_SetClassPanel.PopUpSelectClassPanel();
        // �����Ϸ��ư Ȱ��ȭ
        m_SetClassPanel.SetModifiyButton(true);

        m_CheckClassPanel.PopOffCheckClassPanel();

        if (m_SetClassPanel.ClassPrefabParent.childCount > 0)
        {
            m_SetClassPanel.DestroySelectClassObject();
        }

        if (m_SetClassPanel.ProfessorPrefabParent.childCount > 0)
        {
            m_SetClassPanel.DestroySelectPanelProfessorObject();
        }

        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;
        GameObject _parentObj = _currentObj.transform.parent.gameObject;

        // ���� ��ư�� ������ �� m_SaveData�� ������� �� ������ ���� Ŭ���� ������Ʈ�� ������ �ٽ� ä���ֱ�
        Class _tempData;

        m_ModifyClassString = _parentObj.GetComponent<CompleteClassPrefab>().ClassName.text;

        m_SelectClass.classData.TryGetValue(m_ModifyClassString, out _tempData);

        ProfessorStat _tempProfessor;

        m_ModifyProfessorString = _parentObj.GetComponent<CompleteClassPrefab>().ProfessorName.text;

        m_SelectProfessor.professorData.TryGetValue(m_ModifyProfessorString, out _tempProfessor);

        if (m_SaveData.m_SelectClassDataSave == null || m_SaveData.m_SelectClassDataSave != _tempData)
        {
            m_SaveData.m_SelectClassDataSave = _tempData;
        }

        if (m_SaveData.m_SelectProfessorDataSave == null || m_SaveData.m_SelectProfessorDataSave != _tempProfessor)
        {
            m_SaveData.m_SelectProfessorDataSave = _tempProfessor;
        }

        if (m_Week == "1����")
        {
            m_SetClassPanel.SetWeekText("1����");
            m_SaveDataIndex = 0;
            ResetSelectPanel(_tempData, _tempProfessor);
            // 1������ �����ߴ� ������ �����ش�
            m_SetClassPanel._1WeekPanel.SetActive(true);
            m_SetClassPanel._2WeekPanel.SetActive(false);
            m_SetClassPanel.SetDownButton(true);
            m_SetClassPanel.SetUpButton(false);
        }
        else
        {
            m_SetClassPanel.SetWeekText("2����");
            m_SaveDataIndex = 1;
            ResetSelectPanel(_tempData, _tempProfessor);
            // 2������ �����ߴ� ������ �����ش�
            m_SetClassPanel._1WeekPanel.SetActive(false);
            m_SetClassPanel._2WeekPanel.SetActive(true);
            m_SetClassPanel.SetDownButton(false);
            m_SetClassPanel.SetUpButton(true);
        }
    }

    // ���� �г� ���� �������ֱ�
    private void SetToSeletPanel(List<Class> _nowClass, List<ProfessorStat> _nowProfessor, string _partName,
        List<SaveSelectClassInfoData> _data)
    {
        MakeCommonClass();

        for (int i = 0; i < _nowClass.Count; i++)
        {
            DiscriminateClassOpenDate(_nowClass, i);

            if (_isOpenyearTrue && _isOpenMonthTrue)
            {
                GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_SetClassPanel.ClassPrefabParent);

                _classPrefab.name = _nowClass[i].m_ClassName;
                _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);
                InitStatArr();
                FindStat(_nowClass, i);

                int StatCount = 0;

                for (int j = 0; j < 5; j++)
                {
                    if (m_StatNum[j] != 0 && m_StatName[j] != "")
                    {
                        StatCount += 1;
                    }
                }

                SetClassStats(StatCount, _classPrefab);

                switch (_partName)
                {
                    case "��ȹ":
                    {
                        m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.GameDesigner]);
                        _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.GM];
                    }
                        break;

                    case "��Ʈ":
                    {
                        m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.Art]);
                        _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.Art];
                    }
                        break;

                    case "�ù�":
                    {
                        m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.Programming]);
                        _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.GM];
                    }
                        break;
                }

                _classPrefab.GetComponent<ClassPrefab>().ClassName.text = _nowClass[i].m_ClassName;

                _classPrefab.GetComponent<ClassPrefab>().MoneyText.text = _nowClass[i].m_Money.ToString();
                _classPrefab.GetComponent<ClassPrefab>().HealthText.text = _nowClass[i].m_Health.ToString();
            }
        }

        for (int i = 0; i < _nowProfessor.Count; i++)
        {
            GameObject _professorPrefab = Instantiate(m_ProfessorPrefab, m_SetClassPanel.ProfessorPrefabParent);

            _professorPrefab.name = _nowProfessor[i].m_ProfessorNameValue;
            _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickProfessor);

            _professorPrefab.GetComponent<ProfessorPrefab>().professorName.text = _nowProfessor[i].m_ProfessorNameValue;

            if (_nowProfessor[i].m_ProfessorSet == "����")
            {
                _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_FullTimeProfessor;
            }
            else
            {
                _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_AdjunctProfessor;
            }

            _professorPrefab.GetComponent<ProfessorPrefab>().CurrentHealth.text =
                _nowProfessor[i].m_ProfessorHealth.ToString();
            _professorPrefab.GetComponent<ProfessorPrefab>().CurrentPassion.text =
                _nowProfessor[i].m_ProfessorPassion.ToString();

            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillName.text =
                _nowProfessor[i].m_ProfessorSkills[0];
            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillInfo.text =
                _nowProfessor[i].m_ProfessorSkills[1];
            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorProfileImg.sprite =
                _nowProfessor[i].m_TeacherProfileImg;

            switch (_partName)
            {
                case "��ȹ":
                {
                    m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.GameDesigner]);
                }
                    break;

                case "��Ʈ":
                {
                    m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.Art]);
                }
                    break;

                case "�ù�":
                {
                    m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.Programming]);
                }
                    break;
            }

            //if (_nowProfessor[i].m_ProfessorSkills != null)
            //{
            //    for (int j = 0; j < 3;)
            //    {
            //        if (_nowProfessor[i].m_ProfessorSkills[j] != "")
            //        {
            //            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillUnLockImage[j].SetActive(false);
            //            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillName[j].text = _nowProfessor[i].m_ProfessorSkills[j];

            //            j++;
            //        }
            //        else
            //        {
            //            j = 3;
            //        }
            //    }
            //}
        }

        if (m_SaveDataIndex == 0)
        {
            // 1��
            SetStatAllInfo(_data, 0);
        }
        else
        {
            // 2��
            SetStatAllInfo(_data, 1);
        }
    }

    private void ResetSelectPanel(Class _tempClass, ProfessorStat _tempProfessor)
    {
        if (_tempClass.m_ClassType == StudentType.Art)
        {
            SetToSeletPanel(m_NowClass.ArtClass, m_NowPlayerProfessor.ArtProfessor, "��Ʈ", m_ArtData);
        }
        else if (_tempClass.m_ClassType == StudentType.GameDesigner)
        {
            SetToSeletPanel(m_NowClass.GameDesignerClass, m_NowPlayerProfessor.GameManagerProfessor, "��ȹ",
                m_GameDesignerData);
        }
        else if (_tempClass.m_ClassType == StudentType.Programming)
        {
            SetToSeletPanel(m_NowClass.ProgrammingClass, m_NowPlayerProfessor.ProgrammingProfessor, "�ù�",
                m_ProgrammingData);
        }
        else if (_tempClass.m_ClassType == StudentType.None)
        {
            if (_tempProfessor.m_ProfessorType == StudentType.GameDesigner)
            {
                SetToSeletPanel(m_NowClass.GameDesignerClass, m_NowPlayerProfessor.GameManagerProfessor, "��ȹ",
                    m_GameDesignerData);
            }
            else if (_tempProfessor.m_ProfessorType == StudentType.Art)
            {
                SetToSeletPanel(m_NowClass.ArtClass, m_NowPlayerProfessor.ArtProfessor, "��Ʈ", m_ArtData);
            }
            else if (_tempProfessor.m_ProfessorType == StudentType.Programming)
            {
                SetToSeletPanel(m_NowClass.ProgrammingClass, m_NowPlayerProfessor.ProgrammingProfessor, "�ù�",
                    m_ProgrammingData);
            }
        }
    }

    public void ModifyCompleteButton()
    {
        m_SetClassPanel.SetModifiyButton(false);

        if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.Art)
        {
            m_ArtData.Add(m_SaveData);
            ChangeListIndex(m_ArtData, m_SaveDataIndex, m_SaveData);
            m_ArtData.RemoveAt(2);
        }
        else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.GameDesigner)
        {
            m_GameDesignerData.Add(m_SaveData);
            ChangeListIndex(m_GameDesignerData, m_SaveDataIndex, m_SaveData);
            m_GameDesignerData.RemoveAt(2);
        }
        else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.Programming)
        {
            m_ProgrammingData.Add(m_SaveData);
            ChangeListIndex(m_ProgrammingData, m_SaveDataIndex, m_SaveData);
            m_ProgrammingData.RemoveAt(2);
        }
        else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.None)
        {
            if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.GameDesigner])
            {
                m_GameDesignerData.Add(m_SaveData);
                ChangeListIndex(m_GameDesignerData, m_SaveDataIndex, m_SaveData);
                m_GameDesignerData.RemoveAt(2);
            }
            else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Art])
            {
                m_ArtData.Add(m_SaveData);
                ChangeListIndex(m_ArtData, m_SaveDataIndex, m_SaveData);
                m_ArtData.RemoveAt(2);
            }
            else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Programming])
            {
                m_ProgrammingData.Add(m_SaveData);
                ChangeListIndex(m_ProgrammingData, m_SaveDataIndex, m_SaveData);
                m_ProgrammingData.RemoveAt(2);
            }
        }

        // ��� �а��� ������ ���������� ���� ������ �������� �ѹ��� ��� �� UI�� ������Ѵ�.
        if (m_ArtData[1].m_SelectClassDataSave != null && m_ProgrammingData[1].m_SelectClassDataSave != null
                                                       && m_GameDesignerData[1].m_SelectClassDataSave != null)
        {
            m_SetClassPanel.PopOffSelectClassPanel();
            m_CheckClassPanel.PopUpCheckClassPanel();

            m_CheckClassPanel.SetTotalMoney(CalculateTotalMoneyCheckPanel().ToString());

            m_CheckClassPanel._1WeekButton.onClick.AddListener(MakeCheckClass);
            m_CheckClassPanel._2WeekButton.onClick.AddListener(MakeCheckClass);

            MakeCheckClass();
        }
    }

    public void MakeCheckClass()
    {
        GameObject _currentButton = EventSystem.current.currentSelectedGameObject;

        m_CheckClassPanel.SetCurrentMoney(m_CurrentMoney.ToString());

        if (_currentButton.name != "2WeekButton")
        {
            _currentButton = m_CheckClassPanel._1WeekButton.gameObject;
            m_Week = "1����";
            m_CheckClassPanel._1WeekButton.GetComponent<Image>().sprite = m_WeekButtonSprite[1];
            m_CheckClassPanel._2WeekButton.GetComponent<Image>().sprite = m_WeekButtonSprite[2];
        }
        else
        {
            m_Week = "2����";
            m_CheckClassPanel._1WeekButton.GetComponent<Image>().sprite = m_WeekButtonSprite[0];
            m_CheckClassPanel._2WeekButton.GetComponent<Image>().sprite = m_WeekButtonSprite[3];
        }

        if (m_CheckClassPanel.CompleteClassPrefabParent.childCount > 0)
        {
            m_CheckClassPanel.DestroyCheckClassObject();
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject _checkClassPrefab = Instantiate(m_CheckClassPrefab, m_CheckClassPanel.CompleteClassPrefabParent);

            _checkClassPrefab.GetComponent<CompleteClassPrefab>().ModifiyButton.onClick.AddListener(ModifyButton);

            InitStatArr();

            switch (i)
            {
                case 0:
                {
                    if (_currentButton.name == "1WeekButton")
                    {
                        SetStatCheckClassPrefabInfo(_checkClassPrefab, m_GameDesignerData, 0);
                        MakeTotalInfo(m_GameDesignerData, 1);
                    }
                    else
                    {
                        SetStatCheckClassPrefabInfo(_checkClassPrefab, m_GameDesignerData, 1);
                        MakeTotalInfo(m_GameDesignerData, 1);
                    }
                }
                    break;

                case 1:
                {
                    if (_currentButton.name == "1WeekButton")
                    {
                        SetStatCheckClassPrefabInfo(_checkClassPrefab, m_ArtData, 0);
                        MakeTotalInfo(m_ArtData, 2);
                    }
                    else
                    {
                        SetStatCheckClassPrefabInfo(_checkClassPrefab, m_ArtData, 1);
                        MakeTotalInfo(m_ArtData, 2);
                    }
                }
                    break;

                case 2:
                {
                    if (_currentButton.name == "1WeekButton")
                    {
                        SetStatCheckClassPrefabInfo(_checkClassPrefab, m_ProgrammingData, 0);
                        MakeTotalInfo(m_ProgrammingData, 3);
                    }
                    else
                    {
                        SetStatCheckClassPrefabInfo(_checkClassPrefab, m_ProgrammingData, 1);
                        MakeTotalInfo(m_ProgrammingData, 3);
                    }
                }
                    break;
            }
        }
    }

    public void ClickCompleteButton()
    {
        // ������� ������ȭ������ ������ ü�� ����, ���� ������ �� �����ᰡ ���� ��ȭ�� �Ѿ�� �� �ߴ� ����̴�.
        if (m_SetClassPanel.MoneyWarningMessage.activeSelf == false &&
            m_SetClassPanel.HealthWarningMessage.activeSelf == false &&
            m_SetClassPanel.CurrentMoneyWarningMessage.activeSelf == false)
        {
            if (m_SaveData.m_SelectClassDataSave != null && m_SaveData.m_SelectProfessorDataSave != null)
            {
                if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.Art)
                {
                    m_ArtData.Add(m_SaveData);
                    ChangeListIndex(m_ArtData, m_SaveDataIndex, m_SaveData);
                    m_ArtData.RemoveAt(2);
                }
                else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.GameDesigner)
                {
                    m_GameDesignerData.Add(m_SaveData);
                    ChangeListIndex(m_GameDesignerData, m_SaveDataIndex, m_SaveData);
                    m_GameDesignerData.RemoveAt(2);
                }
                else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.Programming)
                {
                    m_ProgrammingData.Add(m_SaveData);
                    ChangeListIndex(m_ProgrammingData, m_SaveDataIndex, m_SaveData);
                    m_ProgrammingData.RemoveAt(2);

                    // ��� �а��� ������ ���������� ���� ������ �������� �ѹ��� ��� �� UI�� ������Ѵ�.
                    if (m_ArtData[1].m_SelectClassDataSave != null && m_ProgrammingData[1].m_SelectClassDataSave != null
                                                                   && m_GameDesignerData[1].m_SelectClassDataSave !=
                                                                   null)
                    {
                        m_SetClassPanel.SetBackButtonActive(false);

                        m_CheckClassPanel.PopUpCheckClassPanel();
                        m_SetClassPanel.PopOffSelectClassPanel();

                        m_CheckClassPanel.SetTotalMoney(CalculateTotalMoneyCheckPanel().ToString());

                        m_CheckClassPanel._1WeekButton.onClick.AddListener(MakeCheckClass);
                        m_CheckClassPanel._2WeekButton.onClick.AddListener(MakeCheckClass);


                        if (PlayerInfo.Instance.IsFirstClassSetting && m_TutorialCount == 11)
                        {
                            m_Unmask.gameObject.SetActive(true);
                            m_Unmask.fitTarget =
                                m_CheckClassPanel._StartButton.gameObject.GetComponent<RectTransform>();
                            m_CheckClassPanel._StartButton.onClick.AddListener(ContinueTutorial);
                            m_TutorialTextImage.gameObject.SetActive(false);
                            m_TutorialArrowImage.gameObject.SetActive(true);
                            m_TutorialArrowImage.transform.position =
                                m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
                            m_TutorialPanel.SetActive(true);
                            m_TutorialCount++;
                        }

                        MakeCheckClass();
                    }
                }
                else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.None)
                {
                    if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.GameDesigner])
                    {
                        m_GameDesignerData.Add(m_SaveData);
                        ChangeListIndex(m_GameDesignerData, m_SaveDataIndex, m_SaveData);
                        m_GameDesignerData.RemoveAt(2);
                    }
                    else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Art])
                    {
                        m_ArtData.Add(m_SaveData);
                        ChangeListIndex(m_ArtData, m_SaveDataIndex, m_SaveData);
                        m_ArtData.RemoveAt(2);
                    }
                    else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Programming])
                    {
                        m_ProgrammingData.Add(m_SaveData);
                        ChangeListIndex(m_ProgrammingData, m_SaveDataIndex, m_SaveData);
                        m_ProgrammingData.RemoveAt(2);

                        // ��� �а��� ������ ���������� ���� ������ �������� �ѹ��� ��� �� UI�� ������Ѵ�.
                        if (m_ArtData[1].m_SelectClassDataSave != null && m_ProgrammingData[1].m_SelectClassDataSave !=
                                                                       null
                                                                       && m_GameDesignerData[1].m_SelectClassDataSave !=
                                                                       null)
                        {
                            m_CheckClassPanel.PopUpCheckClassPanel();
                            m_SetClassPanel.PopOffSelectClassPanel();

                            m_CheckClassPanel.SetTotalMoney(CalculateTotalMoneyCheckPanel().ToString());

                            m_CheckClassPanel._1WeekButton.onClick.AddListener(MakeCheckClass);
                            m_CheckClassPanel._2WeekButton.onClick.AddListener(MakeCheckClass);

                            if (PlayerInfo.Instance.IsFirstClassSetting && m_TutorialCount == 11)
                            {
                                m_Unmask.gameObject.SetActive(true);
                                m_Unmask.fitTarget =
                                    m_CheckClassPanel._StartButton.gameObject.GetComponent<RectTransform>();
                                m_CheckClassPanel._StartButton.onClick.AddListener(ContinueTutorial);
                                m_TutorialTextImage.gameObject.SetActive(false);
                                m_TutorialArrowImage.gameObject.SetActive(true);
                                m_TutorialArrowImage.transform.position =
                                    m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
                                m_TutorialPanel.SetActive(true);
                                m_TutorialCount++;
                            }

                            MakeCheckClass();
                        }
                    }
                }

                // ���� �а��� �Ѿ�ϱ� ������ �����͸� ��Ƶ� ���� ����ش�.
                InitStatArr();

                m_SetClassPanel.DestroySelectClassObject();
                m_SetClassPanel.DestroySelectPanelProfessorObject();

                InitAllInfoStat();

                m_SetClassPanel.m_ClassPrefab.anchoredPosition = new Vector2(7f, 0f);
                m_SetClassPanel.m_ProfessorPrefab.anchoredPosition = new Vector2(7f, 0f);

                MakeOtherClass();
                MakeOtherProfessor();

                if (m_WeekClassIndex == 1 && m_WeekProfessorIndex == 1 && m_IsChangeWeekend == false)
                {
                    m_IsChangeWeekend = true;
                    m_SetClassPanel.SetWeekText("2����");
                    m_SetClassPanel._1WeekPanel.SetActive(false);
                    m_SetClassPanel._2WeekPanel.SetActive(true);
                    m_SetClassPanel.SetUpButton(true);
                    m_SaveDataIndex++;
                }

                m_SaveData.m_SelectClassDataSave = null;
                m_SaveData.m_SelectProfessorDataSave = null;
                _ClickProfessorData = null;
                _ClickClassData = null;

                // ���� ������ �Ϸ� ������ ������Ű��
                if (m_ClassStack < 5)
                {
                    m_ClassStack++;
                }
            }
        }

        if (PlayerInfo.Instance.IsFirstClassSetting && m_TutorialCount == 9)
        {
            m_Unmask.gameObject.SetActive(false);
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(true);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = new Vector3(2340f / 2f, 1080f / 2f, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
    }

    // ������ ���� �� ���� ����â�� ����ֱ� ���� �Լ�
    public void InitSelecteClass()
    {
        // ���� �а��� �Ѿ�ϱ� ������ �����͸� ��Ƶ� ���� ����ش�.
        InitStatArr();
        //InitData();

        m_SetClassPanel.DestroySelectClassObject();
        m_SetClassPanel.DestroySelectPanelProfessorObject();

        InitAllInfoStat();

        InitTotalInfo(m_SetClassPanel._1WeekPanel);
        InitTotalInfo(m_SetClassPanel._2WeekPanel);

        //MakeOtherClass();
        //MakeOtherProfessor();
        m_SetClassPanel.SetWeekText("1����");

        m_SetClassPanel._2WeekPanel.SetActive(false);
        m_SetClassPanel._1WeekPanel.SetActive(true);
        m_SetClassPanel.SetDownButton(false);
        m_SetClassPanel.SetUpButton(false);

        m_IsChangeWeekend = false;

        for (int i = 0; i < 6; i++)
        {
            m_ClassMoney[i] = 0;
            m_ClassHealth[i] = 0;
        }

        // �� ������
        m_SetClassPanel.TotalMoney(Color.white, "0");
        m_SetClassPanel.TotalHealth("0");
        m_SaveData.m_SelectClassDataSave = null;
        m_SaveData.m_SelectProfessorDataSave = null;
        _ClickProfessorData = null;
        _ClickClassData = null;

        m_WeekClassIndex = 0;
        m_WeekProfessorIndex = 0;
        m_SaveDataIndex = 0;
        m_ClassStack = 0;
    }

    // ���� ������ ������ �� ���ִ°� �ƴ϶�� 0�� �ƴ� 3���� ���ȸ� �������Ѵ�.
    private void FindStat(List<Class> _nowClass, int _index)
    {
        int _tempindex = UnSelectedClassDataStatIndex(_nowClass, _index);

        for (int j = 0; j < _tempindex;)
        {
            if (_nowClass[_index].m_Insight > 0 || _nowClass[_index].m_Insight < 0)
            {
                m_StatNum[j] = _nowClass[_index].m_Insight;
                m_StatName[j] = "����";
                j++;
            }

            if (_nowClass[_index].m_Concentration > 0 || _nowClass[_index].m_Concentration < 0)
            {
                m_StatNum[j] = _nowClass[_index].m_Concentration;
                m_StatName[j] = "����";
                j++;
            }

            if (_nowClass[_index].m_Sense > 0 || _nowClass[_index].m_Sense < 0)
            {
                m_StatNum[j] = _nowClass[_index].m_Sense;
                m_StatName[j] = "����";
                j++;
            }

            if (_nowClass[_index].m_Technique > 0 || _nowClass[_index].m_Technique < 0)
            {
                m_StatNum[j] = _nowClass[_index].m_Technique;
                m_StatName[j] = "���";
                j++;
            }

            if (_nowClass[_index].m_Wit > 0 || _nowClass[_index].m_Wit < 0)
            {
                m_StatNum[j] = _nowClass[_index].m_Wit;
                m_StatName[j] = "��ġ";
                j++;
            }
        }
    }

    // �̹� ������ ������ ���¿��� ������ ������ �� AllInfo�� ������ �ɷ�ġ�� ���� ������ ����ֱ� ���� �Լ�.
    private void FindStatAllInfo()
    {
        int _tempindex = AllInfoDataStatIndex(m_SaveData);

        for (int j = 0; j < _tempindex;)
        {
            if (m_SaveData.m_SelectClassDataSave.m_Insight > 0 || m_SaveData.m_SelectClassDataSave.m_Insight < 0)
            {
                m_AllInfoStatNum[j] = m_SaveData.m_SelectClassDataSave.m_Insight;
                m_AllInfoStatName[j] = "����";
                j++;
            }

            if (m_SaveData.m_SelectClassDataSave.m_Concentration > 0 ||
                m_SaveData.m_SelectClassDataSave.m_Concentration < 0)
            {
                m_AllInfoStatNum[j] = m_SaveData.m_SelectClassDataSave.m_Concentration;
                m_AllInfoStatName[j] = "����";
                j++;
            }

            if (m_SaveData.m_SelectClassDataSave.m_Sense > 0 || m_SaveData.m_SelectClassDataSave.m_Sense < 0)
            {
                m_AllInfoStatNum[j] = m_SaveData.m_SelectClassDataSave.m_Sense;
                m_AllInfoStatName[j] = "����";
                j++;
            }

            if (m_SaveData.m_SelectClassDataSave.m_Technique > 0 || m_SaveData.m_SelectClassDataSave.m_Technique < 0)
            {
                m_AllInfoStatNum[j] = m_SaveData.m_SelectClassDataSave.m_Technique;
                m_AllInfoStatName[j] = "���";
                j++;
            }

            if (m_SaveData.m_SelectClassDataSave.m_Wit > 0 || m_SaveData.m_SelectClassDataSave.m_Wit < 0)
            {
                m_AllInfoStatNum[j] = m_SaveData.m_SelectClassDataSave.m_Wit;
                m_AllInfoStatName[j] = "��ġ";
                j++;
            }
        }
    }

    // ������ ������ �� AllInfo�� ������ ������ ������ �Ȱ��� ����ֱ� ���� �Լ�
    private void SetStat(GameObject _classPrefab)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_classPrefab.GetComponent<ClassPrefab>().AllStat.activeSelf)
            {
                foreach (GameObject _stat in m_SetClassPanel.AllInfoPanelClassStat)
                {
                    _stat.SetActive(false);
                }

                m_SetClassPanel.SetStatsActive(true);
                m_SetClassPanel.SetAllStatText(_classPrefab.GetComponent<ClassPrefab>().AllStatText.text);
                return;
            }

            if (_classPrefab.GetComponent<ClassPrefab>().Stat[i].activeSelf)
            {
                m_SetClassPanel.SetStatsActive(false);

                m_SetClassPanel.AllInfoPanelClassStat[i].SetActive(true);
                m_SetClassPanel.AllInfoPanelClassStatImage[i].sprite =
                    _classPrefab.GetComponent<ClassPrefab>().StatImage[i].sprite;

                string _tempStat = _classPrefab.GetComponent<ClassPrefab>().StatText[i].text;

                // ������ ������ ��Ȳ���� ������ �����ϸ� ���� ������ ������ �����̶� ������ ������ �ɷ�ġ�� ���ؼ� ����� AllInfo�� �����ش�.
                if (m_SaveData.m_SelectProfessorDataSave != null)
                {
                    //int _parshingStat = int.Parse(_tempStat) + m_SaveData.m_SelectProfessorDataSave.professorPower;
                    int _parshingStat = int.Parse(_tempStat) * (int)magnification;

                    m_SetClassPanel.AllInfoPanelClassStatText[i].text = _parshingStat.ToString();
                }
                else
                {
                    m_SetClassPanel.AllInfoPanelClassStatText[i].text = _tempStat;
                }
            }
            else
            {
                m_SetClassPanel.AllInfoPanelClassStat[i].SetActive(false);
            }
        }
    }

    // �ڷΰ��⸦ ������ �� ���� �ߴ� �����鵵 ���� �����ֱ� ���� �ڵ�
    private void RestPanel(GameObject _panel, int _index)
    {
        _panel.transform.GetChild(_index).gameObject.SetActive(false);
        _panel.transform.GetChild(_index).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        _panel.transform.GetChild(_index).GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
    }

    // ������ �������� �� �ٷιٷ� �� �ݾ��� �ٲ� �� �ְ� ���ִ� �Լ�
    private void SaveMoneySelectPaenl(GameObject _Weekpanel, int _2WeekPanelIndex)
    {
        m_TotalMoney = 0;

        for (int i = 0; i < _Weekpanel.transform.childCount - 1; i++)
        {
            if (_Weekpanel.transform.GetChild(i + 1).gameObject.activeSelf == true)
            {
                string _tempGMMoney = _Weekpanel.transform.GetChild(i + 1).GetChild(2).GetComponent<TextMeshProUGUI>()
                    .text;

                m_ClassMoney[i + _2WeekPanelIndex] = int.Parse(_tempGMMoney);
            }
        }
    }

    private void SaveHealthSelectPanel(GameObject _weekPanel, int _2weekPanelIndex, int health)
    {
        m_TotalHealth = 0;

        for (int i = 0; i < _weekPanel.transform.childCount - 1; i++)
        {
            if (_weekPanel.transform.GetChild(i + 1).gameObject.activeSelf)
            {
                m_ClassHealth[i + _2weekPanelIndex] = health;
            }
        }
    }

    private void InitAllInfoStat()
    {
        // ü�°� ��
        m_SetClassPanel.SetClassMoney(Color.white, "");
        m_SetClassPanel.SetClassUseHealth("");

        // �����̸�
        m_SetClassPanel.SetClassName("");

        // �����̸�
        m_SetClassPanel.SetProfessorInfo("", null);

        // ���ȵ�
        for (int i = 0; i < 3; i++)
        {
            if (m_SetClassPanel.AllInfoPanelClassAllStat.activeSelf)
            {
                m_SetClassPanel.AllInfoPanelClassAllStat.SetActive(false);
                m_SetClassPanel.AllInfoPanelClassAllStatText.text = "";
                return;
            }

            if (m_SetClassPanel.AllInfoPanelClassStat[i].activeSelf == true)
            {
                m_SetClassPanel.AllInfoPanelClassStat[i].SetActive(false);
                m_SetClassPanel.AllInfoPanelClassStatText[i].text = "";
            }
        }
    }

    // ���� Ŭ������ �� Ŭ���� ������ ������ AllInfoPanel�� ����ֱ� ���� �Լ�
    private void ClickClass()
    {
        #region _��ư�� Ŭ������ �� ���̶���Ʈ�� �����ְ� �ϱ�

        if (_prevClass != null)
        {
            Button _prevButton = _prevClass.GetComponent<Button>();

            ColorBlock _prevButtonColor = _prevButton.colors;

            _prevButtonColor = ColorBlock.defaultColorBlock;

            _prevButton.colors = _prevButtonColor;
        }

        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        if (_currentObj == null)
        {
            return;
        }

        _prevClass = _currentObj;

        Button _currentButton = _currentObj.GetComponent<Button>();

        ColorBlock _currentButtonColor = _currentButton.colors;

        _currentButtonColor.normalColor = m_HighLightColor;
        _currentButtonColor.highlightedColor = m_HighLightColor;
        _currentButtonColor.pressedColor = m_HighLightColor;
        _currentButtonColor.selectedColor = m_HighLightColor;

        _currentButton.colors = _currentButtonColor;

        #endregion

        m_SelectClass.classData.TryGetValue(_currentObj.name, out _ClickClassData);

        int _compareMoney = int.Parse(m_InGameUICurrentMoney.text);
        int _compareHealth = _ClickClassData.m_Health;

        m_SetClassPanel.SetClassName(_ClickClassData.m_ClassName);
        m_SetClassPanel.SetClassUseHealth(_ClickClassData.m_Health.ToString());

        // ���� ���� �����ϰ� �ִ� ������ �����ᰡ �� ��θ� ��� ����ֱ�
        if (_compareMoney >= _ClickClassData.m_Money)
        {
            m_SetClassPanel.MoneyWarningMessage.SetActive(false);
            m_SetClassPanel.SetClassMoney(Color.white, _ClickClassData.m_Money.ToString());
        }
        else
        {
            m_SetClassPanel.MoneyWarningMessage.SetActive(true);
            m_SetClassPanel.SetClassMoney(Color.red, _ClickClassData.m_Money.ToString());
        }

        if (_ClickProfessorData != null)
        {
            if (_compareHealth <= _ClickProfessorData.m_ProfessorHealth)
            {
                m_SetClassPanel.HealthWarningMessage.SetActive(false);
                m_SetClassPanel.SetProfessorHealth(Color.white);
            }
            else
            {
                m_SetClassPanel.HealthWarningMessage.SetActive(false);
                m_SetClassPanel.SetProfessorHealth(Color.red);
            }

            // ���� ������ ������ �ִٸ� �� ��ư�� ������ ���·� ������ش�.
            for (int i = 0; i < m_SetClassPanel.ProfessorPrefabParent.childCount; i++)
            {
                if (m_SetClassPanel.ProfessorPrefabParent.GetChild(i).name == _ClickProfessorData.m_ProfessorNameValue)
                {
                    ColorBlock _buttonColor =
                        m_SetClassPanel.ProfessorPrefabParent.GetChild(i).GetComponent<Button>().colors;

                    _buttonColor.normalColor = m_HighLightColor;
                    _buttonColor.highlightedColor = m_HighLightColor;
                    _buttonColor.pressedColor = m_HighLightColor;
                    _buttonColor.selectedColor = m_HighLightColor;

                    m_SetClassPanel.ProfessorPrefabParent.GetChild(i).GetComponent<Button>().colors = _buttonColor;

                    break;
                }
            }
        }

        // AllInfo ���Ⱥκ� ����
        SetStat(_currentObj);

        if (m_SetClassPanel.WeekText.text == "1����")
        {
            if (_ClickClassData.m_ClassType == StudentType.GameDesigner)
            {
                m_SetClassPanel.Set1WeekGMPanel(true, _ClickClassData.m_ClassName, _ClickClassData.m_Money.ToString());
            }
            else if (_ClickClassData.m_ClassType == StudentType.Art)
            {
                m_SetClassPanel.Set1WeekArtPanel(true, _ClickClassData.m_ClassName, _ClickClassData.m_Money.ToString());
            }
            else if (_ClickClassData.m_ClassType == StudentType.Programming)
            {
                m_SetClassPanel.Set1WeekProgrammingPanel(true, _ClickClassData.m_ClassName,
                    _ClickClassData.m_Money.ToString());
            }
            else if (_ClickClassData.m_ClassType == StudentType.None)
            {
                if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.GameDesigner])
                {
                    m_SetClassPanel.Set1WeekGMPanel(true, _ClickClassData.m_ClassName,
                        _ClickClassData.m_Money.ToString());
                }
                else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Art])
                {
                    m_SetClassPanel.Set1WeekArtPanel(true, _ClickClassData.m_ClassName,
                        _ClickClassData.m_Money.ToString());
                }
                else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Programming])
                {
                    m_SetClassPanel.Set1WeekProgrammingPanel(true, _ClickClassData.m_ClassName,
                        _ClickClassData.m_Money.ToString());
                }
            }

            m_SaveData.m_Week = "1";
            SaveMoneySelectPaenl(m_SetClassPanel._1WeekPanel, 0);
            SaveHealthSelectPanel(m_SetClassPanel._1WeekPanel, 0, _ClickClassData.m_Health);
        }
        else
        {
            if (_ClickClassData.m_ClassType == StudentType.GameDesigner)
            {
                m_SetClassPanel._1WeekPanel.SetActive(false);
                m_SetClassPanel._2WeekPanel.SetActive(true);
                m_SetClassPanel.SetUpButton(true);
                m_SetClassPanel.Set2WeekGMPanel(true, _ClickClassData.m_ClassName, _ClickClassData.m_Money.ToString());
            }
            else if (_ClickClassData.m_ClassType == StudentType.Art)
            {
                m_SetClassPanel.Set2WeekArtPanel(true, _ClickClassData.m_ClassName, _ClickClassData.m_Money.ToString());
            }
            else if (_ClickClassData.m_ClassType == StudentType.Programming)
            {
                m_SetClassPanel.Set2WeekProgrammingPanel(true, _ClickClassData.m_ClassName,
                    _ClickClassData.m_Money.ToString());
            }
            else if (_ClickClassData.m_ClassType == StudentType.None)
            {
                if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.GameDesigner])
                {
                    m_SetClassPanel.Set2WeekGMPanel(true, _ClickClassData.m_ClassName,
                        _ClickClassData.m_Money.ToString());
                }
                else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Art])
                {
                    m_SetClassPanel.Set2WeekArtPanel(true, _ClickClassData.m_ClassName,
                        _ClickClassData.m_Money.ToString());
                }
                else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Programming])
                {
                    m_SetClassPanel.Set2WeekProgrammingPanel(true, _ClickClassData.m_ClassName,
                        _ClickClassData.m_Money.ToString());
                }
            }

            m_SaveData.m_Week = "2";
            SaveMoneySelectPaenl(m_SetClassPanel._2WeekPanel, 3);
            SaveHealthSelectPanel(m_SetClassPanel._2WeekPanel, 3, _ClickClassData.m_Health);
        }

        for (int i = 0; i < m_ClassMoney.Length; i++)
        {
            m_TotalMoney += m_ClassMoney[i];
        }

        for (int i = 0; i < m_ClassHealth.Length; i++)
        {
            m_TotalHealth += m_ClassHealth[i];
        }

        if (m_TotalMoney > m_CurrentMoney)
        {
            m_SetClassPanel.CurrentMoneyWarningMessage.SetActive(true);
            m_SetClassPanel.TotalMoney(Color.red, m_TotalMoney.ToString());
            // totalmoney 0, 0�� Text ������ ������ֱ�
        }
        else
        {
            m_SetClassPanel.CurrentMoneyWarningMessage.SetActive(false);
            m_SetClassPanel.TotalMoney(Color.white, m_TotalMoney.ToString());
        }

        m_SetClassPanel.TotalHealth(m_TotalHealth.ToString());

        if (PlayerInfo.Instance.IsFirstClassSetting && m_TutorialCount == 7)
        {
            m_SetClassPanel.ClassScrollView.vertical = true;
            m_Unmask.fitTarget = m_SetClassPanel.CompleteButton.gameObject.GetComponent<RectTransform>();
            m_SetClassPanel.CompleteButton.enabled = false;
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(true);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-700, 200, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }

        m_SaveData.m_SelectClassDataSave = _ClickClassData;
    }

    private void ClickProfessor()
    {
        #region _��ư�� Ŭ������ �� ���̶���Ʈ�� �����ְ� �ϱ�

        if (_prevProfessor != null)
        {
            Button _prevButton = _prevProfessor.GetComponent<Button>();

            ColorBlock _prevButtonColor = _prevButton.colors;

            _prevButtonColor = ColorBlock.defaultColorBlock;

            _prevButton.colors = _prevButtonColor;
        }

        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        if (_currentObj == null)
        {
            return;
        }

        _prevProfessor = _currentObj;

        Button _currentButton = _currentObj.GetComponent<Button>();

        ColorBlock _currentButtonColor = _currentButton.colors;

        _currentButtonColor.normalColor = m_HighLightColor;
        _currentButtonColor.highlightedColor = m_HighLightColor;
        _currentButtonColor.pressedColor = m_HighLightColor;
        _currentButtonColor.selectedColor = m_HighLightColor;

        _currentButton.colors = _currentButtonColor;

        #endregion

        m_SelectProfessor.professorData.TryGetValue(_currentObj.name, out _ClickProfessorData);

        m_SetClassPanel.SetProfessorInfo(_ClickProfessorData.m_ProfessorNameValue,
            _ClickProfessorData.m_TeacherProfileImg);

        magnification = m_NowPlayerProfessor.m_StatMagnification[_ClickProfessorData.m_ProfessorPower];

        if (_ClickClassData != null)
        {
            int _compareHealth = _ClickClassData.m_Health;

            if (_compareHealth <= _ClickProfessorData.m_ProfessorHealth)
            {
                m_SetClassPanel.HealthWarningMessage.SetActive(false);
                m_SetClassPanel.SetProfessorHealth(Color.white);
            }
            else
            {
                m_SetClassPanel.HealthWarningMessage.SetActive(true);
                m_SetClassPanel.SetProfessorHealth(Color.red);
            }

            InitStatArr();

            FindStatAllInfo();

            int StatCount = 0;

            for (int i = 0; i < 5; i++)
            {
                if (m_AllInfoStatNum[i] != 0)
                {
                    StatCount += 1;
                }
            }

            if (StatCount == 5)
            {
                foreach (GameObject _stat in m_SetClassPanel.AllInfoPanelClassStat)
                {
                    _stat.SetActive(false);
                }

                m_SetClassPanel.SetStatsActive(true);
                m_SetClassPanel.SetAllStatText((m_StatNum[0]).ToString());
            }
            else
            {
                m_SetClassPanel.SetStatsActive(false);

                for (int i = 0; i < StatCount; i++)
                {
                    m_SetClassPanel.AllInfoPanelClassStat[i].SetActive(true);
                    m_SetClassPanel.AllInfoPanelClassStatText[i].text =
                        (m_AllInfoStatNum[i] * (int)magnification).ToString();
                    Sprite _statSprite = SetStatImage(m_AllInfoStatName[i]);
                    m_SetClassPanel.AllInfoPanelClassStatImage[i].sprite = _statSprite;
                }
            }

            // ���� ������ ������ �ִٸ� �� ��ư�� ������ ���·� ������ش�.
            for (int i = 0; i < m_SetClassPanel.ClassPrefabParent.childCount; i++)
            {
                if (m_SetClassPanel.ClassPrefabParent.GetChild(i).name == _ClickClassData.m_ClassName)
                {
                    ColorBlock _buttonColor =
                        m_SetClassPanel.ClassPrefabParent.GetChild(i).GetComponent<Button>().colors;

                    _buttonColor.normalColor = m_HighLightColor;
                    _buttonColor.highlightedColor = m_HighLightColor;
                    _buttonColor.pressedColor = m_HighLightColor;
                    _buttonColor.selectedColor = m_HighLightColor;

                    m_SetClassPanel.ClassPrefabParent.GetChild(i).GetComponent<Button>().colors = _buttonColor;

                    break;
                }
            }
        }

        if (PlayerInfo.Instance.IsFirstClassSetting && m_TutorialCount == 4)
        {
            m_Unmask.fitTarget = m_SetClassPanel.ClassScrollView.GetComponent<RectTransform>();
            m_SetClassPanel.ClassPrefabParent.GetChild(0).GetComponent<Button>().interactable = false;
            m_SetClassPanel.ClassPrefabParent.GetChild(1).GetComponent<Button>().interactable = false;
            m_SetClassPanel.ClassPrefabParent.GetChild(2).GetComponent<Button>().interactable = false;
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(true);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-700, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
            m_SetClassPanel.ProfessorScrollView.vertical = true;
            m_SetClassPanel.ClassScrollView.vertical = false;
        }

        m_SaveData.m_SelectProfessorDataSave = _ClickProfessorData;
    }

    // �ڷΰ��� ��ư�� ������ �� ���� ������ ������ �����͸� ������ֱ� ���� ������ �������� �����ش�.
    private void RemovalBeforData(List<SaveSelectClassInfoData> _data, int _removeIndex)
    {
        SaveSelectClassInfoData _ClearData = new SaveSelectClassInfoData();

        if (m_SetClassPanel.ClassPrefabParent.childCount > 0)
        {
            m_SetClassPanel.DestroySelectClassObject();
        }

        if (m_SetClassPanel.ProfessorPrefabParent.childCount > 0)
        {
            m_SetClassPanel.DestroySelectPanelProfessorObject();
        }

        InitStatArr();

        if (m_WeekClassIndex == 1 && m_WeekProfessorIndex == 1 && m_IsChangeWeekend == false)
        {
            m_IsChangeWeekend = true;
            m_SetClassPanel.SetWeekText("2����");
            m_SetClassPanel._2WeekPanel.SetActive(true);
            m_SaveDataIndex++;
        }

        m_SaveData.m_SelectClassDataSave = null;
        m_SaveData.m_SelectProfessorDataSave = null;
        _ClickProfessorData = null;
        _ClickClassData = null;

        _data.Add(_ClearData);
        ChangeListIndex(_data, _removeIndex, _ClearData);
        _data.RemoveAt(2);
    }

    // �ڷΰ��� ��ư�� ������ �� ���� ������ �����ߴ� ������ ��� �����صδ� ���
    private void BackSaveData(List<SaveSelectClassInfoData> _data, int _index)
    {
        m_TempSaveData.m_SelectClassDataSave = _data[_index].m_SelectClassDataSave;
        m_TempSaveData.m_SelectProfessorDataSave = _data[_index].m_SelectProfessorDataSave;
        m_TempSaveData.m_Week = _data[_index].m_Week;
    }

    /// TODO : ���� 1���� �г��� �������� �� 2���� �������ÿ��� �ڷΰ��� ��ư�� ������ �ٷ� 2���� �г� �����ֱ�
    // ���� ���� �ִ� ������ �������� ��������Ѵ�.
    private void CheckClassToBackButton()
    {
        switch (m_ClassStack)
        {
            case 1: // 1���� ��Ʈ, 1���� ��ȹ���� �����س�����
            {
                ResetSelectPanel(m_GameDesignerData[0].m_SelectClassDataSave,
                    m_GameDesignerData[0].m_SelectProfessorDataSave);
                BackSaveData(m_GameDesignerData, 0);
                RemovalBeforData(m_GameDesignerData, 0);

                RestPanel(m_SetClassPanel._1WeekPanel, 2);

                MakeClass();
                MakeProfessor();

                // ������ �� ����� �� �ٽ� ���� ������ �����ߴ� ������ �־��ش�.
                m_SaveData = m_TempSaveData;
            }
                break;

            case 2: // 1���� �ù�
            {
                ResetSelectPanel(m_ArtData[0].m_SelectClassDataSave, m_ArtData[0].m_SelectProfessorDataSave);
                BackSaveData(m_ArtData, 0);

                RemovalBeforData(m_ArtData, 0);
                RestPanel(m_SetClassPanel._1WeekPanel, 3);

                MakeOtherClass();
                MakeOtherProfessor();

                m_SaveData = m_TempSaveData;
            }
                break;

            case 3: // 2���� ��ȹ
            {
                // �Ʒ� 6������ 2�������� �ٽ� 1������ �Ѿ�� ������ ���⼭�� ���ִ� ��
                m_WeekClassIndex = 0;
                m_WeekProfessorIndex = 0;
                m_IsChangeWeekend = false;
                m_SetClassPanel.SetWeekText("1����");
                m_SetClassPanel._1WeekPanel.SetActive(true);
                m_SetClassPanel._2WeekPanel.SetActive(false);
                m_SetClassPanel.SetUpButton(false);
                m_SaveDataIndex = 0;

                // ���� 1���� �г��� �������ε� 2���� ��ȹ���� �ڷΰ��� ��ư�� ������ �Ʒ� ȭ��ǥ ��ư�� ��������Ѵ�.
                if (m_SetClassPanel._1WeekPanel.activeSelf)
                {
                    m_SetClassPanel.SetDownButton(false);
                }

                ResetSelectPanel(m_ProgrammingData[0].m_SelectClassDataSave,
                    m_ProgrammingData[0].m_SelectProfessorDataSave);
                BackSaveData(m_ProgrammingData, 0);

                RemovalBeforData(m_ProgrammingData, 0);
                RestPanel(m_SetClassPanel._2WeekPanel, 1);

                MakeOtherClass();
                MakeOtherProfessor();

                m_SaveData = m_TempSaveData;
            }
                break;

            case 4: // 2���� ��Ʈ
            {
                ResetSelectPanel(m_GameDesignerData[1].m_SelectClassDataSave,
                    m_GameDesignerData[1].m_SelectProfessorDataSave);
                BackSaveData(m_GameDesignerData, 1);

                RemovalBeforData(m_GameDesignerData, 1);

                if (m_SetClassPanel._1WeekPanel.activeSelf)
                {
                    m_SetClassPanel._1WeekPanel.SetActive(false);
                    m_SetClassPanel.SetDownButton(false);
                    m_SetClassPanel._2WeekPanel.SetActive(true);
                    m_SetClassPanel.SetUpButton(true);
                }

                RestPanel(m_SetClassPanel._2WeekPanel, 2);

                MakeClass();
                MakeProfessor();

                m_SaveData = m_TempSaveData;
            }
                break;

            case 5: // 2���� �ù�
            {
                ResetSelectPanel(m_ArtData[1].m_SelectClassDataSave, m_ArtData[1].m_SelectProfessorDataSave);
                BackSaveData(m_ArtData, 1);

                RemovalBeforData(m_ArtData, 1);

                if (m_SetClassPanel._1WeekPanel.activeSelf)
                {
                    m_SetClassPanel._1WeekPanel.SetActive(false);
                    m_SetClassPanel.SetDownButton(false);
                    m_SetClassPanel._2WeekPanel.SetActive(true);
                    m_SetClassPanel.SetUpButton(true);
                }

                RestPanel(m_SetClassPanel._2WeekPanel, 3);

                MakeOtherClass();
                MakeOtherProfessor();

                m_SaveData = m_TempSaveData;
            }
                break;
        }
    }

    // SelectPanel�� �ִ� 1������ 2������ �������� �ʱ�ȭ �����ִ� �Լ�
    public void InitTotalInfo(GameObject _panel)
    {
        for (int i = 0; i < _panel.transform.childCount - 1; i++)
        {
            if (_panel.transform.GetChild(i + 1).gameObject.activeSelf == true)
            {
                _panel.transform.GetChild(i + 1).GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                _panel.transform.GetChild(i + 1).gameObject.SetActive(false);
            }
        }
    }

    // �ڷΰ��� ��ư�� ������ �� ������ �Լ�
    public void ClickBackButton()
    {
        CheckClassToBackButton();

        m_SetClassPanel.m_ClassPrefab.anchoredPosition = new Vector2(7f, 0f);
        m_SetClassPanel.m_ProfessorPrefab.anchoredPosition = new Vector2(7f, 0f);

        if (m_ClassStack > 0)
        {
            m_ClassStack--;
        }
    }

    // ���� ��ư�� ������ ��
    public void ClickHideButton()
    {
        //// Ȯ���ϴ� ȭ���̾��ٸ� CheckPanel�� ���ش�.
        if (m_CheckClassPanel.gameObject.activeSelf == true)
        {
            m_CheckClassPanel.gameObject.SetActive(false);
            m_OpenClassButton.gameObject.SetActive(true);
        }
        else if (m_SetClassPanel.gameObject.activeSelf == true)
        {
            m_SetClassPanel.gameObject.SetActive(false);
            m_OpenClassButton.gameObject.SetActive(true);
        }
    }

    // �������⸦ ������ �� ���� �������̾��ٸ� ���� ����â����, ���� Ȯ�����̾��ٸ� Ȯ��â����, �������̾��ٸ� �����ϴ� â �����ֱ�
    public void OpenClassButton()
    {
        if (m_ArtData[1].m_SelectClassDataSave != null && m_GameDesignerData[1].m_SelectClassDataSave != null &&
            m_ProgrammingData[1].m_SelectClassDataSave != null
            && m_SetClassPanel.ModifiyButton.gameObject.activeSelf == true)
        {
            m_SetClassPanel.gameObject.SetActive(true);
            m_OpenClassButton.gameObject.SetActive(false);
            m_SetClassPanel.SetModifiyButton(true);
        }
        else if (m_ArtData[1].m_SelectClassDataSave != null && m_GameDesignerData[1].m_SelectClassDataSave != null &&
                 m_ProgrammingData[1].m_SelectClassDataSave != null)
        {
            m_CheckClassPanel.gameObject.SetActive(true);
            m_OpenClassButton.gameObject.SetActive(false);
        }
        else
        {
            m_SetClassPanel.gameObject.SetActive(true);
            m_OpenClassButton.gameObject.SetActive(false);
        }
    }

    private void ContinueTutorial()
    {
        if (PlayerInfo.Instance.IsFirstClassSetting && m_TutorialCount == 12)
        {
            Time.timeScale = 0;
            m_PDAlarm.SetActive(true);
            m_Unmask.gameObject.SetActive(false);
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_BlackScreen.SetActive(false);
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
    }

    public int CalculateSalary()
    {
        int _totalSalary = 0;

        for (int i = 0; i < 2; i++)
        {
            _totalSalary += m_ArtData[i].m_SelectProfessorDataSave.m_ProfessorPay;
            _totalSalary += m_GameDesignerData[i].m_SelectProfessorDataSave.m_ProfessorPay;
            _totalSalary += m_ProgrammingData[i].m_SelectProfessorDataSave.m_ProfessorPay;
        }

        return _totalSalary;
    }
}