using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AllRoomInfoPanel : MonoBehaviour
{
    [Header("각 방, 시설 정보")]
    [SerializeField] private Button m_GenreRoomButton;
    [SerializeField] private Sprite m_GenreRoomOnImg;
    [SerializeField] private Sprite m_GenreRoomOffImg;
    [SerializeField] private Button m_FacilityButton;
    [SerializeField] private Sprite m_FacilityOnImg;
    [SerializeField] private Sprite m_FacilityOffImg;
    [SerializeField] private Button m_ClassRoomButton;
    [SerializeField] private Sprite m_ClassRoomOnImg;
    [SerializeField] private Sprite m_ClassRoomOffImg;
    [SerializeField] private List<Image> m_GenreRoomImgs;
    [SerializeField] private List<Sprite> m_GenreRoomLevelUpImgs;

    [Space(5f)]
    [Header("장르방 업그레이드")]
    [SerializeField] private Button m_PuzzleUpgradeButton;
    [SerializeField] private Button m_SimulationUpgradeButton;
    [SerializeField] private Button m_RhythmUpgradeButton;
    [SerializeField] private Button m_AdventureUpgradeButton;
    [SerializeField] private Button m_RPGUpgradeButton;
    [SerializeField] private Button m_SportsUpgradeButton;
    [SerializeField] private Button m_ActionUpgradeButton;
    [SerializeField] private Button m_ShootingUpgradeButton;

    [Space(5f)]
    [Header("장르방 업그레이드 잠김 버튼")]
    [SerializeField] private List<Button> m_UpgradeLockButton = new List<Button>();

    [Space(5f)]
    [Header("스크롤뷰")]
    [SerializeField] private GameObject m_GenreRoomScrollView;
    [SerializeField] private GameObject m_FacilityScrollView;
    [SerializeField] private GameObject m_ClassRoomScrollView;

    [Space(5f)]
    [Header("업그레이드 버튼 클릭 시")]
    [SerializeField] private Image m_NowGenreRoomImg;
    [SerializeField] private Image m_UpgradeGenreRoomImg;
    [SerializeField] private GameObject m_Notice;
    [SerializeField] private GameObject m_UpgradeCheck;
    [SerializeField] private Button m_UpgradeYesButton;
    [SerializeField] private Button m_UpgradeNoButton;

    [Space(5f)]
    [Header("기타")]
    [SerializeField] private GameObject m_RoomInfoPanel;
    [SerializeField] private PopOffUI m_AllRoomInfoPanel;
    [SerializeField] private Button m_QuitButton;
    [SerializeField] private TextMeshProUGUI m_InfoText;
    [SerializeField] private Sprite m_MaxImg;
    [SerializeField] private TextMeshProUGUI m_UpgradePoint;

    [Space(5f)]
    [Header("튜토리얼")]
    [SerializeField] private GameObject m_TutorialPanel;
    [SerializeField] private Unmask m_Unmask;
    [SerializeField] private Image m_TutorialTextImage;
    [SerializeField] private Image m_TutorialArrowImage;
    [SerializeField] private TextMeshProUGUI m_TutorialText;
    [SerializeField] private GameObject m_FoldButton;
    [SerializeField] private GameObject m_AllFacilityButton;
    [SerializeField] private GameObject m_BlackScreen;
    [SerializeField] private GameObject m_PDAlarm;
    [SerializeField] private TextMeshProUGUI m_AlarmText;
    [SerializeField] private RectTransform m_UpgradeRect1;
    [SerializeField] private RectTransform m_UpgradeRect2;

    // 0 : 장르방, 1 : 시설, 2 : 교실
    private int m_NowSelectMode = 0;

    private int m_NowClickedGenreRoom;
    private GameObject m_NowClickedUpgradeButton;

    private int m_TutorialCount;
    private int m_ScriptCount;
    

    // Start is called before the first frame update
    void Start()
    {
        m_GenreRoomButton.onClick.AddListener(ChangeInfoPanel);
        m_FacilityButton.onClick.AddListener(ChangeInfoPanel);
        m_ClassRoomButton.onClick.AddListener(ChangeInfoPanel);
        m_QuitButton.onClick.AddListener(QuitInfoPanel);
        m_QuitButton.onClick.AddListener(m_AllRoomInfoPanel.TurnOffUI);
        m_PuzzleUpgradeButton.onClick.AddListener(() => UpgradeCheck("PuzzleRoom"));
        m_SimulationUpgradeButton.onClick.AddListener(() => UpgradeCheck("SimulationRoom"));
        m_RhythmUpgradeButton.onClick.AddListener(() => UpgradeCheck("RhythmRoom"));
        m_AdventureUpgradeButton.onClick.AddListener(() => UpgradeCheck("AdventureRoom"));
        m_RPGUpgradeButton.onClick.AddListener(() => UpgradeCheck("RPGRoom"));
        m_SportsUpgradeButton.onClick.AddListener(() => UpgradeCheck("SportsRoom"));
        m_ActionUpgradeButton.onClick.AddListener(() => UpgradeCheck("ActionRoom"));
        m_ShootingUpgradeButton.onClick.AddListener(() => UpgradeCheck("ShootingRoom"));

        m_UpgradeYesButton.onClick.AddListener(UpgradeYes);
        m_UpgradeNoButton.onClick.AddListener(UpgradeNo);

        m_TutorialCount = 0;
        m_ScriptCount = 0;

        for (var i = 0; i < InteractionManager.Instance.GenreRoomList.Count; i++)
        {
            if (InteractionManager.Instance.GenreRoomList[i].Level == 2)
            {
                if (i == 0)
                {
                    m_PuzzleUpgradeButton.GetComponent<Image>().sprite = m_MaxImg;
                    m_PuzzleUpgradeButton.interactable = false;
                }
                else if (i == 1)
                {
                    m_SimulationUpgradeButton.GetComponent<Image>().sprite = m_MaxImg;
                    m_SimulationUpgradeButton.interactable = false;
                }
                else if (i == 2)
                {
                    m_RhythmUpgradeButton.GetComponent<Image>().sprite = m_MaxImg;
                    m_RhythmUpgradeButton.interactable = false;
                }
                else if (i == 3)
                {
                    m_AdventureUpgradeButton.GetComponent<Image>().sprite = m_MaxImg;
                    m_AdventureUpgradeButton.interactable = false;
                }
                else if (i == 4)
                {
                    m_RPGUpgradeButton.GetComponent<Image>().sprite = m_MaxImg;
                    m_RPGUpgradeButton.interactable = false;
                }
                else if (i == 5)
                {
                    m_SportsUpgradeButton.GetComponent<Image>().sprite = m_MaxImg;
                    m_SportsUpgradeButton.interactable = false;
                }
                else if (i == 6)
                {
                    m_ActionUpgradeButton.GetComponent<Image>().sprite = m_MaxImg;
                    m_ActionUpgradeButton.interactable = false;
                }
                else if (i == 7)
                {
                    m_ShootingUpgradeButton.GetComponent<Image>().sprite = m_MaxImg;
                    m_ShootingUpgradeButton.interactable = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InGameUI.Instance.UIStack.Count == 0 && PlayerInfo.Instance.IsUpgradePossible && m_TutorialCount == 0 && !PlayerInfo.Instance.IsUpgradeTutorialEnd)
        {
            UpgradeTutorial();
        }

        if (PlayerInfo.Instance.IsUpgradePossible && m_TutorialCount > 0 && !PlayerInfo.Instance.IsUpgradeTutorialEnd)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                UpgradeTutorial();
            }
#elif UNITY_ANDROID
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    UpgradeTutorial();
                }
            }
#endif
        }

        if (m_TutorialCount == 2 && m_RoomInfoPanel.activeSelf)
        {
            m_Unmask.fitTarget = m_UpgradeRect1;
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 150, 0);
            m_TutorialCount++;
        }

        if (!m_RoomInfoPanel.activeSelf)
        {
            m_GenreRoomScrollView.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0;
        }
    }

    private void UpgradeTutorial()
    {
        if (m_TutorialCount == 0)
        {
            Time.timeScale = 0;

            foreach (var button in m_UpgradeLockButton)
            {
                button.gameObject.SetActive(false);
            }
            m_PuzzleUpgradeButton.gameObject.SetActive(true);
            m_SimulationUpgradeButton.gameObject.SetActive(true);
            m_RhythmUpgradeButton.gameObject.SetActive(true);
            m_AdventureUpgradeButton.gameObject.SetActive(true);
            m_RPGUpgradeButton.gameObject.SetActive(true);
            m_SportsUpgradeButton.gameObject.SetActive(true);
            m_ActionUpgradeButton.gameObject.SetActive(true);
            m_ShootingUpgradeButton.gameObject.SetActive(true);

            m_TutorialPanel.SetActive(true);
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_BlackScreen.SetActive(false);
            m_Unmask.gameObject.SetActive(false);
            m_PDAlarm.SetActive(true);
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().UpgradeTutorial[m_ScriptCount];

            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 1)
        {
            if (m_FoldButton.GetComponent<PopUpUI>().isSlideMenuPanelOpend == false)
            {
                m_FoldButton.GetComponent<PopUpUI>().AutoSlideMenuUI();
            }

            m_BlackScreen.SetActive(true);
            m_PDAlarm.SetActive(false);
            m_Unmask.gameObject.SetActive(true);
            m_Unmask.fitTarget = m_AllFacilityButton.GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
            m_TutorialCount++;
        }
        //else if (m_TutorialCount == 3)
        //{
        //    m_Unmask.fitTarget = m_UpgradeRect1;
        //    m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 150, 0);
        //    m_TutorialCount++;
        //}
        else if (m_TutorialCount == 5)
        {
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().UpgradeTutorial[m_ScriptCount];

            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 6)
        {
            Time.timeScale = 1;
            m_TutorialPanel.SetActive(false);
            PlayerInfo.Instance.IsUpgradeTutorialEnd = true;
        }
    }



    void ChangeInfoPanel()
    {
        string nowButtonName = EventSystem.current.currentSelectedGameObject.name;

        switch (nowButtonName)
        {
            case "GenreRoomIndexButton":
                if (m_NowSelectMode != 0)
                {
                    m_NowSelectMode = 0;
                    m_GenreRoomScrollView.SetActive(true);
                    m_FacilityScrollView.SetActive(false);
                    m_ClassRoomScrollView.SetActive(false);
                    m_GenreRoomButton.GetComponent<Image>().sprite = m_GenreRoomOnImg;
                    m_FacilityButton.GetComponent<Image>().sprite = m_FacilityOffImg;
                    m_ClassRoomButton.GetComponent<Image>().sprite = m_ClassRoomOffImg;
                    m_InfoText.text = "장르방";
                }
                break;

            case "FacilityIndexButton":
                if (m_NowSelectMode != 1)
                {
                    m_NowSelectMode = 1;
                    m_GenreRoomScrollView.SetActive(false);
                    m_FacilityScrollView.SetActive(true);
                    m_ClassRoomScrollView.SetActive(false);
                    m_GenreRoomButton.GetComponent<Image>().sprite = m_GenreRoomOffImg;
                    m_FacilityButton.GetComponent<Image>().sprite = m_FacilityOnImg;
                    m_ClassRoomButton.GetComponent<Image>().sprite = m_ClassRoomOffImg;
                    m_InfoText.text = "편의시설";
                }
                break;

            case "ClassRoomIndexButton":
                if (m_NowSelectMode != 2)
                {
                    m_NowSelectMode = 2;
                    m_GenreRoomScrollView.SetActive(false);
                    m_FacilityScrollView.SetActive(false);
                    m_ClassRoomScrollView.SetActive(true);
                    m_GenreRoomButton.GetComponent<Image>().sprite = m_GenreRoomOffImg;
                    m_FacilityButton.GetComponent<Image>().sprite = m_FacilityOffImg;
                    m_ClassRoomButton.GetComponent<Image>().sprite = m_ClassRoomOnImg;
                    m_InfoText.text = "수업시설";
                }
                break;
        }
    }

    void QuitInfoPanel()
    {
        if (m_NowSelectMode != 0)
        {
            m_NowSelectMode = 0;
            m_GenreRoomScrollView.SetActive(true);
            m_FacilityScrollView.SetActive(false);
            m_ClassRoomScrollView.SetActive(false);
            m_GenreRoomButton.GetComponent<Image>().sprite = m_GenreRoomOnImg;
            m_FacilityButton.GetComponent<Image>().sprite = m_FacilityOffImg;
            m_ClassRoomButton.GetComponent<Image>().sprite = m_ClassRoomOffImg;
            m_InfoText.text = "장르방";
        }
    }

    void UpgradeCheck(string genre)
    {
        if (m_TutorialCount == 3)
        {
            m_Unmask.fitTarget = m_UpgradeRect2;
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
            m_TutorialCount++;
        }

        m_NowClickedGenreRoom = (int)Enum.Parse(typeof(InteractionManager.SpotName), genre);
        m_NowClickedUpgradeButton = EventSystem.current.currentSelectedGameObject;
        m_NowGenreRoomImg.sprite = m_GenreRoomImgs[m_NowClickedGenreRoom].sprite;
        m_UpgradeGenreRoomImg.sprite = m_GenreRoomLevelUpImgs[m_NowClickedGenreRoom];

        m_UpgradeCheck.SetActive(true);
        if (PlayerInfo.Instance.SpecialPoint < 2000)
        {
            m_UpgradePoint.color = Color.red;
        }
        else
        {
            m_UpgradePoint.color = Color.blue;
        }
    }

    IEnumerator CloseNotice()
    {
        yield return new WaitForSecondsRealtime(2.0f);

        m_Notice.SetActive(false);
    }

    private void UpgradeYes()
    {
        if (PlayerInfo.Instance.SpecialPoint < 2000)
        {
            m_Notice.SetActive(true);
            StartCoroutine(CloseNotice());
        }
        else
        {
            if (PlayerInfo.Instance.IsUpgradePossible && !PlayerInfo.Instance.IsUpgradeTutorialEnd)
            {
                m_TutorialPanel.SetActive(false);
                StartCoroutine(DelayTutorial());
            }

            m_GenreRoomImgs[m_NowClickedGenreRoom].sprite =
                m_GenreRoomLevelUpImgs[m_NowClickedGenreRoom];
            m_NowClickedUpgradeButton.GetComponent<Image>().sprite = m_MaxImg;
            m_NowClickedUpgradeButton.GetComponent<Button>().interactable = false;
            QuitInfoPanel();
            m_AllRoomInfoPanel.TurnOffUI();
            m_UpgradeCheck.SetActive(false);
            InteractionManager.Instance.GenreRoomLevelUp(m_NowClickedGenreRoom);
        }
    }

    private void UpgradeNo()
    {
        m_UpgradeCheck.SetActive(false);
        m_NowClickedGenreRoom = -1;
    }

    IEnumerator DelayTutorial()
    {
        yield return new WaitForSeconds(2f);

        Time.timeScale = 0f;
        m_TutorialPanel.SetActive(true);
        m_TutorialArrowImage.gameObject.SetActive(false);
        m_BlackScreen.SetActive(false);
        m_Unmask.gameObject.SetActive(false);
        m_PDAlarm.SetActive(true);
        m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().UpgradeTutorial[m_ScriptCount];

        m_ScriptCount++;
        m_TutorialCount++;
    }
}
