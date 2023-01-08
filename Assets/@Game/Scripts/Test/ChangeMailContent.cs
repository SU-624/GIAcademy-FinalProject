using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

// ������ ���� ������ ���� Ÿ��
public enum MailType
{
    Month,
    News,
    Satisfaction
}

// ���ϵ��� ������ ���� Ŭ����
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

    private GameObject m_ClickMailButton;   // ��ü ���������� �ű� ���������� ����ؾ��Ѵ�.
    private GameObject m_CheckMailName;     // ���� �������� �ϴ� ������ �������� �˱����� ����

    // ������ �����ϱ� ���� ����Ʈ��
    private Dictionary<string, MailBox> m_Mail = new Dictionary<string, MailBox>();     // �߼ۿ� ���̴� ��ü ����
    private List<MailBox> m_MailList = new List<MailBox>();  // ���ο� ���� ����
    private List<GameObject> m_TempMailTextList = new List<GameObject>(); // ���� ���� ���ϵ��� �׾� �� ����Ʈ
    private List<GameObject> m_AllMailTextList = new List<GameObject>(); // ���°� ���� ���ϵ��� �׾� �� ����Ʈ

    int _i = 0;
    bool _isNewMAilCheck = false;

    private void Start()
    {
        m_ClickMailButton = m_Button;
        MakeMail("�ȳ�ȳ�", "2023�� 01�� 01��", "���ؾ�ī����", "���� �����̴�!", MailType.News);
        MakeMail("�����", "2023�� 01�� 02��", "���ؾ�ī����", " �׸�,,", MailType.News);
    }

    // �ӽ÷� ������ ������ִ� �Լ�
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

    // ����Ʈ�� �ε����� ã���ִ� �Լ�. ������Ʈ�� �̸����� ã�Ƽ� �ε����� ã��.
    public int FindListIndex(List<GameObject> list, string val)
    {
        if (list == null)
        {
            return -1;
        }

        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log("���� ����Ʈ ī��Ʈ : " + list.Count);
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

        // ó�� ��������  ���� �űԸ������� ���;��Ѵ�.
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

    // ������ ����� �������, ������¥�� �ٲ��ش�.
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
            Debug.Log("�űԸ��� �б�");
            PopUpMailContent(m_AllMailTextList);
        }
        else if (m_ClickMailButton.name == "All_Mail_Button ")
        {
            Debug.Log("��ü���� �б�");
            PopUpMailContent(m_AllMailTextList);
        }
    }

    // ������ �б⸦ ������ �ش� ������ ������ ������ ���ְ� ������ �����Կ� �ִ� ����� ���������ش�.
    private void PopUpMailContent(List<GameObject> _list)
    {
        MailBox m_tempContent;

        // �� �θ� ã�� �θ��� �̸��� ������ ��ųʸ����� ���� �̸��� ������ ã�ƿ´�.
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
        Debug.Log("���� ���� ���");
        m_ReadMailContent.SetActive(true);
    }

    // �ű� �������� Ŭ���ϸ� ���ο� ������ �ִ��� �Ǵ����ְ� ��ü �������� Ŭ���ϸ� ������ ���� ���ϵ� ��� ����ֱ�
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

    // �ݱ� ��ư�� ������ ��� ���� ������ ���������ְ� �ű� ������ ���ٸ� �űԸ��� ���� ����ֱ�
    // �űԸ����Կ��� ���°��� ��ü �����Կ��� ���°��� ���� �ʿ�
    public void CloseMail()
    {
        if (m_ClickMailButton.name == "New_Mail_Button")// ���� ���� ��ü�����Կ� �ִ��� �ű� �����Կ� �ִ��� ������ ������Ѵ�.
        {
            string m_TitleName = m_ReadMailContent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;

            int _index = FindListIndex(m_AllMailTextList, m_TitleName);

            m_TempMailTextList.Add(m_AllMailTextList[_index]);

            Debug.Log(_index + " ����");
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
        // ��ü�����Կ����� ���ֱ⸸ �ϸ� �ȴ�.
        else if (m_ClickMailButton.name == "All_Mail_Button ")
        {
            m_ReadMailContent.SetActive(false);
        }
    }

    // �ڷΰ��� ��ư�� ������ ��� �űԸ����� ����ó���� �ٲ��ֱ�
    public void ClickBackButton()
    {
        m_TempMailTextList.Clear();
        #region _�ڷΰ��⸦ ������ �űԸ����� �����ؾ��ϴ� ����� �ڵ� 
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
