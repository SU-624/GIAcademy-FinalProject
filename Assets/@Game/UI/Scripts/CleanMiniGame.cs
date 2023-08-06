using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CleanMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject m_MiniGamePanel;
    [SerializeField] private Image m_BackGround;
    [SerializeField] private List<Sprite> m_BackGroundImgList = new List<Sprite>();

    [Header("미니게임 메인")]
    [SerializeField] private GameObject m_Main;
    [SerializeField] private GameObject m_ManualPanel;
    [SerializeField] private Button m_ExitButton;
    [SerializeField] private Button m_MaualButton;
    [SerializeField] private Button m_MaualCloseButton;
    [SerializeField] private GameObject m_Warning;
    [SerializeField] private Button m_ExitYesButton;
    [SerializeField] private Button m_ExitNoButton;
    [SerializeField] private List<GameObject> m_DustPointList = new List<GameObject>();
    [SerializeField] private GameObject m_Dust1Prefab;
    [SerializeField] private GameObject m_Dust2Prefab;
    [SerializeField] private Sprite m_Dust2BaseImage;
    [SerializeField] private Sprite m_Dust2TouchImage;
    [SerializeField] private int m_GetManagementScore;  // 얻는 운영 점수
    [SerializeField] private int m_DustCount;           // 나오게 할 먼지 수
    [SerializeField] private float m_TouchDelay;        // 먼지 한번 터치하고 난 후 딜레이 시간
    [SerializeField] private GameObject m_BroomStick;

    [Space(5)]
    [Header("게임결과")]
    [SerializeField] private GameObject m_GameResult;
    [SerializeField] private TextMeshProUGUI m_ResultScore;
    [SerializeField] private float m_ResultDelay;        // 전부 청소 후 결과창 딜레이 시간
    [SerializeField] private Button m_OkButton;


    private List<GameObject> m_Dust1List = new List<GameObject>();
    private List<GameObject> m_Dust2List = new List<GameObject>();
    private int m_TempDustCount;
    private bool m_IsTouchPosible;

    private List<int> m_RandomList = new List<int>();

    public bool m_IsGameEnd;

    void Start()
    {
        m_ExitButton.onClick.AddListener(OpenWarningPanel);
        m_ExitButton.onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);
        m_ExitNoButton.onClick.AddListener(CloseWarningPanel);
        m_ExitNoButton.onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);
        m_ExitYesButton.onClick.AddListener(CloseCleanPanel);
        m_ExitYesButton.onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);
        m_OkButton.onClick.AddListener(CloseCleanPanel);
        m_OkButton.onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);
        m_MaualButton.onClick.AddListener(ClickMaualButton);
        m_MaualCloseButton.onClick.AddListener(ClickClosekMaualButton);
        m_IsGameEnd = false;
    }

    void OpenWarningPanel()
    {
        m_Warning.SetActive(true);
        m_Warning.transform.SetAsLastSibling();
    }

    void CloseWarningPanel()
    {
        m_Warning.SetActive(false);
    }

    public void Initialize()
    {
        if (m_Dust1List.Count == 0)
        {
            for (int i = 0; i < m_DustCount; i++)
            {
                GameObject newDust1 = Instantiate(m_Dust1Prefab);
                newDust1.transform.SetParent(this.gameObject.transform);
                newDust1.GetComponent<Button>().onClick.AddListener(ClickedDust1);
                newDust1.SetActive(false);
                m_Dust1List.Add(newDust1);

                GameObject newDust2 = Instantiate(m_Dust2Prefab);
                newDust2.transform.SetParent(this.gameObject.transform);
                newDust2.GetComponent<Button>().onClick.AddListener(ClickedDust2);
                newDust2.SetActive(false);
                m_Dust2List.Add(newDust2);
            }
        }

        m_TempDustCount = m_DustCount;
        m_IsTouchPosible = true;
        m_RandomList.Clear();

        if (m_BackGroundImgList.Count > 0)
        {
            int randomBG = Random.Range(0, m_BackGroundImgList.Count);
            m_BackGround.sprite = m_BackGroundImgList[randomBG];
        }

        for (int i = 0; i < m_DustCount; i++)
        {
            m_Dust1List[i].SetActive(false);
            m_Dust2List[i].SetActive(false);
            m_RandomList.Add(i);
        }

        for (int i = 0; i < m_RandomList.Count; i++)
        {
            int ran1 = Random.Range(0, m_RandomList.Count);
            int ran2 = Random.Range(0, m_RandomList.Count);

            int temp = m_RandomList[ran1];
            m_RandomList[ran1] = m_RandomList[ran2];
            m_RandomList[ran2] = temp;
        }

        for (int i = 0; i < m_DustCount; i++)
        {
            int randomDust = Random.Range(1, 3);

            if (randomDust == 1)
            {
                m_Dust1List[i].transform.position = m_DustPointList[m_RandomList[i]].transform.position;
                m_Dust1List[i].SetActive(true);
            }
            else
            {
                m_Dust2List[i].transform.position = m_DustPointList[m_RandomList[i]].transform.position;
                m_Dust2List[i].SetActive(true);
            }
        }
    }

    /// 메뉴얼 버튼 추가
    private void ClickMaualButton()
    {
        m_ManualPanel.SetActive(true);
    }

    private void ClickClosekMaualButton()
    {
        m_ManualPanel.SetActive(false);
    }

    private void ClickedDust1()
    {
        if (!m_IsTouchPosible)
            return;

        GameObject nowDust = EventSystem.current.currentSelectedGameObject;

        ClickEventManager.Instance.Sound.PlayStickCleaning();
        ClickEventManager.Instance.Sound.PlayDust1Touch();
        int dustIndex = m_Dust1List.IndexOf(nowDust);
        m_BroomStick.SetActive(true);
        m_BroomStick.transform.position = nowDust.transform.position + new Vector3(50, -50);
        m_BroomStick.transform.SetAsLastSibling();
        m_IsTouchPosible = false;
        m_TempDustCount--;
        StartCoroutine(DelayDust1Disappear(1, dustIndex));
    }

    IEnumerator DelayDust1Disappear(int dustType, int index)
    {
        yield return new WaitForSecondsRealtime(m_TouchDelay);

        if (dustType == 1)
        {
            m_Dust1List[index].SetActive(false);
        }
        else
        {
            m_Dust2List[index].GetComponent<Image>().sprite = m_Dust2BaseImage;
            m_Dust2List[index].SetActive(false);
        }
        m_BroomStick.SetActive(false);
        m_IsTouchPosible = true;

        if (m_TempDustCount == 0)
        {
            DustCleanClear();
        }
    }

    private void ClickedDust2()
    {
        if (!m_IsTouchPosible)
            return;

        GameObject nowDust = EventSystem.current.currentSelectedGameObject;

        ClickEventManager.Instance.Sound.PlayStickCleaning();
        ClickEventManager.Instance.Sound.PlayDust2Touch();
        int dustIndex = m_Dust2List.IndexOf(nowDust);
        m_BroomStick.SetActive(true);
        m_BroomStick.transform.position = nowDust.transform.position + new Vector3(50, -50);
        m_BroomStick.transform.SetAsLastSibling();
        m_IsTouchPosible = false;
        if (nowDust.GetComponent<Image>().sprite == m_Dust2BaseImage)
        {
            nowDust.GetComponent<Image>().sprite = m_Dust2TouchImage;
            StartCoroutine(DelayCleanTime());
        }
        else
        {
            m_TempDustCount--;
            StartCoroutine(DelayDust1Disappear(2, dustIndex));
        }
    }

    IEnumerator DelayCleanTime()
    {
        yield return new WaitForSecondsRealtime(m_TouchDelay);

        m_IsTouchPosible = true;
        m_BroomStick.SetActive(false);
    }

    private void DustCleanClear()
    {
        // todo : 클리어 시 이펙트와 클리어 사운드 출력.

        StartCoroutine(DelayResultPanel());
    }

    IEnumerator DelayResultPanel()
    {
        yield return new WaitForSecondsRealtime(m_ResultDelay);
        
        ClickEventManager.Instance.Sound.PlayResultSound();
        m_GameResult.SetActive(true);
        PlayerInfo.Instance.Management += m_GetManagementScore;
        MonthlyReporter.Instance.m_NowMonth.ManagementScore += m_GetManagementScore;
    }

    private void CloseCleanPanel()
    {
        m_GameResult.SetActive(false);
        m_MiniGamePanel.SetActive(false);
        m_Warning.SetActive(false);
        m_ManualPanel.SetActive(false);
        m_IsGameEnd = true;
    }
}
