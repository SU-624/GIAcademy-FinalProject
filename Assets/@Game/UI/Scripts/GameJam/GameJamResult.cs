using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// 게임잼의 결과를 보여주는 결과창에 관련된 스크립트
/// 
/// 23.03.19 Ocean
/// </summary>
public class GameJamResult : MonoBehaviour
{
    [SerializeField] private GameObject m_GameJamRankPanel;
    [SerializeField] private Image m_ConceptSprite;
    [SerializeField] private GameObject m_GameJamStudentStatPanel;
    [SerializeField] private TextMeshProUGUI m_ChangeName;

    [SerializeField] private TextMeshProUGUI m_CurrentMoney;
    [SerializeField] private TextMeshProUGUI m_CurrentSpecialPoints;

    [SerializeField] private TextMeshProUGUI m_AcademyName;
    [SerializeField] private TextMeshProUGUI m_Placeholder;
    [SerializeField] private TextMeshProUGUI m_Concept;
    [SerializeField] private TextMeshProUGUI m_Funny;
    [SerializeField] private TextMeshProUGUI m_Graphic;
    [SerializeField] private TextMeshProUGUI m_Perfection;
    [SerializeField] private TextMeshProUGUI m_Rank;
    [SerializeField] private TextMeshProUGUI m_Score;
    [SerializeField] private TextMeshProUGUI m_Genre;

    [Space(5f)]
    [Header("기획 학생의 기본 정보들")]
    [SerializeField] private Image m_GameDesignerResultStudentImage;
    [SerializeField] private TextMeshProUGUI m_GameDesignerResultStudentName;
    [SerializeField] private TextMeshProUGUI m_GameDesignerResultStudentHealth;
    [SerializeField] private TextMeshProUGUI m_GameDesignerResultStudentPassion;
    [SerializeField] private Image m_GameDesignerRequirementStat1;
    [SerializeField] private Image m_GameDesignerRequirementStat2;

    [Space(5f)]
    [Header("아트 학생의 기본 정보들")]
    [SerializeField] private Image m_ArtResultStudentImage;
    [SerializeField] private TextMeshProUGUI m_ArtResultStudentName;
    [SerializeField] private TextMeshProUGUI m_ArtResultStudentHealth;
    [SerializeField] private TextMeshProUGUI m_ArtResultStudentPassion;
    [SerializeField] private Image m_ArtRequirementStat1;
    [SerializeField] private Image m_ArtRequirementStat2;

    [Space(5f)]
    [Header("플밍 학생의 기본 정보들")]
    [SerializeField] private Image m_ProgrammingResultStudentImage;
    [SerializeField] private TextMeshProUGUI m_ProgrammingResultStudentName;
    [SerializeField] private TextMeshProUGUI m_ProgrammingResultStudentHealth;
    [SerializeField] private TextMeshProUGUI m_ProgrammingResultStudentPassion;
    [SerializeField] private Image m_ProgrammingRequirementStat1;
    [SerializeField] private Image m_ProgrammingRequirementStat2;
    [Space(5f)]
    [Space(5f)]

    [Space(5f)]
    [Header("기획 증가 감소 이미지와 텍스트들")]
    [SerializeField] private Image m_GameDesignerPlusMinusImageHealth;
    [SerializeField] private Image m_GameDesignerPlusMinusImagePassion;
    [SerializeField] private Image m_GameDesignerPlusMinusImageInsight;
    [SerializeField] private Image m_GameDesignerPlusMinusImageConcentraction;
    [SerializeField] private Image m_GameDesignerPlusMinusImageSense;
    [SerializeField] private Image m_GameDesignerPlusMinusImageTechnique;
    [SerializeField] private Image m_GameDesignerPlusMinusImageWit;
    [SerializeField] private TextMeshProUGUI m_GameDesignerPlusMinusTextHealth;
    [SerializeField] private TextMeshProUGUI m_GameDesignerPlusMinusTextPassion;
    [SerializeField] private TextMeshProUGUI m_GameDesignerPlusMinusTextInsight;
    [SerializeField] private TextMeshProUGUI m_GameDesignerPlusMinusTextConcentraction;
    [SerializeField] private TextMeshProUGUI m_GameDesignerPlusMinusTextSense;
    [SerializeField] private TextMeshProUGUI m_GameDesignerPlusMinusTextTechnique;
    [SerializeField] private TextMeshProUGUI m_GameDesignerPlusMinusTextWit;
    [Space(5f)]

    [Space(5f)]
    [Header("아트 증가 감소 이미지와 텍스트들")]
    [SerializeField] private Image m_ArtPlusMinusImageHealth;
    [SerializeField] private Image m_ArtPlusMinusImagePassion;
    [SerializeField] private Image m_ArtPlusMinusImageInsight;
    [SerializeField] private Image m_ArtPlusMinusImageConcentraction;
    [SerializeField] private Image m_ArtPlusMinusImageSense;
    [SerializeField] private Image m_ArtPlusMinusImageTechnique;
    [SerializeField] private Image m_ArtPlusMinusImageWit;
    [SerializeField] private TextMeshProUGUI m_ArtPlusMinusTextHealth;
    [SerializeField] private TextMeshProUGUI m_ArtPlusMinusTextPassion;
    [SerializeField] private TextMeshProUGUI m_ArtPlusMinusTextInsight;
    [SerializeField] private TextMeshProUGUI m_ArtPlusMinusTextConcentraction;
    [SerializeField] private TextMeshProUGUI m_ArtPlusMinusTextSense;
    [SerializeField] private TextMeshProUGUI m_ArtPlusMinusTextTechnique;
    [SerializeField] private TextMeshProUGUI m_ArtPlusMinusTextWit;
    [Space(5f)]

    [Space(5f)]
    [Header("플밍 증가 감소 이미지와 텍스트들")]
    [SerializeField] private Image m_ProgrammingPlusMinusImageHealth;
    [SerializeField] private Image m_ProgrammingPlusMinusImagePassion;
    [SerializeField] private Image m_ProgrammingPlusMinusImageInsight;
    [SerializeField] private Image m_ProgrammingPlusMinusImageConcentraction;
    [SerializeField] private Image m_ProgrammingPlusMinusImageSense;
    [SerializeField] private Image m_ProgrammingPlusMinusImageTechnique;
    [SerializeField] private Image m_ProgrammingPlusMinusImageWit;
    [SerializeField] private TextMeshProUGUI m_ProgrammingPlusMinusTextHealth;
    [SerializeField] private TextMeshProUGUI m_ProgrammingPlusMinusTextPassion;
    [SerializeField] private TextMeshProUGUI m_ProgrammingPlusMinusTextInsight;
    [SerializeField] private TextMeshProUGUI m_ProgrammingPlusMinusTextConcentraction;
    [SerializeField] private TextMeshProUGUI m_ProgrammingPlusMinusTextSense;
    [SerializeField] private TextMeshProUGUI m_ProgrammingPlusMinusTextTechnique;
    [SerializeField] private TextMeshProUGUI m_ProgrammingPlusMinusTextWit;
    [Space(5f)]
    [Space(5f)]

    [SerializeField] private Image m_GenreImage;
    [SerializeField] private Animator m_GenreImageAnim;
    [SerializeField] private Image m_GameSprite;
    [SerializeField] private Image m_GameDesignerResultSliderFillImage;
    [SerializeField] private Image m_ArtResultSliderFillImage;
    [SerializeField] private Image m_ProgrammingResultSliderFillImage;

    [Space(5f)]
    [Header("게임잼에 필요한 최소 스탯 수치를 나타내는 바")]
    [SerializeField] private Image m_GameDesignerResultBar;
    [SerializeField] private Image m_ArtResultBar;
    [SerializeField] private Image m_ProgrammingResultBar;
    [Space(5f)]
    [Space(5f)]

    [Space(5f)]
    [Header("기획 학생의 스탯들")]
    [SerializeField] private Slider m_GameDesignerResultInsightSlider;
    [SerializeField] private Slider m_GameDesignerResultConcentractionSlider;
    [SerializeField] private Slider m_GameDesignerResultSenseSlider;
    [SerializeField] private Slider m_GameDesignerResultWitSlider;
    [SerializeField] private Slider m_GameDesignerResultTechniqueSlider;
    [Space(5f)]
    [Header("기획 보상 스탯")]
    [SerializeField] private Slider m_GameDesignerRewardInsightSlider;
    [SerializeField] private Slider m_GameDesignerRewardConcentractionSlider;
    [SerializeField] private Slider m_GameDesignerRewardSenseSlider;
    [SerializeField] private Slider m_GameDesignerRewardWitSlider;
    [SerializeField] private Slider m_GameDesignerRewardTechniqueSlider;

    [Space(5f)]
    [Header("아트 학생의 스탯")]
    [SerializeField] private Slider m_ArtResultInsightSlider;
    [SerializeField] private Slider m_ArtResultConcentractionSlider;
    [SerializeField] private Slider m_ArtResultSenseSlider;
    [SerializeField] private Slider m_ArtResultWitSlider;
    [SerializeField] private Slider m_ArtResultTechniqueSlider;
    [Space(5f)]
    [Header("아트 보상 스탯들")]
    [SerializeField] private Slider m_ArtRewardInsightSlider;
    [SerializeField] private Slider m_ArtRewardConcentractionSlider;
    [SerializeField] private Slider m_ArtRewardSenseSlider;
    [SerializeField] private Slider m_ArtRewardWitSlider;
    [SerializeField] private Slider m_ArtRewardTechniqueSlider;

    [Space(5f)]
    [Header("플밍 학생의 스탯들")]
    [SerializeField] private Slider m_ProgrammingResultInsightSlider;
    [SerializeField] private Slider m_ProgrammingResultConcentractionSlider;
    [SerializeField] private Slider m_ProgrammingResultSenseSlider;
    [SerializeField] private Slider m_ProgrammingResultWitSlider;
    [SerializeField] private Slider m_ProgrammingResultTechniqueSlider;
    [Space(5f)]
    [Header("플밍 보상 스탯")]
    [SerializeField] private Slider m_ProgrammingRewardInsightSlider;
    [SerializeField] private Slider m_ProgrammingRewardConcentractionSlider;
    [SerializeField] private Slider m_ProgrammingRewardSenseSlider;
    [SerializeField] private Slider m_ProgrammingRewardWitSlider;
    [SerializeField] private Slider m_ProgrammingRewardTechniqueSlider;

    [Space(5f)]
    [Header("기획 학생의 스탯")]
    [SerializeField] private Slider m_GameDesignerRequirementSlider1;
    [SerializeField] private Image m_GameDesignerRequirementBar;

    [Space(5f)]
    [Header("아트 학생의 스탯")]
    [SerializeField] private Slider m_ArtRequirementSlider1;
    [SerializeField] private Image m_ArtRequirementBar;

    [Space(5f)]
    [Header("플밍 학생의 스탯")]
    [SerializeField] private Slider m_ProgrammingRequirementSlider1;
    [SerializeField] private Image m_ProgrammingRequirementBar;

    [Space(5f)]
    [Header("학생MVP 이미지")]
    [SerializeField] private Image m_GameDesignerMvp;
    [SerializeField] private Image m_ArtMvp;
    [SerializeField] private Image m_ProgrammingMvp;

    [Space(5f)]
    [SerializeField] private TMP_InputField m_GameName;
    [SerializeField] private Sprite m_UpArrow;
    [SerializeField] private Sprite m_DownArrow;

    public string _changeGameName { get { return m_GameName.text; } set { m_GameName.text = value; } }
    public Image GameDesignerRequirementStat1 { get { return m_GameDesignerRequirementStat1; } set { m_GameDesignerRequirementStat1 = value; } }
    public Image GameDesignerRequirementStat2 { get { return m_GameDesignerRequirementStat2; } set { m_GameDesignerRequirementStat2 = value; } }
    public Image ArtRequirementStat1 { get { return m_ArtRequirementStat1; } set { m_ArtRequirementStat1 = value; } }
    public Image ArtRequirementStat2 { get { return m_ArtRequirementStat2; } set { m_ArtRequirementStat2 = value; } }
    public Image ProgrammingRequirementStat1 { get { return m_ProgrammingRequirementStat1; } set { m_ProgrammingRequirementStat1 = value; } }
    public Image ProgrammingRequirementStat2 { get { return m_ProgrammingRequirementStat2; } set { m_ProgrammingRequirementStat2 = value; } }


    //public TMP_InputField _gameName
    public PopOffUI _turnOffResultPanel;
    public PopUpUI _turnOnResultPanel;

    // 버튼을 클릭했을 때만 입력할 수 있게 해주기
    public void SetChangeGameName()
    {
        m_GameName.interactable = true;
        m_GameName.ActivateInputField();
        m_Placeholder.text = "이름을 입력하세요";
        m_GameName.text = "";
        m_Placeholder.color = new Color(0, 0, 0, 0.5f);
    }

    public void TurnOffPanel()
    {
        _turnOffResultPanel.TurnOffUI();
    }

    public void TurnOnPanel()
    {
        _turnOnResultPanel.TurnOnUI();
    }

    public void SetResultPanelMoneyAndSpecialPoint()
    {
        m_CurrentMoney.text = string.Format("{0:#,0}", PlayerInfo.Instance.MyMoney);
        m_CurrentSpecialPoints.text = string.Format("{0:#,0}", PlayerInfo.Instance.SpecialPoint);
    }

    // 입력이 끝나면 다시 버튼을 클릭하기 전까지는 입력하지 못하게 하기
    public void EndEditName()
    {
        m_GameName.interactable = false;
    }

    public void SetInfo(string _academyName, string _placeholder, string _concept, string _funny, string _graphic, string _perfection, string _score, string _rank, string _genre)
    {
        m_AcademyName.text = _academyName;
        m_GameName.text = _placeholder;
        m_Concept.text = _concept;
        m_Funny.text = _funny;
        m_Graphic.text = _graphic;
        m_Perfection.text = _perfection;
        m_Score.text = _score;
        m_Rank.text = _rank;
        m_Genre.text = _genre;
    }

    public void ChangeGenreSprite(Sprite _main)
    {
        m_GenreImage.sprite = _main;
    }

    public void ChangeConceptSprite(Sprite _sprite)
    {
        m_ConceptSprite.sprite = _sprite;
    }

    // 옆으로 가기 버튼을 눌렀을 때 나를 기준으로 나는 끄고 다음 패널을 켜준다.
    public void IsPanelActive()
    {
        if (m_GameJamRankPanel.activeSelf)
        {
            m_GameJamRankPanel.SetActive(false);
            m_GameJamStudentStatPanel.SetActive(true);
        }
        else if (m_GameJamStudentStatPanel.activeSelf)
        {
            m_GameJamRankPanel.SetActive(true);
            m_GameJamStudentStatPanel.SetActive(false);
        }
    }

    public void ClickPreButton()
    {
        if (m_GameJamStudentStatPanel.activeSelf)
        {
            m_GameJamRankPanel.SetActive(true);
            m_GameJamStudentStatPanel.SetActive(false);
        }
    }

    // 결과 확인 후 패널의 순서를 돌려놓기 위한 함수
    public void InitPanelActive()
    {
        m_GameJamRankPanel.SetActive(true);
        m_GameJamStudentStatPanel.SetActive(false);
    }

    // 여기 부분 코드 고치기,,
    // 내가 선택했던 학생들의 스탯을 슬라이더로 보여주기 위한 함수 
    public void FillRequirementSlider(StudentType _type, int _value, int _stat1, int _stat2)
    {
        if (_type == StudentType.GameDesigner)
        {
            m_GameDesignerRequirementSlider1.value = _value;

            if (_stat2 == 0)
            {
                float _posX = (_stat1 / 150f) * 721f;
                m_GameDesignerResultBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);  // 이 게임잼에 필요한 최소 스탯 수치를 나타내는 바
            }
            else
            {
                int _temp = _stat1 + _stat2;
                float _posX = (_temp / 150f) * 721f;
                m_GameDesignerResultBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);
            }
        }
        else if (_type == StudentType.Art)
        {
            m_ArtRequirementSlider1.value = _value;

            if (_stat2 == 0)
            {
                float _posX = (_stat1 / 150f) * 721f;
                m_GameDesignerResultBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);  // 이 게임잼에 필요한 최소 스탯 수치를 나타내는 바
            }
            else
            {
                int _temp = _stat1 + _stat2;
                float _posX = (_temp / 150f) * 721f;
                m_GameDesignerResultBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);
            }
        }
        else if (_type == StudentType.Programming)
        {
            m_ProgrammingRequirementSlider1.value = _value;

            if (_stat2 == 0)
            {
                float _posX = (_stat1 / 150f) * 721f;
                m_GameDesignerResultBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);  // 이 게임잼에 필요한 최소 스탯 수치를 나타내는 바
            }
            else
            {
                int _temp = _stat1 + _stat2;
                float _posX = (_temp / 150f) * 721f;
                m_GameDesignerResultBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);
            }
        }
    }

    // 내가 선택했던 학생들의 스탯이 최소치를넘었으면 슬라이더 색상을 파란색으로 아니라면 빨간색으로 바꿔주기.
    public void ChangeColorFillImage(StudentType _type, int _sliderValue, int _needValue, Sprite _satisfy, Sprite _disSatisfy)
    {
        if (_type == StudentType.GameDesigner)
        {
            if (_sliderValue > _needValue)
            {
                m_GameDesignerResultSliderFillImage.sprite = _satisfy;        // 파란색
            }
            else
            {
                m_GameDesignerResultSliderFillImage.sprite = _disSatisfy;        // 빨간색
            }
        }
        else if (_type == StudentType.Art)
        {
            if (_sliderValue > _needValue)
            {
                m_ArtResultSliderFillImage.sprite = _satisfy;        // 파란색
            }
            else
            {
                m_ArtResultSliderFillImage.sprite = _disSatisfy;       // 빨간색
            }

        }
        else if (_type == StudentType.Programming)
        {
            if (_sliderValue > _needValue)
            {
                m_ProgrammingResultSliderFillImage.sprite = _satisfy;        // 파란색
            }
            else
            {
                m_ProgrammingResultSliderFillImage.sprite = _disSatisfy;        // 빨간색
            }
        }
    }

    // 내가 선택했던 학생들의 정보를 띄워준다.
    public void ChangeResultStudentInfo(StudentType _type, string _name)
    {
        List<Student> _list = ObjectManager.Instance.m_StudentList;

        switch (_type)
        {
            case StudentType.GameDesigner:
            {
                for (int i = 0; i < _list.Count; i++)
                {
                    if (_name == _list[i].m_StudentStat.m_StudentName)
                    {
                        m_GameDesignerResultInsightSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Insight];
                        m_GameDesignerResultConcentractionSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Concentration];
                        m_GameDesignerResultSenseSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Sense];
                        m_GameDesignerResultWitSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Wit];
                        m_GameDesignerResultTechniqueSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Technique];
                        if (_list[i].m_StudentStat.m_UserSettingName != "")
                        {
                            m_GameDesignerResultStudentName.text = _list[i].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            m_GameDesignerResultStudentName.text = _name;
                        }
                        m_GameDesignerResultStudentImage.sprite = _list[i].StudentProfileImg;
                    }
                }
            }
            break;
            case StudentType.Art:
            {
                for (int i = 0; i < _list.Count; i++)
                {
                    if (_name == _list[i].m_StudentStat.m_StudentName)
                    {
                        m_ArtResultInsightSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Insight];
                        m_ArtResultConcentractionSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Concentration];
                        m_ArtResultSenseSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Sense];
                        m_ArtResultWitSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Wit];
                        m_ArtResultTechniqueSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Technique];
                        if (_list[i].m_StudentStat.m_UserSettingName != "")
                        {
                            m_ArtResultStudentName.text = _list[i].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            m_ArtResultStudentName.text = _name;
                        }
                        m_ArtResultStudentImage.sprite = _list[i].StudentProfileImg;
                    }
                }
            }
            break;
            case StudentType.Programming:
            {
                for (int i = 0; i < _list.Count; i++)
                {
                    if (_name == _list[i].m_StudentStat.m_StudentName)
                    {
                        m_ProgrammingResultInsightSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Insight];
                        m_ProgrammingResultConcentractionSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Concentration];
                        m_ProgrammingResultSenseSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Sense];
                        m_ProgrammingResultWitSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Wit];
                        m_ProgrammingResultTechniqueSlider.value = _list[i].m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Technique];
                        if (_list[i].m_StudentStat.m_UserSettingName != "")
                        {
                            m_ProgrammingResultStudentName.text = _list[i].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            m_ProgrammingResultStudentName.text = _name;
                        }
                        m_ProgrammingResultStudentImage.sprite = _list[i].StudentProfileImg;
                    }
                }
            }

            break;
        }
    }

    // 학생의 체력과 열정이 증가했는지 감소했는지, 보상으로 학생의 스탯을 증가시켜주는 함수
    public void SetPlusMinus(StudentType _type, string _rank, string _studentName, int[] _arr, GameJamSaveData _data)
    {
        int[] _arryReward = new int[5];

        switch (_rank)
        {
            case "AAA":
            {
                ChangeHealthAndPassion(_type, _studentName, _data);

                for (int i = 0; i < _arr.Length; i++)
                {
                    if (_arr[i] == 0)
                    {
                        _arryReward[i] = 0;
                    }
                    else
                    {
                        _arryReward[i] = 15;
                    }

                }

                SetStudentStatRewardSliderValue(_type, _arryReward);
                SetStatImageText(_type, _arryReward);
            }
            break;

            case "AA":
            {
                ChangeHealthAndPassion(_type, _studentName, _data);

                for (int i = 0; i < _arr.Length; i++)
                {
                    if (_arr[i] == 0)
                    {
                        _arryReward[i] = 0;
                    }
                    else
                    {
                        _arryReward[i] = 10;
                    }

                }
                SetStudentStatRewardSliderValue(_type, _arryReward);
                SetStatImageText(_type, _arryReward);
            }
            break;

            case "A":
            {
                ChangeHealthAndPassion(_type, _studentName, _data);

                for (int i = 0; i < _arr.Length; i++)
                {
                    if (_arr[i] == 0)
                    {
                        _arryReward[i] = 0;
                    }
                    else
                    {
                        _arryReward[i] = 5;
                    }

                }

                SetStudentStatRewardSliderValue(_type, _arryReward);
                SetStatImageText(_type, _arryReward);
            }
            break;

            case "B":
            {
                ChangeHealthAndPassion(_type, _studentName, _data);

                for (int i = 0; i < _arr.Length; i++)
                {
                    if (_arr[i] == 0)
                    {
                        _arryReward[i] = 0;
                    }
                    else
                    {
                        _arryReward[i] = 3;
                    }

                }

                SetStudentStatRewardSliderValue(_type, _arryReward);
                SetStatImageText(_type, _arryReward);
            }
            break;

            default:
            {
                //m_ProgrammingResultStudentHealth.text = _list.m_StudentStat.m_Health.ToString();
                //m_ProgrammingResultStudentPassion.text = _list.m_StudentStat.m_Passion.ToString();
                SetStudentStatRewardSliderValue(_type, _arryReward);
                SetStatImageText(_type, _arryReward);
            }
            break;
        }

    }

    public void SetRequirementStatImage()
    {

    }

    // 체력과 열정의 스프라이트를 바꿔주는 함수
    private void ChangeHealthOrPassionImageAndText(int _value, int _listvalue, TextMeshProUGUI _text, TextMeshProUGUI _resultText)
    {
        //if (_value < 0)     // 감소가 파란색
        //{
        //    _text.text = _value.ToString();
        //    //_imgae.color = new Color(0, 0, 255, 255);

        //    int _temp = _listvalue + _value;

        //    _resultText.text = _temp.ToString();
        //}
        //else
        //{
        //}
        _text.text = _value.ToString();
        //_imgae.color = new Color(255, 0, 0, 255);

        int _temp = _listvalue - _value;

        _resultText.text = _temp.ToString();

    }

    // 학생 타입에 따라 바뀐 체력과 열정을 띄워준다.
    private void ChangeHealthAndPassion(StudentType _type, string _studentName, GameJamSaveData _gamejamData)
    {
        GameJamInfo _data = GameJam.SearchAllGameJamInfo((int)_gamejamData.m_GameJamID);
        Student _student = ObjectManager.Instance.SearchStudentInfo(_studentName);

        if (_type == StudentType.GameDesigner)
        {
            int _health = _data.m_StudentHealth;
            int _passion = _data.m_StudentPassion;

            //SetActiveStatImage(m_GameDesignerPlusMinusImageHealth, _health);
            //SetActiveStatImage(m_GameDesignerPlusMinusImagePassion, _passion);


            ChangeHealthOrPassionImageAndText(_health, _student.m_StudentStat.m_Health, m_GameDesignerPlusMinusTextHealth, m_GameDesignerResultStudentHealth);
            ChangeHealthOrPassionImageAndText(_passion, _student.m_StudentStat.m_Passion, m_GameDesignerPlusMinusTextPassion, m_GameDesignerResultStudentPassion);

        }
        else if (_type == StudentType.Art)
        {
            int _health = _data.m_StudentHealth;
            int _passion = _data.m_StudentPassion;

            //SetActiveStatImage(m_ArtPlusMinusImageHealth, _health);
            //SetActiveStatImage(m_ArtPlusMinusImagePassion, _passion);

            ChangeHealthOrPassionImageAndText(_health, _student.m_StudentStat.m_Health, m_ArtPlusMinusTextHealth, m_ArtResultStudentHealth);
            ChangeHealthOrPassionImageAndText(_passion, _student.m_StudentStat.m_Passion, m_ArtPlusMinusTextPassion, m_ArtResultStudentPassion);

        }
        else if (_type == StudentType.Programming)
        {
            int _health = _data.m_StudentHealth;
            int _passion = _data.m_StudentPassion;

            //SetActiveStatImage(m_ProgrammingPlusMinusImageHealth, _health);
            //SetActiveStatImage(m_ProgrammingPlusMinusImagePassion, _passion);

            ChangeHealthOrPassionImageAndText(_health, _student.m_StudentStat.m_Health, m_ProgrammingPlusMinusTextHealth, m_ProgrammingResultStudentHealth);
            ChangeHealthOrPassionImageAndText(_passion, _student.m_StudentStat.m_Passion, m_ProgrammingPlusMinusTextPassion, m_ProgrammingResultStudentPassion);
        }
    }

    private void SetStudentStatRewardSliderValue(StudentType _type, int[] _stat)
    {
        if (_type == StudentType.GameDesigner)
        {
            m_GameDesignerRewardInsightSlider.value = _stat[(int)AbilityType.Insight] + m_GameDesignerResultInsightSlider.value;
            m_GameDesignerRewardConcentractionSlider.value = _stat[(int)AbilityType.Concentration] + m_GameDesignerResultConcentractionSlider.value;
            m_GameDesignerRewardSenseSlider.value = _stat[(int)AbilityType.Sense] + m_GameDesignerResultSenseSlider.value;
            m_GameDesignerRewardTechniqueSlider.value = _stat[(int)AbilityType.Technique] + m_GameDesignerResultTechniqueSlider.value;
            m_GameDesignerRewardWitSlider.value = _stat[(int)AbilityType.Wit] + m_GameDesignerResultWitSlider.value;

        }
        else if (_type == StudentType.Art)
        {
            m_ArtRewardInsightSlider.value = _stat[(int)AbilityType.Insight] + m_ArtResultInsightSlider.value;
            m_ArtRewardConcentractionSlider.value = _stat[(int)AbilityType.Concentration] + m_ArtResultConcentractionSlider.value;
            m_ArtRewardSenseSlider.value = _stat[(int)AbilityType.Sense] + m_ArtResultSenseSlider.value;
            m_ArtRewardTechniqueSlider.value = _stat[(int)AbilityType.Technique] + m_ArtResultTechniqueSlider.value;
            m_ArtRewardWitSlider.value = _stat[(int)AbilityType.Wit] + m_ArtResultWitSlider.value;
        }
        else
        {
            m_ProgrammingRewardInsightSlider.value = _stat[(int)AbilityType.Insight] + m_ProgrammingResultInsightSlider.value;
            m_ProgrammingRewardConcentractionSlider.value = _stat[(int)AbilityType.Concentration] + m_ProgrammingResultConcentractionSlider.value;
            m_ProgrammingRewardSenseSlider.value = _stat[(int)AbilityType.Sense] + m_ProgrammingResultSenseSlider.value;
            m_ProgrammingRewardTechniqueSlider.value = _stat[(int)AbilityType.Technique] + m_ProgrammingResultTechniqueSlider.value;
            m_ProgrammingRewardWitSlider.value = _stat[(int)AbilityType.Wit] + m_ProgrammingResultWitSlider.value;
        }
    }

    // 스탯들이 증가한건지 감소한건지 이미지의 색상과 얼만큼 바뀌었는지 텍스트를 바꿔주는 함수
    public void SetStatImageText(StudentType _type, int[] _stat)
    {
        if (_type == StudentType.GameDesigner)
        {
            //m_GameDesignerPlusMinusImageInsight.color = (_stat[(int)AbilityType.Insight] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);
            //m_GameDesignerPlusMinusImageConcentraction.color = (_stat[(int)AbilityType.Concentration] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);
            //m_GameDesignerPlusMinusImageSense.color = (_stat[(int)AbilityType.Sense] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);
            //m_GameDesignerPlusMinusImageTechnique.color = (_stat[(int)AbilityType.Technique] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);
            //m_GameDesignerPlusMinusImageWit.color = (_stat[(int)AbilityType.Wit] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);

            SetActiveStatImage(m_GameDesignerPlusMinusImageInsight, _stat[(int)AbilityType.Insight]);
            SetRewardStatText(m_GameDesignerPlusMinusTextInsight, _stat[(int)AbilityType.Insight]);

            SetActiveStatImage(m_GameDesignerPlusMinusImageConcentraction, _stat[(int)AbilityType.Concentration]);
            SetRewardStatText(m_GameDesignerPlusMinusTextConcentraction, _stat[(int)AbilityType.Concentration]);

            SetActiveStatImage(m_GameDesignerPlusMinusImageSense, _stat[(int)AbilityType.Sense]);
            SetRewardStatText(m_GameDesignerPlusMinusTextSense, _stat[(int)AbilityType.Sense]);

            SetActiveStatImage(m_GameDesignerPlusMinusImageTechnique, _stat[(int)AbilityType.Technique]);
            SetRewardStatText(m_GameDesignerPlusMinusTextTechnique, _stat[(int)AbilityType.Technique]);

            SetActiveStatImage(m_GameDesignerPlusMinusImageWit, _stat[(int)AbilityType.Wit]);
            SetRewardStatText(m_GameDesignerPlusMinusTextWit, _stat[(int)AbilityType.Wit]);
        }
        else if (_type == StudentType.Art)
        {
            //m_ArtPlusMinusImageInsight.color = (_stat[(int)AbilityType.Insight] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);
            //m_ArtPlusMinusImageConcentraction.color = (_stat[(int)AbilityType.Concentration] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);
            //m_ArtPlusMinusImageSense.color = (_stat[(int)AbilityType.Sense] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);
            //m_ArtPlusMinusImageTechnique.color = (_stat[(int)AbilityType.Technique] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);
            //m_ArtPlusMinusImageWit.color = (_stat[(int)AbilityType.Wit] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);

            SetActiveStatImage(m_ArtPlusMinusImageInsight, _stat[(int)AbilityType.Insight]);
            SetRewardStatText(m_ArtPlusMinusTextInsight, _stat[(int)AbilityType.Insight]);

            SetActiveStatImage(m_ArtPlusMinusImageConcentraction, _stat[(int)AbilityType.Concentration]);
            SetRewardStatText(m_ArtPlusMinusTextConcentraction, _stat[(int)AbilityType.Concentration]);

            SetActiveStatImage(m_ArtPlusMinusImageSense, _stat[(int)AbilityType.Sense]);
            SetRewardStatText(m_ArtPlusMinusTextSense, _stat[(int)AbilityType.Sense]);

            SetActiveStatImage(m_ArtPlusMinusImageTechnique, _stat[(int)AbilityType.Technique]);
            SetRewardStatText(m_ArtPlusMinusTextTechnique, _stat[(int)AbilityType.Technique]);

            SetActiveStatImage(m_ArtPlusMinusImageWit, _stat[(int)AbilityType.Wit]);
            SetRewardStatText(m_ArtPlusMinusTextWit, _stat[(int)AbilityType.Wit]);
        }
        else if (_type == StudentType.Programming)
        {
            // m_ProgrammingPlusMinusImageInsight.color = (_stat[(int)AbilityType.Insight] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);
            // m_ProgrammingPlusMinusImageConcentraction.color = (_stat[(int)AbilityType.Concentration] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);
            // m_ProgrammingPlusMinusImageSense.color = (_stat[(int)AbilityType.Sense] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);
            // m_ProgrammingPlusMinusImageTechnique.color = (_stat[(int)AbilityType.Technique] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);
            // m_ProgrammingPlusMinusImageWit.color = (_stat[(int)AbilityType.Wit] > 0) ? new Color(255, 0, 0, 255) : new Color(0, 0, 255, 255);

            SetActiveStatImage(m_ProgrammingPlusMinusImageInsight, _stat[(int)AbilityType.Insight]);
            SetRewardStatText(m_ProgrammingPlusMinusTextInsight, _stat[(int)AbilityType.Insight]);

            SetActiveStatImage(m_ProgrammingPlusMinusImageConcentraction, _stat[(int)AbilityType.Concentration]);
            SetRewardStatText(m_ProgrammingPlusMinusTextConcentraction, _stat[(int)AbilityType.Concentration]);

            SetActiveStatImage(m_ProgrammingPlusMinusImageSense, _stat[(int)AbilityType.Sense]);
            SetRewardStatText(m_ProgrammingPlusMinusTextSense, _stat[(int)AbilityType.Sense]);

            SetActiveStatImage(m_ProgrammingPlusMinusImageTechnique, _stat[(int)AbilityType.Technique]);
            SetRewardStatText(m_ProgrammingPlusMinusTextTechnique, _stat[(int)AbilityType.Technique]);

            SetActiveStatImage(m_ProgrammingPlusMinusImageWit, _stat[(int)AbilityType.Wit]);
            SetRewardStatText(m_ProgrammingPlusMinusTextWit, _stat[(int)AbilityType.Wit]);
        }
    }

    // 파트별로 증가하거나 감소하는 값이 있다면 이미지를 켜주고 아니라면 꺼준다.
    private void SetActiveStatImage(Image image, int _value)
    {
        if (_value > 0 || _value < 0)
        {
            image.gameObject.SetActive(true);
            image.sprite = _value > 0 ? m_UpArrow : m_DownArrow;
        }
        else if (_value == 0)
        {
            image.gameObject.SetActive(false);
        }
    }

    private void SetRewardStatText(TextMeshProUGUI _text, int _value)
    {
        string _tempString = _value.ToString();

        if (_tempString.Contains("-"))
        {
            string _str = _tempString.Substring(1);
            _text.text = _str;
        }
        _text.text = _tempString;
    }

    // 스탯이 제일 높은 학생이 MVP이다.
    public void ChangeMVPImageResultPanel(double _gmStudent, double _artStudent, double _programmingStudent)
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

}
