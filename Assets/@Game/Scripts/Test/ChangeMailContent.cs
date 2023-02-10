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
    [SerializeField] private Button m_NewMailButton;        // 신규메일 버튼
    [SerializeField] private GameObject m_Button;           // 처음 메일함을 누르면 무조선 신규메일로 만들어주기 위한 오브젝트
    [SerializeField] private GameObject m_ReadMailContent;  // 메일 내용이 있는 오브젝트. 메일 내용들을 넣어줘야한다.
    [SerializeField] private Transform m_MailBox;           // 발송된 메일들이 어디에 생성될 지 결정해주는 Transform
    [SerializeField] private GameObject m_NotNewMail;       // 새로운 메일이 없으면 "신규메일이 없습니다"를 띄워주기 위한 오브젝트
    [SerializeField] private GameObject m_BulbIcon;         // 신규메일이 발송되면 메일함에 띄울 이미지
    [SerializeField] private GameObject m_Filter;           // 타입별로 메일을 볼 수있게 하기위한 오브젝트

    private GameObject m_ClickMailButton;   // 전체 메일함인지 신규 메일함인지 기억해야한다.
    private GameObject m_CheckMailName;     // 내가 읽으려고 하는 메일이 무엇인지 알기위한 변수
    private GameObject m_CheckFilter;       // 내가 띄우려고 하는 메일이 무슨 타입인지 확인하기 위한 변수

    // 메일을 관리하기 위한 리스트들
    private Dictionary<string, MailBox> m_Mail = new Dictionary<string, MailBox>();     // 발송에 씌이는 전체 메일
    private List<MailBox> m_MailList = new List<MailBox>();                             // 새로온 메일 내용
    private List<GameObject> m_NewMailTextList = new List<GameObject>();                // 새로 받은 메일들을 쌓아 둘 리스트
    private List<GameObject> m_AllMailTextList = new List<GameObject>();                // 여태것 받은 메일들을 쌓아 둘 리스트

    int _i = 0;                     // 전체 메일함에서 순서대로 메일들을 발송해주기 위해 만든 int값. 메일을 발송하고 나면 증가시켜준다(추후 메일 컨텐츠가 나온다면 바꿔야할 거 같다).
    bool _isNewMAilCheck = false;   // 현재 새로온 메일이 있는지 확인해주는 bool값
    bool _isClick = false;          // 필터 버튼을 눌렀을 때 필터를 껐다 켰다 할 수 있게 해주려고 만든 bool값


    private void Start()
    {
        m_ClickMailButton = m_Button;
        MakeMail("안녕하세요", "2023년 03월 01일", "망극아카데미", "오늘은 3월 1일 입니다.", MailType.News);
        MakeMail("반갑습니다", "2023년 03월 02일", "망극아카데미", "오늘은 3월 2일 입니다.", MailType.Month);
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
    public int FindListIndex(List<MailBox> list, string val)
    {
        if (list == null)
        {
            return -1;
        }

        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log("현재 리스트 카운트 : " + list.Count);
            if (list[i].m_MailTitle == val)
            {
                return i;
            }
        }

        return -1;
    }

    // 인게임 UI 메일함을 눌렀을 때 새로온 메일이 있나 없나 확인해보고 새로운 메일이 없습니다를 띄워주는 역할
    public void SetButtonColor()
    {
        m_NewMailButton.Select();

        // 처음 메일함을  들어가면 신규메일함이 나와야한다.
        if (m_ClickMailButton.name == "All_Mail_Button")
        {
            m_ClickMailButton = m_Button;
        }

        bool _isNewMail = false;

        foreach (var mail in m_MailList)
        {
            if (mail.m_IsNewMail)
            {
                _isNewMail = true;
                _isNewMAilCheck = true; // 여기 불값만 조정하면 완성될 듯..
                break;
            }
        }

        if (!_isNewMail)
        {
            m_NotNewMail.SetActive(true);
        }
        else
        {
            m_NotNewMail.SetActive(false);
        }
    }

    // 메일의 제목과 보낸사람, 보낸날짜를 바꿔준다.
    public void ChangeDateAndFrom()
    {
        m_MailList.Add(m_Mail.ElementAt(_i).Value);

        GameObject m_AllMailText;

        if (m_MailList[_i].m_IsNewMail)
        {
            m_AllMailText = MailObjectPool.GetObject(m_MailBox);

            m_AllMailText.transform.localScale = new Vector3(1f, 1f, 1f);
            m_AllMailText.name = m_MailList[_i].m_MailTitle;
            m_AllMailText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_MailList[_i].m_MailTitle;
            m_AllMailText.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[_i].m_SendMailDate;
            m_AllMailText.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[_i].m_FromMail;
            m_AllMailText.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ReadMail);

            m_NewMailTextList.Add(m_AllMailText);
            m_AllMailTextList.Add(m_AllMailText);
            _i++;
            _isNewMAilCheck = true;
        }
    }

    private void ReadMail()
    {
        if (m_ClickMailButton.name == "New_Mail_Button")
        {
            Debug.Log("신규메일 읽기");
            MailBox m_tempContent;

            // 내 부모를 찾아 부모의 이름을 가져와 딕셔너리에서 같은 이름의 메일을 찾아온다.
            m_CheckMailName = EventSystem.current.currentSelectedGameObject;
            string m_Parent = m_CheckMailName.transform.parent.name;
            Debug.Log(m_Parent);

            m_Mail.TryGetValue(m_Parent, out m_tempContent);

            int _index = FindListIndex(m_MailList, m_tempContent.m_MailTitle);

            m_MailList[_index].m_IsNewMail = false;
            _isNewMAilCheck = false;

            m_ReadMailContent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_tempContent.m_MailTitle;
            m_ReadMailContent.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = m_tempContent.m_FromMail;
            m_ReadMailContent.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = m_tempContent.m_MailContent;
            Debug.Log("메일 내용 출력");
            m_ReadMailContent.SetActive(true);
        }
        else if (m_ClickMailButton.name == "All_Mail_Button")
        {
            Debug.Log("전체메일 읽기");
            MailBox m_tempContent;

            // 내 부모를 찾아 부모의 이름을 가져와 딕셔너리에서 같은 이름의 메일을 찾아온다.
            m_CheckMailName = EventSystem.current.currentSelectedGameObject;
            string m_Parent = m_CheckMailName.transform.parent.name;
            Debug.Log(m_Parent);

            m_Mail.TryGetValue(m_Parent, out m_tempContent);

            m_ReadMailContent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_tempContent.m_MailTitle;
            m_ReadMailContent.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = m_tempContent.m_FromMail;
            m_ReadMailContent.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = m_tempContent.m_MailContent;
            Debug.Log("메일 내용 출력");
            m_ReadMailContent.SetActive(true);
        }
    }

    // 신규 메일함을 클릭하면 새로온 메일이 있는지 판단해주고 전체 메일함을 클릭하면 이제껏 받은 메일들 목록 띄워주기
    public void ClickMailBox()
    {
        m_ClickMailButton = EventSystem.current.currentSelectedGameObject;

        int _DestroyMailCount = m_MailBox.childCount;

        if (m_ClickMailButton.name == "New_Mail_Button")
        {
            GameObject m_NewMailText; // = m_AllMailTextList[i]

            if (m_MailBox.childCount != 0)
            {
                for (int i = _DestroyMailCount - 1; i >= 0; i--)
                {
                    MailObjectPool.ReturnObject(m_MailBox.transform.GetChild(i).gameObject);
                }

                for (int i = 0; i < m_MailList.Count; i++)
                {
                    if (m_MailList[i].m_IsNewMail == true)
                    {
                        m_NewMailText = MailObjectPool.GetObject(m_MailBox);

                        m_NewMailText.transform.localScale = new Vector3(1f, 1f, 1f);
                        m_NewMailText.name = m_MailList[i].m_MailTitle;
                        m_NewMailText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_MailTitle;
                        m_NewMailText.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_SendMailDate;
                        m_NewMailText.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_FromMail;
                        m_NewMailText.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ReadMail);

                        _isNewMAilCheck = true;
                    }
                }

                if (!_isNewMAilCheck)
                {
                    m_NotNewMail.SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < m_MailList.Count; i++)
                {
                    if (m_MailList[i].m_IsNewMail == true)
                    {
                        m_NewMailText = MailObjectPool.GetObject(m_MailBox);

                        m_NewMailText.transform.localScale = new Vector3(1f, 1f, 1f);
                        m_NewMailText.name = m_MailList[i].m_MailTitle;
                        m_NewMailText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_MailTitle;
                        m_NewMailText.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_SendMailDate;
                        m_NewMailText.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_FromMail;
                        m_NewMailText.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ReadMail);

                        _isNewMAilCheck = true;
                    }
                }

                if (m_MailBox.childCount == 0)
                {
                    m_NotNewMail.SetActive(true);
                }
            }
        }
        else if (m_ClickMailButton.name == "All_Mail_Button")
        {
            if (m_MailBox.childCount != 0)
            {
                for (int i = _DestroyMailCount - 1; i >= 0; i--)
                {
                    MailObjectPool.ReturnObject(m_MailBox.transform.GetChild(i).gameObject);
                }
            }

            for (int i = 0; i < m_AllMailTextList.Count; i++)
            {
                GameObject m_AllMailText; //= m_AllMailTextList[i];

                m_AllMailText = MailObjectPool.GetObject(m_MailBox);

                m_AllMailText.transform.localScale = new Vector3(1f, 1f, 1f);
                m_AllMailText.name = m_MailList[i].m_MailTitle;
                m_AllMailText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_MailTitle;
                m_AllMailText.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_SendMailDate;
                m_AllMailText.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_FromMail;
                m_AllMailText.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ReadMail);
            }
            m_NotNewMail.SetActive(false);
        }
    }

    // 닫기 버튼을 누르면 방금 읽은 메일을 삭제시켜주고 신규 메일이 없다면 신규메일 없음 띄워주기
    // 신규메일함에서 보는건지 전체 메일함에서 보는건지 구분 필요
    public void CloseMail()
    {
        if (m_ClickMailButton.name == "New_Mail_Button")// 내가 현재 전체메일함에 있는지 신규 메일함에 있는지 구분을 해줘야한다.
        {
            string m_TitleName = m_ReadMailContent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;

            int _index = FindListIndex(m_MailList, m_TitleName);

            Debug.Log(_index + " 삭제");
            if (m_MailBox.childCount > 1)
            {
                MailObjectPool.ReturnObject(m_MailBox.transform.GetChild(_index).gameObject);
                m_NewMailTextList.RemoveAt(_index);
            }
            else
            {
                int _mailBoxCount = m_MailBox.childCount - 1;
                MailObjectPool.ReturnObject(m_MailBox.transform.GetChild(_mailBoxCount).gameObject);
                if (m_NewMailTextList.Count == 1 && _index != 0)
                {
                    m_NewMailTextList.RemoveAt(_index - 1);
                }
                else
                {
                    m_NewMailTextList.RemoveAt(_index);
                }
            }

            bool _isNewMail = false;

            foreach (var mail in m_MailList)
            {
                if (mail.m_IsNewMail == true)
                {
                    _isNewMail = true;
                    _isNewMAilCheck = true;
                    break;
                }
            }

            if (!_isNewMail)
            {
                m_NotNewMail.SetActive(true);
                _isNewMAilCheck = false;
            }

            m_ReadMailContent.SetActive(false);

            if (m_MailBox.childCount == 0)
            {
                m_NotNewMail.SetActive(true);
            }
        }
        // 전체메일함에서는 꺼주기만 하면 된다.
        else if (m_ClickMailButton.name == "All_Mail_Button")
        {
            _isNewMAilCheck = false;
            m_ReadMailContent.SetActive(false);
        }
    }

    // 뒤로가기 버튼을 누르면 모든 신규메일을 읽음처리로 바꿔주기
    public void ClickBackButton()
    {
        m_NewMailTextList.Clear();
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

        int _DestroyMailObj = m_MailBox.childCount;

        if (m_ClickMailButton.name == "New_Mail_Button")
        {
            if (m_MailBox.childCount != 0)
            {
                for (int i = _DestroyMailObj - 1; i >= 0; i--)
                {
                    MailObjectPool.ReturnObject(m_MailBox.transform.GetChild(i).gameObject);
                }
            }

            foreach (var mail in m_MailList)
            {
                if (mail.m_IsNewMail)
                {
                    mail.m_IsNewMail = false;
                    _isNewMAilCheck = false;
                }
            }
            m_Filter.SetActive(false);
            m_BulbIcon.SetActive(false);
        }
        else if (m_ClickMailButton.name == "All_Mail_Button") // 띄어쓰기 확인할 것....
        {
            for (int i = m_AllMailTextList.Count - 1; i >= 0; i--)
            {
                MailObjectPool.ReturnObject(m_MailBox.transform.GetChild(i).gameObject);
            }
            _isNewMAilCheck = false;
            m_Filter.SetActive(false);
            m_BulbIcon.SetActive(false);
        }
    }

    public void ClickFilter()
    {
        if (_isClick == false)
        {
            m_Filter.SetActive(true);
            _isClick = true;
        }
        else
        {
            m_Filter.SetActive(false);
            _isClick = false;
        }
    }

    private void MakeNewMailForType(MailType _type)
    {
        if (m_NewMailTextList.Count != 0)
        {
            int _DestroyMailCount = m_MailBox.childCount;

            GameObject m_NewMailText;

            if (m_MailBox.childCount != 0)
            {
                for (int i = _DestroyMailCount - 1; i >= 0; i--)
                {
                    MailObjectPool.ReturnObject(m_MailBox.transform.GetChild(i).gameObject);
                }

                for (int i = 0; i < m_NewMailTextList.Count; i++)
                {
                    if (m_MailList[i].m_Type == _type)
                    {
                        m_NewMailText = MailObjectPool.GetObject(m_MailBox);

                        m_NewMailText.transform.localScale = new Vector3(1f, 1f, 1f);
                        m_NewMailText.name = m_MailList[i].m_MailTitle;
                        m_NewMailText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_MailTitle;
                        m_NewMailText.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_SendMailDate;
                        m_NewMailText.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_FromMail;
                        m_NewMailText.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ReadMail);

                        _isNewMAilCheck = true;
                    }
                }
                m_NotNewMail.SetActive(false);

                if (m_MailBox.childCount == 0)
                {
                    m_NotNewMail.SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < m_NewMailTextList.Count; i++)
                {
                    if (m_MailList[i].m_Type == _type)
                    {
                        m_NewMailText = MailObjectPool.GetObject(m_MailBox);

                        m_NewMailText.transform.localScale = new Vector3(1f, 1f, 1f);
                        m_NewMailText.name = m_MailList[i].m_MailTitle;
                        m_NewMailText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_MailTitle;
                        m_NewMailText.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_SendMailDate;
                        m_NewMailText.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_FromMail;
                        m_NewMailText.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ReadMail);

                        _isNewMAilCheck = true;
                    }
                }
                m_NotNewMail.SetActive(false);

                if (m_MailBox.childCount == 0)
                {
                    m_NotNewMail.SetActive(true);
                }
            }
        }
    }

    private void MakeAllMailForType(MailType _type)
    {
        int _DestroyMailCount = m_MailBox.childCount;

        if (m_MailBox.childCount != 0)
        {
            for (int i = _DestroyMailCount - 1; i >= 0; i--)
            {
                MailObjectPool.ReturnObject(m_MailBox.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < m_AllMailTextList.Count; i++)
        {
            GameObject m_AllMailText; //= m_AllMailTextList[i];

            if (m_MailList[i].m_Type == _type)
            {
                m_AllMailText = MailObjectPool.GetObject(m_MailBox);

                m_AllMailText.transform.localScale = new Vector3(1f, 1f, 1f);
                m_AllMailText.name = m_MailList[i].m_MailTitle;
                m_AllMailText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_MailTitle;
                m_AllMailText.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_SendMailDate;
                m_AllMailText.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_FromMail;
                m_AllMailText.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ReadMail);
            }
        }
        m_NotNewMail.SetActive(false);

        if (m_MailBox.childCount == 0)
        {
            m_NotNewMail.SetActive(true);
        }
    }

    public void SetMailForType()
    {
        m_CheckFilter = EventSystem.current.currentSelectedGameObject;

        if (m_ClickMailButton.name == "New_Mail_Button")
        {
            if (m_CheckFilter.name == "Month_Report")
            {
                MakeNewMailForType(MailType.Month);
            }
            else if (m_CheckFilter.name == "Game_News")
            {
                MakeNewMailForType(MailType.News);
            }
            else if (m_CheckFilter.name == "Satisfaction")
            {
                MakeNewMailForType(MailType.Satisfaction);
            }
            else if (m_CheckFilter.name == "All")
            {
                GameObject m_NewMailText;
                int _DestroyMailCount = m_MailBox.childCount;

                if (m_MailBox.childCount != 0)
                {
                    for (int i = _DestroyMailCount - 1; i >= 0; i--)
                    {
                        MailObjectPool.ReturnObject(m_MailBox.transform.GetChild(i).gameObject);
                    }

                    for (int i = 0; i < m_MailList.Count; i++)
                    {
                        if (m_MailList[i].m_IsNewMail == true)
                        {
                            m_NewMailText = MailObjectPool.GetObject(m_MailBox);

                            m_NewMailText.transform.localScale = new Vector3(1f, 1f, 1f);
                            m_NewMailText.name = m_MailList[i].m_MailTitle;
                            m_NewMailText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_MailTitle;
                            m_NewMailText.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_SendMailDate;
                            m_NewMailText.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_FromMail;
                            m_NewMailText.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ReadMail);

                            _isNewMAilCheck = true;
                        }
                    }

                    if (!_isNewMAilCheck)
                    {
                        m_NotNewMail.SetActive(true);
                    }

                }
                else if (m_MailBox.childCount == 0 && m_NewMailTextList.Count != 0)
                {
                    for (int i = 0; i < m_MailList.Count; i++)
                    {
                        if (m_MailList[i].m_IsNewMail == true)
                        {
                            m_NewMailText = MailObjectPool.GetObject(m_MailBox);

                            m_NewMailText.transform.localScale = new Vector3(1f, 1f, 1f);
                            m_NewMailText.name = m_MailList[i].m_MailTitle;
                            m_NewMailText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_MailTitle;
                            m_NewMailText.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_SendMailDate;
                            m_NewMailText.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_FromMail;
                            m_NewMailText.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ReadMail);

                            _isNewMAilCheck = true;
                        }
                    }
                    m_NotNewMail.SetActive(false);
                }
                else if (m_NewMailTextList.Count == 0)
                {
                    m_NotNewMail.SetActive(true);
                }
            }
        }
        else if (m_ClickMailButton.name == "All_Mail_Button")
        {
            if (m_CheckFilter.name == "Month_Report")
            {
                MakeAllMailForType(MailType.Month);
            }
            else if (m_CheckFilter.name == "Game_News")
            {
                MakeAllMailForType(MailType.News);
            }
            else if (m_CheckFilter.name == "Satisfaction")
            {
                MakeAllMailForType(MailType.Satisfaction);
            }
            else if (m_CheckFilter.name == "All")
            {
                if (m_MailBox.childCount != 0)
                {
                    int _DestroyMailCount = m_MailBox.childCount;

                    for (int i = _DestroyMailCount - 1; i >= 0; i--)
                    {
                        MailObjectPool.ReturnObject(m_MailBox.transform.GetChild(i).gameObject);
                    }
                }

                for (int i = 0; i < m_AllMailTextList.Count; i++)
                {
                    GameObject m_AllMailText;

                    m_AllMailText = MailObjectPool.GetObject(m_MailBox);

                    m_AllMailText.transform.localScale = new Vector3(1f, 1f, 1f);
                    m_AllMailText.name = m_MailList[i].m_MailTitle;
                    m_AllMailText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_MailTitle;
                    m_AllMailText.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_SendMailDate;
                    m_AllMailText.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_MailList[i].m_FromMail;
                    m_AllMailText.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ReadMail);
                }
                m_NotNewMail.SetActive(false);
            }
        }
    }
}
