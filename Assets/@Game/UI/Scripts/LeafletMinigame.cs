using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeafletMinigame : MonoBehaviour
{
    [SerializeField] private GameObject m_MiniGamePanel;

    [Header("미니게임 메인")]
    [SerializeField] private GameObject m_Main;
    [SerializeField] private GameObject m_ManualPanel;
    [SerializeField] private Button m_StartButton;
    [SerializeField] private Button m_ManualButton;
    [SerializeField] private Button m_CloseManualButton;
    [SerializeField] private Button m_ExitButton;
    [SerializeField] private GameObject m_Warning;
    [SerializeField] private int m_NeedMoney;
    [SerializeField] private GameObject m_MoneyFadeOut;
    [SerializeField] private TextMeshProUGUI m_MoneyText;
    [SerializeField] private Image m_MoneyImage;
    [SerializeField] private float m_MoneyPosX;
    [SerializeField] private float m_MoneyPosY;
    [SerializeField] private float m_FadeOutDelay;


    [Space(5)]
    [Header("미니게임 시작")]
    [SerializeField] private GameObject m_Game;
    [SerializeField] private GameObject m_ReadyImg;
    [SerializeField] private GameObject m_StartImg;
    [SerializeField] private GameObject m_PolicePrefab;
    [SerializeField] private GameObject m_NormalPerson1Prefab;
    [SerializeField] private GameObject m_NormalPerson2Prefab;
    [SerializeField] private RectTransform m_Player;
    [SerializeField] private List<RectTransform> m_SpawnPointList = new List<RectTransform>();
    [SerializeField] private TextMeshProUGUI m_ScoreText;
    [SerializeField] private TextMeshProUGUI m_TimerText;
    [SerializeField] private float m_Person1Speed;
    [SerializeField] private float m_Person2Speed;
    [SerializeField] private float m_PoliceSpeed;
    [SerializeField] private float m_Timer;
    [SerializeField] private List<GameObject> m_HeartObj = new List<GameObject>();
    [SerializeField] private Slider m_TimeBar;

    [Space(5)]
    [Header("게임오버")]
    [SerializeField] private GameObject m_GameOver;
    [SerializeField] private Button m_ReturnButton;

    [Space(5)]
    [Header("게임결과")]
    [SerializeField] private GameObject m_GameFinish;
    [SerializeField] private GameObject m_GameResult;
    [SerializeField] private TextMeshProUGUI m_ResultText;
    [SerializeField] private TextMeshProUGUI m_ResultScore;
    [SerializeField] private Button m_ScoreOkButton;
    [SerializeField] private Button m_RewardOkButton;

    private IEnumerator m_SpawnCoroutine;
    private int m_PrevScore;
    private int m_Score;
    private bool m_IsPlaying;
    private List<GameObject> m_NormalPerson1List = new List<GameObject>();
    private List<GameObject> m_NormalPerson2List = new List<GameObject>();
    private List<GameObject> m_PoliceList = new List<GameObject>();
    private List<Vector3> m_Person1Direction = new List<Vector3>();
    private List<Vector3> m_Person2Direction = new List<Vector3>();
    private List<Vector3> m_PoliceDirection = new List<Vector3>();

    private const int NormalPerson1Count = 40;
    private const int NormalPerson2Count = 15;
    private const int PoliceCount = 15;
    private int m_Normal1Index;
    private int m_Normal2Index;
    private int m_PoliceIndex;
    private float m_LimitTime;
    private int m_Phase;
    private float m_SpeedMagnification;
    private int m_Life;
    private float m_SpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        m_StartButton.onClick.AddListener(MiniGameStart);
        m_StartButton.onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);
        m_ExitButton.onClick.AddListener(MiniGamePanelClose);
        m_ExitButton.onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);
        m_ReturnButton.onClick.AddListener(MiniGamePanelClose);
        m_ReturnButton.onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);
        m_RewardOkButton.onClick.AddListener(MiniGamePanelClose);
        m_RewardOkButton.onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);
        m_ScoreOkButton.onClick.AddListener(GetReward);
        m_ScoreOkButton.onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);

        for (int i = 0; i < NormalPerson1Count; i++)
        {
            GameObject newNormalPerson = Instantiate(m_NormalPerson1Prefab);
            newNormalPerson.transform.SetParent(this.gameObject.transform);
            newNormalPerson.SetActive(false);
            newNormalPerson.GetComponent<Button>().onClick.AddListener(ClickedPerson1);
            m_NormalPerson1List.Add(newNormalPerson);
            m_Person1Direction.Add(Vector3.zero);
        }

        for (int i = 0; i < NormalPerson2Count; i++)
        {
            GameObject newNormalPerson = Instantiate(m_NormalPerson2Prefab);
            newNormalPerson.transform.SetParent(this.gameObject.transform);
            newNormalPerson.SetActive(false);
            newNormalPerson.GetComponent<Button>().onClick.AddListener(ClickedPerson2);
            m_NormalPerson2List.Add(newNormalPerson);
            m_Person2Direction.Add(Vector3.zero);
        }

        for (int i = 0; i < PoliceCount; i++)
        {
            GameObject newPolice = Instantiate(m_PolicePrefab);
            newPolice.transform.SetParent(this.gameObject.transform);
            newPolice.SetActive(false);
            newPolice.GetComponent<Button>().onClick.AddListener(ClickedPolice);
            m_PoliceList.Add(newPolice);
            m_PoliceDirection.Add(Vector3.zero);
        }

        m_Normal1Index = 0;
        m_Normal2Index = 0;
        m_PoliceIndex = 0;
        m_Score = 0;
        m_PrevScore = 0;
        m_LimitTime = 0;
        m_TimerText.text = ((int)m_Timer).ToString();
        m_ScoreText.text = "SCORE : 0";
        m_Phase = 1;
        m_SpeedMagnification = 1;
        m_Life = 2;
        m_SpawnTime = 0.3f;
        m_MoneyText.text = m_NeedMoney.ToString();
        m_MoneyFadeOut.transform.position = m_StartButton.transform.position + new Vector3(m_MoneyPosX, m_MoneyPosY);
        m_ManualButton.onClick.AddListener(ClickManualButton);
        m_CloseManualButton.onClick.AddListener(ClickManualCloseButton);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsPlaying)
        {
            for (int i = 0; i < NormalPerson1Count; i++)
            {
                if (m_NormalPerson1List[i].activeSelf)
                {
                    m_NormalPerson1List[i].transform.Translate(m_Person1Direction[i] * (m_Person1Speed * m_SpeedMagnification));
                }
            }
            for (int i = 0; i < NormalPerson2Count; i++)
            {
                if (m_NormalPerson2List[i].activeSelf)
                {
                    m_NormalPerson2List[i].transform.Translate(m_Person2Direction[i] * (m_Person2Speed * m_SpeedMagnification));
                }
            }
            for (int i = 0; i < PoliceCount; i++)
            {
                if (m_PoliceList[i].activeSelf)
                {
                    m_PoliceList[i].transform.Translate(m_PoliceDirection[i] * (m_PoliceSpeed * m_SpeedMagnification));
                }
            }
            if (m_PrevScore != m_Score)
            {
                m_PrevScore = m_Score;
                m_ScoreText.text = "SCORE : " + m_Score.ToString();
            }
            
            m_TimerText.text = ((int)(m_Timer + (m_LimitTime - Time.realtimeSinceStartup))).ToString();
            m_TimeBar.value = m_Timer + (m_LimitTime - Time.realtimeSinceStartup);

            // 게임종료
            if (m_Timer + (m_LimitTime - Time.realtimeSinceStartup) <= 0)
            {
                ClickEventManager.Instance.Sound.PlayLeafletFinishSound();
                m_ResultText.text = "획득 점수";
                StopCoroutine(m_SpawnCoroutine);
                ResultPanel();
                ResetMiniGame();
                //MiniGamePanelClose();
            }
            // 3페이즈
            if (m_Timer + (m_LimitTime - Time.realtimeSinceStartup) <= 6 && m_Phase == 2)
            {
                ClickEventManager.Instance.Sound.PlaySpeedUpSound();
                m_Phase = 3;
                m_SpeedMagnification = 1.2f;
                m_SpawnTime = 0.2f;
            }
            // 2페이즈
            if (m_Timer + (m_LimitTime - Time.realtimeSinceStartup) <= 13 && m_Phase == 1)
            {
                m_Phase = 2;
                m_SpeedMagnification = 1f;
            }
        }
    }

    /// 메뉴얼 버튼을 눌렀을 때 메뉴얼 창이 뜨게 만들기
    private void ClickManualButton()
    {
        m_ManualPanel.SetActive(true);
    }

    /// 메뉴얼 창에서 확인 버튼을 누르면 닫게 하기
    private void ClickManualCloseButton()
    {
        m_ManualPanel.SetActive(false);
    }

    private void ResultPanel()
    {
        m_ResultScore.text = m_Score.ToString();
        m_GameFinish.SetActive(true);

        StartCoroutine(DelayResultPanel());
    }

    IEnumerator DelayResultPanel()
    {
        yield return new WaitForSecondsRealtime(2.0f);

        m_GameFinish.SetActive(false);
        ClickEventManager.Instance.Sound.PlayResultSound();
        m_GameResult.SetActive(true);
        m_ManualPanel.SetActive(false);
    }

    private void ResetMiniGame()
    {
        m_LimitTime = 0;
        m_Normal1Index = 0;
        m_Normal2Index = 0;
        m_PoliceIndex = 0;
        m_Score = 0;
        m_PrevScore = 0;
        m_IsPlaying = false;
        m_Phase = 1;
        m_SpeedMagnification = 1f;
        m_Life = 2;
        m_Player.transform.position = Vector3.zero;
        m_HeartObj[0].SetActive(true);
        m_HeartObj[1].SetActive(true);
        m_SpawnTime = 0.3f;
        m_TimerText.text = m_Timer.ToString();
        m_ScoreText.text = "SCORE : 0";

        for (int i = 0; i < NormalPerson1Count; i++)
        {
            if (m_NormalPerson1List[i].activeSelf)
            {
                m_NormalPerson1List[i].SetActive(false);
            }
            m_Person1Direction[i] = Vector3.zero;
        }
        for (int i = 0; i < NormalPerson2Count; i++)
        {
            if (m_NormalPerson2List[i].activeSelf)
            {
                m_NormalPerson2List[i].SetActive(false);
            }
            m_Person2Direction[i] = Vector3.zero;
        }
        for (int i = 0; i < PoliceCount; i++)
        {
            if (m_PoliceList[i].activeSelf)
            {
                m_PoliceList[i].SetActive(false);
            }
            m_PoliceDirection[i] = Vector3.zero;
        }
    }

    private void MiniGameStart()
    {
        if (PlayerInfo.Instance.MyMoney >= m_NeedMoney)
        {
            m_Main.SetActive(false);
            m_Game.SetActive(true);
            m_ReadyImg.SetActive(true);

            PlayerInfo.Instance.MyMoney -= m_NeedMoney;
            MonthlyReporter.Instance.m_NowMonth.ExpensesActivity += m_NeedMoney;

            StartCoroutine(MoneyFadeOut());

            ClickEventManager.Instance.Sound.PlayReadySound();
            StartCoroutine(MiniGameStartCor());
        }
        else
        {
            m_Warning.SetActive(true);
            StartCoroutine(DelayWarningClose());
        }
    }

    IEnumerator MoneyFadeOut()
    {
        m_MoneyFadeOut.SetActive(true);

        while (m_MoneyText.color.a > 0)
        {
            m_MoneyFadeOut.transform.position += new Vector3(0, 0.3f, 0);
            m_MoneyText.color = new Color(m_MoneyText.color.r, m_MoneyText.color.g, m_MoneyText.color.b, m_MoneyText.color.a - m_FadeOutDelay);
            m_MoneyImage.color = new Color(m_MoneyImage.color.r, m_MoneyImage.color.g, m_MoneyImage.color.b, m_MoneyImage.color.a - m_FadeOutDelay);
            
            yield return null;
        }

        m_MoneyFadeOut.transform.position = m_StartButton.transform.position + new Vector3(m_MoneyPosX, m_MoneyPosY);
        m_MoneyText.color = new Color(m_MoneyText.color.r, m_MoneyText.color.g, m_MoneyText.color.b, 1);
        m_MoneyImage.color = new Color(m_MoneyImage.color.r, m_MoneyImage.color.g, m_MoneyImage.color.b, 1);
        m_MoneyFadeOut.SetActive(false);
    }

    IEnumerator DelayWarningClose()
    {
        yield return new WaitForSecondsRealtime(2.0f);

        m_Warning.SetActive(false);
    }

    IEnumerator MiniGameStartCor()
    {
        yield return new WaitForSecondsRealtime(2.0f);

        ClickEventManager.Instance.Sound.PlayStartSound();
        m_ReadyImg.SetActive(false);
        m_StartImg.SetActive(true);

        yield return new WaitForSecondsRealtime(1.0f);

        m_StartImg.SetActive(false);
        m_IsPlaying = true;

        m_LimitTime = Time.realtimeSinceStartup;
        m_SpawnCoroutine = SpawnPerson();
        StartCoroutine(m_SpawnCoroutine);
    }

    IEnumerator SpawnPerson()
    {
        while (m_IsPlaying)
        {
            int randomSpawnPerson = Random.Range(1, 101);
            int randomPoint = Random.Range(0, m_SpawnPointList.Count);
            int nowPersonSelect = 0;

            if (m_Phase == 1)
            {
                if (randomSpawnPerson <= 85)
                {
                    m_NormalPerson1List[m_Normal1Index].SetActive(true);
                    m_NormalPerson1List[m_Normal1Index].transform.position = m_SpawnPointList[randomPoint].position;
                    nowPersonSelect = 2;
                }
                else
                {
                    m_NormalPerson2List[m_Normal2Index].SetActive(true);
                    m_NormalPerson2List[m_Normal2Index].transform.position = m_SpawnPointList[randomPoint].position;
                    nowPersonSelect = 3;
                }
            }
            else if (m_Phase == 2)
            {
                if (randomSpawnPerson <= 15)
                {
                    m_PoliceList[m_PoliceIndex].SetActive(true);
                    m_PoliceList[m_PoliceIndex].transform.position = m_SpawnPointList[randomPoint].position;
                    nowPersonSelect = 1;
                }
                else if (randomSpawnPerson <= 65)
                {
                    m_NormalPerson1List[m_Normal1Index].SetActive(true);
                    m_NormalPerson1List[m_Normal1Index].transform.position = m_SpawnPointList[randomPoint].position;
                    nowPersonSelect = 2;
                }
                else
                {
                    m_NormalPerson2List[m_Normal2Index].SetActive(true);
                    m_NormalPerson2List[m_Normal2Index].transform.position = m_SpawnPointList[randomPoint].position;
                    nowPersonSelect = 3;
                }
            }
            else if (m_Phase == 3)
            {
                if (randomSpawnPerson <= 30)
                {
                    m_PoliceList[m_PoliceIndex].SetActive(true);
                    m_PoliceList[m_PoliceIndex].transform.position = m_SpawnPointList[randomPoint].position;
                    nowPersonSelect = 1;
                }
                else if (randomSpawnPerson <= 70)
                {
                    m_NormalPerson1List[m_Normal1Index].SetActive(true);
                    m_NormalPerson1List[m_Normal1Index].transform.position = m_SpawnPointList[randomPoint].position;
                    nowPersonSelect = 2;
                }
                else
                {
                    m_NormalPerson2List[m_Normal2Index].SetActive(true);
                    m_NormalPerson2List[m_Normal2Index].transform.position = m_SpawnPointList[randomPoint].position;
                    nowPersonSelect = 3;
                }
            }

            Vector3 newDirection = new Vector3();
            float randomX = 0;
            float randomY = 0;
            if (randomPoint == 0)
            {
                randomX = Random.Range(0, 1.0f);
                randomY = Random.Range(-1.0f, 0);
            }
            else if (randomPoint >= 1 && randomPoint <= 5)
            {
                randomX = Random.Range(-1.0f, 1.0f);
                randomY = Random.Range(-1.0f, -0.3f);
            }
            else if (randomPoint == 6)
            {
                randomX = Random.Range(-1.0f, 0);
                randomY = Random.Range(-1.0f, 0);
            }
            else if (randomPoint == 7)
            {
                randomX = Random.Range(0, 1.0f);
                randomY = Random.Range(0, 1.0f);
            }
            else if (randomPoint >= 8 && randomPoint <= 12)
            {
                randomX = Random.Range(-1.0f, 1.0f);
                randomY = Random.Range(0.3f, 1.0f);
            }
            else if (randomPoint == 13)
            {
                randomX = Random.Range(-1.0f, 0);
                randomY = Random.Range(0, 1.0f);
            }
            else if (randomPoint >= 14 && randomPoint <= 15)
            {
                randomX = Random.Range(0.3f, 1.0f);
                randomY = Random.Range(-1.0f, 1.0f);
            }
            else if (randomPoint >= 16 && randomPoint <= 17)
            {
                randomX = Random.Range(-1.0f, -0.3f);
                randomY = Random.Range(-1.0f, 1.0f);
            }

            newDirection.x = randomX;
            newDirection.y = randomY;
            newDirection = newDirection.normalized;

            if (nowPersonSelect == 1)
            {
                if (randomX < 0 && m_PoliceList[m_PoliceIndex].transform.localScale.x > 0)
                {
                    m_PoliceList[m_PoliceIndex].transform.localScale = new Vector3(m_PoliceList[m_PoliceIndex].transform.localScale.x * -1,
                        m_PoliceList[m_PoliceIndex].transform.localScale.y, m_PoliceList[m_PoliceIndex].transform.localScale.z);
                }
                else if (randomX > 0 && m_PoliceList[m_PoliceIndex].transform.localScale.x < 0)
                {
                    m_PoliceList[m_PoliceIndex].transform.localScale = new Vector3(m_PoliceList[m_PoliceIndex].transform.localScale.x * -1,
                        m_PoliceList[m_PoliceIndex].transform.localScale.y, m_PoliceList[m_PoliceIndex].transform.localScale.z);
                }

                m_PoliceDirection[m_PoliceIndex] = newDirection;
                StartCoroutine(DisappearPerson(nowPersonSelect, m_PoliceIndex));

                m_PoliceIndex++;
                if (m_PoliceIndex >= PoliceCount)
                {
                    m_PoliceIndex = 0;
                }
            }
            else if (nowPersonSelect == 2)
            {
                if (randomX < 0 && m_NormalPerson1List[m_Normal1Index].transform.localScale.x > 0)
                {
                    m_NormalPerson1List[m_Normal1Index].transform.localScale = new Vector3(m_NormalPerson1List[m_Normal1Index].transform.localScale.x * -1,
                        m_NormalPerson1List[m_Normal1Index].transform.localScale.y, m_NormalPerson1List[m_Normal1Index].transform.localScale.z);
                }
                else if (randomX > 0 && m_NormalPerson1List[m_Normal1Index].transform.localScale.x < 0)
                {
                    m_NormalPerson1List[m_Normal1Index].transform.localScale = new Vector3(m_NormalPerson1List[m_Normal1Index].transform.localScale.x * -1,
                        m_NormalPerson1List[m_Normal1Index].transform.localScale.y, m_NormalPerson1List[m_Normal1Index].transform.localScale.z);
                }

                m_Person1Direction[m_Normal1Index] = newDirection;
                StartCoroutine(DisappearPerson(nowPersonSelect, m_Normal1Index));

                m_Normal1Index++;
                if (m_Normal1Index >= NormalPerson1Count)
                {
                    m_Normal1Index = 0;
                }
            }
            else
            {
                if (randomX < 0 && m_NormalPerson2List[m_Normal2Index].transform.localScale.x > 0)
                {
                    m_NormalPerson2List[m_Normal2Index].transform.localScale = new Vector3(m_NormalPerson2List[m_Normal2Index].transform.localScale.x * -1,
                        m_NormalPerson2List[m_Normal2Index].transform.localScale.y, m_NormalPerson2List[m_Normal2Index].transform.localScale.z);
                }
                else if (randomX > 0 && m_NormalPerson2List[m_Normal2Index].transform.localScale.x < 0)
                {
                    m_NormalPerson2List[m_Normal2Index].transform.localScale = new Vector3(m_NormalPerson2List[m_Normal2Index].transform.localScale.x * -1,
                        m_NormalPerson2List[m_Normal2Index].transform.localScale.y, m_NormalPerson2List[m_Normal2Index].transform.localScale.z);
                }

                m_Person2Direction[m_Normal2Index] = newDirection;
                StartCoroutine(DisappearPerson(nowPersonSelect, m_Normal2Index));

                m_Normal2Index++;
                if (m_Normal2Index >= NormalPerson2Count)
                {
                    m_Normal2Index = 0;
                }
            }

            yield return new WaitForSecondsRealtime(m_SpawnTime);
        }
    }

    private void ClickedPerson1()
    {
        GameObject nowPerson = EventSystem.current.currentSelectedGameObject;

        ClickEventManager.Instance.Sound.PlayCorrectTouchSound();
        ClickEventManager.Instance.Sound.PlayDashSound();

        m_Score += 5;
        int index = m_NormalPerson1List.IndexOf(nowPerson);
        if (m_Player.transform.position.x > m_NormalPerson1List[index].transform.position.x && 
            m_Player.transform.localScale.x > 0)
        {
            m_Player.transform.localScale = new Vector3(m_Player.transform.localScale.x * -1, m_Player.transform.localScale.y, m_Player.transform.localScale.z);
        }
        else if (m_Player.transform.position.x < m_NormalPerson1List[index].transform.position.x &&
                 m_Player.transform.localScale.x < 0)
        {
            m_Player.transform.localScale = new Vector3(m_Player.transform.localScale.x * -1, m_Player.transform.localScale.y, m_Player.transform.localScale.z);
        }

        m_Player.transform.position = m_NormalPerson1List[index].transform.position;
        m_NormalPerson1List[index].SetActive(false);
        m_Person1Direction[index] = Vector3.zero;
    }

    private void ClickedPerson2()
    {
        GameObject nowPerson = EventSystem.current.currentSelectedGameObject;

        ClickEventManager.Instance.Sound.PlayCorrectTouchSound();
        ClickEventManager.Instance.Sound.PlayDashSound();

        m_Score += 10;
        int index = m_NormalPerson2List.IndexOf(nowPerson);
        if (m_Player.transform.position.x > m_NormalPerson2List[index].transform.position.x &&
            m_Player.transform.localScale.x > 0)
        {
            m_Player.transform.localScale = new Vector3(m_Player.transform.localScale.x * -1, m_Player.transform.localScale.y, m_Player.transform.localScale.z);
        }
        else if (m_Player.transform.position.x < m_NormalPerson2List[index].transform.position.x &&
                 m_Player.transform.localScale.x < 0)
        {
            m_Player.transform.localScale = new Vector3(m_Player.transform.localScale.x * -1, m_Player.transform.localScale.y, m_Player.transform.localScale.z);
        }

        m_Player.transform.position = m_NormalPerson2List[index].transform.position;
        m_NormalPerson2List[index].SetActive(false);
        m_Person2Direction[index] = Vector3.zero;
    }

    private void ClickedPolice()
    {
        GameObject nowPerson = EventSystem.current.currentSelectedGameObject;

        ClickEventManager.Instance.Sound.PlayWrongTouchSound();
        ClickEventManager.Instance.Sound.PlayDashSound();

        m_Score -= 100;
        if (m_Score < 0)
        {
            m_Score = 0;
        }

        int index = m_PoliceList.IndexOf(nowPerson);
        if (m_Player.transform.position.x > m_PoliceList[index].transform.position.x &&
            m_Player.transform.localScale.x > 0)
        {
            m_Player.transform.localScale = new Vector3(m_Player.transform.localScale.x * -1, m_Player.transform.localScale.y, m_Player.transform.localScale.z);
        }
        else if (m_Player.transform.position.x < m_PoliceList[index].transform.position.x &&
                 m_Player.transform.localScale.x < 0)
        {
            m_Player.transform.localScale = new Vector3(m_Player.transform.localScale.x * -1, m_Player.transform.localScale.y, m_Player.transform.localScale.z);
        }

        m_Player.transform.position = m_PoliceList[index].transform.position;
        m_PoliceList[index].SetActive(false);
        m_PoliceDirection[index] = Vector3.zero;

        if (m_Life == 2)
        {
            m_Life--;
            m_HeartObj[1].SetActive(false);
        }
        else
        {
            m_Life--;
            m_HeartObj[0].SetActive(false);
            m_GameOver.SetActive(true);
            StartCoroutine(GameOverReturnButton());
            ResetMiniGame();
        }
    }

    IEnumerator GameOverReturnButton()
    {
        yield return new WaitForSecondsRealtime(2.0f);

        m_ReturnButton.gameObject.SetActive(true);
    }



    IEnumerator DisappearPerson(int selectPerson, int index)
    {
        //yield return new WaitForSecondsRealtime(10.0f);

        if (selectPerson == 1)
        {
            yield return new WaitUntil(() => m_PoliceList[index].transform.localPosition.x <= -950 ||
                                             m_PoliceList[index].transform.localPosition.x >= 950 || 
                                             m_PoliceList[index].transform.localPosition.y <= -350 ||
                                             m_PoliceList[index].transform.localPosition.y >= 350);
            m_PoliceList[index].SetActive(false);
            m_PoliceDirection[index] = Vector3.zero;
        }
        else if (selectPerson == 2)
        {
            yield return new WaitUntil(() => m_NormalPerson1List[index].transform.localPosition.x <= -950 ||
                                             m_NormalPerson1List[index].transform.localPosition.x >= 950 ||
                                             m_NormalPerson1List[index].transform.localPosition.y <= -350 ||
                                             m_NormalPerson1List[index].transform.localPosition.y >= 350);
            m_NormalPerson1List[index].SetActive(false);
            m_Person1Direction[index] = Vector3.zero;
        }
        else
        {
            yield return new WaitUntil(() => m_NormalPerson2List[index].transform.localPosition.x <= -950 ||
                                             m_NormalPerson2List[index].transform.localPosition.x >= 950 ||
                                             m_NormalPerson2List[index].transform.localPosition.y <= -350 ||
                                             m_NormalPerson2List[index].transform.localPosition.y >= 350);
            m_NormalPerson2List[index].SetActive(false);
            m_Person2Direction[index] = Vector3.zero;
        }
    }

    private void MiniGamePanelClose()
    {
        ClickEventManager.Instance.Sound.ReplayInGameBgm();
        ClickEventManager.Instance.Sound.StopMain2Audio();
        m_GameResult.SetActive(false);
        m_GameOver.SetActive(false);
        m_ReturnButton.gameObject.SetActive(false);
        m_Game.SetActive(false);
        m_Main.SetActive(true);
        m_ScoreOkButton.gameObject.SetActive(true);
        m_RewardOkButton.gameObject.SetActive(false);
        m_MiniGamePanel.SetActive(false);
    }

    private void GetReward()
    {
        m_ResultText.text = "보상";
        int rewardFamous = 0;
        int score = int.Parse(m_ResultScore.text);
        if (score >= 400)
        {
            rewardFamous = 150;
        }
        else if (score >= 350)
        {
            rewardFamous = 120;
        }
        else if (score >= 320)
        {
            rewardFamous = 100;
        }
        else if (score >= 300)
        {
            rewardFamous = 80;
        }
        else if (score >= 250)
        {
            rewardFamous = 60;
        }
        else if (score >= 200)
        {
            rewardFamous = 50;
        }
        else if (score >= 150)
        {
            rewardFamous = 30;
        }
        else if (score >= 100)
        {
            rewardFamous = 20;
        }
        else
        {
            rewardFamous = 0;
        }

        m_ScoreOkButton.gameObject.SetActive(false);
        m_RewardOkButton.gameObject.SetActive(true);
        PlayerInfo.Instance.Famous += rewardFamous;
        MonthlyReporter.Instance.m_NowMonth.FamousScore += rewardFamous;
        m_ResultScore.text = "유명 점수 +" + rewardFamous.ToString();
    }
}
