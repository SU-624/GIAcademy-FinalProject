using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 23. 01. 02 Mang
/// 
/// InGameScene 의 메인 화면에서 쓰일 UI 들을 관리 할 스크립트
/// </summary>
public class InGameUI : MonoBehaviour
{
    private static InGameUI instance = null;

    public static InGameUI Instance
    {
        get
        {
            return instance;
        }

        set { instance = value; }
    }
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // 켜진 UI 들을 다 담아 둘 스택
    public Stack<GameObject> UIStack;

    [SerializeField]
    PopUpUI m_popUpInstant;     // Title -> InGame 씬으로 이동 시 제일 먼저 띄워질 팝업창

    public TextMeshProUGUI m_nowAcademyName;
    public TextMeshProUGUI m_nowDirectorName;

    public TextMeshProUGUI m_nowMoney;
    public TextMeshProUGUI m_touchcount;

    public Image m_TimeBar;

    // public TextMeshProUGUI m_CamerapointX;
    // public TextMeshProUGUI m_CamerapointY;
    // public TextMeshProUGUI m_CamerapointZ;
    //
    // public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        // UI에 그릴 데이터 로드 & 기본값 설정
        m_nowAcademyName.text = PlayerInfo.Instance.m_AcademyName;
        m_nowDirectorName.text = PlayerInfo.Instance.m_DirectorName;
        m_nowMoney.text = PlayerInfo.Instance.m_MyMoney.ToString();

        m_TimeBar.fillAmount = 0.2f;

        UIStack = new Stack<GameObject>();
        Debug.Log("stack count : " + UIStack.Count);
        // 
        m_popUpInstant.DelayTurnOnUI();

    }

    // Update is called once per frame
    void Update()
    {
        GameTime.Instance.CheckPerSecond(m_TimeBar);       // 하루(6초) 체크

        m_nowMoney.text = PlayerInfo.Instance.m_MyMoney.ToString();

        if (Input.touchCount == 0)
        {
            m_touchcount.text = "0";
        }
        if (Input.touchCount == 1)
        {
            m_touchcount.text = "1";


            // m_touchpointX.text = ;
        }
        if (Input.touchCount == 2)
        {
            m_touchcount.text = "2";

        }


        //m_CamerapointX.text = camera.transform.position.x.ToString();
        //m_CamerapointY.text = camera.transform.position.y.ToString();
        //m_CamerapointZ.text = camera.transform.position.z.ToString();


    }
}
