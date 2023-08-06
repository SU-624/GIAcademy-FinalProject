using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// 2022. 11. 22 Mang
/// 2023.05.25 Ocean 언니의 로딩씬 클래스에 이것저것 추가!
/// 
/// 게임에서 씬 이동을 담당할 클래스
/// </summary>
public class MoveSceneManager : MonoBehaviour
{
    public static MoveSceneManager m_Instance = null;       // Manager 변수는 싱글톤으로 사용

    [SerializeField] private Slider LoadingBar;
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI m_TipScript;
    public GameObject TitleCamera;
    public GameObject LoadingPanel;
    private List<string> m_Tips = new List<string>();
    private int m_ScriptListIndex;
    private string loadSceneName;
    private bool m_IsTitle;

    public bool UseLoadingData = false; // 불러오기 기능을 사용할 것인지 체크

    // 싱글톤으로 만든 변수를 안전하게 생성하고 사용하기 위한 초기화(?) 방법
    private void Awake()
    {
        // Instance 가 존재한다면 gameObject 제거
        if (m_Instance != null)
        {
            Destroy(this.gameObject);

            return;
        }

        // 시작될 때 인스턴스 초기화, 씬을 넘어갈때도 유지되기 위한 처리
        m_Instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        m_ScriptListIndex = 0;
        InitTipScriptList();
        m_ScriptListIndex = m_Tips.Count;

        ChangeTipScript();

        m_IsTitle = true;
    }

    private void InitTipScriptList()
    {
        m_Tips.Add("<line-height=120%>게임잼 장르와 보너스 점수\n<size=80%>게임잼에서 요구하는 장르를 맞춰 게임을 제작할 경우, 보너스 점수를 획득할 수 있습니다.</size>");
        m_Tips.Add("<line-height=120%>게임잼의 선호 장르\n<size=80%>게임잼마다 선호하는 게임의 장르가 존재합니다.</size>");
        m_Tips.Add("<line-height=120%>게임쇼 심사위원\n<size=80%>게임쇼 심사위원을 눈여겨보세요. 선호하는 장르가 숨겨져있습니다.</size>");
        m_Tips.Add("<line-height=120%>학생의 성격\n<size=80%>학생들은 각각의 고유한 성격을 지니고있습니다.</size>");

        m_Tips.Add("<line-height=120%>인재추천\n<size=80%>2월달에 학생들을 회사에 추천하여 취업시킬 수 있습니다.</size>");
        m_Tips.Add("<line-height=120%>인재추천\n<size=80%>특정 회사는 특수한 능력을 가진 학생을 원합니다.</size>");
        m_Tips.Add("<line-height=120%>학생 능력\n<size=80%>학생의 특수 능력은 게임잼이나 게임쇼 등을 통해 얻을 수 있습니다.</size>");
        m_Tips.Add("<line-height=120%>퍼즐방\n<size=80%>퍼즐방의 로켓은 20221004번 회전하고 있습니다.</size>");
        m_Tips.Add("<line-height=120%>액션방\n<size=80%>액션방의 대결은 1000번째 이뤄지고 있습니다.</size>");
        m_Tips.Add("<line-height=120%>시뮬레이션방\n<size=80%>어디서 아침을 울리는 바람소리가 들리지 않나요?</size>");
        m_Tips.Add("<line-height=120%>어드벤쳐방\n<size=80%>버섯집 굴뚝은 150년간 연기가 피어오르고 있습니다.</size>");
    }

    public void UseLoadingDataBtn()
    {
        if (!UseLoadingData)
            UseLoadingData = true;
        else if (UseLoadingData)
            UseLoadingData = false;
    }

    public void MoveToInGameScene()
    {
        if (UseLoadingData)
        {
            Json.Instance.UseLoadingData = true;
            Json.Instance.PressLadingBtn();
        }

        Debug.Log("버튼눌러서 씬 전환~");
        PopUpUI pop = new PopUpUI();

        pop.PopUpUIOnTitleScene(LoadingPanel);

        TitleCamera.SetActive(false);
        LoadScene("InGameScene");
        //SceneManager.LoadScene("InGameScene");
    }

    public void MoveToTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        //SceneManager.sceneLoaded += LoadSceneEnd;
        loadSceneName = sceneName;
        StartCoroutine(Load(sceneName));
        //StartCoroutine(LoadingSceneScript());
    }

    private void ChangeTipScript()
    {
        int _index = Random.Range(0, m_ScriptListIndex);

        m_TipScript.text = m_Tips[_index];
        m_ScriptListIndex -= 1;
    }

    private IEnumerator LoadingSceneScript()
    {
        while (m_IsTitle)
        {
            yield return null;

        }
    }

    private IEnumerator Load(string sceneName)
    {
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float timer = 0.0f;

        while (!op.isDone)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;

            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                if (m_ScriptListIndex == 0)
                {
                    m_ScriptListIndex = m_Tips.Count;
                }

                ChangeTipScript();
            }

            //int _index = Random.Range(0, m_Tips.Count);
            //m_TipScript.text = m_Tips[_index];

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    m_IsTitle = false;
                    yield break;
                }
            }
        }
    }

    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= LoadSceneEnd;
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;

        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 2f;
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}
