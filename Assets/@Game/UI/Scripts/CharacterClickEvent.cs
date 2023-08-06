using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Mang 
/// 
/// 학생개인정보창의 클릭 이벤트를 위해 만들어진 클래스
/// </summary>
public class CharacterClickEvent : MonoBehaviour
{
    private bool isPopUpCharacterNamePlate = false;
    public Image CharacterMark;
    GameObject nowClickCharacter;

    [Space(5f)]
    [Header("팝업, 팝오프를 위한 학생정보패널")]
    [SerializeField] private PopUpUI PopUpStudentInfoPanel;
    [SerializeField] private PopOffUI PopOffCharacterInfoPanel;

    [Space(5f)]
    [Header("팝업, 팝오프를 위한 강사정보패널")]
    [SerializeField] private PopUpUI PopUpInstructorInfoPanel;
    [SerializeField] private PopOffUI PopOffInstructorInfoPanel;

    [Space(5f)]
    [Header("캐릭터 인포 창을 띄우기 위한 패널 모음")]
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
            // ray 를 지역변수로 만들자 최적화를 위해
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);       // 스크린 좌표 기준 ray 쏴서 오브젝트와 충돌, 스크린 좌표계 -> 월드스페이스 좌표계
            RaycastHit hit;     // 레이캐스트 맞을 물체
            GameObject nowClick = EventSystem.current.currentSelectedGameObject;

            if (GameTime.Instance.IsGameMode == true && isPopUpCharacterNamePlate == false/* && ObjectManager.Instance.m_StudentList.Count == 18*/)         // 캐릭터 이름표 / 캐릭터정보창 둘 다 안떠있을 때
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask & m_layerMask))
                {
                    Debug.Log("오브젝트의 태그 ? : " + hit.transform.name);
                    Debug.DrawRay(transform.position, transform.forward * 1000, Color.red);

                    if (!EventSystem.current.IsPointerOverGameObject())      // raycast가 UI 를 만나지 않을 때 (UI 부딪히면 true 반환)
                    {
                        Debug.Log("학생인포 뜨기");

                        if (hit.transform.tag == "Student" && ObjectManager.Instance.m_StudentList.Count == 18)     // 학생 인식
                        {
                            Debug.Log("학생입니다");
                            NowClickCharacterTag = hit.transform.tag;

                            GameObject character = hit.transform.gameObject;        // 레이캐스트 맞은 캐릭터
                            var CPositon = character.transform.position;        // 캐릭터의 머리위 포지션S
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
                        else if (hit.transform.tag == "Instructor")     // 강사 인식
                        {
                            Debug.Log("강사입니다");
                            NowClickCharacterTag = hit.transform.tag;

                            GameObject character = hit.transform.gameObject;        // 레이캐스트 맞은 캐릭터
                            var CPositon = character.transform.position;        // 캐릭터의 머리위 포지션

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
            else if (GameTime.Instance.IsGameMode == true && isPopUpCharacterNamePlate == true)           // 캐릭터 이름창 띄움
            {
                if (nowClick != null)
                {
                    if (nowClick.name == CharacterNamePlate.InfoButton.name)
                    {
                        Debug.Log("캐릭터정보창 클릭? > 창띄우기");

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
                            Debug.Log("학생입니다");
                            NowClickCharacterTag = hit.transform.tag;

                            GameObject character = hit.transform.gameObject;        // 레이캐스트 맞은 캐릭터
                            var CPositon = character.transform.position;        // 캐릭터의 머리위 포지션
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
                        else if (hit.transform.tag == "Instructor")     // 강사 인식
                        {
                            Debug.Log("강사입니다");
                            NowClickCharacterTag = hit.transform.tag;

                            GameObject character = hit.transform.gameObject;        // 레이캐스트 맞은 캐릭터
                            var CPositon = character.transform.position;        // 캐릭터의 머리위 포지션

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
                        Debug.Log("머리위 유아이 끄기");
                        Camera.main.GetComponent<TestCamera>().ResetCameraPosition();

                        if (CharacterNamePlate.transform.gameObject.activeSelf == true)
                        {
                            CharacterNamePlate.gameObject.SetActive(false);
                        }

                    }
                }
            }
            else if (GameTime.Instance.IsGameMode == false && isPopUpCharacterNamePlate == true)     // 캐릭터 이름표 / 캐릭터정보창 둘 다 떠있을 때
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
                                    InstructorInfoPage.gameObject.SetActive(false);       // 강사 구분해서 하기
                                }
                                break;
                        }
                        Time.timeScale = InGameUI.Instance.m_NowGameSpeed; ;
                        if (InGameUI.Instance.UIStack != null)
                        {
                            InGameUI.Instance.UIStack.Pop();
                            Debug.Log("팝업 창 띄여진 갯수 : " + InGameUI.Instance.UIStack.Count);
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
        // Debug.Log("안드로이드 디버그 확인용");
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Debug.Log("터치 중일 때 들어오는가");
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Debug.Log("손가락을 뗄 때 들어오는가");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);       // 스크린 좌표 기준 ray 쏴서 오브젝트와 충돌, 스크린 좌표계 -> 월드스페이스 좌표계
            RaycastHit hit;     // 레이캐스트 맞을 물체
            Touch touch = Input.GetTouch(0);

            Vector3 touchPosToVector3 = new Vector3(touch.position.x, touch.position.y);    // 터치한 위치 받기
            ray = Camera.main.ScreenPointToRay(touchPosToVector3);                          // 월드좌표계를 

            if (GameTime.Instance.IsGameMode == true && isPopUpCharacterNamePlate == false)         // 캐릭터 이름표 / 캐릭터정보창 둘 다 안떠있을 때
            {
                isPopUpCharacterNamePlate = true;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask & m_layerMask))          //레이캐스트 맞았다?
                {
                    Debug.Log("오브젝트의 태그 ? : " + hit.transform.name);

                    if (!EventSystem.current.IsPointerOverGameObject(0))      // raycast가 UI 를 만나지 않을 때 (UI 부딪히면 true 반환)
                    {
                        Debug.Log("학생인포 뜨기");

                        if (hit.transform.tag == "Student")
                        {
                            Debug.Log("학생입니다");

                            //Time.timeScale = 0;     // 시간정지

                            GameObject character = hit.transform.gameObject;        // 레이캐스트 맞은 캐릭터
                            var CPositon = character.transform.position;        // 캐릭터의 머리위 포지션
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
                        else if (hit.transform.tag == "Instructor")     // 강사 인식
                        {
                            Debug.Log("강사입니다");

                            GameObject character = hit.transform.gameObject;        // 레이캐스트 맞은 캐릭터
                            var CPositon = character.transform.position;        // 캐릭터의 머리위 포지션

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
                        Debug.Log("캐릭터정보창 클릭? > 창띄우기");

                        PopUpCharacterInfo(nowClickCharacter);
                    }
                }
                else
                {
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask & m_layerMask))
                    {
                        if (hit.transform.tag == "Student")
                        {
                            Debug.Log("학생입니다");

                            //Time.timeScale = 0;     // 시간정지

                            GameObject character = hit.transform.gameObject;        // 레이캐스트 맞은 캐릭터
                            var CPositon = character.transform.position;        // 캐릭터의 머리위 포지션
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
                        else if (hit.transform.tag == "Instructor")     // 강사 인식
                        {
                            Debug.Log("강사입니다");

                            GameObject character = hit.transform.gameObject;        // 레이캐스트 맞은 캐릭터
                            var CPositon = character.transform.position;        // 캐릭터의 머리위 포지션

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
                        Debug.Log("머리위 유아이 끄기");
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
                                    InstructorInfoPage.gameObject.SetActive(false);       // 강사 구분해서 하기
                                }
                                break;
                        }

                        Time.timeScale = InGameUI.Instance.m_NowGameSpeed;
                        if (InGameUI.Instance.UIStack != null)
                        {
                            InGameUI.Instance.UIStack.Pop();
                            Debug.Log("팝업 창 띄여진 갯수 : " + InGameUI.Instance.UIStack.Count);
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

            // 인게임씬에서 꺼줘야 하는 친구들 이거저거 추가하기
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
