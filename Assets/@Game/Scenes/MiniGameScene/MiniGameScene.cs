using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MiniGameScene : MonoBehaviour
{
    // 게임잼
    [SerializeField] private GameJam m_GameJam;
    [Header("Resources")] [SerializeField] private GameObject m_Prefab_CommandItem;
    [SerializeField] private Sprite[] m_SpriteArray;
    [SerializeField] private Sprite[] m_SpecialSpriteArray;
    [SerializeField] private Sprite[] m_ButtonSpriteArray;
    [SerializeField] private Sprite m_BugSprite;

    [SerializeField] private GameObject m_Prefab_Fx_CorrectCommand;
    [SerializeField] private GameObject m_Prefab_Fx_WrongCommand;
    [SerializeField] private GameObject m_Prefab_Fx_Bug;
    [SerializeField] private GameObject m_Prefab_Mini1_Panel;

    [SerializeField] private Sprite m_Sprite_BackgroundNormal;
    [SerializeField] private Sprite m_Sprite_BackgroundWrong;

    [Header("Scene References")]
    [SerializeField]
    private MiniGame m_Game;

    [SerializeField] private PopOffUI m_PopOffMiniGamePanel;
    [SerializeField] private PopOffUI m_PopOffPDPanel;
    [SerializeField] private PopUpUI m_PopUpPDPanel;
    [SerializeField] private TextMeshProUGUI m_PDText;

    [SerializeField] private Image m_UI_TopPanel;
    [SerializeField] private GameObject m_CommandParentBottom;
    [SerializeField] private GameObject m_CommandParentTop;
    [SerializeField] private Button[] m_UI_ButtonArray;
    [SerializeField] private Slider m_UI_TimeSlider;
    [SerializeField] private TextMeshProUGUI m_UI_ScoreText;
    [SerializeField] private TextMeshProUGUI m_UI_ResultScoreText;
    [SerializeField] private TextMeshProUGUI m_UI_RemainingBugCountText;
    [SerializeField] private Image m_UI_BugIcon;
    [SerializeField] private GameObject m_UI_CommandFeedbackBad;
    [SerializeField] private GameObject m_UI_CommandFeedbackGood;
    [SerializeField] private GameObject m_UI_CommandFeedbackExcellent;
    [SerializeField] private GameObject m_UI_Ready;
    [SerializeField] private GameObject m_UI_Start;
    [SerializeField] private GameObject m_UI_Finish;
    [SerializeField] private GameObject m_UI_ResultScorePanel;
    [SerializeField] private GameObject m_UI_Phase3Panel;

    [SerializeField] private SoundManager m_SoundManager;

    [Header("Values")] [SerializeField] private float m_ReadyGameDelay = 1.0f;
    [Header("Values")] [SerializeField] private float m_StartGameDelay = 1.0f;
    [SerializeField] private float m_EndPhaseDelay = 1.5f;

    [SerializeField] private float m_Phase1Duration = 4.0f;
    [SerializeField] private int m_Phase1CommandCount = 5;

    [SerializeField] private float m_Phase2Duration = 10.0f;
    [SerializeField] private int m_Phase2CommandCount = 10;
    [SerializeField] private int m_Phase2SpecialCommandCount = 3;

    [SerializeField] private float m_Phase3Duration = 5.0f;
    [SerializeField] private int m_Phase3MaxCommandCount = 50;

    [SerializeField] private float m_WrongBackgroundDuration = 0.2f;

    private Coroutine m_ChangeBackgroundWrongCoroutine;
    private Queue<string> m_PDScript = new Queue<string>();

    private int m_Score;

    private GameObject m_MinigameCanvas;

    public int score
    {
        get => m_Score;
        set
        {
            if (m_Score == value)
                return;

            m_Score = value;
            onScoreChanged?.Invoke(value);
        }
    }

    public event System.Action onFinishGame;
    public event System.Action<int> onScoreChanged; // param: <new_socre>
    public event System.Action<int> onPressCommandButton; // param: <command>

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    void Start()
    {
        m_MinigameCanvas = GameObject.Find("MinigameCanvas");
    }
    private IEnumerator StartGameCoroutine()
    {
        Action<int, int> _onFinishPhaseAction;
        Action<int> _onCorrectCommandAction;
        Action<int> _onWrongCommandAction;
        Action<int> _onSpecialCommandPressedAction;
        Action<int> _onPressCommandButtonAction;
        Action<List<int>, List<bool>> _onCommandListRefreshedAction;

        List<Image> _commandItemImageList = new List<Image>();
        bool _playClockTicking = false;

        // common
        m_SoundManager.PlayGameJamMiniGameBgm();
        m_SoundManager.StopInGameBgm();

        m_Game.onCorrectCommand += _commandIndex => { m_SoundManager.PlayCorrectTouchSound(); };

        m_Game.onWrongCommand += _commandIndex => { m_SoundManager.PlayGameJamTouchWrong(); };

        m_Game.onRemainingTimeChanged += (_percentage, _time) =>
        {
            if (_time < 2
                && _playClockTicking == false)
            {
                _playClockTicking = true;
                m_SoundManager.PlayClockTickingSound();
            }
        };

        // ready, start
        Initialize();

        for (int i = 0; i < 3; ++i)
        {
            // 모든 버튼의 이미지를 각 파트 이미지로 변경합니다.
            Button button = m_UI_ButtonArray[i];
            button.transform.GetComponent<Image>().sprite = m_ButtonSpriteArray[i];
        }

        m_SoundManager.PlayReadySound();
        m_UI_Ready.SetActive(true);
        yield return new WaitForSecondsRealtime(m_ReadyGameDelay);
        m_UI_Ready.SetActive(false);

        m_SoundManager.PlayStartSound();
        m_UI_Start.SetActive(true);
        yield return new WaitForSecondsRealtime(m_StartGameDelay);
        m_UI_Start.SetActive(false);

        // phase 1
        Initialize();
        m_CommandParentTop.SetActive(true);

        _onFinishPhaseAction = (_phaseNumber, _count) =>
        {
            if (_count >= m_Phase1CommandCount)
            {
                m_UI_CommandFeedbackExcellent.SetActive(true);
                m_SoundManager.PlayExcellent();
            }
            else if (_count >= 2)
            {
                m_UI_CommandFeedbackGood.SetActive(true);
                m_SoundManager.PlayGood();
            }
            else
            {
                m_UI_CommandFeedbackBad.SetActive(true);
                m_SoundManager.PlayBad();
            }
        };
        m_Game.onFinishPhase += _onFinishPhaseAction;

        _onCommandListRefreshedAction = (_commandList, _isSpecialCommandList) =>
        {
            for (int i = 0; i < _commandList.Count; ++i)
            {
                int _command = _commandList[i];

                // 스프라이트를 고르고 초기화합니다.
                var _commandItem = GameObject.Instantiate(m_Prefab_CommandItem, m_CommandParentTop.transform);
                var _commandItemImage = _commandItem.transform.GetComponent<Image>();
                _commandItemImage.sprite = _isSpecialCommandList[i] ? m_SpecialSpriteArray[_command] : m_SpriteArray[_command];

                _commandItemImageList.Add(_commandItemImage);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(m_CommandParentTop.GetComponent<RectTransform>());
        };
        m_Game.onCommandListRefreshed += _onCommandListRefreshedAction;

        _onCorrectCommandAction = (_commandIndex) =>
        {
            ++score;

            _commandItemImageList[_commandIndex].color = new Color(1, 1, 1, 0.0f);
            if (m_Prefab_Fx_CorrectCommand != null)
            {
                var _fx = GameObject.Instantiate(m_Prefab_Fx_CorrectCommand, m_MinigameCanvas.transform);
                _fx.transform.position = _commandItemImageList[_commandIndex].gameObject.transform.position;
                Destroy(_fx, 2.0f);
            }
        };
        m_Game.onCorrectCommand += _onCorrectCommandAction;

        _onWrongCommandAction = (_commandIndex) =>
        {
            _commandItemImageList[_commandIndex].color = new Color(1, 1, 1, 0.0f);
            if (m_Prefab_Fx_WrongCommand != null)
            {
                var _fx = GameObject.Instantiate(m_Prefab_Fx_WrongCommand, m_MinigameCanvas.transform);
                _fx.transform.position = _commandItemImageList[_commandIndex].gameObject.transform.position;
                Destroy(_fx, 2.0f);
            }

            if (m_ChangeBackgroundWrongCoroutine != null)
                StopCoroutine(m_ChangeBackgroundWrongCoroutine);
            m_ChangeBackgroundWrongCoroutine = StartCoroutine(ChangeBackgroundWrong());
        };
        m_Game.onWrongCommand += _onWrongCommandAction;

        yield return StartCoroutine(
            m_Game.Phase12Coroutine(1, m_Phase1Duration, m_Phase1CommandCount, 0));
        yield return null;

        _commandItemImageList.Clear();
        _playClockTicking = false;
        m_Game.onFinishPhase -= _onFinishPhaseAction;
        m_Game.onCorrectCommand -= _onCorrectCommandAction;
        m_Game.onCommandListRefreshed -= _onCommandListRefreshedAction;
        m_Game.onWrongCommand -= _onWrongCommandAction;

        yield return new WaitForSecondsRealtime(m_EndPhaseDelay);

        // phase 2
        Initialize();
        m_CommandParentTop.SetActive(true);
        m_CommandParentBottom.SetActive(true);

        _onFinishPhaseAction = (_phaseNumber, _count) =>
        {
            if (_count >= m_Phase2CommandCount)
            {
                m_UI_CommandFeedbackExcellent.SetActive(true);
                m_SoundManager.PlayExcellent();
            }
            else if (_count >= 4)
            {
                m_UI_CommandFeedbackGood.SetActive(true);
                m_SoundManager.PlayGood();
            }
            else
            {
                m_UI_CommandFeedbackBad.SetActive(true);
                m_SoundManager.PlayBad();
            }
        };
        m_Game.onFinishPhase += _onFinishPhaseAction;

        _onCommandListRefreshedAction = (_commandList, _isSpecialCommandList) =>
        {
            for (int i = 0; i < _commandList.Count; ++i)
            {
                int _command = _commandList[i];

                // 스프라이트를 고르고 초기화합니다.
                var _commandItem = GameObject.Instantiate(
                    m_Prefab_CommandItem,
                    i < _commandList.Count / 2
                        ? m_CommandParentTop.transform
                        : m_CommandParentBottom.transform);
                var _commandItemImage = _commandItem.transform.GetComponent<Image>();
                _commandItemImage.sprite = _isSpecialCommandList[i] ? m_SpecialSpriteArray[_command] : m_SpriteArray[_command];

                _commandItemImageList.Add(_commandItemImage);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(m_CommandParentTop.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_CommandParentBottom.GetComponent<RectTransform>());
        };
        m_Game.onCommandListRefreshed += _onCommandListRefreshedAction;

        _onCorrectCommandAction = (_commandIndex) =>
        {
            ++score;

            _commandItemImageList[_commandIndex].color = new Color(1, 1, 1, 0.0f);
            if (m_Prefab_Fx_CorrectCommand != null)
            {
                var _fx = GameObject.Instantiate(m_Prefab_Fx_CorrectCommand, m_MinigameCanvas.transform);
                _fx.transform.position = _commandItemImageList[_commandIndex].gameObject.transform.position;
                Destroy(_fx, 2.0f);
            }
        };
        m_Game.onCorrectCommand += _onCorrectCommandAction;

        _onWrongCommandAction = (_commandIndex) =>
        {
            _commandItemImageList[_commandIndex].color = new Color(1, 1, 1, 0.0f);
            if (m_Prefab_Fx_WrongCommand != null)
            {
                var _fx = GameObject.Instantiate(m_Prefab_Fx_WrongCommand, m_MinigameCanvas.transform);
                _fx.transform.position = _commandItemImageList[_commandIndex].gameObject.transform.position;
                Destroy(_fx, 2.0f);
            }

            if (m_ChangeBackgroundWrongCoroutine != null)
                StopCoroutine(m_ChangeBackgroundWrongCoroutine);
            m_ChangeBackgroundWrongCoroutine = StartCoroutine(ChangeBackgroundWrong());
        };
        m_Game.onWrongCommand += _onWrongCommandAction;

        _onSpecialCommandPressedAction = (_commandIndex) =>
        {
            _commandItemImageList[_commandIndex].gameObject.GetComponent<Animator>().SetTrigger("Shake");
        };
        m_Game.onSpecialCommandPressed += _onSpecialCommandPressedAction;

        m_CommandParentTop.SetActive(true);
        yield return StartCoroutine(
            m_Game.Phase12Coroutine(2, m_Phase2Duration, m_Phase2CommandCount, m_Phase2SpecialCommandCount));
        yield return null;

        _commandItemImageList.Clear();
        _playClockTicking = false;
        m_Game.onFinishPhase -= _onFinishPhaseAction;
        m_Game.onCorrectCommand -= _onCorrectCommandAction;
        m_Game.onCommandListRefreshed -= _onCommandListRefreshedAction;
        m_Game.onWrongCommand -= _onWrongCommandAction;
        m_Game.onSpecialCommandPressed -= _onSpecialCommandPressedAction;

        yield return new WaitForSecondsRealtime(m_EndPhaseDelay);
        m_CommandParentTop.SetActive(false);

        // phase 3

        int _catchedBugCount = 0;

        foreach (var button in m_UI_ButtonArray)
        {
            // 모든 버튼의 이미지를 bug 이미지로 변경합니다.
            button.transform.GetComponent<Image>().sprite = m_BugSprite;
        }

        Initialize();
        m_UI_Phase3Panel.SetActive(true);

        _onFinishPhaseAction = (_phaseNumber, _count) => { m_UI_Finish.SetActive(true); };
        m_Game.onFinishPhase += _onFinishPhaseAction;

        _onPressCommandButtonAction = _index =>
        {
            ++score;
            ++_catchedBugCount;

            int _remainingBugCount = m_Phase3MaxCommandCount - _catchedBugCount;

            if (_remainingBugCount == 0)
            {
                m_Game.forceStopPhase3 = true;
            }

            m_UI_RemainingBugCountText.text = _remainingBugCount.ToString();

            var _fx = GameObject.Instantiate(m_Prefab_Fx_Bug, m_MinigameCanvas.transform);
            _fx.transform.position = m_UI_BugIcon.transform.position;
            Destroy(_fx, 2.0f);

            m_SoundManager.PlayBugCatch();
        };
        onPressCommandButton += _onPressCommandButtonAction;


        yield return StartCoroutine(
            m_Game.Phase3Coroutine(3, m_Phase3Duration));
        yield return null;

        _playClockTicking = false;
        m_Game.onFinishPhase -= _onFinishPhaseAction;
        onPressCommandButton -= _onPressCommandButtonAction;

        yield return new WaitForSecondsRealtime(m_EndPhaseDelay);

        // end game

        m_UI_ResultScoreText.text = $"{score}점";
        m_UI_ResultScorePanel.SetActive(true);
        m_SoundManager.PlayResultSound();

        onFinishGame?.Invoke();
    }

    private void Validate()
    {
        // Assert.AreEqual(3, m_Game.m_SpriteArray.Length);
        Assert.AreEqual(3, m_UI_ButtonArray.Length);
    }

    private void Initialize()
    {
        for (int i = m_CommandParentTop.transform.childCount - 1; i >= 0; --i)
            Destroy(m_CommandParentTop.transform.GetChild(i).gameObject);

        for (int i = m_CommandParentBottom.transform.childCount - 1; i >= 0; --i)
            Destroy(m_CommandParentBottom.transform.GetChild(i).gameObject);

        LayoutRebuilder.ForceRebuildLayoutImmediate(m_CommandParentTop.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(m_CommandParentBottom.GetComponent<RectTransform>());

        m_CommandParentTop.SetActive(false);
        m_CommandParentBottom.SetActive(false);

        m_UI_TimeSlider.value = 1.0f;
        m_UI_ScoreText.text = m_Score.ToString();
        m_UI_RemainingBugCountText.text = m_Phase3MaxCommandCount.ToString();
        m_UI_CommandFeedbackBad.SetActive(false);
        m_UI_CommandFeedbackGood.SetActive(false);
        m_UI_CommandFeedbackExcellent.SetActive(false);
        m_UI_Finish.SetActive(false);
        m_UI_ResultScorePanel.SetActive(false);
        m_UI_Phase3Panel.SetActive(false);
    }

    public void ClickPDScript()
    {
        if (m_PDScript.Count > 0)
        {
            m_PDText.text = m_PDScript.Dequeue();
        }
        else
        {
            PlayerInfo.Instance.IsGameJamMiniGameFirst = false;

            m_PopOffPDPanel.TurnOffUI();
        }
    }

    public void FinishGame()
    {
        m_PopOffMiniGamePanel.TurnOffUI();

        m_Prefab_Mini1_Panel.SetActive(true);

        m_SoundManager.ReplayInGameBgm();
        m_SoundManager.StopMain2Audio();

        for (int i = 0; i < 3; ++i)
        {
            // 모든 버튼의 이미지를 각 파트 이미지로 변경합니다.
            Button button = m_UI_ButtonArray[i];
            button.transform.GetComponent<Image>().sprite = m_ButtonSpriteArray[i];
        }

        m_GameJam.MiniGameScore = m_Score;

        m_Score = 0;

        Initialize();

        InitScript();

        m_PDText.text = m_PDScript.Dequeue();

        m_PopUpPDPanel.TurnOnUI();
    }

    private IEnumerator ChangeBackgroundWrong()
    {
        m_UI_TopPanel.sprite = m_Sprite_BackgroundWrong;
        yield return new WaitForSecondsRealtime(m_WrongBackgroundDuration);
        m_UI_TopPanel.sprite = m_Sprite_BackgroundNormal;
        m_ChangeBackgroundWrongCoroutine = null;
    }

    public void OnClickCommandButton(int _index)
    {
        onPressCommandButton?.Invoke(_index);
    }

    // for test
    /// 미니 게임잼 시작할 때 불러주기
    public void SetGameStart()
    {
        Validate();

        m_Score = 0;

        m_Prefab_Mini1_Panel.SetActive(false);

        // 남은 시간 변화에 따라 UI를 갱신합니다.
        m_Game.onRemainingTimeChanged += (_percentage, _time) => { m_UI_TimeSlider.value = _percentage; };

        // 점수 변화에 따라 UI를 갱신합니다.
        onScoreChanged += _newScore => m_UI_ScoreText.text = _newScore.ToString();
        m_UI_ScoreText.text = m_Score.ToString();

        // 버튼을 누르면 게임 씬에 버튼 눌림을 전달합니다.
        onPressCommandButton += _command => m_Game.PressCommandButton(_command);

        // 게임을 시작하지
        StartGame();
    }

    private void InitScript()
    {
        m_PDScript.Clear();

        if (PlayerInfo.Instance.IsGameJamMiniGameFirst == true)
        {
            m_PDScript.Enqueue("원장님의 도움으로 재밌는 게임이 나올 것 같아요!");
            m_PDScript.Enqueue("게임잼 게임 개발의 마무리가 잘 이뤄질 것 같네요.");
            m_PDScript.Enqueue("고생하셨습니다!");
        }
        else
        {
            m_PDScript.Enqueue("고생하셨습니다!\n학생들의 게임 개발 마무리가 원활하게 이루어질 것 같아요~");
        }
    }
}