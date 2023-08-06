using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InJaeRecommendResult : MonoBehaviour
{
    [Header("결과창 패널의 속성들")]
    [SerializeField] private GameObject[] m_GameDesignerStudentResult;
    [SerializeField] private GameObject[] m_ArtStudentResult;
    [SerializeField] private GameObject[] m_ProgrammingStudentResult;
    [SerializeField] private Transform m_GameDesignerStudentResultParent;
    [SerializeField] private Transform m_ArtStudentResultParent;
    [SerializeField] private Transform m_ProgrammingStudentResultParent;
    [SerializeField] private GameObject m_DetailResultPanel;
    [SerializeField] private PopUpUI m_PopUpRecommendResultPanel;
    [SerializeField] private PopOffUI m_PopOffRecommendResultPanel;
    [SerializeField] private Button m_OKButton;

    [Space(5f)]
    [Header("디테일 패널의 속성들")]
    [SerializeField] private Button m_DetailPanelCloseButton;
    [SerializeField] private Image m_DetailPanelPassFailImage;
    [SerializeField] private Image m_DetailStudentImage;
    [SerializeField] private TextMeshProUGUI m_DetailStudentName;
    [SerializeField] private TextMeshProUGUI m_DetailCompanyName;

    public Button DetailPanelCloseButton { get { return m_DetailPanelCloseButton; } set { m_DetailPanelCloseButton = value; } }
    public Button OKButton { get { return m_OKButton; } set { m_OKButton = value; } }

    private void Start()
    {
        m_OKButton.onClick.AddListener(PopOffResultPanel);
        m_OKButton.onClick.AddListener(PopOffDetailPanel);
        m_DetailPanelCloseButton.onClick.AddListener(PopOffDetailPanel);
    }

    public void PopUpResultPanel()
    {
        m_PopUpRecommendResultPanel.TurnOnUI();
    }

    public void PopOffResultPanel()
    {
        m_PopOffRecommendResultPanel.TurnOffUI();
    }

    public void PopUpDetailPanel()
    {
        m_DetailResultPanel.SetActive(true);
    }

    public void PopOffDetailPanel()
    {
        m_DetailResultPanel.SetActive(false);
    }

    public void ResultPanelStudentNameChange(string[] _gmStudent, string[] _artStudent, string[] _programmingStudent)
    {
        for (int i = 0; i < 6; i++)
        {
            m_GameDesignerStudentResult[i].GetComponent<RecommendResultStudentPrefab>().m_StudentName.text = _gmStudent[i];
            m_ArtStudentResult[i].GetComponent<RecommendResultStudentPrefab>().m_StudentName.text = _artStudent[i];
            m_ProgrammingStudentResult[i].GetComponent<RecommendResultStudentPrefab>().m_StudentName.text = _programmingStudent[i];
        }
    }

    public void ResultPanelStudentImageChange(Sprite[] _gmStudent, Sprite[] _artStudent, Sprite[] _programmingStudent)
    {
        for (int i = 0; i < 6; i++)
        {
            m_GameDesignerStudentResult[i].GetComponent<RecommendResultStudentPrefab>().m_StudnetImage.sprite = _gmStudent[i];
            m_ArtStudentResult[i].GetComponent<RecommendResultStudentPrefab>().m_StudnetImage.sprite = _artStudent[i];
            m_ProgrammingStudentResult[i].GetComponent<RecommendResultStudentPrefab>().m_StudnetImage.sprite = _programmingStudent[i];
        }
    }

    public void ResultPanelStudentPassFail(Sprite[] _gmPassFail, Sprite[] _artPassFail, Sprite[] _programmingPassFail)
    {
        for (int i = 0; i < 6; i++)
        {
            m_GameDesignerStudentResult[i].GetComponent<RecommendResultStudentPrefab>().m_PassFail.sprite = _gmPassFail[i];
            m_ArtStudentResult[i].GetComponent<RecommendResultStudentPrefab>().m_PassFail.sprite = _artPassFail[i];
            m_ProgrammingStudentResult[i].GetComponent<RecommendResultStudentPrefab>().m_PassFail.sprite = _programmingPassFail[i];
        }
    }

    public void SetDetailPanelPassFailImage(Sprite _passFail)
    {
        m_DetailPanelPassFailImage.sprite = _passFail;
    }

    public void SetDetailPanelStudentImage(Sprite _studentImage)
    {
        m_DetailStudentImage.sprite = _studentImage;
    }

    public void SetDetailStudentName(string _studentName)
    {
        m_DetailStudentName.text = _studentName;
    }

    public void SetDetailCompanyName(string _companyName)
    {
        m_DetailCompanyName.text = _companyName;
    }
}
