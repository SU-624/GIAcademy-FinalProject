using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ������ ������� ��������� �� �� �ִ� �г�. ��޺���, �帣���� �� �� �ְ� �ؾ��Ѵ�.
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
    [Header("���� ��޿� ���� ����ִ� ��ư")]
    [SerializeField] private Button m_AllButton;
    [SerializeField] private Button m_AAAButton;
    [SerializeField] private Button m_AAButton;
    [SerializeField] private Button m_AButton;
    [SerializeField] private Button m_BButton;
    [SerializeField] private Button m_IncompleteButton;

    [Space(5f)]
    [Header("���� �帣�� ���� ����ִ� ��ư��")]
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
    [SerializeField] private GameObject m_GameListPanel;                    // �������� ������ Ȯ���Ϸ��� ��ư�� ������ �� �г��� ������ �ٸ� �г��� ���;��Ѵ�.
    [SerializeField] private GameObject m_GameJamListPanel;                 // ���� �������� �ֻ��� �θ�. SetActive�� �״� ����ٰ��� �ϸ� �ȴ�.
    [SerializeField] private GameObject m_GameJamInfoPanel;                 // �� Panel�� ���������� � ���� ����� �����ٴ� �Ҹ��̴� ���� panel�� ���ְ� ���� ȭ���� �����ش�.
    [SerializeField] private GameObject m_GameJamListParentObj;
    [SerializeField] private GameObject m_GameScore;
    [SerializeField] private GameObject m_StudentStat;
    [SerializeField] private GameObject m_GameJamList;
    [SerializeField] private Transform m_GameListContentParent;             // ������ ������� ���ӵ��� ����� �θ� �� transform

    [Space(5f)]
    [Header("������ �̸��� ������ ��¥�� ������Ѵ�")]
    [SerializeField] private TextMeshProUGUI m_GameDate;
    [SerializeField] private TextMeshProUGUI m_GameName;
    [SerializeField] private TextMeshProUGUI m_GameTitle;

    [Space(5f)]
    [Header("�����뿡 ������ �� �а� �л����� �̸�, ������")]
    [SerializeField] private TextMeshProUGUI m_GameDesignerName;
    [SerializeField] private TextMeshProUGUI m_ArtName;
    [SerializeField] private TextMeshProUGUI m_ProgrammingName;
    [SerializeField] private Image m_GameDesignerImage;
    [SerializeField] private Image m_ArtImage;
    [SerializeField] private Image m_ProgrammingImage;
    [Space(5f)]

    [SerializeField] private Image m_GameImage;             // ������ �̹���

    [Space(5f)]
    [Header("�������� ��޿� ���� �⿩���� ���� �л����� MVP�� ���ش�.")]
    [SerializeField] private Image m_GameDesignerMvp;
    [SerializeField] private Image m_ArtMvp;
    [SerializeField] private Image m_ProgrammingMvp;

    [Space(5f)]
    [Header("�����뿡�� ���� ������ ���� ������ �帣�� �̹����� �ٲ�����Ѵ�.")]
    [SerializeField] private Image m_GenreImage;
    [SerializeField] private TextMeshProUGUI m_Concept;
    [SerializeField] private TextMeshProUGUI m_Funny;
    [SerializeField] private TextMeshProUGUI m_Graphic;
    [SerializeField] private TextMeshProUGUI m_Perfection;
    [SerializeField] private TextMeshProUGUI m_GenreBonus;
    [SerializeField] private TextMeshProUGUI m_Score;
    [SerializeField] private TextMeshProUGUI m_Rank;

    [Space(5f)]
    [Header("������ �� �а� �л����� �ɷ�ġ�� ��Ÿ���� �����̴�")]
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

    /// ��ư���� Start���� AddListener�� ���ָ� Inspectorâ���� ������ ����� �� �������ص� ���� ������,,
    private void Start()
    {
        m_BackButton.onClick.AddListener(ClickBackButton);
        m_HomeButton.onClick.AddListener(clickQuitGameJamButton);
        m_PreButton.onClick.AddListener(ClickResultScoreNextOrPreButton);
        m_NextButton.onClick.AddListener(ClickResultScoreNextOrPreButton);
        m_GenreFilter.onClick.AddListener(ClickFilter);
        m_AllButton.Select();
    }

    // �ڷ� ���� ��ư�� ������ �� ���� ó���̸� ���� ��� â���� ���� �ؾ��Ѵ�.
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
        // �����̾����� ���� ���� ��ư ������ �� 
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

    // ���� ���� ���� ����� �ƴ� ���� ������ �� �г��� ���������� ��ư�� �������� �ϱ����� �Լ�
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

    // ������� �������� ������ ������ ���� �г��� ������.
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

    // ������ ���� ���� �л��� MVP�̴�.
    public void ChangeMVPImageGameListPanel(double _gmStudent, double _artStudent, double _programmingStudent)
    {
        double[] arr = { _gmStudent, _artStudent, _programmingStudent };
        string[] studentType = { "��ȹ", "��Ʈ", "�ù�" };

        double max = _gmStudent;
        string part = "��ȹ";

        for (int i = 0; i < arr.Length; i++)
        {
            if (max < arr[i])
            {
                max = arr[i];
                part = studentType[i];
            }
        }

        if (part == "��ȹ")
        {
            m_GameDesignerMvp.gameObject.SetActive(true);
            m_ArtMvp.gameObject.SetActive(false);
            m_ProgrammingMvp.gameObject.SetActive(false);
        }
        else if (part == "��Ʈ")
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
            m_GameDesignerSliderBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);  // �� �����뿡 �ʿ��� �ּ� ���� ��ġ�� ��Ÿ���� ��
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
