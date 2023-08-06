using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Mang 
/// 
/// �л���������â�� Ŭ�� �̺�Ʈ�� ���� ������� Ŭ����
/// </summary>
public class CharacterClickEvent : MonoBehaviour
{
    private bool isPopUpCharacterNamePlate = false;
    public Image CharacterMark;
    GameObject nowClickCharacter;

    [Space(5f)]
    [Header("�˾�, �˿����� ���� �л������г�")]
    [SerializeField] private PopUpUI PopUpStudentInfoPanel;
    [SerializeField] private PopOffUI PopOffCharacterInfoPanel;

    [Space(5f)]
    [Header("�˾�, �˿����� ���� ���������г�")]
    [SerializeField] private PopUpUI PopUpInstructorInfoPanel;
    [SerializeField] private PopOffUI PopOffInstructorInfoPanel;

    [Space(5f)]
    [Header("ĳ���� ���� â�� ���� ���� �г� ����")]
    [SerializeField] private characterInfo CharacterNamePlate;
    [SerializeField] private CharacterInfoPanel StudentInfoPage;
    [SerializeField] private InstructorInfoPanel InstructorInfoPage;

    string NowClickCharacterTag;

    [SerializeField] private Sprite GameDesignerImage;
    [SerializeField] private Sprite ArtImage;
    [SerializeField] private Sprite ProgrammingImage;

    private int m_CameraLayerMask = 1 << 7;
    private int m_layerMask = 1 << 8;
    private int m_uiLayerMask = 1 << 5;

    public bool GetisPopUpCharacterNamePlate
    {
        get { return isPopUpCharacterNamePlate; }
        set { isPopUpCharacterNamePlate = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        TurnOffCharacterNamePlate();
#if UNITY_EDITOR || UNITY_EDITOR_WIN
        CharacterInfoScreenWhenUnityEditor();

#elif UNITY_ANDROID
         CharacterInfoScreenWhenAndroid();
#endif
    }


    void PopUpCharacterInfo(GameObject nowClick)
    {
        switch (nowClick.tag)
        {
            case "Student":
                {
                    PopUpStudentInfoPanel.TurnOnUI();

                    if (PlayerInfo.Instance.StudentProfileClickCount <= 10)
                    {
                        PlayerInfo.Instance.StudentProfileClickCount++;
                    }
                }
                break;
            case "Instructor":
                {
                    PopUpInstructorInfoPanel.TurnOnUI();

                    if (PlayerInfo.Instance.TeacherProfileClickCount <= 10)
                    {
                        PlayerInfo.Instance.TeacherProfileClickCount++;
                    }
                }
                break;
        }
    }

    void CharacterInfoScreenWhenUnityEditor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ray �� ���������� ������ ����ȭ�� ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);       // ��ũ�� ��ǥ ���� ray ���� ������Ʈ�� �浹, ��ũ�� ��ǥ�� -> ���彺���̽� ��ǥ��
            RaycastHit hit;     // ����ĳ��Ʈ ���� ��ü
            GameObject nowClick = EventSystem.current.currentSelectedGameObject;

            if (GameTime.Instance.IsGameMode == true && isPopUpCharacterNamePlate == false/* && ObjectManager.Instance.m_StudentList.Count == 18*/)         // ĳ���� �̸�ǥ / ĳ��������â �� �� �ȶ����� ��
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask & m_layerMask))
                {
                    Debug.Log("������Ʈ�� �±� ? : " + hit.transform.name);
                    Debug.DrawRay(transform.position, transform.forward * 1000, Color.red);

                    if (!EventSystem.current.IsPointerOverGameObject())      // raycast�� UI �� ������ ���� �� (UI �ε����� true ��ȯ)
                    {
                        Debug.Log("�л����� �߱�");

                        if (hit.transform.tag == "Student" && ObjectManager.Instance.m_StudentList.Count == 18)     // �л� �ν�
                        {
                            Debug.Log("�л��Դϴ�");
                            NowClickCharacterTag = hit.transform.tag;

                            GameObject character = hit.transform.gameObject;        // ����ĳ��Ʈ ���� ĳ����
                            var CPositon = character.transform.position;        // ĳ������ �Ӹ��� ������S
                            StudentStat tempStudentStat =
                                hit.transform.gameObject.GetComponent<Student>().m_StudentStat;
                            // CharacterNamePlate.GetCharacterImage.sprite = GameDesignerImage;

                            if (tempStudentStat.m_UserSettingName != "")
                            {
                                CharacterNamePlate.CName.text = tempStudentStat.m_UserSettingName;
                            }
                            else
                            {
                                CharacterNamePlate.CName.text = character.name;
                            }

                            StudentInfoPage.ShowStudentBasicInfo(character);
                            StudentInfoPage.ShowCharacterDetailedStat(character);

                            Student _temp = character.GetComponent<Student>();
                            StudentInfoPage.SetStudentBonusSkill(_temp);

                            CharacterNamePlate.transform.gameObject.GetComponent<FollowTarget>().m_Target = character.transform.GetChild(0);

                            CharacterNamePlate.transform.gameObject.SetActive(true);

                            isPopUpCharacterNamePlate = true;

                            nowClickCharacter = character;

                            Camera.main.GetComponent<TestCamera>().IsFixed = true;
                            Camera.main.GetComponent<TestCamera>().FixedObject = character;
                        }
                        else if (hit.transform.tag == "Instructor")     // ���� �ν�
                        {
                            Debug.Log("�����Դϴ�");
                            NowClickCharacterTag = hit.transform.tag;

                            GameObject character = hit.transform.gameObject;        // ����ĳ��Ʈ ���� ĳ����
                            var CPositon = character.transform.position;        // ĳ������ �Ӹ��� ������

                            // CharacterNamePlate.GetCharacterImage.sprite = ArtImage;
                            CharacterNamePlate.CName.text = character.name;
                            InstructorInfoPage.ShowInstructorBasicInfo(character);
                            InstructorInfoPage.ShowInsrtuctorSkillInfo(character);

                            CharacterNamePlate.transform.gameObject.GetComponent<FollowTarget>().m_Target = character.transform.GetChild(0);

                            CharacterNamePlate.transform.gameObject.SetActive(true);

                            isPopUpCharacterNamePlate = true;

                            nowClickCharacter = character;

                            Camera.main.GetComponent<TestCamera>().IsFixed = true;
                            Camera.main.GetComponent<TestCamera>().FixedObject = character;
                        }
                    }
                }
            }
            else if (GameTime.Instance.IsGameMode == true && isPopUpCharacterNamePlate == true)           // ĳ���� �̸�â ���
            {
                if (nowClick != null)
                {
                    if (nowClick.name == CharacterNamePlate.InfoButton.name)
                    {
                        Debug.Log("ĳ��������â Ŭ��? > â����");

                        PopUpCharacterInfo(nowClickCharacter);
                        isPopUpCharacterNamePlate = false;
                    }
                }
                else
                {
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask & m_layerMask))
                    {
                        if (hit.transform.tag == "Student")
                        {
                            Debug.Log("�л��Դϴ�");
                            NowClickCharacterTag = hit.transform.tag;

                            GameObject character = hit.transform.gameObject;        // ����ĳ��Ʈ ���� ĳ����
                            var CPositon = character.transform.position;        // ĳ������ �Ӹ��� ������
                            var CHeadPositon = character.transform.GetChild(0).position;

                            StudentStat tempStudentStat =
                                hit.transform.gameObject.GetComponent<Student>().m_StudentStat;

                            if (tempStudentStat.m_UserSettingName != "")
                            {
                                CharacterNamePlate.CName.text = tempStudentStat.m_UserSettingName;
                            }
                            else
                            {
                                CharacterNamePlate.CName.text = character.name;
                            }
                            StudentInfoPage.ShowStudentBasicInfo(character);
                            StudentInfoPage.ShowCharacterDetailedStat(character);

                            Student _temp = character.GetComponent<Student>();
                            StudentInfoPage.SetStudentBonusSkill(_temp);

                            CharacterNamePlate.transform.gameObject.GetComponent<FollowTarget>().m_Target = character.transform.GetChild(0);

                            CharacterNamePlate.transform.gameObject.SetActive(true);

                            nowClickCharacter = character;

                            Camera.main.GetComponent<TestCamera>().IsFixed = true;
                            Camera.main.GetComponent<TestCamera>().FixedObject = character;
                        }
                        else if (hit.transform.tag == "Instructor")     // ���� �ν�
                        {
                            Debug.Log("�����Դϴ�");
                            NowClickCharacterTag = hit.transform.tag;

                            GameObject character = hit.transform.gameObject;        // ����ĳ��Ʈ ���� ĳ����
                            var CPositon = character.transform.position;        // ĳ������ �Ӹ��� ������

                            CharacterNamePlate.CName.text = character.name;
                            InstructorInfoPage.ShowInstructorBasicInfo(character);
                            InstructorInfoPage.ShowInsrtuctorSkillInfo(character);

                            CharacterNamePlate.transform.gameObject.GetComponent<FollowTarget>().m_Target = character.transform.GetChild(0);

                            CharacterNamePlate.transform.gameObject.SetActive(true);

                            isPopUpCharacterNamePlate = true;

                            nowClickCharacter = character;

                            Camera.main.GetComponent<TestCamera>().IsFixed = true;
                            Camera.main.GetComponent<TestCamera>().FixedObject = character;
                        }
                    }
                    else
                    {
                        isPopUpCharacterNamePlate = false;
                        Debug.Log("�Ӹ��� ������ ����");
                        Camera.main.GetComponent<TestCamera>().ResetCameraPosition();

                        if (CharacterNamePlate.transform.gameObject.activeSelf == true)
                        {
                            CharacterNamePlate.gameObject.SetActive(false);
                        }

                    }
                }
            }
            else if (GameTime.Instance.IsGameMode == false && isPopUpCharacterNamePlate == true)     // ĳ���� �̸�ǥ / ĳ��������â �� �� ������ ��
            {
                if (nowClick != null)
                {
                    if (nowClick.name == "QuitButton")
                    {
                        CharacterNamePlate.transform.gameObject.SetActive(false);

                        switch (NowClickCharacterTag)
                        {
                            case "Student":
                                {
                                    StudentInfoPage.gameObject.SetActive(false);
                                }
                                break;
                            case "Instructor":
                                {
                                    InstructorInfoPage.gameObject.SetActive(false);       // ���� �����ؼ� �ϱ�
                                }
                                break;
                        }
                        Time.timeScale = InGameUI.Instance.m_NowGameSpeed; ;
                        if (InGameUI.Instance.UIStack != null)
                        {
                            InGameUI.Instance.UIStack.Pop();
                            Debug.Log("�˾� â �翩�� ���� : " + InGameUI.Instance.UIStack.Count);
                        }
                        GameTime.Instance.IsGameMode = true;
                        isPopUpCharacterNamePlate = false;
                    }
                }
            }
        }
    }

    void CharacterInfoScreenWhenAndroid()
    {
        // Debug.Log("�ȵ���̵� ����� Ȯ�ο�");
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Debug.Log("��ġ ���� �� �����°�");
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Debug.Log("�հ����� �� �� �����°�");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);       // ��ũ�� ��ǥ ���� ray ���� ������Ʈ�� �浹, ��ũ�� ��ǥ�� -> ���彺���̽� ��ǥ��
            RaycastHit hit;     // ����ĳ��Ʈ ���� ��ü
            Touch touch = Input.GetTouch(0);

            Vector3 touchPosToVector3 = new Vector3(touch.position.x, touch.position.y);    // ��ġ�� ��ġ �ޱ�
            ray = Camera.main.ScreenPointToRay(touchPosToVector3);                          // ������ǥ�踦 

            if (GameTime.Instance.IsGameMode == true && isPopUpCharacterNamePlate == false)         // ĳ���� �̸�ǥ / ĳ��������â �� �� �ȶ����� ��
            {
                isPopUpCharacterNamePlate = true;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask & m_layerMask))          //����ĳ��Ʈ �¾Ҵ�?
                {
                    Debug.Log("������Ʈ�� �±� ? : " + hit.transform.name);

                    if (!EventSystem.current.IsPointerOverGameObject(0))      // raycast�� UI �� ������ ���� �� (UI �ε����� true ��ȯ)
                    {
                        Debug.Log("�л����� �߱�");

                        if (hit.transform.tag == "Student")
                        {
                            Debug.Log("�л��Դϴ�");

                            //Time.timeScale = 0;     // �ð�����

                            GameObject character = hit.transform.gameObject;        // ����ĳ��Ʈ ���� ĳ����
                            var CPositon = character.transform.position;        // ĳ������ �Ӹ��� ������
                            var CHeadPositon = character.transform.GetChild(0).position;

                            StudentStat tempStudentStat =
                                hit.transform.gameObject.GetComponent<Student>().m_StudentStat;

                            if (tempStudentStat.m_UserSettingName != "")
                            {
                                CharacterNamePlate.CName.text = tempStudentStat.m_UserSettingName;
                            }
                            else
                            {
                                CharacterNamePlate.CName.text = character.name;
                            }
                            StudentInfoPage.ShowStudentBasicInfo(character);
                            StudentInfoPage.ShowCharacterDetailedStat(character);

                            Student _temp = character.GetComponent<Student>();
                            StudentInfoPage.SetStudentBonusSkill(_temp);

                            CharacterNamePlate.transform.gameObject.GetComponent<FollowTarget>().m_Target = character.transform.GetChild(0);

                            CharacterNamePlate.transform.gameObject.SetActive(true);

                            nowClickCharacter = character;

                            Camera.main.GetComponent<TestCamera>().IsFixed = true;
                            Camera.main.GetComponent<TestCamera>().FixedObject = character;
                        }
                        else if (hit.transform.tag == "Instructor")     // ���� �ν�
                        {
                            Debug.Log("�����Դϴ�");

                            GameObject character = hit.transform.gameObject;        // ����ĳ��Ʈ ���� ĳ����
                            var CPositon = character.transform.position;        // ĳ������ �Ӹ��� ������

                            CharacterNamePlate.CName.text = character.name;
                            InstructorInfoPage.ShowInstructorBasicInfo(character);
                            InstructorInfoPage.ShowInsrtuctorSkillInfo(character);

                            CharacterNamePlate.transform.gameObject.GetComponent<FollowTarget>().m_Target = character.transform.GetChild(0);

                            CharacterNamePlate.transform.gameObject.SetActive(true);

                            isPopUpCharacterNamePlate = true;

                            nowClickCharacter = character;

                            Camera.main.GetComponent<TestCamera>().IsFixed = true;
                            Camera.main.GetComponent<TestCamera>().FixedObject = character;
                        }
                    }
                }
            }
            else if (GameTime.Instance.IsGameMode == true && isPopUpCharacterNamePlate == true)
            {
                GameObject nowClick = EventSystem.current.currentSelectedGameObject;

                if (nowClick != null)
                {
                    if (nowClick.name == CharacterNamePlate.InfoButton.name)
                    {
                        Debug.Log("ĳ��������â Ŭ��? > â����");

                        PopUpCharacterInfo(nowClickCharacter);
                    }
                }
                else
                {
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask & m_layerMask))
                    {
                        if (hit.transform.tag == "Student")
                        {
                            Debug.Log("�л��Դϴ�");

                            //Time.timeScale = 0;     // �ð�����

                            GameObject character = hit.transform.gameObject;        // ����ĳ��Ʈ ���� ĳ����
                            var CPositon = character.transform.position;        // ĳ������ �Ӹ��� ������
                            var CHeadPositon = character.transform.GetChild(0).position;

                            StudentStat tempStudentStat =
                                hit.transform.gameObject.GetComponent<Student>().m_StudentStat;

                            if (tempStudentStat.m_UserSettingName != "")
                            {
                                CharacterNamePlate.CName.text = tempStudentStat.m_UserSettingName;
                            }
                            else
                            {
                                CharacterNamePlate.CName.text = character.name;
                            }
                            StudentInfoPage.ShowStudentBasicInfo(character);
                            StudentInfoPage.ShowCharacterDetailedStat(character);

                            Student _temp = character.GetComponent<Student>();
                            StudentInfoPage.SetStudentBonusSkill(_temp);

                            CharacterNamePlate.transform.gameObject.GetComponent<FollowTarget>().m_Target = character.transform.GetChild(0);

                            CharacterNamePlate.transform.gameObject.SetActive(true);

                            nowClickCharacter = character;

                            Camera.main.GetComponent<TestCamera>().IsFixed = true;
                            Camera.main.GetComponent<TestCamera>().FixedObject = character;
                        }
                        else if (hit.transform.tag == "Instructor")     // ���� �ν�
                        {
                            Debug.Log("�����Դϴ�");

                            GameObject character = hit.transform.gameObject;        // ����ĳ��Ʈ ���� ĳ����
                            var CPositon = character.transform.position;        // ĳ������ �Ӹ��� ������

                            CharacterNamePlate.CName.text = character.name;
                            InstructorInfoPage.ShowInstructorBasicInfo(character);
                            InstructorInfoPage.ShowInsrtuctorSkillInfo(character);

                            CharacterNamePlate.transform.gameObject.GetComponent<FollowTarget>().m_Target = character.transform.GetChild(0);

                            CharacterNamePlate.transform.gameObject.SetActive(true);

                            isPopUpCharacterNamePlate = true;

                            nowClickCharacter = character;

                            Camera.main.GetComponent<TestCamera>().IsFixed = true;
                            Camera.main.GetComponent<TestCamera>().FixedObject = character;
                        }
                    }
                    else
                    {
                        isPopUpCharacterNamePlate = false;
                        Debug.Log("�Ӹ��� ������ ����");
                        Camera.main.GetComponent<TestCamera>().ResetCameraPosition();

                        if (CharacterNamePlate.transform.gameObject.activeSelf == true)
                        {
                            CharacterNamePlate.gameObject.SetActive(false);
                        }
                    }
                }
            }
            else if (GameTime.Instance.IsGameMode == false && isPopUpCharacterNamePlate == true)
            {
                GameObject nowClick = EventSystem.current.currentSelectedGameObject;

                if (nowClick != null)
                {
                    if (nowClick.name == "QuitButton")
                    {
                        CharacterNamePlate.transform.gameObject.SetActive(false);

                        switch (NowClickCharacterTag)
                        {
                            case "Student":
                                {
                                    StudentInfoPage.gameObject.SetActive(false);
                                }
                                break;
                            case "Instructor":
                                {
                                    InstructorInfoPage.gameObject.SetActive(false);       // ���� �����ؼ� �ϱ�
                                }
                                break;
                        }

                        Time.timeScale = InGameUI.Instance.m_NowGameSpeed;
                        if (InGameUI.Instance.UIStack != null)
                        {
                            InGameUI.Instance.UIStack.Pop();
                            Debug.Log("�˾� â �翩�� ���� : " + InGameUI.Instance.UIStack.Count);
                        }
                        GameTime.Instance.IsGameMode = true;
                        isPopUpCharacterNamePlate = false;
                    }
                }
            }
        }
    }

    public void TurnOffCharacterNamePlate()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            CharacterNamePlate.gameObject.SetActive(false);

            // �ΰ��Ӿ����� ����� �ϴ� ģ���� �̰����� �߰��ϱ�
            Camera.main.GetComponent<TestCamera>().ResetCameraPosition();

        }
#elif UNITY_ANDROID
if (Input.touchCount > 0)
        {
            CharacterNamePlate.gameObject.SetActive(false);
            Camera.main.GetComponent<TestCamera>().ResetCameraPosition();
        }
#endif
    }
}
