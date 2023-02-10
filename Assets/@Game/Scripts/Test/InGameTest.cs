using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Linq;
using StatData.Runtime;
using Conditiondata.Runtime;

public enum ClassState
{
    nothing,
    ClassStart,
    ClassEnd,
    StudyStart,
    Studying
}

/// <summary>
/// 인게임에서 돌아갈 테스트 클래스. 게임 매니저와 같은 느낌의 스크립트이다.
/// 현재는 싱글톤으로 만들었고 학생을 만드려면 버튼을 누르고 그렇게 만들어진
/// 학생의 정보를 리스트화 시켜 보여주는 버튼도 있다.
/// 
/// 2022.11.04
/// </summary>
/// 

public class InGameTest : MonoBehaviour
{
    private static InGameTest _instance = null;

    [SerializeField]
    GameObject _testStudentStat;
    [SerializeField]
    GameObject _studentStatBox;
    [SerializeField]
    Button _studentInfoButton;
    [SerializeField]
    Button _classInfoBox;
    [SerializeField]
    GameObject _testStudentInfo;
    [SerializeField]
    GameObject _testClassInfo;
    [SerializeField]
    GameObject _cancleClassInfoButton;
    [SerializeField]
    GameObject _startClassButton;
    [SerializeField]
    private ClassController _classInfo;
    [SerializeField]
    private StatController _statController;

    [SerializeField] private ClassPrefab _classPrefab;
    [SerializeField] private PopOffUI _popOffClassPanel;
    [SerializeField] private Button m_ChangeColorArtButton;
    [SerializeField] private Button m_ChangeColorProductManagerButton;
    [SerializeField] private Button m_ChangeColorProgrammingButton;

    public bool _isRepeatClass = false;                     // 수업 선택 후 3달동안 수업을 실행시키기 위해
    public int _classCount = 1;                             // 수업을 총 3번만 들을 수 있게 해주기 위한 

    private List<GameObject> _studentInfoList = new List<GameObject>();
    private List<GameObject> _SelectStudentList = new List<GameObject>();
    private List<Button> _classInfoList = new List<Button>();

    private Dictionary<string, Class> _CheckClass = new Dictionary<string, Class>();
    private List<Student> _startClassStudent = new List<Student>();
    private List<StatModifier> _startClassMagnitude = new List<StatModifier>();

    public List<string> _interactionScript;

    public ClassState m_ClassState = ClassState.nothing;

    public bool _isSelectClassNotNull = false;


    public static InGameTest Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }

    private void Awake()
    {
        m_ClassState = ClassState.nothing;

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(m_ClassState == ClassState.ClassStart)
        {
            foreach (var student in ObjectManager.Instance.m_StudentList)
            {
                // isArrivedClass가 false이면 아직 학생들이 도착을 안했다는 뜻.
                if (student.m_IsArrivedClass == false)
                {
                    m_ClassState = ClassState.ClassStart;
                    break;
                }

                m_ClassState = ClassState.StudyStart;
            }
        }
        
        if(m_ClassState == ClassState.StudyStart)
        {
            StartStudy();
        }

        if(m_ClassState == ClassState.Studying)
        {
            if (GameTime.Instance.FlowTime.NowWeek == 3)
            {
                EndClass();
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            m_ClassState = ClassState.ClassStart;

            _isRepeatClass = true;

            foreach (var student in ObjectManager.Instance.m_StudentBehaviorList)
            {
                student.GetComponent<Student>().m_IsDesSetting = false;
            }
            _popOffClassPanel.TurnOffUI();

            ClassSchedule.Instance.ChangeColorButton(m_ChangeColorArtButton, Color.white);
            ClassSchedule.Instance.ChangeColorButton(m_ChangeColorProductManagerButton, Color.white);
            ClassSchedule.Instance.ChangeColorButton(m_ChangeColorProgrammingButton, Color.white);
        }
    }

    #region _안쓰는 코드들
    // 버튼을 눌렀을 때 캐릭터가 생성이 되게 해보기
    //public void ClickButtonTest()
    //{
    //    ObjectManager.Instance.CreateStudent();
    //}

    //// 만들어진 학생들의 정보를 UI로 동적 생성해주는 함수
    //public void CreateStudentInfo()
    //{
    //    _testStudentStat.SetActive(true);
    //    _studentInfoButton.interactable = false;

    //    for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
    //    {
    //        var newStatsBox = Instantiate(_studentStatBox, new Vector3(0, 0, 0), Quaternion.identity);
    //        newStatsBox.transform.SetParent(GameObject.Find("Content").transform);
    //        _studentInfoList.Add(newStatsBox);
    //    }

    //    CheckStudentInfo();
    //}
    //// 현재 보유하고있는 수업의 정보를 UI로 동적 생성해주는 함수
    //public void CreateClassInfo()
    //{
    //    _testClassInfo.SetActive(true);
    //    _testStudentInfo.SetActive(true);
    //    _cancleClassInfoButton.SetActive(true);
    //    _startClassButton.SetActive(true);

    //    for (int i = 0; i < _classInfo.classData.Count; i++)
    //    {
    //        var newClassBox = Instantiate(_classInfoBox);
    //        newClassBox.transform.SetParent(GameObject.Find("ClassInfoContent").transform);
    //        newClassBox.onClick.AddListener(ClickClass);
    //        _classInfoList.Add(newClassBox);
    //    }

    //    ClickStudy();
    //}

    //// UI에 학생들의 정보를 채워주는 함수
    //public void CheckStudentInfo()
    //{
    //    if (_studentInfoList.Count == 0)
    //    {
    //        return;
    //    }

    //    for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
    //    {
    //        StudentStat _stat = ObjectManager.Instance.m_StudentList[i].m_StudentData;
    //        StudentCondition _condition = ObjectManager.Instance.m_StudentList[i].m_StudentCondition;
    //        Student.Doing _doing = ObjectManager.Instance.m_StudentList[i].m_Doing;
    //        StudentType _type = ObjectManager.Instance.m_StudentList[i].m_StudentData.m_StudentType;

    //        _studentInfoList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ObjectManager.Instance.m_StudentList[i].m_NameStudent;
    //        //_studentInfoList[i].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _stat.m_StudentSystemValue.ToString();
    //        //_studentInfoList[i].transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _stat.m_StudentContentsValue.ToString();
    //        //_studentInfoList[i].transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = _stat.m_StudentBalanceValue.ToString();
    //        _studentInfoList[i].transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = _condition.m_StudentHungryValue.ToString();
    //        _studentInfoList[i].transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = _condition.m_StudentTiredValue.ToString();
    //        _studentInfoList[i].transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = _doing.ToString();
    //        _studentInfoList[i].transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().text = _type.ToString();
    //    }
    //}

    //// 수업을 선택했을 때 해당하는 타입의 학생들을 모두 불러와서 화면에 띄워주는 함수
    //public void SelectClassAndStudent()
    //{
    //    if (_SelectStudentList.Count == 0)
    //    {
    //        return;
    //    }

    //    for (int i = 0; i < _SelectStudentList.Count; i++)
    //    {
    //        StudentStat _stat = _startClassStudent[i].m_StudentData;
    //        StudentCondition _condition = _startClassStudent[i].m_StudentCondition;
    //        Student.Doing _doing = _startClassStudent[i].m_Doing;
    //        StudentType _type = _startClassStudent[i].m_StudentData.m_StudentType;

    //        _SelectStudentList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _startClassStudent[i].m_NameStudent;
    //        //_SelectStudentList[i].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _stat.m_StudentSystemValue.ToString();
    //        //_SelectStudentList[i].transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _stat.m_StudentContentsValue.ToString();
    //        //_SelectStudentList[i].transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = _stat.m_StudentBalanceValue.ToString();
    //        _SelectStudentList[i].transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = _condition.m_StudentHungryValue.ToString();
    //        _SelectStudentList[i].transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = _condition.m_StudentTiredValue.ToString();
    //        _SelectStudentList[i].transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = _doing.ToString();
    //        _SelectStudentList[i].transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().text = _type.ToString();
    //    }
    //}

    //// 학생들의 정보를 본 후 Cancle버튼을 누르면 리스트의 정보를 지워주는 함수
    //public void ClickCancle()
    //{
    //    _testStudentStat.SetActive(false);
    //    _studentInfoButton.interactable = true;

    //    for (int i = 0; i < _studentInfoList.Count; i++)
    //    {
    //        Destroy(_studentInfoList[i]);
    //    }
    //    _studentInfoList.Clear();
    //}

    //// ClassInfo버튼을 누르면 해당하는 수업의 이름을 UI에 띄워주는 함수
    //public void ClickStudy()
    //{
    //    if (_classInfoList.Count == 0)
    //    {
    //        return;
    //    }

    //    for (int i = 0; i < _classInfo.classData.Count; i++)
    //    {
    //        Class _class = _classInfo.classData.ElementAt(i).Value;
    //        _CheckClass.Add(_class.m_ClassName, _class);
    //        _classInfoList[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = _class.m_ClassName;
    //    }

    //}

    //// 수업을 클릭했을 때 속성에 따라 해당하는 학생들을 리스트에 보여주기 위한 함수
    //public void ClickClass()
    //{
    //    GameObject _clickObject = EventSystem.current.currentSelectedGameObject; // 방금 클릭한 UI의 정보와 이름을 가져오기 위한 이벤트 함수

    //    Class _class = _CheckClass[_clickObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text];

    //    StatModifier statModifier;

    //    if (_SelectStudentList.Count != 0)
    //    {
    //        for (int i = 0; i < _SelectStudentList.Count; i++)
    //        {
    //            Destroy(_SelectStudentList[i]);
    //        }
    //        _SelectStudentList.Clear();
    //        _startClassMagnitude.Clear();
    //        _startClassStudent.Clear();
    //    }

    //    switch (_class.m_ClassType)
    //    {
    //        case StudentType.Art:
    //        {
    //            for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
    //            {
    //                if (ObjectManager.Instance.m_StudentList[i].m_StudentData.m_StudentType == StudentType.Art)
    //                {
    //                    var newStatsBox = Instantiate(_studentStatBox, new Vector3(0, 0, 0), Quaternion.identity);
    //                    newStatsBox.transform.SetParent(GameObject.Find("StudentContent").transform);
    //                    _SelectStudentList.Add(newStatsBox);
    //                    _startClassStudent.Add(ObjectManager.Instance.m_StudentList[i]);

    //                    statModifier = new StatModifier();

    //                    statModifier.StatsModifierInfo[ModifierStatType.System] = _CheckClass[_class.m_ClassName].m_ClassSystemValue;
    //                    statModifier.StatsModifierInfo[ModifierStatType.Contents] = _CheckClass[_class.m_ClassName].m_ClassContentsValue;
    //                    statModifier.StatsModifierInfo[ModifierStatType.Balance] = _CheckClass[_class.m_ClassName].m_ClassBalanceValue;

    //                    _startClassMagnitude.Add(statModifier);
    //                }
    //            }
    //            SelectClassAndStudent();

    //            Debug.Log("아트");
    //        }
    //        break;

    //        case StudentType.GameDesigner:
    //        {
    //            for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
    //            {
    //                if (ObjectManager.Instance.m_StudentList[i].m_StudentData.m_StudentType == StudentType.GameDesigner)
    //                {
    //                    var newStatsBox = Instantiate(_studentStatBox, new Vector3(0, 0, 0), Quaternion.identity);
    //                    newStatsBox.transform.SetParent(GameObject.Find("StudentContent").transform);
    //                    _SelectStudentList.Add(newStatsBox);
    //                    _startClassStudent.Add(ObjectManager.Instance.m_StudentList[i]);

    //                    statModifier = new StatModifier();

    //                    statModifier.StatsModifierInfo[ModifierStatType.System] = _CheckClass[_class.m_ClassName].m_ClassSystemValue;
    //                    statModifier.StatsModifierInfo[ModifierStatType.Contents] = _CheckClass[_class.m_ClassName].m_ClassContentsValue;
    //                    statModifier.StatsModifierInfo[ModifierStatType.Balance] = _CheckClass[_class.m_ClassName].m_ClassBalanceValue;

    //                    _startClassMagnitude.Add(statModifier);
    //                }
    //            }
    //            SelectClassAndStudent();

    //            Debug.Log("기획");

    //        }
    //        break;

    //        case StudentType.Programming:
    //        {
    //            for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
    //            {
    //                if (ObjectManager.Instance.m_StudentList[i].m_StudentData.m_StudentType == StudentType.Programming)
    //                {
    //                    var newStatsBox = Instantiate(_studentStatBox, new Vector3(0, 0, 0), Quaternion.identity);
    //                    newStatsBox.transform.SetParent(GameObject.Find("StudentContent").transform);
    //                    _SelectStudentList.Add(newStatsBox);
    //                    _startClassStudent.Add(ObjectManager.Instance.m_StudentList[i]);

    //                    statModifier = new StatModifier();

    //                    statModifier.StatsModifierInfo[ModifierStatType.System] = _CheckClass[_class.m_ClassName].m_ClassSystemValue;
    //                    statModifier.StatsModifierInfo[ModifierStatType.Contents] = _CheckClass[_class.m_ClassName].m_ClassContentsValue;
    //                    statModifier.StatsModifierInfo[ModifierStatType.Balance] = _CheckClass[_class.m_ClassName].m_ClassBalanceValue;

    //                    _startClassMagnitude.Add(statModifier);
    //                }
    //            }
    //            SelectClassAndStudent();

    //            Debug.Log("프로그래밍");
    //        }
    //        break;
    //    }
    //}

    #endregion

    // 버튼을 누르면 3달치의 수업을 미리 저장하여 매 달 첫째 주에 m_ClassState가 ClassStart가 될 수 있도록 해주기.
    public void StarClass()
    {
        _isSelectClassNotNull = false;

        for (int i = 0; i < 3; i++)
        {
            if (_classPrefab.m_ArtData[i].m_ClassName == null ||
                _classPrefab.m_ProductManagerData[i].m_ClassName == null ||
                _classPrefab.m_ProgrammingData[i].m_ClassName == null)
            {
                _isSelectClassNotNull = false;
                break;
            }
            else
            {
                _isSelectClassNotNull = true;
            }
        }

        if (_isSelectClassNotNull == true)
        {
            m_ClassState = ClassState.ClassStart;

            _isRepeatClass = true;

            foreach (var student in ObjectManager.Instance.m_StudentBehaviorList)
            {
                student.GetComponent<Student>().m_IsDesSetting = false;
            }
            _popOffClassPanel.TurnOffUI();

            ClassSchedule.Instance.ChangeColorButton(m_ChangeColorArtButton, Color.white);
            ClassSchedule.Instance.ChangeColorButton(m_ChangeColorProductManagerButton, Color.white);
            ClassSchedule.Instance.ChangeColorButton(m_ChangeColorProgrammingButton, Color.white);
        }

    }

    // 수업 선택이 끝나고 다음달이 되면 실행시켜줄 함수.
    public void NextClassStart()
    {
        m_ClassState = ClassState.ClassStart;

        foreach (var student in ObjectManager.Instance.m_StudentBehaviorList)
        {
            student.GetComponent<Student>().m_IsDesSetting = false;
        }
    }

    void StartStudy()
    {
        m_ClassState = ClassState.Studying;
    }

    // 수업이 끝나면 상태를 수업 종료로 바꿔주고 모든 학생들의 상태는 FreeWakl가 가능한 상태로 만들어준다.
    // 여기서 학생들의 스탯을 수업들은 만큼 올려준다.
    void EndClass()
    {
        m_ClassState = ClassState.ClassEnd;
        foreach (var student in ObjectManager.Instance.m_StudentList)
        {
            student.m_IsArrivedClass = false;
            student.m_IsDesSetting = false;
            student.m_IsInteracting = false;
            student.GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        }

        // 수업을 3번 반복했으니 이제 다시 수업 셋팅을 할 수 있게 초기화해준다.
        if (_classCount == 3)
        {
            _classCount = 1;
            _isRepeatClass = false;
        }

        Invoke("StateInit", 4f);
    }

    // 방금 들은 수업의 정보를 지워준다.
    void StateInit()
    {
        m_ClassState = ClassState.nothing;
    }

}


