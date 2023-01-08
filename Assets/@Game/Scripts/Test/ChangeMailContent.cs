using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

// 메일의 필터 적용을 위한 타입
public enum MailType
{
    Month,
    News,
    Satisfaction
}

// 메일들의 정보를 담은 클래스
public class MailBox
{
    public string m_MailTitle;
    public string m_SendMailDate;
    public string m_FromMail;
    public string m_MailContent;
    public MailType m_Type;
    public bool m_IsNewMail;
}

public class ChangeMailContent : MonoBehaviour
{
    [SerializeField] private Button m_NewMailButton;
    [SerializeField] private GameObject m_Button;
    [SerializeField] private MailSystem m_IsNewMail;
    [SerializeField] private GameObject m_NotNewMail;
    [SerializeField] private GameObject m_NewMailPrefab;
    [SerializeField] private GameObject m_ReadMailContent;
    [SerializeField] private Transform m_MailBox;
    [SerializeField] private Transform m_NotNewMailBox;
    [SerializeField] private GameObject m_TempNotNewMail;
    [SerializeField] private GameObject m_BulbIcon;

    private GameObject m_ClickMailButton;   // 전체 메일함인지 신규 메일함인지 기억해야한다.
    private GameObject m_CheckMailName;     // 내가 읽으려고 하는 메일이 무엇인지 알기위한 변수

    // 메일을 관리하기 위한 리스트들
    private Dictionary<string, MailBox> m_Mail = new Dictionary<string, MailBox>();     // 발송에 씌이는 전체 메일
    private List<MailBox> m_MailList = new List<MailBox>();  // 새로온 메일 내용
    private List<GameObject> m_TempMailTextList = new List<GameObject>(); // 새로 받은 메일들을 쌓아 둘 리스트
    private List<GameObject> m_AllMailTextList = new List<GameObject>(); // 여태것 받은 메일들을 쌓아 둘 리스트

    int _i = 0;
    bool _isNewMAilCheck = false;

    private void Start()
    {
        m_ClickMailButton = m_Button;
        MakeMail("안녕안녕", "2023년 01월 01일", "망극아카데미", "나는 망극이다!", MailType.News);
        MakeMail("살려줘", "2023년 01월 02일", "망극아카데미", " 그만,,", MailType.News);
    }

    // 임시로 메일을 만들어주는 함수
    private void MakeMail(string _title, string _date, string _from, string _content, MailType _type)
    {
        MailBox m_MailComposition = new MailBox();
        m_MailComposition.m_MailTitle = _title;
        m_MailComposition.m_SendMailDate = _date;
        m_MailComposition.m_FromMail = _from;
        m_MailComposition.m_MailContent = _content;
        m_MailComposition.m_Type = _type;
        m_MailComposition.m_IsNewMail = true;
        m_Mail.Add(m_MailComposition.m_MailTitle, m_MailComposition);
    }

    // 리스트의 인덱스를 찾아주는 함수. 오브젝트의 이름으로 찾아서 인덱스를 찾자.
    public int FindListIndex(List<GameObject> list, string val)
    {
        if (list == null)
        {
            return -1;
        }

        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log("현재 리스트 카운트 : " + list.Count);
            if (list[i].name == val)
            {
                return i;
            }
        }

        return -1;
    }

    public void SetButtonColor()
    {
        m_NewMailButton.Select();

        // 처음 메일함을  들어가면 신규메일함이 나와야한다.
        m_ClickMailButton.name = "New_Mail_Button";

        bool _isNewMail = false;

        foreach (var mail in m_MailList)
        {
            if (mail.m_IsNewMail)
            {
                _isNewMail = true;
                _isNewMAilCheck = true;
                break;
            }
        }

        if (!_isNewMail)
        {
            m_TempNotNewMail.SetActive(true);
        }
        else
        {
            m_TempNotNewMail.SetActive(false);
        }
    }

    // 메일의 제목과 보낸사람, 보낸날짜를 바꿔준다.
    public void ChangeDateAndFrom()
    {
        m_MailList.Add(m_Mail.ElementAt(_i).Value);

        //GameObject m_NewMailText = new GameObject();
        GameObject m_AllMailText = new GameObject();

        if (m_MailList[_i].m_IsNewMail)
        {
            m_AllMailText = Instantiate(m_NewMailPrefab, m_MailBox);
            m_AllMailText.name = m_MailList[_i].m_MailTitle;
            m_AllMailText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_MailList[_i].m_MailTitle;
            m_AllMailText.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[_i].m_SendMailDate;
            m_AllMailText.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[_i].m_FromMail;
            m_AllMailText.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ReadMail);

            //m_NewMailText = Instantiate(m_NewMailPrefab, m_MailBox);
            //m_NewMailText.name = m_MailList[_i].m_MailTitle;
            //m_NewMailText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_MailList[_i].m_MailTitle;
            //m_NewMailText.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[_i].m_SendMailDate;
            //m_NewMailText.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[_i].m_FromMail;
            //m_NewMailText.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ReadMail);

            //m_NewMailTextList.Add(m_NewMailText);
            m_AllMailTextList.Add(m_AllMailText);

            _i++;
        }
    }

    private void ReadMail()
    {
        if (m_ClickMailButton.name == "New_Mail_Button")
        {
            Debug.Log("신규메일 읽기");
            PopUpMailContent(m_AllMailTextList);
        }
        else if (m_ClickMailButton.name == "All_Mail_Button ")
        {
            Debug.Log("전체메일 읽기");
            PopUpMailContent(m_AllMailTextList);
        }
    }

    // 메일의 읽기를 누르면 해당 메일의 내용이 나오게 해주고 기존의 메일함에 있는 목록을 삭제시켜준다.
    private void PopUpMailContent(List<GameObject> _list)
    {
        MailBox m_tempContent;

        // 내 부모를 찾아 부모의 이름을 가져와 딕셔너리에서 같은 이름의 메일을 찾아온다.
        m_CheckMailName = EventSystem.current.currentSelectedGameObject;
        string m_Parent = m_CheckMailName.transform.parent.name;
        Debug.Log(m_Parent);

        m_Mail.TryGetValue(m_Parent, out m_tempContent);

        int _index = FindListIndex(_list, m_tempContent.m_MailTitle);

        if (m_MailList.Count > 1)
        {
            while (m_MailList[_index].m_IsNewMail == false)
            {
                _index++;
            }
        }
        m_MailList[_index].m_IsNewMail = false;
        _isNewMAilCheck = false;

        m_ReadMailContent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_tempContent.m_MailTitle;
        m_ReadMailContent.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = m_tempContent.m_FromMail;
        m_ReadMailContent.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = m_tempContent.m_MailContent;
        Debug.Log("메일 내용 출력");
        m_ReadMailContent.SetActive(true);
    }

    // 신규 메일함을 클릭하면 새로온 메일이 있는지 판단해주고 전체 메일함을 클릭하면 이제껏 받은 메일들 목록 띄워주기
    public void ClickMailBox()
    {
        m_ClickMailButton = EventSystem.current.currentSelectedGameObject;

        if (m_ClickMailButton.name == "New_Mail_Button")
        {
            if (m_MailBox.childCount != 0)
            {

                for (int i = 0; i < m_MailBox.childCount; i++)
                {
                    Destroy(m_MailBox.transform.GetChild(i).gameObject);
                    //m_MailBox.GetChild(i).GetComponent<GameObject>().SetActive(false);
                }

                for (int i = 0; i < m_AllMailTextList.Count; i++)
                {
                    if (m_MailList[i].m_IsNewMail == true)
                    {
                        GameObject m_NewMailText; // = m_AllMailTextList[i]

                        m_NewMailText = Instantiate(m_NewMailPrefab, m_MailBox);
                        m_NewMailText.name = m_MailList[i].m_MailTitle;
                        m_NewMailText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_MailTitle;
                        m_NewMailText.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_SendMailDate;
                        m_NewMailText.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_FromMail;
                        m_NewMailText.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ReadMail);
                        m_AllMailTextList.Clear();
                        m_AllMailTextList.Add(m_NewMailText);

                        _isNewMAilCheck = true;
                    }
                }

                if (!_isNewMAilCheck)
                {
                    m_TempNotNewMail.SetActive(true);
                }
            }
            else
            {
                m_TempNotNewMail.SetActive(true);
            }
        }
        else if (m_ClickMailButton.name == "All_Mail_Button ")
        {
            for (int i = 0; i < m_MailBox.childCount; i++)
            {
                Destroy(m_MailBox.transform.GetChild(i).gameObject);
                //m_MailBox.GetChild(i).GetComponent<GameObject>().SetActive(false);
            }

            for (int i = 0; i < m_AllMailTextList.Count; i++)
            {
                GameObject m_AllMailText = m_AllMailTextList[i];

                //m_MailBox.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = m_AllMailTextList[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;

                m_AllMailText = Instantiate(m_NewMailPrefab, m_MailBox);
                m_AllMailText.name = m_MailList[i].m_MailTitle;
                m_AllMailText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_MailTitle;
                m_AllMailText.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_SendMailDate;
                m_AllMailText.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_FromMail;
                m_AllMailText.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ReadMail);
                m_AllMailTextList.Clear();
                m_AllMailTextList.Add(m_AllMailText);
            }
            m_TempNotNewMail.SetActive(false);
        }
    }

    // 닫기 버튼을 누르면 방금 읽은 메일을 삭제시켜주고 신규 메일이 없다면 신규메일 없음 띄워주기
    // 신규메일함에서 보는건지 전체 메일함에서 보는건지 구분 필요
    public void CloseMail()
    {
        if (m_ClickMailButton.name == "New_Mail_Button")// 내가 현재 전체메일함에 있는지 신규 메일함에 있는지 구분을 해줘야한다.
        {
            string m_TitleName = m_ReadMailContent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;

            int _index = FindListIndex(m_AllMailTextList, m_TitleName);

            m_TempMailTextList.Add(m_AllMailTextList[_index]);

            Debug.Log(_index + " 삭제");
            Destroy(m_MailBox.transform.GetChild(_index).gameObject);
            m_AllMailTextList.Clear();

            m_AllMailTextList.Add(m_TempMailTextList[_index]);

            bool _isNewMail = false;

            foreach (var mail in m_MailList)
            {
                if (mail.m_IsNewMail)
                {
                    _isNewMail = true;
                    _isNewMAilCheck = true;
                    break;
                }
            }

            if (!_isNewMail)
            {
                m_TempNotNewMail.SetActive(true);
                _isNewMAilCheck = false;
                //m_NewMailTextList.Clear();
            }

            m_ReadMailContent.SetActive(false);
        }
        // 전체메일함에서는 꺼주기만 하면 된다.
        else if (m_ClickMailButton.name == "All_Mail_Button ")
        {
            m_ReadMailContent.SetActive(false);
        }
    }

    // 뒤로가기 버튼을 누르면 모든 신규메일을 읽음처리로 바꿔주기
    public void ClickBackButton()
    {
        m_TempMailTextList.Clear();
        #region _뒤로가기를 눌러도 신규메일을 유지해야하는 경우의 코드 
        //bool _isNewMail = false;

        //foreach (var mail in m_MailList)
        //{
        //    if (mail.m_IsNewMail)
        //    {
        //        _isNewMail = true;
        //        break;
        //    }
        //}


        //if (!_isNewMail)
        //{
        //    m_BulbIcon.SetActive(false);
        //}
        #endregion

        if (m_ClickMailButton.name == "New_Mail_Button")
        {
            if (m_MailBox.childCount != 0)
            {
                for (int i = 0; i < m_AllMailTextList.Count; i++)
                {
                    m_TempMailTextList.Add(m_AllMailTextList[i]);
                    Destroy(m_MailBox.transform.GetChild(i).gameObject);
                }

                m_AllMailTextList.Clear();

                for (int i = 0; i < m_TempMailTextList.Count; i++)
                {
                    m_AllMailTextList.Add(m_TempMailTextList[i]);
                }
            }
            //m_NewMailTextList.Clear();

            foreach (var mail in m_MailList)
            {
                if (mail.m_IsNewMail)
                {
                    mail.m_IsNewMail = false;
                    _isNewMAilCheck = false;
                }
            }

            m_BulbIcon.SetActive(false);
        }

        else if(m_ClickMailButton.name == "All_Mail_Button")
        {
            m_BulbIcon.SetActive(false);
        }
    }
}
