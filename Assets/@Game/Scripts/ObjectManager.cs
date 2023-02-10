using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatData.Runtime;
using Newtonsoft.Json;
using BehaviorDesigner.Runtime;

/// <summary>
/// 학생과 교수의 생성, 삭제를 관리해주는 스크립트
/// 
/// 원본은 프리팹으로 리소스로서 존재하며, 끌어넣기를 썼다.
/// 
/// 2022. 10. 31 Ocean
/// </summary>
/// 

public class StudentNameListClass
{
    public List<string> _studentName;
}

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager _instance = null;

    [SerializeField] private StatController m_StatController;
    [SerializeField] private ConditionController m_conditionData;
    [SerializeField] private List<GameObject> m_CharacterPartsHair = new List<GameObject>();
    [SerializeField] private List<GameObject> m_CharacterPartsTop = new List<GameObject>();
    [SerializeField] private List<GameObject> m_CharacterPartsBottom = new List<GameObject>();
    [SerializeField] private ExternalBehaviorTree studentTree;

    public GameObject StudentOriginal;    /// 원본으로 사용할 프리팹(을 받는 GameObject)

    public List<Student> m_StudentList = new List<Student>();
    public List<GameObject> m_StudentBehaviorList = new List<GameObject>();
    private List<string> m_tempname = new List<string>();

    public Dictionary<string, GameObject> _programmingSeatDic = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> _ProductManagerSeatDic = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> _artSeatDic = new Dictionary<string, GameObject>();

    public static ObjectManager Instance
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
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        m_tempname.Add("김영희");
        m_tempname.Add("이철수");
        m_tempname.Add("염미경");
        m_tempname.Add("강상연");
    }

    private void Start()
    {
        Debug.LogWarning("오브젝트");
        //var _testJson = JsonConvert.DeserializeObject<List<string>>(Resources.Load<TextAsset>("Json/Name").ToString());

       // StudentNameListClass _newName = JsonConvert.DeserializeObject<StudentNameListClass>(Resources.Load<TextAsset>("Json/Name").ToString());

        //m_tempname.Add(_newName._studentName);
        //m_tempname = JsonConvert.DeserializeObject<List<StudentNameListClass>>(Resources.Load<TextAsset>("Json/Name").ToString());

        CreateStudent(0);
        CreateStudent(1);
        //CreateStudent(2);
        //CreateStudent(3);
        //CreateStudent(0);
        //CreateStudent(1);
    }

    // 학생 오브젝트를 필요할 때 동적으로 생성해주기 위한 함수
    // 랜덤한 숫자를 가져와서 데이터베이스에 있는 학생의 정보를 무작위로 가져온다
    // 학생을 생성할 때 파츠를 오브젝트로 생성하는게 아니라 Mesh를 바꿔주는걸로 해주기
    public void CreateStudent(int i)
    {
        /// S. Monobehavior를 사용한 녀석들은 new를 하면 안된다.
        // GameObject student = new GameObject();

        /// Instantiate를 하는 방법
        /// (사본으로부터 원본을 만든다.)
        /// 1. 리소스화 된 프리팹을 특정 오브젝트로 끌어넣고, 그것을 원본으로 사용한다.
        //GameObject _newStudentObject = GameObject.Instantiate(StudentOriginal) as GameObject;   // 범용적인 함수
        GameObject _newStudentObject = InGameObjectPool.GetStudentObject(transform);                                                                                      //GameObject _newStudentObjec2 = GameObject.Instantiate(StudentOriginal);                 // 오리지널이 게임오브젝트일 때
                                                                                                                                                                              //GameObject _newStudentObject3 = GameObject.Instantiate<GameObject>(StudentOriginal);    // 타입을 Generic으로 지정해주는 버전
        #region _프리팹을 동적으로 만드는 방법 
        /// (사본으로부터 원본을 만든다.)
        /// 2. 리소스화 된 프리팹을 리소스 폴더로부터 바로 로드해서, 그것을 원본으로 사용한다.
        //GameObject _newStudentObject = GameObject.Instantiate(Resources.Load("Student")) as GameObject;

        /// (아무것도 없는 상태에서, 하나하나 다 만들고 싶을 때)
        /// 3. GameObject를 하나 만들고, 컴포넌트들을 하나하나 만들어서 붙인다.
        //GameObject _newStudentObject2 = new GameObject();
        //_newStudentObject2.AddComponent<Student>();
        //_newStudentObject2.AddComponent<MeshFilter>();
        //MeshFilter _newFilter = _newStudentObject2.GetComponent<MeshFilter>();
        //_newFilter.mesh = new Mesh();

        // (참고) 컴포넌트만 가져오고 싶은 경우
        //Student _student2 = StudentOriginal.GetComponent<Student>();
        //Student _newStudentComponent = GameObject.Instantiate(_student2);   // 범용적인 함수
        // 캐릭터를 생성할 때 헤어랑 옷을 랜덤으로 만들어준다.
        //int _hairNum = Random.Range(0, m_CharacterPartsHair.Count);
        //int _topNum = Random.Range(0, m_CharacterPartsTop.Count);
        //int _bottomNum = Random.Range(0, m_CharacterPartsBottom.Count);

        // 머리카락과 옷을 생성할 때 부모를 _newStudentObject로 설정해준다.
        //GameObject.Instantiate(m_CharacterPartsHair[_hairNum], _newStudentObject.transform.GetChild(0).transform);
        //GameObject.Instantiate(m_CharacterPartsTop[_topNum], _newStudentObject.transform.GetChild(0).transform);
        //GameObject.Instantiate(m_CharacterPartsBottom[_bottomNum], _newStudentObject.transform.GetChild(0).transform);

        #endregion

        // 엔티티로부터 스크립트를 얻어낸다
        Student _student = _newStudentObject.GetComponent<Student>();

        // 그 스크립트로부터 이런 저런 처리를 한다.

        // 처음 생성 시 3명이니 3,3 근데 이건 학생 생성 완료후로 넘겨야 할 듯.
        
        // 랜덤한 int값이 들어가는 스탯들
        StudentStat _studentStat = new StudentStat();
        
        RandomStat(_studentStat,i);
        _studentStat.m_Skills = null;
        _studentStat.m_StudentType = StudentType.Art;
        string _studentName = _studentStat.m_StudentName;

        //StudentCondition _studentCondition = new StudentCondition(m_conditionData.dataBase.studentCondition[0]);

        _student.Initialize(_studentStat, _studentName);

        // 학생에 BT컴포넌트를 붙여준다
        _newStudentObject.GetComponent<BehaviorTree>().StartWhenEnabled = true;
        _newStudentObject.GetComponent<BehaviorTree>().PauseWhenDisabled = true;
        _newStudentObject.GetComponent<BehaviorTree>().RestartWhenComplete = true;

        _newStudentObject.GetComponent<BehaviorTree>().DisableBehavior();

        ExternalBehavior studentTreeInstance = Instantiate(studentTree);
        studentTreeInstance.Init();

        // 학생들의 변수값 설정해주기
        studentTreeInstance.SetVariableValue("Student", _newStudentObject);

        string _seatName;
        string _clssName;

        if (_student.m_StudentData.m_StudentType == StudentType.Art)
        {
            _clssName = "CheckArtClass";
            studentTreeInstance.SetVariableValue("ClassEntrance", _clssName);
            _seatName = string.Format("ArtFixedSeat{0}", _artSeatDic.Count + 1);
            studentTreeInstance.SetVariableValue("ClassSeat", _seatName);
            _artSeatDic.Add("ArtFixedSeat" + _artSeatDic.Count + 1, _newStudentObject);
        }
        else if (_student.m_StudentData.m_StudentType == StudentType.GameDesigner)
        {
            _clssName = "CheckProductManagerClass";
            studentTreeInstance.SetVariableValue("ClassEntrance", _clssName);
            _seatName = string.Format("ProductManagerFixedSeat{0}", _ProductManagerSeatDic.Count + 1);
            studentTreeInstance.SetVariableValue("ClassSeat", _seatName);
            _ProductManagerSeatDic.Add("ProductManagerFixedSeat" + _ProductManagerSeatDic.Count + 1, _newStudentObject);
        }
        else if (_student.m_StudentData.m_StudentType == StudentType.Programming)
        {
            _clssName = "CheckProgrammingClass";
            studentTreeInstance.SetVariableValue("ClassEntrance", _clssName);
            _seatName = string.Format("ProgrammingFixedSeat{0}", _programmingSeatDic.Count + 1);
            studentTreeInstance.SetVariableValue("ClassSeat", _seatName);
            _programmingSeatDic.Add("ProgrammingFixedSeat" + _programmingSeatDic.Count + 1, _newStudentObject);
        }

        studentTreeInstance.SetVariableValue("FreeWalk1", "FreeWalk1");
        studentTreeInstance.SetVariableValue("FreeWalk2", "FreeWalk2");
        studentTreeInstance.SetVariableValue("FreeWalk3", "FreeWalk3");
        studentTreeInstance.SetVariableValue("StudentTag", "Student");

        _newStudentObject.GetComponent<BehaviorTree>().ExternalBehavior = studentTreeInstance;
        _newStudentObject.GetComponent<BehaviorTree>().EnableBehavior();

        // 새로 만든 학생 오브젝트의 위치를 0으로 돌린다.
        _student.transform.position = new Vector3(0, 0, 0);

        // 하이어라키 상에서 부모를 지정해준다.
        _student.transform.parent = transform;

        // 만들어진 오브젝트를 특정 풀에 넣는다.
        m_StudentList.Add(_student);
        m_StudentBehaviorList.Add(_newStudentObject);
    }

    // 교수 오브젝트를 필요할 때 동적으로 생성해주기 위한 함수
    public void CreateProfessor()
    {

    }

    public void RandomStat(StudentStat _studentStat, int i)
    {
        // 이름은 나중에 바꿔줘야함.
        _studentStat.m_StudentName = m_tempname[i];
        _studentStat.m_Health = Random.Range(1, 101);
        _studentStat.m_Passion = Random.Range(1, 101);

        _studentStat.m_Sense = Random.Range(1, 101);
        _studentStat.m_Concentraction = Random.Range(1, 101);
        _studentStat.m_Wit = Random.Range(1, 101);
        _studentStat.m_Technique = Random.Range(1, 101);
        _studentStat.m_Insight = Random.Range(1, 101);

        _studentStat.m_Action = Random.Range(1, 101);
        _studentStat.m_Adventure = Random.Range(1, 101);
        _studentStat.m_Shooting = Random.Range(1, 101);
        _studentStat.m_RPG = Random.Range(1, 101);
        _studentStat.m_Puzzle = Random.Range(1, 101);
        _studentStat.m_Rythm = Random.Range(1, 101);
        _studentStat.m_Sport = Random.Range(1, 101);
    }

    //public Node CreateNode(Student _student)
    //{
    //    // 최상단 노드
    //    Selector _root = new Selector();

    //    // 최상단 노드에 들어갈 Composite노드들
    //    Sequnce _studySeq = new Sequnce();
    //    Sequnce _goSeq = new Sequnce();
    //    Sequnce _eatSeq = new Sequnce();
    //    Sequnce _restSeq = new Sequnce();
    //    Sequnce _idelSeq = new Sequnce();
    //    //Sequnce _freeWalkSeq = new Sequnce();

    //    // 학생들의 행동을 담당할 노드들
    //    Studing _studingLeaf = new Studing();
    //    _studingLeaf.m_MyStudent = _student;

    //    GotoRestaurant _gotoRestourantLeaf = new GotoRestaurant();
    //    _gotoRestourantLeaf.m_MyStudent = _student;

    //    Eat _eatLeaf = new Eat();
    //    _eatLeaf.m_MyStudent = _student;

    //    Rest _restLeaf = new Rest();
    //    _restLeaf.m_MyStudent = _student;


    //    Idel _idelLeaf = new Idel();
    //    _idelLeaf.m_MyStudent = _student;

    //    //FreeWalk _freeWalkLeaf = new FreeWalk();
    //    //_freeWalkLeaf.m_MyStudent = _student;

    //    _studySeq.AddChildNode(_studingLeaf);

    //    _goSeq.AddChildNode(_gotoRestourantLeaf);

    //    _eatSeq.AddChildNode(_eatLeaf);

    //    _restSeq.AddChildNode(_restLeaf);

    //    _idelSeq.AddChildNode(_idelLeaf);
    //    //_freeWalkSeq.AddChildNode(_freeWalkLeaf);

    //    _root.AddChildNode(_studySeq);
    //    _root.AddChildNode(_goSeq);
    //    _root.AddChildNode(_eatSeq);
    //    _root.AddChildNode(_restSeq);
    //    _root.AddChildNode(_idelSeq);
    //    //_root.AddChildNode(_freeWalkSeq);

    //    return _root;
    //}
}
