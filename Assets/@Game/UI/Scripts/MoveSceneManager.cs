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
        m_ScriptListIndex = m_Tips.Count;
        m_IsTitle = true;
        m_Tips.Add("<line-height=120%>게임잼 장르와 보너스 점수\n<size=80%>게임잼에서 요구하는 장르를 맞춰 게임을 제작할 경우, 보너스 점수를 획득할 수 있습니다.</size>");
        m_Tips.Add("<line-height=120%>게임잼의 선호 장르\n<size=80%>게임잼마다 선호하는 게임의 장르가 존재합니다.</size>");
        m_Tips.Add("<line-height=120%>게임쇼 심사위원\n<size=80%>게임쇼 심사위원을 눈여겨보세요. 선호하는 장르가 숨겨져있습니다.</size>");
        m_Tips.Add("<line-height=120%>학생의 성격\n<size=80%>학생들은 각각의 고유한 성격을 지니고있습니다.</size>");
    }    

    public void MoveToInGameScene()
    {
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

                int _index = Random.Range(0, m_ScriptListIndex);

                m_TipScript.text = m_Tips[_index];

                m_ScriptListIndex -= 1;
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
