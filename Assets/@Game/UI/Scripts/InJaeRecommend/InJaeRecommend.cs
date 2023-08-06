using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;
using Coffee.UIExtensions;

public class InJaeRecommend : MonoBehaviour
{
    [SerializeField] private StatsRadar m_CompanyStatsRadar;
    [SerializeField] private StatsRadar m_StudentStatsRadar;
    [SerializeField] private GameObject m_Alram;
    [SerializeField] private GameObject m_RecommendAni;

    [Space(5f)]
    [Header("������õâ�� ����� ��ư ������")]
    [SerializeField] private GameObject m_CompanyPrefab;
    [SerializeField] private GameObject m_RecommendPrefab;
    [SerializeField] private GameObject m_RequiredSkillPrefab;
    [SerializeField] private GameObject m_StudentPrefab;

    [Space(5f)]
    [Header("������õȮ��â�� ����� �л� ������")]
    [SerializeField] private GameObject m_CheckPanelStudentPrefab;

    [Space(5f)]
    [Header("ȸ�� ��ũ�� ���� �ٲ��� �̹�����")]
    [SerializeField] private Sprite m_SRankSprite;
    [SerializeField] private Sprite m_ARankSprite;
    [SerializeField] private Sprite m_BRankSprite;
    [SerializeField] private Sprite m_CRankSprite;
    [Space(5f)]

    [Space(5f)]
    [Header("�л��� ������ �䱸���ȿ� �����ϴ����� ���� �̹�����")]
    [SerializeField] private Sprite m_SatisfiedSprite;
    [SerializeField] private Sprite m_NotSatisfiedSprite;
    [Space(5f)]

    [Space(5f)]
    [Header("��õ Ȯ��â���� ���õ� �л����� �հݷ��� ���� �̹�����")]
    [SerializeField] private Sprite m_UpToEightyPercentSprite;
    [SerializeField] private Sprite m_UpToSixtyPercentSprite;
    [SerializeField] private Sprite m_UpToFourtyPercentSprite;
    [SerializeField] private Sprite m_UpToZeroPercentSprite;

    [Space(5f)]
    [Header("���â�� ����� �հ�, ���հ� �̹���")]
    [SerializeField] private Sprite m_Pass;
    [SerializeField] private Sprite m_Fail;

    [SerializeField] private Sprite m_DetailPass;
    [SerializeField] private Sprite m_DetailFail;

    [SerializeField] private InJaeRecommendList m_RecommendListPanel;
    [SerializeField] private InJaeRecommendInfo m_RecommendInfoPanel;
    [SerializeField] private InJaeRecommendResult m_RecommendResultPanel;
    [SerializeField] private Sprite[] m_PartImage;

    [Space(5f)]
    [Header("Ʃ�丮���")]
    [SerializeField] private GameObject m_TutorialPanel;
    [SerializeField] private Unmask m_Unmask;
    [SerializeField] private Image m_TutorialTextImage;
    [SerializeField] private Image m_TutorialArrowImage;
    [SerializeField] private TextMeshProUGUI m_TutorialText;
    [SerializeField] private Button m_NextButton;
    [SerializeField] private GameObject m_FoldButton;
    [SerializeField] private GameObject m_RecommendButton;
    [SerializeField] private GameObject m_BlackScreen;
    [SerializeField] private GameObject m_PDAlarm;
    [SerializeField] private TextMeshProUGUI m_AlarmText;

    private bool m_IsRecommendCreation;                                             // ���� 2���� ���� �� �ѹ��� ������ֱ� ���ؼ�
    private bool m_IsRecommendFalseCheck;                                           // ���� 2�� 3���� 3���� ���� �� �л� �Ѹ��̶� �����õ�� ���ߴٸ� �ѹ��� �г��� ���� ����
    //private bool m_IsRecommendResult;                                               // ���� 2�� 4���� 5���� ���� �� �л����� �հ� ���� �г��� �ѹ��� ����ַ���
    private bool _isPass;
    private int m_CompanySense;                                                   // ȸ�簡 �䱸�ϴ� ��ġ��
    private int m_CompanyConcentration;                                           //
    private int m_CompanyWit;                                                     //
    private int m_CompanyTechinque;                                               // 
    private int m_CompanyInsight;                                                 //
    private int m_NowEmploymentIndex;                                               // ���� ������ ������ m_SortCompany���ִ� EmploymentList�� �ε���
    private string m_CompanyName;                                                   // ���� ������ ȸ���� �̸��� string������ ������ �ִ´�
    private int m_NowRecommendCount;
    private double Average;                                                         // ���� ����� Ȯ��
    private string[] GameDesignerStudentName = new string[6];                                 // ���� ������ �л����� �̸�
    private string[] ArtStudentName = new string[6];                                //
    private string[] ProgrammingStudentName = new string[6];                        //
    private Sprite[] GameDesignerStudentImgae = new Sprite[6];                                 // ���� ������ �л����� �̸�
    private Sprite[] ArtStudentImgae = new Sprite[6];                                //
    private Sprite[] ProgrammingStudentImgae = new Sprite[6];

    public List<Company> m_CompanyList = new List<Company>();                       // json���� ���� �����͵��� ȸ�翡 ���� �з����ֱ� ���� ����Ʈ
    private List<Company> m_SortCompany = new List<Company>();                      // ȸ���� ������� �������ִ� ����Ʈ
    private List<Student> m_SelectedStudent = new List<Student>();                  // ���� ������ �л����� ������ �ӽ÷� ������ ����Ʈ
    private List<double> m_SelectedStudentPercent = new List<double>();             // ���� ������ �л��� ���� ���������� �����ߴ� �л��� �����͸� ���������.
    private List<string> m_CompanyRequiredSkillName = new List<string>();            // ���� ������ ȸ���� �䱸 ��ų �̸� ���
    private Sprite[] GameDesignerStudentPassFail = new Sprite[6];                             // ���� �����ߴ� �л����� �հ�, ���հ� ���θ� ������ ��������Ʈ�� �־�����Ѵ�.
    private Sprite[] ArtStudentPassFail = new Sprite[6];
    private Sprite[] ProgrammingStudentPassFail = new Sprite[6];
    private GameObject[] m_SkillPrefab = new GameObject[3];                         // ������õ�� �ʿ��� ��ų ������Ʈ�� ���� �� ���⿡ ��Ƶ״ٰ� ȭ���� ���� �� ��������Ѵ�.

    private int m_PassStudentCount;

    private int m_TutorialCount;
    private int m_ScriptCount;

    private int m_RecommendScript1Count;
    private int m_RecommendScript2Count;
    private int m_RecommendResult;

    private bool m_IsScript2Play;

    private void Start()
    {
        m_IsRecommendCreation = false;
        m_IsRecommendFalseCheck = false;
        m_NowRecommendCount = 0;

        FillCompanyList(); // ������õ �����͸� �ִ� �Լ�

        m_RecommendInfoPanel.RecommendButton.onClick.AddListener(ClickRecommendButtonToPart);
        m_RecommendInfoPanel.CheckPanelOKButton.onClick.AddListener(ClickCheckPanelOKButton);

        m_TutorialCount = 0;
        m_ScriptCount = 0;
        m_RecommendScript1Count = 0;
        m_RecommendScript2Count = 0;
        m_PassStudentCount = 0;
        m_IsScript2Play = false;

        m_NextButton.onClick.AddListener(TutorialContinue);
        m_NextButton.onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);
    }

    private void Update()
    {
        if (GameTime.Instance.FlowTime.NowMonth == 2 && !m_IsRecommendCreation)
        {
            m_Alram.SetActive(true);

            MakeCompanyButton();

            m_IsRecommendCreation = true;

            if (PlayerInfo.Instance.IsFirstInJaeRecommend && m_TutorialCount == 0)
                TutorialStart();
        }
        // �л��� �Ѹ��̶� ������õ�� ���ߴٸ� ������ ��õ ȭ�� ����ֱ�
        else if (GameTime.Instance.FlowTime.NowMonth == 2 && GameTime.Instance.FlowTime.NowWeek == 3
            && GameTime.Instance.FlowTime.NowDay == 3 && !m_IsRecommendFalseCheck)
        {
            for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
            {
                if (ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_IsRecommend == false)
                {
                    m_IsRecommendFalseCheck = true;
                    m_RecommendListPanel.PopUpRecommendListPanel();
                    m_RecommendListPanel.LockCloseButton(false);
                    m_RecommendInfoPanel.LockInfoPanelCloseButton(false);
                    break;
                }
            }
        }
        else if (GameTime.Instance.FlowTime.NowMonth == 2 && GameTime.Instance.FlowTime.NowWeek == 3
            && GameTime.Instance.FlowTime.NowDay == 5) // �������� ������ �ð��� ���ַ��� �ð�üũ�ؼ� ������Ѵ�.
        {
            m_RecommendAni.SetActive(false);
        }
        //else if (GameTime.Instance.FlowTime.NowMonth == 2 && GameTime.Instance.FlowTime.NowWeek == 4
        //    && GameTime.Instance.FlowTime.NowDay == 5 && !m_IsRecommendResult)
        //{
        //    m_IsRecommendResult = true;
        //    m_RecommendResultPanel.PopUpResultPanel();
        //}
        else if (GameTime.Instance.FlowTime.NowMonth == 2 && GameTime.Instance.FlowTime.NowWeek == 4 &&
                 GameTime.Instance.FlowTime.NowDay == 5 && m_RecommendScript1Count == 0 &&
                 GameTime.Instance.m_IsRecommendNotice)
        {
            //m_IsRecommendResult = true;
            Time.timeScale = 0;
            m_TutorialPanel.SetActive(true);
            m_BlackScreen.SetActive(true);
            m_Unmask.gameObject.SetActive(false);
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(false);
            //m_NextButton.gameObject.SetActive(true);
            m_PDAlarm.SetActive(true);
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().RecommendResult[0][m_RecommendScript1Count];
            m_RecommendScript1Count++;

            if (PlayerInfo.Instance.IsFirstVacation)
            {
                m_RecommendResult = 1;
            }
            else
            {
                // ��ü �л� �� �հ��� �л� ����
                float passPercent = (float)m_PassStudentCount / (float)18;
                if (passPercent >= 80)
                {
                    m_RecommendResult = 2;
                }
                else
                {
                    m_RecommendResult = 3;
                }
            }
        }
        else if (GameTime.Instance.FlowTime.NowMonth == 3)
        {
            m_IsRecommendCreation = false;
            m_IsRecommendFalseCheck = false;
            m_RecommendScript1Count = 0;
            m_RecommendScript2Count = 0;
            m_PassStudentCount = 0;
            m_NowRecommendCount = 0;
        }

//        if (PlayerInfo.Instance.IsFirstInJaeRecommend && m_TutorialCount > 0)
//        {
//#if UNITY_EDITOR
//            if (Input.GetMouseButtonDown(0))
//            {
//                TutorialContinue();
//            }
//#elif UNITY_ANDROID
//            if (Input.touchCount > 0)
//            {
//                Touch touch = Input.GetTouch(0);
//                if (touch.phase == TouchPhase.Ended)
//                {
//                    TutorialContinue();
//                }
//            }
//#endif
//        }

        if (GameTime.Instance.m_IsRecommendNotice && m_RecommendScript1Count > 0)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (!m_IsScript2Play)
                {
                    RecommendContinue();
                }
                else
                {
                    RecommendResultContinue();
                }
            }
#elif UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    if (!m_IsScript2Play)
                    {
                        RecommendContinue();
                    }
                    else
                    {
                        RecommendResultContinue();
                    }
                }
            }
#endif
        }
    }

    private void RecommendContinue()
    {
        if (m_RecommendScript1Count == 1)
        {
            Time.timeScale = 0;
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().RecommendResult[0][m_RecommendScript1Count];
            m_RecommendScript1Count++;
        }
        else if (m_RecommendScript1Count == 2)
        {
            Time.timeScale = 0;
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().RecommendResult[0][m_RecommendScript1Count];
            m_RecommendScript1Count++;
        }
        else if (m_RecommendScript1Count == 3)
        {
            Time.timeScale = 0;
            m_TutorialPanel.SetActive(false);
            m_RecommendResultPanel.PopUpResultPanel();
            m_RecommendResultPanel.OKButton.onClick.AddListener(RecommendResultContinue);
            m_RecommendScript1Count++;
        }
    }

    private void RecommendResultContinue()
    {
        if (m_RecommendScript2Count < m_TutorialPanel.GetComponent<Tutorial>().RecommendResult[m_RecommendResult].Count)
        {
            Time.timeScale = 0;
            m_IsScript2Play = true;
            m_TutorialPanel.SetActive(true);

            string nowScript =
                m_TutorialPanel.GetComponent<Tutorial>().RecommendResult[m_RecommendResult][m_RecommendScript2Count];

            int splitIndex1 = nowScript.IndexOf("[");
            int splitIndex2 = nowScript.IndexOf("]");
            int splitText = nowScript.IndexOf("��ī���̸�");

            if (splitIndex1 != -1 && splitText != -1)
            {
                char lastText = PlayerInfo.Instance.AcademyName.ElementAt(PlayerInfo.Instance.AcademyName.Length - 1);

                string selectText = (lastText - 0xAC00) % 28 > 0 ? "��" : "��";

                m_AlarmText.text = nowScript.Substring(0, splitIndex1) + PlayerInfo.Instance.AcademyName + selectText + nowScript.Substring(splitIndex2 + 2);
            }
            else
            {
                m_AlarmText.text = nowScript;
            }
            m_RecommendScript2Count++;
        }
        else
        {
            m_TutorialPanel.SetActive(false);
            m_IsScript2Play = true;
            GameTime.Instance.m_IsRecommendNotice = false;
            if (PlayerInfo.Instance.IsFirstVacation)
            {
                PlayerInfo.Instance.IsFirstVacation = false;
            }

            m_IsScript2Play = false;
            Time.timeScale = 1;
        }
    }

    private void TutorialStart()
    {
        Time.timeScale = 0;
        m_TutorialPanel.SetActive(true);
        m_NextButton.gameObject.SetActive(true);
        m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
        m_ScriptCount++;
        m_TutorialCount++;
    }

    private void TutorialContinue()
    {
        if (m_TutorialCount == 1)
        {
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 2)
        {
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 3)
        {
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 4)
        {
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 5)
        {
            if (m_FoldButton.GetComponent<PopUpUI>().isSlideMenuPanelOpend == false)
            {
                m_FoldButton.GetComponent<PopUpUI>().AutoSlideMenuUI();
            }

            m_BlackScreen.SetActive(true);
            m_PDAlarm.SetActive(false);
            m_Unmask.gameObject.SetActive(true);
            m_Unmask.fitTarget = m_RecommendButton.GetComponent<RectTransform>();
            m_TutorialTextImage.gameObject.SetActive(false);
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 6)
        {
            m_RecommendListPanel.PopUpRecommendListPanel();
            m_RecommendListPanel.Companycontent.vertical = false;
            m_RecommendListPanel.CompanyContentTransform.GetChild(0).GetComponent<Button>().interactable = false;
            m_RecommendListPanel.CompanyContentTransform.GetChild(1).GetComponent<Button>().interactable = false;
            m_RecommendListPanel.CompanyContentTransform.GetChild(2).GetComponent<Button>().interactable = false;
            m_RecommendListPanel.CompanyContentTransform.GetChild(3).GetComponent<Button>().interactable = false;
            m_RecommendListPanel.CompanyContentTransform.GetChild(4).GetComponent<Button>().interactable = false;

            m_Unmask.fitTarget = m_RecommendListPanel.CompanyListRect;
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(true);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(450, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 7)
        {
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 8)
        {
            m_RecommendListPanel.CompanyContentTransform.GetChild(0).GetComponent<Button>().interactable = true;
            m_RecommendListPanel.CompanyContentTransform.GetChild(1).GetComponent<Button>().interactable = true;
            m_RecommendListPanel.CompanyContentTransform.GetChild(2).GetComponent<Button>().interactable = true;
            m_RecommendListPanel.CompanyContentTransform.GetChild(3).GetComponent<Button>().interactable = true;
            m_RecommendListPanel.CompanyContentTransform.GetChild(4).GetComponent<Button>().interactable = true;

            m_Unmask.fitTarget = m_RecommendListPanel.CompanyContentTransform.GetChild(0).GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 150, 0);
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 10)
        {
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 11)
        {
            m_RecommendListPanel.RecommendContentTransform.GetChild(0).GetComponent<Button>().interactable = true;
            m_RecommendListPanel.RecommendContentTransform.GetChild(1).GetComponent<Button>().interactable = true;
            //m_RecommendListPanel.RecommendContentTransform.GetChild(2).GetComponent<Button>().interactable = true;
            //m_RecommendListPanel.RecommendContentTransform.GetChild(3).GetComponent<Button>().interactable = true;
            //m_RecommendListPanel.RecommendContentTransform.GetChild(4).GetComponent<Button>().interactable = true;

            m_Unmask.fitTarget = m_RecommendListPanel.RecommendContentTransform.GetChild(0).GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 150, 0);
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 13)
        {
            m_Unmask.fitTarget = m_RecommendInfoPanel.PartSkillInfoRect;
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(550, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 14)
        {
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 15)
        {
            m_Unmask.fitTarget = m_RecommendInfoPanel.StatInfoRect;
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-500, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 16)
        {
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 17)
        {
            m_Unmask.fitTarget = m_RecommendInfoPanel.StudentListRect;
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-550, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 18)
        {
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 19)
        {
            m_Unmask.fitTarget = m_RecommendInfoPanel.PercentRect;
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-350, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 20)
        {
            m_Unmask.gameObject.SetActive(false);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            //m_TutorialTextImage.transform.position = new Vector3(2340f / 2f, 1080f / 2f, 0);
            m_TutorialTextImage.transform.position = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 21)
        {
            m_TutorialPanel.SetActive(false);
            m_NextButton.gameObject.SetActive(false);
            m_TutorialCount++;
            PlayerInfo.Instance.IsFirstInJaeRecommend = false;
        }
    }

    // ��� ȸ��� ������� ȸ�� �̸��� �������� ������ش�.
    private void FillCompanyList()
    {
        for (int i = 0; i < AllOriginalJsonData.Instance.OriginalInJaeRecommendData.Count; i++)
        {
            if (m_CompanyList.Find(x => x.CompanyName == AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyName) == null)
            {
                Company _newCompany = new Company();
                _newCompany.CompanyID = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyID;
                _newCompany.CompanyName = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyName;
                _newCompany.CompanyGrade = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyGrade_ID;
                _newCompany.CompanyGameNameID = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyGameNameID;
                _newCompany.IsNewCompany = true;
                _newCompany.IsUnLockCompany = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyUnLock;
                _newCompany.CompanyIcon = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyIcon;

                _newCompany.EmploymentList = new List<Employment>();

                Employment _newEmployment = new Employment();
                _newEmployment.EmploymentID = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentID;
                _newEmployment.EmploymentName = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentName;
                _newEmployment.EmploymentText = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentText;
                _newEmployment.CompanyJob = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyJob;
                _newEmployment.CompanySalary = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanySalary;
                _newEmployment.CompanyPart = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyPart;
                string[] _tempEmploymentYear = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentYear.Split(",");
                _newEmployment.EmploymentYear = new int[_tempEmploymentYear.Length];

                for (int j = 0; j < _tempEmploymentYear.Length; j++)
                {
                    _newEmployment.EmploymentYear[j] = int.Parse(_tempEmploymentYear[j]);
                }

                if (AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementSkill_ID.Contains(","))
                {
                    string[] _tempSkill = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementSkill_ID.Split(",");
                    _newEmployment.CompanyRequirementSkill = new int[_tempSkill.Length];

                    for (int j = 0; j < _tempSkill.Length; j++)
                    {
                        _newEmployment.CompanyRequirementSkill[j] = int.Parse(_tempSkill[j]);
                    }
                }
                else
                {
                    _newEmployment.CompanyRequirementSkill = new int[1];
                    _newEmployment.CompanyRequirementSkill[0] = int.Parse(AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementSkill_ID);
                }
                _newEmployment.FailStudent = new List<Student>();
                _newEmployment.PassStudent = new List<Student>();
                _newEmployment.CompanyRequirementStats = new Dictionary<string, int>();

                _newEmployment.CompanyRequirementStats = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementStats;
                _newCompany.EmploymentList.Add(_newEmployment);
                m_CompanyList.Add(_newCompany);
            }
            else
            {
                Company _nowCompany = m_CompanyList.Find(x => x.CompanyName == AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyName);

                Employment _newEmployment = new Employment();
                _newEmployment.EmploymentID = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentID;
                _newEmployment.EmploymentName = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentName;
                _newEmployment.EmploymentText = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentText;
                _newEmployment.CompanyJob = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyJob;
                _newEmployment.CompanySalary = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanySalary;
                _newEmployment.CompanyPart = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyPart;

                string[] _tempEmploymentYear = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentYear.Split(",");
                _newEmployment.EmploymentYear = new int[_tempEmploymentYear.Length];

                for (int j = 0; j < _tempEmploymentYear.Length; j++)
                {
                    _newEmployment.EmploymentYear[j] = int.Parse(_tempEmploymentYear[j]);
                }

                if (AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementSkill_ID.Contains(","))
                {
                    string[] _tempSkill = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementSkill_ID.Split(",");
                    _newEmployment.CompanyRequirementSkill = new int[_tempSkill.Length];

                    for (int j = 0; j < _tempSkill.Length; j++)
                    {
                        _newEmployment.CompanyRequirementSkill[j] = int.Parse(_tempSkill[j]);
                    }
                }
                else
                {
                    _newEmployment.CompanyRequirementSkill = new int[1];
                    _newEmployment.CompanyRequirementSkill[0] = int.Parse(AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementSkill_ID);
                }
                _newEmployment.FailStudent = new List<Student>();
                _newEmployment.PassStudent = new List<Student>();
                _newEmployment.CompanyRequirementStats = new Dictionary<string, int>();

                _newEmployment.CompanyRequirementStats = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementStats;
                _nowCompany.EmploymentList.Add(_newEmployment);
            }
        }
    }

    // ȸ�� ��ư�� ������ش�.
    public void MakeCompanyButton()
    {
        Transform _companyparent = m_RecommendListPanel.CompanyContent();

        if (_companyparent.childCount != 0)
        {
            m_RecommendListPanel.DestroyCompanyContent();
        }

        CheckConditionToMakeCompany();
    }

    // ȸ�� �̸����� ���� ��ư�� ������ ���� �������� �� �����. ȸ�� ��ũ �̹����� �־�����Ѵ�.
    private void CheckConditionToMakeCompany()
    {
        // ������ �ְ� ���� ���ǿ� ���� �������ֱ�. ������ ����� �ٽ� ������ ���ֱ� ���� ���⿡ ����
        m_SortCompany = SortCompanyToConditions();

        for (int i = 0; i < m_SortCompany.Count; i++)
        {
            // ���� ������ �ش� ��ư�� ������ ������ �� ���� �������ֱ�
            if (m_SortCompany[i].IsUnLockCompany && CheckEmployment(m_SortCompany[i].EmploymentList) == true)
            {
                Transform _companyButtonParent = m_RecommendListPanel.CompanyContent();
                GameObject _companyButton = Instantiate(m_CompanyPrefab, _companyButtonParent);

                _companyButton.name = m_SortCompany[i].CompanyName;
                _companyButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_SortCompany[i].CompanyName;

                if (m_SortCompany[i].IsNewCompany)
                {
                    _companyButton.transform.GetChild(2).gameObject.SetActive(true);
                }
                else
                {
                    _companyButton.transform.GetChild(2).gameObject.SetActive(false);
                }

                switch (m_SortCompany[i].CompanyGrade)
                {
                    case 0: _companyButton.transform.GetChild(0).GetComponent<Image>().sprite = m_SRankSprite; break;
                    case 1: _companyButton.transform.GetChild(0).GetComponent<Image>().sprite = m_ARankSprite; break;
                    case 2: _companyButton.transform.GetChild(0).GetComponent<Image>().sprite = m_BRankSprite; break;
                    case 3: _companyButton.transform.GetChild(0).GetComponent<Image>().sprite = m_CRankSprite; break;
                }
                _companyButton.GetComponent<Button>().onClick.AddListener(MakeRecommend);
            }
        }
    }

    private bool CheckEmployment(List<Employment> _employment)
    {
        for (int i = 0; i < _employment.Count; i++)
        {
            for (int j = 0; j < _employment[i].EmploymentYear.Length; j++)
            {
                if (_employment[i].EmploymentYear[j] == 0 || _employment[i].EmploymentYear[j] == GameTime.Instance.FlowTime.NowYear)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // �ش� ȸ�簡 ������ �ִ� ������� �����ִ� �Լ�. 
    private void MakeRecommend()
    {
        m_RecommendListPanel.DestroyRecommendList();

        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        if (_currentObj == null)
        {
            return;
        }

        m_CompanyName = _currentObj.name;

        List<Employment> _sortListToPart = SortRecommendToConditions();

        for (int i = 0; i < _sortListToPart.Count; i++)
        {
            for (int j = 0; j < _sortListToPart[i].EmploymentYear.Length; j++)
            {
                if (_sortListToPart[i].EmploymentYear[j] == 0 || _sortListToPart[i].EmploymentYear[j] == GameTime.Instance.FlowTime.NowYear)
                {
                    Transform _recommendButtonParent = m_RecommendListPanel.RecommendeContent();
                    GameObject _recommendButton = Instantiate(m_RecommendPrefab, _recommendButtonParent);

                    _recommendButton.name = _sortListToPart[i].EmploymentName;
                    _recommendButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _sortListToPart[i].EmploymentName;
                    // ������ ��Ʈ �̹����� �־��ִ� �κ�
                    if (_sortListToPart[i].CompanyPart == 1)
                    {
                        _recommendButton.transform.GetChild(1).GetComponent<Image>().sprite = m_PartImage[0];
                    }
                    else if (_sortListToPart[i].CompanyPart == 2)
                    {
                        _recommendButton.transform.GetChild(1).GetComponent<Image>().sprite = m_PartImage[1];
                    }
                    else
                    {
                        _recommendButton.transform.GetChild(1).GetComponent<Image>().sprite = m_PartImage[2];
                    }
                    _recommendButton.GetComponent<Button>().onClick.AddListener(MakeRecommendInfo);
                }
            }
        }

        if (m_SelectedStudent.Count != 0)
        {
            m_SelectedStudent.Clear();
            m_SelectedStudentPercent.Clear();
        }

        if (PlayerInfo.Instance.IsFirstInJaeRecommend && m_TutorialCount == 9)
        {
            m_RecommendListPanel.Companycontent.vertical = true;
            m_RecommendListPanel.Recommendcontent.vertical = false;
            m_RecommendListPanel.RecommendContentTransform.GetChild(0).GetComponent<Button>().interactable = false;
            m_RecommendListPanel.RecommendContentTransform.GetChild(1).GetComponent<Button>().interactable = false;
            //m_RecommendListPanel.RecommendContentTransform.GetChild(2).GetComponent<Button>().interactable = false;
            //m_RecommendListPanel.RecommendContentTransform.GetChild(3).GetComponent<Button>().interactable = false;
            //m_RecommendListPanel.RecommendContentTransform.GetChild(4).GetComponent<Button>().interactable = false;

            m_Unmask.fitTarget = m_RecommendListPanel.RecommendListRect;
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(true);
            m_NextButton.gameObject.SetActive(true);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-800, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
    }

    // ���� ������ ������ ���� ������ �ٲ��ش�. �л����� ó�� ���� �� ��ȹ�л����� ������ �ϱ�
    private void MakeRecommendInfo()
    {
        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;
        string _recommendName = _currentObj.name;

        int _index = m_SortCompany.FindIndex(x => x.CompanyName == m_CompanyName);
        int _companyIndex = m_CompanyList.FindIndex(x => x.CompanyName == m_CompanyName);
        m_NowEmploymentIndex = m_SortCompany[_index].EmploymentList.FindIndex(x => x.EmploymentName == _recommendName);

        string _companyName = m_CompanyName;
        string _employmentName = m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].EmploymentName;
        string _employmentText = m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].EmploymentText;
        string _salary = m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanySalary.ToString();
        string _task = m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyJob;
        int _part = m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyPart;
        //string _companyGrade = m_SortCompany[_index].CompanyGrade;
        //int _companyID = m_ClassifyData[m_CompanyName][i].m_CompanyID;

        m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyRequirementStats.TryGetValue("Sense", out m_CompanySense);
        m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyRequirementStats.TryGetValue("Concentration", out m_CompanyConcentration);
        m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyRequirementStats.TryGetValue("Wit", out m_CompanyWit);
        m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyRequirementStats.TryGetValue("Technique", out m_CompanyTechinque);
        m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyRequirementStats.TryGetValue("Insight", out m_CompanyInsight);

        m_RecommendInfoPanel.SetNotification("����  " + m_CompanySense.ToString(), "����  " + m_CompanyConcentration.ToString(), "��ġ  " + m_CompanyWit.ToString(), "���  " + m_CompanyTechinque.ToString(), "����  " + m_CompanyInsight.ToString());
        m_RecommendInfoPanel.SetRecommendInfoListText(_companyName, _employmentName, _employmentText, _salary, _task);
        m_RecommendInfoPanel.SetRecruitmentPart(_part);

        MakeSkills(m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyRequirementSkill);
        MakeCompanyPentagon(m_CompanySense, m_CompanyConcentration, m_CompanyWit, m_CompanyTechinque, m_CompanyInsight);

        // �ʱ�ȭ
        m_RecommendInfoPanel.SetPercent("?? %");
        m_RecommendInfoPanel.StudentStatObj.SetActive(false);
        MakeStudentPentagon(0, 0, 0, 0, 0);

        MakeStudent(StudentType.GameDesigner);
        m_RecommendInfoPanel.PartGameDesignerButton.Select();

        m_RecommendListPanel.PopOffRecommendListPanel();
        m_RecommendInfoPanel.PopUpRecommendInfoPanel();

        if (m_CompanyList[_companyIndex].IsNewCompany == true)
        {
            m_CompanyList[_companyIndex].IsNewCompany = false;
        }

        //MakeCompanyButton();

        if (PlayerInfo.Instance.IsFirstInJaeRecommend && m_TutorialCount == 12)
        {
            m_RecommendListPanel.Recommendcontent.vertical = true;

            m_Unmask.fitTarget = m_RecommendInfoPanel.RecommendInfoRect;
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(true);
            m_NextButton.gameObject.SetActive(true);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().InJaeRecommendTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(600, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
    }

    // ��Ÿ���� �־��� ������ ������ִ� �Լ�
    private void MakeCompanyPentagon(int _sense, int _concentration, int _wit, int _technique, int _insight)
    {
        PentagonStats stats = new PentagonStats(_sense, _concentration, _wit, _technique, _insight);

        m_CompanyStatsRadar.SetStats(stats);
    }

    private void MakeStudentPentagon(int _sense, int _concentration, int _wit, int _technique, int _insight)
    {
        PentagonStats stats = new PentagonStats(_sense, _concentration, _wit, _technique, _insight);
        m_StudentStatsRadar.SetStats(stats);
    }

    /// TODO : �л��� ���ʽ� ��ų�� �����ϴ� ��. ��ų �Ǻ��� ������Ѵ�.
    private void MakeSkills(int[] _skillList)
    {
        DestroySkillPrefab();

        string _skillName = "";
        int _skillObjWidth = 180 + 23;           // ������ ���� ��ų�� ���̿��� 23�� �� �� ���� �־���� ������ �� ���´�.
        int _x = 89;                        // 89�� �Ǿ��ִ� ������ ��ų�� ��������� �ϴ� ��ġ�� 89���� �����ϱ� ����    
        m_CompanyRequiredSkillName.Clear();
        Transform _skillParent = m_RecommendInfoPanel.RequiredSkillParentTransform();

        for (int i = 0; i < _skillList.Length; i++)
        {
            // 0�̸� ���ٴ� ���̴� �Լ��� �������ش�.
            if (_skillList[i] == 0)
            {
                break;
            }

            _skillName = StudentSkills.FindSkillName(_skillList[i]);
            GameObject _skill = Instantiate(m_RequiredSkillPrefab, _skillParent);
            _skill.name = _skillName;
            m_CompanyRequiredSkillName.Add(_skillName);
            _skill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _skillName;
            _skill.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(_x, -385);
            m_SkillPrefab[i] = _skill;
            _x += _skillObjWidth;
        }
    }

    // ��õ ���θ� �������� �л����� ���Ľ����ش�.
    private List<Student> SortStudentToRecommend()
    {
        List<Student> _studentList = new List<Student>(ObjectManager.Instance.m_StudentList);

        _studentList.Sort((x, y) =>
        {
            if (x.m_StudentStat.m_IsRecommend && !y.m_StudentStat.m_IsRecommend) return 1;
            else if (!x.m_StudentStat.m_IsRecommend && y.m_StudentStat.m_IsRecommend) return -1;
            else return 0;
        });

        return _studentList;
    }

    // ȸ���ư ��¼����� ���� ����
    private List<Company> SortCompanyToConditions()
    {
        //List<InJaeRecommendData> _injaeDataList = new List<InJaeRecommendData>(m_RecommendList);
        List<Company> _companyList = new List<Company>(m_CompanyList);

        // -1�� ������ ���� �ϴ°�
        _companyList.Sort((x, y) =>
        {
            if (!x.IsUnLockCompany && y.IsUnLockCompany) return -1;
            else if (x.IsUnLockCompany && !y.IsUnLockCompany) return 1;
            else if (x.IsNewCompany && !y.IsNewCompany) return -1;
            else if (!x.IsNewCompany && y.IsNewCompany) return 1;
            else if (x.CompanyGrade < y.CompanyGrade) return -1;
            else if (x.CompanyGrade > y.CompanyGrade) return 1;
            else if (x.CompanyID < y.CompanyID) return -1;
            else if (x.CompanyID > y.CompanyID) return 1;
            else return 0;
        });

        return _companyList;
    }

    // ȸ�� ���� ��¼����� ���� ����
    private List<Employment> SortRecommendToConditions()
    {
        int _index = m_SortCompany.FindIndex(x => x.CompanyName == m_CompanyName);
        List<Employment> _sortList = new List<Employment>(m_SortCompany[_index].EmploymentList);

        _sortList.Sort((x, y) =>
        {
            if (x.CompanyPart < y.CompanyPart) return -1;
            else return 1;
        });

        return _sortList;
    }

    // ������ �ִ� �л����� ������Ʈ�� �����ְ� �ٽ� �л����� ������ش�.
    private void MakeStudent(StudentType _studentType)
    {
        m_RecommendInfoPanel.DestroyStudentList();

        List<Student> _studentList = SortStudentToRecommend();
        Transform _studentPrefabParent = m_RecommendInfoPanel.StudentListContent;

        for (int i = 0; i < _studentList.Count; i++)
        {
            if (_studentList[i].m_StudentStat.m_StudentType == _studentType)
            {
                GameObject _student = Instantiate(m_StudentPrefab, _studentPrefabParent);
                _student.name = _studentList[i].m_StudentStat.m_StudentName;

                if (_studentList[i].m_StudentStat.m_IsRecommend)
                {
                    _student.GetComponent<RecommendStudentPrefab>().m_RecommendTrue.SetActive(true);
                    _student.GetComponent<Button>().interactable = false;
                }
                else
                {
                    _student.GetComponent<RecommendStudentPrefab>().m_RecommendTrue.SetActive(false);
                    _student.GetComponent<Button>().interactable = true;
                }

                CompareNameAndTurnOnCheckImage(_student);

                if (_studentList[i].m_StudentStat.m_UserSettingName != "")
                {
                    _student.GetComponent<RecommendStudentPrefab>().m_StudentName.text = _studentList[i].m_StudentStat.m_UserSettingName;
                }
                else
                {
                    _student.GetComponent<RecommendStudentPrefab>().m_StudentName.text = _studentList[i].m_StudentStat.m_StudentName;
                }
                _student.GetComponent<RecommendStudentPrefab>().m_StudentImgae.sprite = _studentList[i].StudentProfileImg;
                _student.GetComponent<Button>().onClick.AddListener(ClickStudent);
                // �л� �̹��� ��ü�� ����� ��
            }
        }
    }

    // ���� ������ �л��� �ִٸ� üũ ǥ�� �ٽ� ���ֱ�
    private void CompareNameAndTurnOnCheckImage(GameObject _student)
    {
        if (m_SelectedStudent.Count != 0)
        {
            for (int i = 0; i < m_SelectedStudent.Count; i++)
            {
                if (m_SelectedStudent[i].m_StudentStat.m_StudentName == _student.name)
                {
                    _student.GetComponent<RecommendStudentPrefab>().m_CheckImage.SetActive(true);
                }
            }
        }
    }

    // Ŭ���� �л��� ������ ��������Ѵ�.
    private void ClickStudent()
    {
        GameObject _clickStudent = EventSystem.current.currentSelectedGameObject;
        List<Student> _studentList = new List<Student>(ObjectManager.Instance.m_StudentList);

        int _sense;
        int _concentration;
        int _wit;
        int _technique;
        int _insight;

        for (int i = 0; i < _studentList.Count; i++)
        {
            if (_studentList[i].m_StudentStat.m_StudentName == _clickStudent.name)
            {
                _sense = _studentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Sense];
                _concentration = _studentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Concentration];
                _wit = _studentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Wit];
                _technique = _studentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Technique];
                _insight = _studentList[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Insight];

                if (!m_SelectedStudent.Contains(_studentList[i]))
                {
                    m_SelectedStudent.Add(_studentList[i]);
                }

                SetPassPercentage(_studentList[i]);
                m_RecommendInfoPanel.SetStudentStatText("����  " + _sense.ToString(), "����  " + _concentration.ToString(), "��ġ  " + _wit.ToString(), "���  " + _technique.ToString(), "����  " + _insight.ToString());
                DecideStudentStatImage(_sense, _concentration, _wit, _technique, _insight);
                MakeStudentPentagon(_sense, _concentration, _wit, _technique, _insight);
            }
        }

        // 1�� ��õ�Ϸ�, 0�� üũ
        // ��õ�Ϸᰡ ���� ��
        if (!_clickStudent.GetComponent<RecommendStudentPrefab>().m_RecommendTrue.activeSelf)
        {
            if (_clickStudent.GetComponent<RecommendStudentPrefab>().m_CheckImage.activeSelf)
            {
                // üũǥ�� ���ֱ�
                _clickStudent.GetComponent<RecommendStudentPrefab>().m_CheckImage.SetActive(false);

                if (m_SelectedStudent.Count >= 1)
                {
                    for (int i = 0; i < _studentList.Count; i++)
                    {
                        if (_clickStudent.name == m_SelectedStudent[i].m_StudentStat.m_StudentName)
                        {
                            m_SelectedStudent.RemoveAt(i);
                            m_SelectedStudentPercent.RemoveAt(i);

                            m_RecommendInfoPanel.StudentStatObj.SetActive(false);
                            m_RecommendInfoPanel.SetPercent("?? %");
                            MakeStudentPentagon(0, 0, 0, 0, 0);
                            break;
                        }
                    }
                }
            }
            else
            {
                m_SelectedStudentPercent.Add(Average);
                _clickStudent.GetComponent<RecommendStudentPrefab>().m_CheckImage.SetActive(true);
                m_RecommendInfoPanel.StudentStatObj.SetActive(true);
            }
        }
        else
        {
            _clickStudent.GetComponent<RecommendStudentPrefab>().m_CheckImage.SetActive(false);
        }
    }

    // Ŭ���� �л��� �հݷ��� ������ִ� �Լ�
    private void SetPassPercentage(Student _student)
    {
        double _companyrequiredSense = m_CompanySense;
        double _companyrequiredConcentration = m_CompanyConcentration;
        double _companyrequiredWit = m_CompanyWit;
        double _companyrequiredTechinque = m_CompanyTechinque;
        double _companyrequiredInsight = m_CompanyInsight;

        double _selectStudentSense = _student.m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Sense];
        double _selectStudentConcentration = _student.m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Concentration];
        double _selectStudentWit = _student.m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Wit];
        double _selectStudentTechinque = _student.m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Technique];
        double _selectStudentInsight = _student.m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Insight];

        double _sensePercent = (_selectStudentSense / _companyrequiredSense * 100 >= 100) ? 100 : _selectStudentSense / _companyrequiredSense * 100;
        double _concentrationPercent = (_selectStudentConcentration / _companyrequiredConcentration * 100 >= 100) ? 100 : _selectStudentConcentration / _companyrequiredConcentration * 100;
        double _witPercent = (_selectStudentWit / _companyrequiredWit * 100 >= 100) ? 100 : _selectStudentWit / _companyrequiredWit * 100;
        double _techinquePercent = (_selectStudentTechinque / _companyrequiredTechinque * 100 >= 100) ? 100 : _selectStudentTechinque / _companyrequiredTechinque * 100;
        double _insightPercent = (_selectStudentInsight / _companyrequiredInsight * 100 >= 100) ? 100 : _selectStudentInsight / _companyrequiredInsight * 100;

        double _totalPercent = _sensePercent + _concentrationPercent + _witPercent + _techinquePercent + _insightPercent;
        Average = _totalPercent / 5;

        Average = Math.Truncate(Average);
        m_RecommendInfoPanel.SetPercent(Average.ToString() + " %");
    }

    // ȸ�簡 �䱸�� ���ȿ� ���������� �Ķ������� �ƴ϶�� ���������� �ٲ��ֱ�
    private void DecideStudentStatImage(int _StudentSense, int _StudentConcentration, int _StudentWit, int _StudentTechnique, int _StudentInsight)
    {
        Sprite _senseSprite = m_CompanySense <= _StudentSense ? m_SatisfiedSprite : m_NotSatisfiedSprite;
        Sprite _concentrationSprite = m_CompanyConcentration <= _StudentConcentration ? m_SatisfiedSprite : m_NotSatisfiedSprite;
        Sprite _witSprite = m_CompanyWit <= _StudentWit ? m_SatisfiedSprite : m_NotSatisfiedSprite;
        Sprite _techniqueSprite = m_CompanyTechinque <= _StudentTechnique ? m_SatisfiedSprite : m_NotSatisfiedSprite;
        Sprite _insightSprite = m_CompanyInsight <= _StudentInsight ? m_SatisfiedSprite : m_NotSatisfiedSprite;

        m_RecommendInfoPanel.SetStudentStatImage(_senseSprite, _concentrationSprite, _witSprite, _techniqueSprite, _insightSprite);
    }

    // ��Ʈ���� �л��� �����ִ� �Լ� (�ֵ� üũǥ�ô� â�� ���� �� ���ֱ�)
    public void ClickPartButton()
    {
        m_RecommendInfoPanel.DestroyStudentList();

        GameObject _currentbutton = EventSystem.current.currentSelectedGameObject;
        string _buttonName = _currentbutton.name;

        switch (_buttonName)
        {
            case "ArtTab": MakeStudent(StudentType.Art); break;
            case "GameDesignerTab": MakeStudent(StudentType.GameDesigner); break;
            case "ProgrammingTab": MakeStudent(StudentType.Programming); break;
        }
        m_RecommendInfoPanel.ChangeScrollRectTransform();
    }

    // ��õ�ϱ� ��ư�� ������ ���� ���ǿ� �´� �л����� �����ߴ��� Ȯ�����ִ� �Լ�(���� ��Ʈ)
    public void ClickRecommendButtonToPart()
    {
        int _index = m_SortCompany.FindIndex(x => x.CompanyName == m_CompanyName);
        int _part = m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyPart;
        string _warnningMessage = "";

        _isPass = false;
        StudentType _type;

        // �� ������ ����ֱ�
        m_RecommendInfoPanel.ChangeScrollRectTransform();

        switch (_part)
        {
            case 0: _type = StudentType.None; break;
            case 1: _type = StudentType.GameDesigner; break;
            case 2: _type = StudentType.Art; break;
            case 3: _type = StudentType.Programming; break;
            default: _type = StudentType.None; break;
        }

        if (m_SelectedStudent.Count == 0)
        {
            _warnningMessage = "�л��� �������ּ���!";
        }
        else
        {
            _warnningMessage = "��õ�� �Ұ��� �л��� �����մϴ�!";
        }

        for (int i = 0; i < m_SelectedStudent.Count; i++)
        {
            if (_type == StudentType.None)
            {
                // �а��� ��� . ��ų���� Ȯ���ϱ� 
                ClickRecommendButtonToSkill();
            }
            else if (_type != m_SelectedStudent[i].m_StudentStat.m_StudentType)
            {
                // �ٸ��ٸ� ��ų Ȯ�α��� �Ȱ��� �׳� ��õ�� �Ұ��� �л��� �ִٴ� ��� ����ֱ�
                _isPass = false;
                break;
            }
            else
            {
                // �а��� ��� ��ųȮ���ϱ�.
                _isPass = true;
            }
        }

        if (_isPass)
        {
            ClickRecommendButtonToSkill();
        }
        else
        {
            // �ٸ��ٸ� ��ų Ȯ�α��� �Ȱ��� �׳� ��õ�� �Ұ��� �л��� �ִٴ� ��� ����ֱ�
            m_RecommendInfoPanel.SetActiveImpossibleRecommendPanel(true, _warnningMessage);

            StartCoroutine(HideImpossibleRecommendPanel());
        }
    }

    // ��õ�ϱ� ��ư�� ������ ���� ���ǿ� �´� �л����� �����ߴ��� Ȯ�����ִ� �Լ�(�䱸 ��ų)
    public void ClickRecommendButtonToSkill()
    {
        _isPass = false;

        if (m_CompanyRequiredSkillName.Count != 0)
        {
            for (int i = 0; i < m_SelectedStudent.Count; i++)
            {
                for (int j = 0; j < m_SelectedStudent[i].m_StudentStat.m_Skills.Count; i++)
                {
                    if (m_CompanyRequiredSkillName[i] == m_SelectedStudent[i].m_StudentStat.m_Skills[i])
                    {
                        _isPass = true;
                    }
                    else
                    {
                        _isPass = false;
                        break;
                    }
                }

                if (!_isPass)
                {
                    break;
                }
            }
        }
        else
        {
            _isPass = true;
        }

        // ��ų�� ������ �ִٸ�... ������ ��õ �Ұ� �л��� �ִٰ� ����ֱ�
        if (!_isPass)
        {
            m_RecommendInfoPanel.SetActiveImpossibleRecommendPanel(true, "��õ �Ұ����� �л��� �ֽ��ϴ�!");

            StartCoroutine(HideImpossibleRecommendPanel());
        }
        else
        {
            m_RecommendInfoPanel.SetCheckRecommendPanel();
            m_RecommendInfoPanel.DestroyCheckPanelStudentList();

            int _index = m_CompanyList.FindIndex(x => x.CompanyName == m_CompanyName);
            int _part = m_CompanyList[_index].EmploymentList[m_NowEmploymentIndex].CompanyPart;

            int _companyGrade = m_CompanyList[_index].CompanyGrade;
            string _employmentName = m_CompanyList[_index].EmploymentList[m_NowEmploymentIndex].EmploymentName;

            Transform _checkStudentParentTransform = m_RecommendInfoPanel.CheckPanelStudentParent;

            switch (_companyGrade)
            {
                case 0: m_RecommendInfoPanel.SetCheckPanelCompanyGradeIcon(m_SRankSprite); break;
                case 1: m_RecommendInfoPanel.SetCheckPanelCompanyGradeIcon(m_ARankSprite); break;
                case 2: m_RecommendInfoPanel.SetCheckPanelCompanyGradeIcon(m_BRankSprite); break;
            }

            m_RecommendInfoPanel.SetCheckPanelCompanyName(_employmentName);

            for (int i = 0; i < m_SelectedStudent.Count; i++)
            {
                GameObject _student = Instantiate(m_CheckPanelStudentPrefab, _checkStudentParentTransform);
                _student.name = m_SelectedStudent[i].m_StudentStat.m_StudentName;

                double _percent = m_SelectedStudentPercent[i];

                if (_percent >= 80)
                {
                    _student.GetComponent<CheckRecommendStudentPrefab>().m_PercentImage.sprite = m_UpToEightyPercentSprite;
                }
                else if (_percent >= 60)
                {
                    _student.GetComponent<CheckRecommendStudentPrefab>().m_PercentImage.sprite = m_UpToSixtyPercentSprite;
                }
                else if (_percent >= 40)
                {
                    _student.GetComponent<CheckRecommendStudentPrefab>().m_PercentImage.sprite = m_UpToFourtyPercentSprite;
                }
                else
                {
                    _student.GetComponent<CheckRecommendStudentPrefab>().m_PercentImage.sprite = m_UpToZeroPercentSprite;
                }

                _student.GetComponent<CheckRecommendStudentPrefab>().m_PassPercent.text = _percent + " %";

                if (m_SelectedStudent[i].m_StudentStat.m_UserSettingName != "")
                {
                    _student.GetComponent<CheckRecommendStudentPrefab>().m_StudentName.text = m_SelectedStudent[i].m_StudentStat.m_UserSettingName;
                }
                else
                {
                    _student.GetComponent<CheckRecommendStudentPrefab>().m_StudentName.text = m_SelectedStudent[i].m_StudentStat.m_StudentName;
                }

                _student.GetComponent<CheckRecommendStudentPrefab>().m_StudentImage.sprite = m_SelectedStudent[i].StudentProfileImg;
            }

            // �л� �������� ������ ��Ŀ�� �������༭ 4������� ����� ���ְ� 4���� �Ѿ�� �������� ���ش�.
            if (_checkStudentParentTransform.childCount <= 4)
            {
                // ���
                _checkStudentParentTransform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
                _checkStudentParentTransform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
            }
            else
            {
                // ����
                _checkStudentParentTransform.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
                _checkStudentParentTransform.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
            }

            m_RecommendAni.SetActive(true);

            m_RecommendInfoPanel.ChangeScrollRectTransform();
        }
    }

    // ��õ�� Ȯ���ϴ� �гο��� OK��ư�� ������ �� ���� �����ߴ� �л����� ������ �հݷ��� �������ش�.
    private void ClickCheckPanelOKButton()
    {
        m_RecommendInfoPanel.PopOffRecommendInfoPanel();
        m_RecommendListPanel.PopUpRecommendListPanel();
        m_RecommendInfoPanel.ClickCheckPanelCancleButton();

        int _companyIndex = m_CompanyList.FindIndex(x => x.CompanyName == m_CompanyName);

        for (int i = 0; i < m_SelectedStudent.Count; i++)
        {
            for (int j = 0; j < ObjectManager.Instance.m_StudentList.Count; j++)
            {
                if (m_SelectedStudent[i].m_StudentStat.m_StudentName == ObjectManager.Instance.m_StudentList[j].m_StudentStat.m_StudentName)
                {
                    ObjectManager.Instance.m_StudentList[j].m_StudentStat.m_IsRecommend = true;
                    // ������ �����ϴ°��� �л����� �����Ѵ�.

                    int _randNum = UnityEngine.Random.Range(1, 101);

                    if (_randNum < m_SelectedStudentPercent[i])
                    {
                        m_CompanyList[_companyIndex].EmploymentList[m_NowEmploymentIndex].PassStudent.Add(m_SelectedStudent[i]);
                        m_CompanyList[_companyIndex].PassStudentCount += 1;
                        m_PassStudentCount++;
                    }
                    else
                    {
                        m_CompanyList[_companyIndex].EmploymentList[m_NowEmploymentIndex].FailStudent.Add(m_SelectedStudent[i]);
                    }

                    m_NowRecommendCount += 1;
                    break;
                }
            }
        }

        // content�� ������ �÷��ֱ�
        m_RecommendInfoPanel.ChangeScrollRectTransform();
        //DestroySkillPrefab();
        // ��� ������ �Ŀ� ���â�� ��� �л��� �̸� �־��ֱ�
        if (m_NowRecommendCount == 18)
        {
            SetResultPanelStudentPassFail();
            m_RecommendListPanel.LockCloseButton(true);
            m_RecommendInfoPanel.LockInfoPanelCloseButton(true);
        }

        if (m_Alram.activeSelf)
        {
            m_Alram.SetActive(false);
        }

        m_SelectedStudent.Clear();
        m_SelectedStudentPercent.Clear();
    }

    // ���â�� ���� �����ߴ� �л����� �̸��� ����ֱ� ���� �л����� �� �а��� �迭�� �̸��� �־��ش�.
    // ���� ������ �л����� �հݷ��� ���� �հ� ���հ� �̹����� �гο� �ִ� �Լ�
    private void SetResultPanelStudentPassFail()
    {
        int _gmIndex = 0;
        int _artIndex = 0;
        int _programmingIndex = 0;

        for (int i = 0; i < m_CompanyList.Count; i++)
        {
            for (int j = 0; j < m_CompanyList[i].EmploymentList.Count; j++)
            {
                for (int k = 0; k < m_CompanyList[i].EmploymentList[j].PassStudent.Count; k++)
                {
                    if (m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_StudentType == StudentType.GameDesigner)
                    {
                        if (m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_UserSettingName != "")
                        {
                            GameDesignerStudentName[_gmIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            GameDesignerStudentName[_gmIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_StudentName;
                        }
                        GameDesignerStudentPassFail[_gmIndex] = m_Pass;
                        GameDesignerStudentImgae[_gmIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].StudentProfileImg;
                        _gmIndex++;
                    }
                    else if (m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_StudentType == StudentType.Art)
                    {
                        if (m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_UserSettingName != "")
                        {
                            ArtStudentName[_artIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            ArtStudentName[_artIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_StudentName;
                        }
                        ArtStudentPassFail[_artIndex] = m_Pass;
                        ArtStudentImgae[_artIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].StudentProfileImg;
                        _artIndex++;
                    }
                    else
                    {
                        if (m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_UserSettingName != "")
                        {
                            ProgrammingStudentName[_programmingIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            ProgrammingStudentName[_programmingIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_StudentName;
                        }
                        ProgrammingStudentPassFail[_programmingIndex] = m_Pass;
                        ProgrammingStudentImgae[_programmingIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].StudentProfileImg;
                        _programmingIndex++;
                    }
                }

                for (int k = 0; k < m_CompanyList[i].EmploymentList[j].FailStudent.Count; k++)
                {

                    if (m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_StudentType == StudentType.GameDesigner)
                    {
                        if (m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_UserSettingName != "")
                        {
                            GameDesignerStudentName[_gmIndex] = m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            GameDesignerStudentName[_gmIndex] = m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_StudentName;
                        }
                        GameDesignerStudentPassFail[_gmIndex] = m_Fail;
                        GameDesignerStudentImgae[_gmIndex] = m_CompanyList[i].EmploymentList[j].FailStudent[k].StudentProfileImg;
                        _gmIndex++;
                    }
                    else if (m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_StudentType == StudentType.Art)
                    {
                        if (m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_UserSettingName != "")
                        {
                            ArtStudentName[_artIndex] = m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            ArtStudentName[_artIndex] = m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_StudentName;
                        }
                        ArtStudentPassFail[_artIndex] = m_Fail;
                        ArtStudentImgae[_artIndex] = m_CompanyList[i].EmploymentList[j].FailStudent[k].StudentProfileImg;
                        _artIndex++;
                    }
                    else
                    {
                        if (m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_UserSettingName != "")
                        {
                            ProgrammingStudentName[_programmingIndex] = m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            ProgrammingStudentName[_programmingIndex] = m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_StudentName;
                        }
                        ProgrammingStudentPassFail[_programmingIndex] = m_Fail;
                        ProgrammingStudentImgae[_programmingIndex] = m_CompanyList[i].EmploymentList[j].FailStudent[k].StudentProfileImg;
                        _programmingIndex++;
                    }
                }
            }
        }

        m_RecommendResultPanel.ResultPanelStudentNameChange(GameDesignerStudentName, ArtStudentName, ProgrammingStudentName);
        m_RecommendResultPanel.ResultPanelStudentImageChange(GameDesignerStudentImgae, ArtStudentImgae, ProgrammingStudentImgae);
        m_RecommendResultPanel.ResultPanelStudentPassFail(GameDesignerStudentPassFail, ArtStudentPassFail, ProgrammingStudentPassFail);
    }

    // ���â���� �л��� Ŭ������ �� ����� ��â
    public void ClickResultPanelStudent()
    {
        GameObject _currentStudent = EventSystem.current.currentSelectedGameObject;
        string _name = _currentStudent.GetComponent<RecommendResultStudentPrefab>().m_StudentName.text;

        m_RecommendResultPanel.PopUpDetailPanel();

        for (int i = 0; i < m_CompanyList.Count; i++)
        {
            for (int j = 0; j < m_CompanyList[i].EmploymentList.Count; j++)
            {
                for (int k = 0; k < m_CompanyList[i].EmploymentList[j].PassStudent.Count; k++)
                {
                    if (m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_StudentName == _name)
                    {
                        if (m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_UserSettingName != "")
                        {
                            m_RecommendResultPanel.SetDetailStudentName(m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_UserSettingName);
                        }
                        else
                        {
                            m_RecommendResultPanel.SetDetailStudentName(_name);
                        }
                        m_RecommendResultPanel.SetDetailPanelPassFailImage(m_DetailPass);
                        m_RecommendResultPanel.SetDetailPanelStudentImage(m_CompanyList[i].EmploymentList[j].PassStudent[k].StudentProfileImg);
                        m_RecommendResultPanel.SetDetailCompanyName(m_CompanyList[i].EmploymentList[j].EmploymentName);
                        break;
                    }
                }

                for (int k = 0; k < m_CompanyList[i].EmploymentList[j].FailStudent.Count; k++)
                {
                    if (m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_StudentName == _name)
                    {
                        if (m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_UserSettingName != "")
                        {
                            m_RecommendResultPanel.SetDetailStudentName(m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_UserSettingName);
                        }
                        else
                        {
                            m_RecommendResultPanel.SetDetailStudentName(_name);
                        }
                        //m_RecommendResultPanel.SetDetailStudentName(_name);   // �л� �̸�
                        m_RecommendResultPanel.SetDetailPanelPassFailImage(m_DetailFail);  // ���հ� �̹���
                        m_RecommendResultPanel.SetDetailPanelStudentImage(m_CompanyList[i].EmploymentList[j].FailStudent[k].StudentProfileImg);
                        m_RecommendResultPanel.SetDetailCompanyName(m_CompanyList[i].EmploymentList[j].EmploymentName);  // ������ �����̸�
                        break;
                    }
                }
            }
        }
    }

    IEnumerator HideImpossibleRecommendPanel()
    {
        yield return new WaitForSecondsRealtime(3f);
        m_RecommendInfoPanel.SetActiveImpossibleRecommendPanel(false);
    }

    // ������� ��ų�� �ִٸ� ȭ���� ���� �� ��ų �������� ��������Ѵ�.
    public void DestroySkillPrefab()
    {
        for (int i = 0; i < m_SkillPrefab.Length; i++)
        {
            Destroy(m_SkillPrefab[i]);
        }
    }
}

public class Company
{
    public int CompanyID;
    public string CompanyName;
    public int CompanyGrade;
    public int CompanyGameNameID;
    public int PassStudentCount;
    public int CompanyIcon;
    public bool IsNewCompany;
    public bool IsUnLockCompany;
    public List<Employment> EmploymentList;
}

public class Employment
{
    public int EmploymentID;
    public string EmploymentName;
    public string EmploymentText;
    public string CompanyJob;
    public int CompanySalary;
    public int CompanyPart;
    public int[] CompanyRequirementSkill;
    public int[] EmploymentYear;
    public List<Student> PassStudent;
    public List<Student> FailStudent;
    public Dictionary<string, int> CompanyRequirementStats;
}
