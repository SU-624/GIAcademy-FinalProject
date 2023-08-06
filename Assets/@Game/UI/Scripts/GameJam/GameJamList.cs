using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 이전에 만들었던 게임잼들을 볼 수 있는 패널. 등급별로, 장르별로 볼 수 있게 해야한다.
/// 
/// 23.03.24 Ocean
/// </summary>
public class GameJamList : MonoBehaviour
{
    [SerializeField] private FinalGameJamResult m_FinalGameJamResult;
    [SerializeField] private GameJam m_GameJam;
    [SerializeField] private Button m_BackButton;
    [SerializeField] private Button m_HomeButton;
    [SerializeField] private Button m_PreButton;
    [SerializeField] private Button m_NextButton;
    [SerializeField] private Button m_GenreFilter;
    [SerializeField] private Image m_GenreFilterButtons;

    [Space(5f)]
    [Header("게임 등급에 맞춰 띄워주는 버튼")]
    [SerializeField] private Button m_AllButton;
    [SerializeField] private Button m_AAAButton;
    [SerializeField] private Button m_AAButton;
    [SerializeField] private Button m_AButton;
    [SerializeField] private Button m_BButton;
    [SerializeField] private Button m_IncompleteButton;

    [Space(5f)]
    [Header("게임 장르에 맞춰 띄워주는 버튼들")]
    [SerializeField] private Button m_AllGenreButton;
    [SerializeField] private Button m_ActionButton;
    [SerializeField] private Button m_SimulationButton;
    [SerializeField] private Button m_AdventureButton;
    [SerializeField] private Button m_ShootingButton;
    [SerializeField] private Button m_RPGButton;
    [SerializeField] private Button m_PuzzleButton;
    [SerializeField] private Button m_RythmButton;
    [SerializeField] private Button m_SportButton;
    [SerializeField] private TextMeshProUGUI m_FilterName;

    [Space(5f)]
    [SerializeField] private GameObject m_GameListPanel;                    // 게임잼의 정보를 확인하려고 버튼을 누르면 이 패널이 꺼지고 다른 패널이 나와야한다.
    [SerializeField] private GameObject m_GameJamListPanel;                 // 과거 게임잼의 최상위 부모. SetActive로 켰다 꺼줬다가만 하면 된다.
    [SerializeField] private GameObject m_GameJamInfoPanel;                 // 이 Panel이 켜져있으면 어떤 게임 목록을 눌렀다는 소리이니 현재 panel을 꺼주고 이전 화면을 보여준다.
    [SerializeField] private GameObject m_GameJamListParentObj;
    [SerializeField] private GameObject m_GameScore;
    [SerializeField] private GameObject m_StudentStat;
    [SerializeField] private GameObject m_GameJamList;
    [SerializeField] private Transform m_GameListContentParent;             // 이전에 만들었던 게임들의 목록의 부모가 될 transform

    [Space(5f)]
    [Header("게임의 이름과 실행한 날짜를 써줘야한다")]
    [SerializeField] private TextMeshProUGUI m_GameDate;
    [SerializeField] private TextMeshProUGUI m_GameName;
    [SerializeField] private TextMeshProUGUI m_GameTitle;

    [Space(5f)]
    [Header("게임잼에 참여한 각 학과 학생들의 이름, 프로필")]
    [SerializeField] private TextMeshProUGUI m_GameDesignerName;
    [SerializeField] private TextMeshProUGUI m_ArtName;
    [SerializeField] private TextMeshProUGUI m_ProgrammingName;
    [SerializeField] private Image m_GameDesignerImage;
    [SerializeField] private Image m_ArtImage;
    [SerializeField] private Image m_ProgrammingImage;
    [Space(5f)]

    [SerializeField] private Image m_GameImage;             // 게임의 이미지

    [Space(5f)]
    [Header("게임잼의 등급에 가장 기여도가 높은 학생에게 MVP를 켜준다.")]
    [SerializeField] private Image m_GameDesignerMvp;
    [SerializeField] private Image m_ArtMvp;
    [SerializeField] private Image m_ProgrammingMvp;

    [Space(5f)]
    [Header("게임잼에서 만든 게임의 최종 점수들 장르는 이미지로 바꿔줘야한다.")]
    [SerializeField] private Image m_GenreImage;
    [SerializeField] private TextMeshProUGUI m_Concept;
    [SerializeField] private TextMeshProUGUI m_Funny;
    [SerializeField] private TextMeshProUGUI m_Graphic;
    [SerializeField] private TextMeshProUGUI m_Perfection;
    [SerializeField] private TextMeshProUGUI m_GenreBonus;
    [SerializeField] private TextMeshProUGUI m_Score;
    [SerializeField] private TextMeshProUGUI m_Rank;

    [Space(5f)]
    [Header("선택한 각 학과 학생들의 능력치를 나타내줄 슬라이더")]
    [SerializeField] private Image m_GameDesignerRequiredStat1;
    [SerializeField] private Image m_GameDesignerRequiredStat2;
    [SerializeField] private Image m_ArtRequiredStat1;
    [SerializeField] private Image m_ArtRequiredStat2;
    [SerializeField] private Image m_ProgrammingRequiredStat1;
    [SerializeField] private Image m_ProgrammingRequiredStat2;
    [SerializeField] private Slider m_GameDesignerSlider;
    [SerializeField] private Slider m_ArtSlider;
    [SerializeField] private Slider m_ProgrammingSlider;
    [SerializeField] private Image m_GameDesignerSliderImage;
    [SerializeField] private Image m_ArtSliderImage;
    [SerializeField] private Image m_ProgrammingSliderImage;
    [SerializeField] private Image m_GameDesignerSliderBar;
    [SerializeField] private Image m_ArtSliderBar;
    [SerializeField] private Image m_ProgrammingSliderBar;

    [SerializeField] private TextMeshProUGUI m_CurrentMoney;
    [SerializeField] private TextMeshProUGUI m_CurrentSpecialPoints;

    [SerializeField] private PopOffUI m_PopOffGameJamListPanel;

    public Transform _gamejameContentParent { get { return m_GameListContentParent; } }
    public Button AllButton { get { return m_AllButton; } set { m_AllButton = value; } }
    public TextMeshProUGUI _genreButtonName { get { return m_FilterName; } set { m_FilterName = value; } }
    public Image GameDesignerRequiredStatImage1 { get { return m_GameDesignerRequiredStat1; } set { m_GameDesignerRequiredStat1 = value; } }
    public Image GameDesignerRequiredStatImage2 { get { return m_GameDesignerRequiredStat2; } set { m_GameDesignerRequiredStat2 = value; } }
    public Image ArtRequiredStatImage1 { get { return m_ArtRequiredStat1; } set { m_ArtRequiredStat1 = value; } }
    public Image ArtRequiredStatImage2 { get { return m_ArtRequiredStat2; } set { m_ArtRequiredStat2 = value; } }
    public Image ProgrammingRequiredStatImage1 { get { return m_ProgrammingRequiredStat1; } set { m_ProgrammingRequiredStat1 = value; } }
    public Image ProgrammingRequiredStatImage2 { get { return m_ProgrammingRequiredStat2; } set { m_ProgrammingRequiredStat2 = value; } }

    /// 버튼들을 Start에서 AddListener를 해주면 Inspector창에서 연결이 끊기는 걸 걱정안해도 되지 않을까,,
    private void Start()
    {
        m_BackButton.onClick.AddListener(ClickBackButton);
        m_HomeButton.onClick.AddListener(clickQuitGameJamButton);
        m_PreButton.onClick.AddListener(ClickResultScoreNextOrPreButton);
        m_NextButton.onClick.AddListener(ClickResultScoreNextOrPreButton);
        m_GenreFilter.onClick.AddListener(ClickFilter);
        m_AllButton.Select();
    }

    // 뒤로 가기 버튼을 눌렀을 때 제일 처음이면 게임 결과 창으로 가게 해야한다.
    private void ClickBackButton()
    {
        if (m_FinalGameJamResult.gameObject.activeSelf)
        {
            if (m_GameJamInfoPanel.activeSelf == true)
            {
                m_GameJamListPanel.SetActive(true);
                m_GameJamInfoPanel.SetActive(false);
                m_GameJamList.SetActive(true);
                m_GenreFilter.gameObject.SetActive(true);
            }
            else
            {
                m_GameJamListPanel.SetActive(false);
                m_GenreFilterButtons.gameObject.SetActive(false);
                m_GenreFilter.gameObject.SetActive(false);

                m_GameDesignerMvp.gameObject.SetActive(false);
                m_ArtMvp.gameObject.SetActive(false);
                m_ProgrammingMvp.gameObject.SetActive(false);
            }

            m_FinalGameJamResult.SetMoneyAndSpecialPoints();
        }
        // 유아이씬에서 만든 게임 버튼 눌렀을 때 
        else
        {
            if (m_GameJamInfoPanel.activeSelf == true)
            {
                m_GameScore.SetActive(true);
                m_StudentStat.SetActive(false);
                m_GameJamInfoPanel.SetActive(false);
                m_GameJamList.SetActive(true);
                m_GenreFilter.gameObject.SetActive(true);
            }
            else
            {
                m_GenreFilterButtons.gameObject.SetActive(false);
                m_GenreFilter.gameObject.SetActive(false);

                m_GameDesignerMvp.gameObject.SetActive(false);
                m_ArtMvp.gameObject.SetActive(false);
                m_ProgrammingMvp.gameObject.SetActive(false);
                m_PopOffGameJamListPanel.TurnOffUI();
            }
        }
    }

    public void ChangeGenreSprite(Sprite _genre)
    {
        m_GenreImage.sprite = _genre;
    }

    public void ClickRankButton()
    {
        m_GameListPanel.SetActive(true);
        m_GameJamInfoPanel.SetActive(false);
        m_GenreFilter.gameObject.SetActive(true);
        m_GameScore.SetActive(true);
        m_StudentStat.SetActive(false);
    }

    public void ClickFilter()
    {
        if (m_GenreFilterButtons.gameObject.activeSelf == true)
        {
            m_GenreFilterButtons.gameObject.SetActive(false);
        }
        else
        {
            m_GenreFilterButtons.gameObject.SetActive(true);
        }
    }

    // 만약 이전 게임 목록이 아닌 이전 게임의 상세 패널이 켜져있으면 버튼을 못누르게 하기위한 함수
    public bool IsInfoPanelActive()
    {
        bool _isActive;

        if (m_GameJamInfoPanel.activeSelf == true)
        {
            _isActive = true;
        }
        else
        {
            _isActive = false;
        }

        return _isActive;
    }

    public void ClickResultScoreNextOrPreButton()
    {
        if (m_GameScore.activeSelf == true)
        {
            m_GameScore.SetActive(false);
            m_StudentStat.SetActive(true);
        }
        else
        {
            m_GameScore.SetActive(true);
            m_StudentStat.SetActive(false);
        }
    }

    public void clickPreGameJamButton()
    {
        m_GameJamListPanel.SetActive(true);
        m_GameJamList.SetActive(true);
        m_GameJamInfoPanel.SetActive(false);

        m_CurrentMoney.text = string.Format("{0:#,0}", PlayerInfo.Instance.MyMoney);
        m_CurrentSpecialPoints.text = string.Format("{0:#,0}", PlayerInfo.Instance.SpecialPoint);
    }

    public void clickQuitGameJamButton()
    {
        if (m_FinalGameJamResult.gameObject.activeSelf)
        {
            m_GameJamInfoPanel.SetActive(false);
            m_GameJamList.SetActive(true);
            m_StudentStat.SetActive(false);

            m_GameDesignerMvp.gameObject.SetActive(false);
            m_ArtMvp.gameObject.SetActive(false);
            m_ProgrammingMvp.gameObject.SetActive(false);
            m_GenreFilter.gameObject.SetActive(false);

            m_GameJam.ClickHomeButtonYes();
            m_GameJamListPanel.SetActive(false);

            m_FinalGameJamResult.ClickQuitButton();
        }
        else
        {
            m_GameJamInfoPanel.SetActive(false);
            m_GameJamList.SetActive(true);
            m_StudentStat.SetActive(false);

            m_GameDesignerMvp.gameObject.SetActive(false);
            m_ArtMvp.gameObject.SetActive(false);
            m_ProgrammingMvp.gameObject.SetActive(false);
            m_GenreFilter.gameObject.SetActive(false);

            m_PopOffGameJamListPanel.TurnOffUI();
        }
    }

    // 만들어진 게임잼을 누르면 게임잼 정보 패널이 켜진다.
    public void ClickGameJamPrefab()
    {
        m_GameJamList.SetActive(false);
        m_GameJamInfoPanel.SetActive(true);
        m_GenreFilterButtons.gameObject.SetActive(false);
        m_GenreFilter.gameObject.SetActive(false);
    }

    public void MoveGameJamList(GameObject _gameObject, Transform _parent)
    {
        _gameObject.transform.parent = _parent;
        _gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void ChangeGameInfo(string _date, string _name, string _title)
    {
        m_GameDate.text = _date;
        m_GameName.text = _name;
        m_GameTitle.text = _title;
    }

    public void ChangeStudentInfo(string _gmName, string _artName, string _programmingName, Sprite _gmProfile, Sprite _artProfile, Sprite _programmingProfile)
    {
        m_GameDesignerName.text = _gmName;
        m_ArtName.text = _artName;
        m_ProgrammingName.text = _programmingName;
        m_GameDesignerImage.sprite = _gmProfile;
        m_ArtImage.sprite = _artProfile;
        m_ProgrammingImage.sprite = _programmingProfile;
    }

    // 스탯이 제일 높은 학생이 MVP이다.
    public void ChangeMVPImageGameListPanel(double _gmStudent, double _artStudent, double _programmingStudent)
    {
        double[] arr = { _gmStudent, _artStudent, _programmingStudent };
        string[] studentType = { "기획", "아트", "플밍" };

        double max = _gmStudent;
        string part = "기획";

        for (int i = 0; i < arr.Length; i++)
        {
            if (max < arr[i])
            {
                max = arr[i];
                part = studentType[i];
            }
        }

        if (part == "기획")
        {
            m_GameDesignerMvp.gameObject.SetActive(true);
            m_ArtMvp.gameObject.SetActive(false);
            m_ProgrammingMvp.gameObject.SetActive(false);
        }
        else if (part == "아트")
        {
            m_GameDesignerMvp.gameObject.SetActive(false);
            m_ArtMvp.gameObject.SetActive(true);
            m_ProgrammingMvp.gameObject.SetActive(false);
        }
        else
        {
            m_GameDesignerMvp.gameObject.SetActive(false);
            m_ArtMvp.gameObject.SetActive(false);
            m_ProgrammingMvp.gameObject.SetActive(true);
        }
    }

    public void ChangeConceptSprite(Sprite _concept)
    {
        m_GameImage.sprite = _concept;
    }

    public void ChangeGameJamScorePanel(string _concept, string _funny, string _graphic,
                                            string _perfection, string _genreBonus, string _score, string _rank)
    {
        m_Concept.text = _concept;
        m_Funny.text = _funny;
        m_Graphic.text = _graphic;
        m_Perfection.text = _perfection;
        m_GenreBonus.text = _genreBonus;
        m_Score.text = _score;
        m_Rank.text = _rank;
    }

    public void SetPreGameStatSliderValue(StudentType _type, int _Statvalue, int requiredStat)
    {
        float _posX = (requiredStat / 150f) * 596f;

        if (_type == StudentType.GameDesigner)
        {
            m_GameDesignerSlider.value = _Statvalue;
            m_GameDesignerSliderBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);  // 이 게임잼에 필요한 최소 스탯 수치를 나타내는 바
        }
        else if (_type == StudentType.Art)
        {
            m_ArtSlider.value = _Statvalue;

            m_ArtSliderBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);
        }
        else if (_type == StudentType.Programming)
        {
            m_ProgrammingSlider.value = _Statvalue;

            m_ProgrammingSliderBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);
        }
    }

    public void SetGameDesignerPreGameStatSliderColor(Sprite _satisfy)
    {
        m_GameDesignerSliderImage.sprite = _satisfy;
    }

    public void SetArtPreGameStatSliderColor(Sprite _satisfy)
    {
        m_ArtSliderImage.sprite = _satisfy;
    }

    public void SetProgrammingPreGameStatSliderColor(Sprite _satisfy)
    {
        m_ProgrammingSliderImage.sprite = _satisfy;
    }

    //public void SetSliderBar(int _gmValue, int _artValue, int _programmingValue)
    //{
    //    m_GameDesignerSliderBar.rectTransform.anchoredPosition = new Vector3(_gmValue * 6, 0, 0);
    //    m_ArtSliderBar.rectTransform.anchoredPosition = new Vector3(_artValue * 6, 0, 0);
    //    m_ProgrammingSliderBar.rectTransform.anchoredPosition = new Vector3(_programmingValue * 6, 0, 0);
    //}

    public void DestroyObj()
    {
        Transform[] _childCount = m_GameJamListParentObj.GetComponentsInChildren<Transform>();

        if (_childCount != null)
        {
            for (int i = 0; i < _childCount.Length; i++)
            {
                if (_childCount[i] != m_GameJamListParentObj.transform)
                {
                    Destroy(_childCount[i].gameObject);
                }
            }
        }
    }
}
