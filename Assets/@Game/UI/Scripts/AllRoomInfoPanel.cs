using System.Collections;
using System.Collections.Generic;
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
    [Header("스크롤뷰")]
    [SerializeField] private GameObject m_GenreRoomScrollView;
    [SerializeField] private GameObject m_FacilityScrollView;
    [SerializeField] private GameObject m_ClassRoomScrollView;

    [Space(5f)]
    [Header("업그레이드 버튼 클릭 시")]
    [SerializeField] private GameObject m_Notice;
    [SerializeField] private GameObject m_UpgradeCheck;
    [SerializeField] private Button m_UpgradeYesButton;
    [SerializeField] private Button m_UpgradeNoButton;

    [Space(5f)]
    [Header("기타")]
    [SerializeField] private PopOffUI m_AllRoomInfoPanel;
    [SerializeField] private Button m_QuitButton;
    [SerializeField] private TextMeshProUGUI m_InfoText;
    [SerializeField] private Sprite m_MaxImg;
    [SerializeField] private TextMeshProUGUI m_UpgradePoint;

    // 0 : 장르방, 1 : 시설, 2 : 교실
    private int m_NowSelectMode = 0;

    private string m_NowClickedGenreRoom;
    private GameObject m_NowClickedUpgradeButton;

    // Start is called before the first frame update
    void Start()
    {
        m_GenreRoomButton.onClick.AddListener(ChangeInfoPanel);
        m_FacilityButton.onClick.AddListener(ChangeInfoPanel);
        m_ClassRoomButton.onClick.AddListener(ChangeInfoPanel);
        m_QuitButton.onClick.AddListener(QuitInfoPanel);
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
        m_NowClickedGenreRoom = genre;
        m_NowClickedUpgradeButton = EventSystem.current.currentSelectedGameObject;
        m_UpgradeCheck.SetActive(true);
        if (PlayerInfo.Instance.m_SpecialPoint < 2000)
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
        if (PlayerInfo.Instance.m_SpecialPoint < 2000)
        {
            m_Notice.SetActive(true);
            StartCoroutine(CloseNotice());
        }
        else
        {
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
        m_NowClickedGenreRoom = "";
    }
}
