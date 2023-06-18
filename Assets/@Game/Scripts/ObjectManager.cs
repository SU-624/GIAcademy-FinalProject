using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StatData.Runtime;
using Newtonsoft.Json;
using BehaviorDesigner.Runtime;
using System.Linq;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

/// <summary>
/// �л��� ������ ����, ������ �������ִ� ��ũ��Ʈ
/// 
/// ������ ���������� ���ҽ��μ� �����ϸ�, ����ֱ⸦ ���.
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

    [SerializeField] private ExternalBehaviorTree studentTree;

    public List<Student> m_StudentList = new List<Student>(); // ���� �п��� �ִ� �л� ����Ʈ
    public List<Instructor> m_InstructorList = new List<Instructor>(); // ���� �п��� ���� ����Ʈ
    public List<GameObject> m_StudentBehaviorList = new List<GameObject>();
    public Professor nowProfessor = new Professor(); // ���� ���� ����ϰ� �ִ� �����
    private List<string> m_OriginalFeMaleName = new List<string>();
    private List<string> m_OriginalMaleName = new List<string>();

    private List<string> m_FeMaleName = new List<string>();
    private List<string> m_MaleName = new List<string>();

    private List<StudentRandomStatRange> m_StudentRandomStatRangeList = new List<StudentRandomStatRange>();
    private List<StudentPersonality> m_StudentPersonality = new List<StudentPersonality>();
    private List<StudentBasicSkills> m_SkillRangeList = new List<StudentBasicSkills>();

    public List<List<int>> m_Friendship = new List<List<int>>(); // ��� �л��� ������ ģ�е� (�л�18 ����3)

    // �� ���� �л� �� �ľ�
    private int m_nArt;
    private int m_nProgramming;
    private int m_nProductManager;

    // ���� �����Ǿ��ִ� ���� ������Ʈ(���Ƿ� ������ ���� �Ϸ� ����)
    [SerializeField] private GameObject Professor1;
    [SerializeField] private GameObject Professor2;

    [SerializeField] private GameObject Professor3;
    // ��� ���� ������ ���� �Ŵϱ� �̰Ŵ� ����Ʈ<professorStat>
    // [SerializeField] private List<ProfessorStat> AllProfessor = new List<ProfessorStat>();

    public Transform AcademyEntrance;

    private bool LoadStudentOnce = true;

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
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitStudentStatRangeByRank();
        InitStudentPersonality();
        InitFemaleAndMaleName();
        InitBasicSkills();

        m_InstructorList.Add(Professor1.GetComponent<Instructor>());
        m_InstructorList.Add(Professor2.GetComponent<Instructor>());
        m_InstructorList.Add(Professor3.GetComponent<Instructor>());
    }

    private void InitStudentStatRangeByRank()
    {
        m_StudentRandomStatRangeList.Add(new StudentRandomStatRange(Rank.SSS, 25, 37, 18, 26));
        m_StudentRandomStatRangeList.Add(new StudentRandomStatRange(Rank.SS, 22, 34, 15, 24));
        m_StudentRandomStatRangeList.Add(new StudentRandomStatRange(Rank.S, 19, 31, 13, 22));
        m_StudentRandomStatRangeList.Add(new StudentRandomStatRange(Rank.A, 16, 28, 11, 20));
        m_StudentRandomStatRangeList.Add(new StudentRandomStatRange(Rank.B, 13, 25, 9, 18));
        m_StudentRandomStatRangeList.Add(new StudentRandomStatRange(Rank.C, 10, 22, 7, 15));
        m_StudentRandomStatRangeList.Add(new StudentRandomStatRange(Rank.D, 7, 19, 5, 13));
        m_StudentRandomStatRangeList.Add(new StudentRandomStatRange(Rank.E, 4, 16, 3, 11));
        m_StudentRandomStatRangeList.Add(new StudentRandomStatRange(Rank.F, 1, 13, 1, 9));
    }

    private void InitStudentPersonality()
    {
        m_StudentPersonality.Add(new StudentPersonality(1, "���", "������ ����մϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(2, "�Ϻ�������", "¦¦�� �縻�� ������ 3�ϳ��� �Ǹ����ߴϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(3, "������", "����� ��ũ���׽����� ��θ� ������ �Ǳ�� �߽��ϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(4, "��¥", "1+1�� â���̶�� ���ϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(5, "����������", "MBTI�� �ȹϽ��ϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(6, "����������", "���ΰ� �޺��Ʒ��� ��罺�ߴ°� ���ϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(7, "����������", "�Ӹ��� �ɴ޾ҽ��ϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(8, "������ȣ��", "���� 168���� ������ Ű��� �ֽ��ϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(9, "������", "���Ѿ��̶�� �Ҹ��� ���� ����ϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(10, "�ھƵ���", "�� �ʹ��̻� �� �ʹ��߻���� �� �ְ��"));
        m_StudentPersonality.Add(new StudentPersonality(11, "�����", "���� �� ģ�� ������ ������� ���Դϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(12, "��������", "������ �ʹ� �����ѰŶ�� ���ϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(13, "�ν�", "ȥ�� ��� ȥ�� ��Դ°� ���ظ��մϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(14, "���� �н���", "�ϳ��� �˸� ���� ����Ĩ�ϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(15, "�����", "��� �������� �ϰ��ִ°���?"));
        m_StudentPersonality.Add(new StudentPersonality(16, "������", "�ֱٿ� �ξ� ����� �����ϰڴٰ� �����Դϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(17, "�߽ɰ�", "���������� ���Դϴ�."));
        m_StudentPersonality.Add(new StudentPersonality(18, "�˺�����", "������ �׸𳳴ϴ�. �𸣼̴ٰ��? �����ϱ��."));
        m_StudentPersonality.Add(new StudentPersonality(19, "������ ����Ŀ", "���ӿ� �ʼ� �����Դϴ�. �ٵ� ���ִ� ���ϴ�.."));
        m_StudentPersonality.Add(new StudentPersonality(20, "�Ĺ� ��ȣ��", "������ ���ٵ�ٰ� ��ȴµ� �����־��!"));
    }

    private void InitFemaleAndMaleName()
    {
        m_OriginalMaleName.Add("��ö��");
        m_OriginalMaleName.Add("����");
        m_OriginalMaleName.Add("��ȿ��");
        m_OriginalMaleName.Add("���ؼ�");
        m_OriginalMaleName.Add("�̱���");
        m_OriginalMaleName.Add("�ż���");
        m_OriginalMaleName.Add("���翵");
        m_OriginalMaleName.Add("��â��");
        m_OriginalMaleName.Add("������");

        m_OriginalFeMaleName.Add("���ֿ�");
        m_OriginalFeMaleName.Add("���̰�");
        m_OriginalFeMaleName.Add("��ȿ��");
        m_OriginalFeMaleName.Add("������");
        m_OriginalFeMaleName.Add("ȫ����");
        m_OriginalFeMaleName.Add("������");
        m_OriginalFeMaleName.Add("������");
        m_OriginalFeMaleName.Add("�����");
        m_OriginalFeMaleName.Add("�ڹ���");
        m_OriginalFeMaleName.Add("������");
        m_OriginalFeMaleName.Add("������");
        m_OriginalFeMaleName.Add("�ȼ���");
    }

    private void InitBasicSkills()
    {
        m_SkillRangeList.Add(new StudentBasicSkills(1, 1, 10));
        m_SkillRangeList.Add(new StudentBasicSkills(2, 11, 20));
        m_SkillRangeList.Add(new StudentBasicSkills(3, 21, 30));
        m_SkillRangeList.Add(new StudentBasicSkills(4, 31, 40));
        m_SkillRangeList.Add(new StudentBasicSkills(5, 41, 50));
    }

    public int CheckStatSkills(int _statValue)
    {
        foreach (StudentBasicSkills table in m_SkillRangeList)
        {
            if (_statValue >= table.MinStat && _statValue <= table.MaxStat)
            {
                return table.Level;
            }
        }

        return 0;
    }

    public void CreateAllStudent()
    {
        // 2������ ���� �����͸� �ҷ����� �ȵǱ� ������ ���� �ʿ�
        if (Json.Instance.IsSavedDataExists && LoadStudentOnce)
        {
            CreateLoadedStudent();
            LoadStudentOnce = false;
        }

        // �ε��� �����ϰų� ������ ������ �л��� ���ٸ� ��ư �����.
        if (m_StudentList.Count == 0)
        {
            m_MaleName.Clear();
            m_FeMaleName.Clear();

            m_MaleName = m_OriginalMaleName.ToList();
            m_FeMaleName = m_OriginalFeMaleName.ToList();

            m_nArt = 1;
            m_nProgramming = 1;
            m_nProductManager = 1;

            for (int i = 0; i < 2; i++)
            {
                int _nameIndex = Random.Range(0, m_MaleName.Count);

                CreateStudent(_nameIndex, StudentType.Art, 1);
                m_MaleName.RemoveAt(_nameIndex);
            }

            for (int i = 0; i < 4; i++)
            {
                int _nameIndex = Random.Range(0, m_FeMaleName.Count);

                CreateStudent(_nameIndex, StudentType.Art, 2);
                m_FeMaleName.RemoveAt(_nameIndex);
            }

            for (int i = 0; i < 3; i++)
            {
                int _nameIndex = Random.Range(0, m_MaleName.Count);

                CreateStudent(_nameIndex, StudentType.GameDesigner, 1);
                m_MaleName.RemoveAt(_nameIndex);
            }

            for (int i = 0; i < 3; i++)
            {
                int _nameIndex = Random.Range(0, m_FeMaleName.Count);

                CreateStudent(_nameIndex, StudentType.GameDesigner, 2);
                m_FeMaleName.RemoveAt(_nameIndex);
            }

            for (int i = 0; i < 4; i++)
            {
                int _nameIndex = Random.Range(0, m_MaleName.Count);

                CreateStudent(_nameIndex, StudentType.Programming, 1);
                m_MaleName.RemoveAt(_nameIndex);
            }

            for (int i = 0; i < 2; i++)
            {
                int _nameIndex = Random.Range(0, m_FeMaleName.Count);

                CreateStudent(_nameIndex, StudentType.Programming, 2);
                m_FeMaleName.RemoveAt(_nameIndex);
            }

            //for (int i = 6; i < 12; i++)
            //{
            //    CreateStudent(i, StudentType.GameDesigner);
            //}

            //for (int i = 12; i < 18; i++)
            //{
            //    CreateStudent(i, StudentType.Programming);
            //}

            m_Friendship.Clear();

            // �л�18, ����3 ģ�е� �ʱ�ȭ
            for (int i = 0; i < 18; i++)
            {
                List<int> studentFriendship = new List<int>();
                for (int j = 0; j < 21; j++)
                {
                    studentFriendship.Add(0);
                }

                m_Friendship.Add(studentFriendship);
            }
        }
    }

    //private void InitProfessor()
    //{
    //    for (int i = 0; i < m_SelectProfessor.professorData.Count; i++)
    //    {
    //        if (m_SelectProfessor.professorData.ElementAt(i).Value.m_ProfessorType == StudentType.Art)
    //        {
    //            m_NowPlayerProfessor.ArtProcessor.Add(m_SelectProfessor.professorData.ElementAt(i).Value);
    //        }

    //        if (m_SelectProfessor.professorData.ElementAt(i).Value.m_ProfessorType == StudentType.GameDesigner)
    //        {
    //            m_NowPlayerProfessor.GameManagerProcessor.Add(m_SelectProfessor.professorData.ElementAt(i).Value);
    //        }

    //        if (m_SelectProfessor.professorData.ElementAt(i).Value.m_ProfessorType == StudentType.Programming)
    //        {
    //            m_NowPlayerProfessor.ProgrammingProfessor.Add(m_SelectProfessor.professorData.ElementAt(i).Value);
    //        }

    //    }
    //}

    // �л� ������Ʈ�� �ʿ��� �� �������� �������ֱ� ���� �Լ�
    // ������ ���ڸ� �����ͼ� �����ͺ��̽��� �ִ� �л��� ������ �������� �����´�
    // �л��� ������ �� ������ ������Ʈ�� �����ϴ°� �ƴ϶� Mesh�� �ٲ��ִ°ɷ� ���ֱ�
    public void CreateStudent(int _nameIndex, StudentType _type, int _gender)
    {
        /// S. Monobehavior�� ����� �༮���� new�� �ϸ� �ȵȴ�.
        // GameObject student = new GameObject();
        GameObject _newStudentObject;
        /// Instantiate�� �ϴ� ���
        /// (�纻���κ��� ������ �����.)
        /// 1. ���ҽ�ȭ �� �������� Ư�� ������Ʈ�� ����ְ�, �װ��� �������� ����Ѵ�.
        //GameObject _newStudentObject = GameObject.Instantiate(StudentOriginal) as GameObject;   // �������� �Լ�

        // �𵨸��� �������ش�.
        if (_gender == 1)
        {
            _newStudentObject = InGameObjectPool.GetMaleStudentObject(transform);
        }
        else
        {
            _newStudentObject = InGameObjectPool.GetFemaleStudentObject(transform);
        }

        //GameObject _newStudentObjec2 = GameObject.Instantiate(StudentOriginal);                 // ���������� ���ӿ�����Ʈ�� ��
        //GameObject _newStudentObject3 = GameObject.Instantiate<GameObject>(StudentOriginal);    // Ÿ���� Generic���� �������ִ� ����

        #region _�������� �������� ����� ���

        /// (�纻���κ��� ������ �����.)
        /// 2. ���ҽ�ȭ �� �������� ���ҽ� �����κ��� �ٷ� �ε��ؼ�, �װ��� �������� ����Ѵ�.
        //GameObject _newStudentObject = GameObject.Instantiate(Resources.Load("Student")) as GameObject;

        /// (�ƹ��͵� ���� ���¿���, �ϳ��ϳ� �� ����� ���� ��)
        /// 3. GameObject�� �ϳ� �����, ������Ʈ���� �ϳ��ϳ� ���� ���δ�.
        //GameObject _newStudentObject2 = new GameObject();
        //_newStudentObject2.AddComponent<Student>();
        //_newStudentObject2.AddComponent<MeshFilter>();
        //MeshFilter _newFilter = _newStudentObject2.GetComponent<MeshFilter>();
        //_newFilter.mesh = new Mesh();

        // (����) ������Ʈ�� �������� ���� ���
        //Student _student2 = StudentOriginal.GetComponent<Student>();
        //Student _newStudentComponent = GameObject.Instantiate(_student2);   // �������� �Լ�
        // ĳ���͸� ������ �� ���� ���� �������� ������ش�.
        //int _hairNum = Random.Range(0, m_CharacterPartsHair.Count);
        //int _topNum = Random.Range(0, m_CharacterPartsTop.Count);
        //int _bottomNum = Random.Range(0, m_CharacterPartsBottom.Count);

        // �Ӹ�ī���� ���� ������ �� �θ� _newStudentObject�� �������ش�.
        //GameObject.Instantiate(m_CharacterPartsHair[_hairNum], _newStudentObject.transform.GetChild(0).transform);
        //GameObject.Instantiate(m_CharacterPartsTop[_topNum], _newStudentObject.transform.GetChild(0).transform);
        //GameObject.Instantiate(m_CharacterPartsBottom[_bottomNum], _newStudentObject.transform.GetChild(0).transform);

        #endregion

        // ��ƼƼ�κ��� ��ũ��Ʈ�� ����
        Student _student = _newStudentObject.GetComponent<Student>();

        // �� ��ũ��Ʈ�κ��� �̷� ���� ó���� �Ѵ�.

        // ó�� ���� �� 3���̴� 3,3 �ٵ� �̰� �л� ���� �Ϸ��ķ� �Ѱܾ� �� ��.

        StudentStat _studentStat = new StudentStat();

        // if (_loadData)
        // {
        //     _studentStat = _loadStat;
        // }
        // else
        {
            // ������ int���� ���� ���ȵ�
            RandomStat(_studentStat, _nameIndex, _gender);
            _studentStat.m_Skills = new List<string>();

            // �л����� �⺻ ��ų�� ���ȿ� �°� �ʱ�ȭ ���ش�.
            for (int i = 0; i < 5; i++)
            {
                _studentStat.m_AbilitySkills[i] = CheckStatSkills(_studentStat.m_AbilityAmountList[i]);
                if (_studentStat.m_AbilitySkills[i] <= 0)
                {
                    Debug.LogWarning("���Ƚ�ų �̻��� : " + _studentStat.m_AbilitySkills[i]);
                }
            }

            _studentStat.m_StudentType = _type;
            _studentStat.m_NumberOfEntries = 1;
            _studentStat.m_Gender = _gender;
            //StudentCondition _studentCondition = new StudentCondition(m_conditionData.dataBase.studentCondition[0]);
        }

        _student.Initialize(_studentStat);


        // �л��� BT������Ʈ�� �ٿ��ش�
        _newStudentObject.GetComponent<BehaviorTree>().StartWhenEnabled = true;
        _newStudentObject.GetComponent<BehaviorTree>().PauseWhenDisabled = true;
        _newStudentObject.GetComponent<BehaviorTree>().RestartWhenComplete = true;

        _newStudentObject.GetComponent<BehaviorTree>().DisableBehavior();

        ExternalBehavior studentTreeInstance = Instantiate(studentTree);
        studentTreeInstance.Init();

        if (_student.m_StudentStat.m_StudentType == StudentType.Art)
        {
            Vector3 classEntrance = GameObject.Find("CheckArtClass").transform.position;
            studentTreeInstance.SetVariableValue("MyClassEntrance", classEntrance);
            Vector3 classSeat = GameObject.Find("ArtFixedSeat" + m_nArt).transform.position;
            studentTreeInstance.SetVariableValue("MyClassSeat", classSeat);
            Transform SeatTransform = GameObject.Find("ArtFixedSeat" + m_nArt).transform;
            studentTreeInstance.SetVariableValue("SeatTransform", SeatTransform);
            m_nArt++;
        }
        else if (_student.m_StudentStat.m_StudentType == StudentType.GameDesigner)
        {
            Vector3 classEntrance = GameObject.Find("CheckProductManagerClass").transform.position;
            studentTreeInstance.SetVariableValue("MyClassEntrance", classEntrance);
            Vector3 classSeat = GameObject.Find("ProductManagerFixedSeat" + m_nProductManager).transform.position;
            studentTreeInstance.SetVariableValue("MyClassSeat", classSeat);
            Transform SeatTransform = GameObject.Find("ProductManagerFixedSeat" + m_nProductManager).transform;
            studentTreeInstance.SetVariableValue("SeatTransform", SeatTransform);
            m_nProductManager++;
        }
        else if (_student.m_StudentStat.m_StudentType == StudentType.Programming)
        {
            Vector3 classEntrance = GameObject.Find("CheckProgrammingClass").transform.position;
            studentTreeInstance.SetVariableValue("MyClassEntrance", classEntrance);
            Vector3 classSeat = GameObject.Find("ProgrammingFixedSeat" + m_nProgramming).transform.position;
            studentTreeInstance.SetVariableValue("MyClassSeat", classSeat);
            Transform SeatTransform = GameObject.Find("ProgrammingFixedSeat" + m_nProgramming).transform;
            studentTreeInstance.SetVariableValue("SeatTransform", SeatTransform);
            m_nProgramming++;
        }

        _newStudentObject.GetComponent<BehaviorTree>().ExternalBehavior = studentTreeInstance;
        _newStudentObject.GetComponent<BehaviorTree>().EnableBehavior();

        // ���� ���� �л� ������Ʈ�� ��ġ�� 0���� ������. �׺�Ž��� �������·� �ٲٸ� �ȹٲ�����.
        _newStudentObject.GetComponent<NavMeshAgent>().enabled = false;
        _newStudentObject.transform.position = AcademyEntrance.position;
        _newStudentObject.GetComponent<NavMeshAgent>().enabled = true;

        // ������� ������Ʈ�� Ư�� Ǯ�� �ִ´�.
        m_StudentList.Add(_student);
        m_StudentBehaviorList.Add(_newStudentObject);
    }

    // �ε��� �����ͷ� �л��� �����غ���.
    private void CreateLoadedStudent()
    {
        List<StudentStat> forTest = new List<StudentStat>();

        m_nArt = 1;
        m_nProgramming = 1;
        m_nProductManager = 1;

        for (int i = 0; i < 18; i++)
        {
            List<int> studentFriendship = new List<int>();
            m_Friendship.Add(studentFriendship);
        }

        // �ҷ��� �л����� �Ѹ��Ѹ� �����´�.
        foreach (var nowStudentData in AllInOneData.Instance.StudentData)
        {
            GameObject _newStudentObject;

            // ȣ���� ����

            List<int> studentFriendship = nowStudentData.Friendship;
            m_Friendship[nowStudentData.FriendshipIndex] = studentFriendship;


            // �𵨸��� �������ش�.
            if (nowStudentData.Gender == 1)
            {
                _newStudentObject = InGameObjectPool.GetMaleStudentObject(transform);
            }
            else
            {
                _newStudentObject = InGameObjectPool.GetFemaleStudentObject(transform);
            }

            // ��ƼƼ�κ��� ��ũ��Ʈ�� ����
            Student _newStudent = _newStudentObject.GetComponent<Student>();
            
            
            
            // ���� ���� �־��ֱ� ���� Reflection ����غ��Ҵ�!
            StudentStat _newStudentStat = new StudentStat();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var statFields = typeof(StudentStat).GetFields(flags);
            var saveFields = typeof(StudentSaveData).GetProperties(flags);

            foreach (var newStat in statFields)
            {
                foreach (var saveStat in saveFields)
                {
                    if (newStat.Name == "m_" + saveStat.Name)
                    {
                        var value = saveStat.GetValue(nowStudentData);
                        newStat.SetValue(_newStudentStat, value);
                        break;
                    }
                }
            }

            _newStudent.Initialize(_newStudentStat);

            _newStudent.DoingValue = Student.Doing.EndInteracting;

            // �л��� BT������Ʈ�� �ٿ��ش�
            _newStudentObject.GetComponent<BehaviorTree>().StartWhenEnabled = true;
            _newStudentObject.GetComponent<BehaviorTree>().PauseWhenDisabled = true;
            _newStudentObject.GetComponent<BehaviorTree>().RestartWhenComplete = true;

            _newStudentObject.GetComponent<BehaviorTree>().DisableBehavior();

            ExternalBehavior studentTreeInstance = Instantiate(studentTree);
            studentTreeInstance.Init();

            if (_newStudent.m_StudentStat.m_StudentType == StudentType.Art)
            {
                Vector3 classEntrance = GameObject.Find("CheckArtClass").transform.position;
                studentTreeInstance.SetVariableValue("MyClassEntrance", classEntrance);
                Vector3 classSeat = GameObject.Find("ArtFixedSeat" + m_nArt).transform.position;
                studentTreeInstance.SetVariableValue("MyClassSeat", classSeat);
                Transform SeatTransform = GameObject.Find("ArtFixedSeat" + m_nArt).transform;
                studentTreeInstance.SetVariableValue("SeatTransform", SeatTransform);
                m_nArt++;
            }
            else if (_newStudent.m_StudentStat.m_StudentType == StudentType.GameDesigner)
            {
                Vector3 classEntrance = GameObject.Find("CheckProductManagerClass").transform.position;
                studentTreeInstance.SetVariableValue("MyClassEntrance", classEntrance);
                Vector3 classSeat = GameObject.Find("ProductManagerFixedSeat" + m_nProductManager).transform.position;
                studentTreeInstance.SetVariableValue("MyClassSeat", classSeat);
                Transform SeatTransform = GameObject.Find("ProductManagerFixedSeat" + m_nProductManager).transform;
                studentTreeInstance.SetVariableValue("SeatTransform", SeatTransform);
                m_nProductManager++;
            }
            else if (_newStudent.m_StudentStat.m_StudentType == StudentType.Programming)
            {
                Vector3 classEntrance = GameObject.Find("CheckProgrammingClass").transform.position;
                studentTreeInstance.SetVariableValue("MyClassEntrance", classEntrance);
                Vector3 classSeat = GameObject.Find("ProgrammingFixedSeat" + m_nProgramming).transform.position;
                studentTreeInstance.SetVariableValue("MyClassSeat", classSeat);
                Transform SeatTransform = GameObject.Find("ProgrammingFixedSeat" + m_nProgramming).transform;
                studentTreeInstance.SetVariableValue("SeatTransform", SeatTransform);
                m_nProgramming++;
            }

            _newStudentObject.GetComponent<BehaviorTree>().ExternalBehavior = studentTreeInstance;
            _newStudentObject.GetComponent<BehaviorTree>().EnableBehavior();

            // ���� ���� �л� ������Ʈ�� ��ġ�� 0���� ������. �׺�Ž��� �������·� �ٲٸ� �ȹٲ�����.
            _newStudentObject.GetComponent<NavMeshAgent>().enabled = false;
            _newStudentObject.transform.position = AcademyEntrance.position;
            _newStudentObject.GetComponent<NavMeshAgent>().enabled = true;

            // ������� ������Ʈ�� Ư�� Ǯ�� �ִ´�.
            m_StudentList.Add(_newStudent);
            m_StudentBehaviorList.Add(_newStudentObject);
        }
            Debug.Log("����� �л� ���� �Ϸ�...!!");
    }

    public void RandomStat(StudentStat _studentStat, int _index, int _gender)
    {
        int _randomIndex = Random.Range(0, 20);

        // �̸��� ���߿� �ٲ������.
        if (_gender == 1)
        {
            _studentStat.m_StudentName = m_MaleName[_index];
        }
        else
        {
            _studentStat.m_StudentName = m_FeMaleName[_index];
        }

        _studentStat.m_Health = 100;
        _studentStat.m_Passion = 100;
        _studentStat.m_IsActiving = false;

        _studentStat.m_Personality = m_StudentPersonality[_randomIndex].Name;

        for (int i = 0; i < (int)AbilityType.Count; ++i)
        {
            _studentStat.m_AbilityAmountList[i] = GetRandomStat(PlayerInfo.Instance.m_CurrentRank, "����");

            if (_studentStat.m_AbilityAmountList[i] <= 0)
            {
                Debug.LogWarning("���Ȱ��� �̻��ϰ� �� : " + _studentStat.m_AbilityAmountList[i]);
            }
        }

        for (int i = 0; i < (int)GenreStat.Count; ++i)
        {
            _studentStat.m_GenreAmountList[i] = GetRandomStat(PlayerInfo.Instance.m_CurrentRank, "�帣");
        }
        // Random.Range(5, 101)

        // ���� ���߿� �л��� ���ȿ� ���߾ ��ų���� �ο�������
        // ���� �������� �̷���
    }

    // ��ī���� ��޿� ���� �л� ������ �ɷ�ġ �ٸ��� ���ֱ�
    private int GetRandomStat(Rank myAcademyRank, string genreOrStat)
    {
        foreach (StudentRandomStatRange table in m_StudentRandomStatRangeList)
        {
            if (table.AcademyRank == myAcademyRank && genreOrStat == "����")
            {
                return Random.Range(table.StatMinValue, table.StatMaxValue + 1);
            }
            else if (table.AcademyRank == myAcademyRank && genreOrStat == "�帣")
            {
                return Random.Range(table.GenreMinValue, table.GenreMaxValue + 1);
            }
        }

        return 0;
    }

    // �л��� ���� �̸��� ������ �ش��ϴ� ��ũ��Ʈ�� ��������.
    public string GetPersonalityScript(string _name)
    {
        string _script = "";

        foreach (StudentPersonality table in m_StudentPersonality)
        {
            if (_name == table.Name)
            {
                return _script = table.Script;
            }
        }

        return _script;
    }

    // ���Ƿ� �������� ���� ������Ʈ�� ������ ����]
    public void LinkInstructorDataToObject(Professor m_NowPlayerProfessor)
    {
        // ObjectManager professor;
        // ���� �������� ������ �޾ƿͼ� �����͸� �־��ش� �ѹ��� �ϸ� ���۷����� �� �ϳ��� �ߺ��� ���� ���� �ȴ�
        ProfessorStat professorStat1 = new ProfessorStat();
        ProfessorStat professorStat2 = new ProfessorStat();
        ProfessorStat professorStat3 = new ProfessorStat();

        // ��ȹ ������ �ֱ�
        for (int i = 0; i < m_NowPlayerProfessor.GameManagerProfessor.Count; i++)
        {
            if (m_NowPlayerProfessor.GameManagerProfessor[i].m_ProfessorType == StudentType.GameDesigner &&
                m_NowPlayerProfessor.GameManagerProfessor[i].m_ProfessorSet == "����")
            {
                nowProfessor.GameManagerProfessor = m_NowPlayerProfessor.GameManagerProfessor;

                professorStat1.m_ProfessorType = StudentType.GameDesigner;
                professorStat1.m_ProfessorNameValue = nowProfessor.GameManagerProfessor[i].m_ProfessorNameValue;
                professorStat1.m_ProfessorSet = nowProfessor.GameManagerProfessor[i].m_ProfessorSet;
                professorStat1.m_ProfessorPay = nowProfessor.GameManagerProfessor[i].m_ProfessorPay;

                professorStat1.m_ProfessorPassion = nowProfessor.GameManagerProfessor[i].m_ProfessorPassion;
                professorStat1.m_ProfessorHealth = nowProfessor.GameManagerProfessor[i].m_ProfessorHealth;
                professorStat1.m_ProfessorPower = nowProfessor.GameManagerProfessor[i].m_ProfessorPower;

                for (int j = 0; j < nowProfessor.GameManagerProfessor[i].professorSkills.Count; j++)
                {
                    professorStat1.m_ProfessorSkills.Add(nowProfessor.GameManagerProfessor[i].professorSkills[j]);
                }

                // ���⼭ ������ ���� �����͸� �� �־��ִ� ���̴�.
                //Professor1.GetComponent<Instructor>().Initialize(professorStat1);
                Professor1.GetComponent<Instructor>().Initialize(m_NowPlayerProfessor.GameManagerProfessor[i]);
            }
        }

        // ��Ʈ ������ �ֱ�
        for (int i = 0; i < m_NowPlayerProfessor.ArtProfessor.Count; i++)
        {
            if (m_NowPlayerProfessor.ArtProfessor[i].m_ProfessorType == StudentType.Art &&
                m_NowPlayerProfessor.ArtProfessor[i].m_ProfessorSet == "����")
            {
                nowProfessor.ArtProfessor = m_NowPlayerProfessor.ArtProfessor;

                professorStat2.m_ProfessorType = StudentType.Art;
                professorStat2.m_ProfessorNameValue = nowProfessor.ArtProfessor[i].m_ProfessorNameValue;
                professorStat2.m_ProfessorSet = nowProfessor.ArtProfessor[i].m_ProfessorSet;
                professorStat2.m_ProfessorPay = nowProfessor.ArtProfessor[i].m_ProfessorPay;

                professorStat2.m_ProfessorPassion = nowProfessor.ArtProfessor[i].m_ProfessorPassion;
                professorStat2.m_ProfessorHealth = nowProfessor.ArtProfessor[i].m_ProfessorHealth;
                professorStat2.m_ProfessorPower = nowProfessor.ArtProfessor[i].m_ProfessorPower;

                for (int j = 0; j < nowProfessor.ArtProfessor[i].professorSkills.Count; j++)
                {
                    professorStat2.m_ProfessorSkills.Add(nowProfessor.ArtProfessor[i].professorSkills[j]);
                }

                // ���⼭ ������ ���� �����͸� �� �־��ִ� ���̴�.
                //Professor2.GetComponent<Instructor>().Initialize(professorStat2);
                Professor2.GetComponent<Instructor>().Initialize(m_NowPlayerProfessor.ArtProfessor[i]);
            }
        }

        // �ù� ������ �ֱ�
        for (int i = 0; i < m_NowPlayerProfessor.ProgrammingProfessor.Count; i++)
        {
            if (m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorType == StudentType.Programming &&
                m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorSet == "����") // �ܷ��� �ӽ÷� �Ѵ�
            {
                nowProfessor.ProgrammingProfessor = m_NowPlayerProfessor.ProgrammingProfessor;

                professorStat3.m_ProfessorType = StudentType.Programming;
                professorStat3.m_ProfessorNameValue = nowProfessor.ProgrammingProfessor[i].m_ProfessorNameValue;
                professorStat3.m_ProfessorSet = nowProfessor.ProgrammingProfessor[i].m_ProfessorSet;
                professorStat3.m_ProfessorPay = nowProfessor.ProgrammingProfessor[i].m_ProfessorPay;

                professorStat3.m_ProfessorPassion = nowProfessor.ProgrammingProfessor[i].m_ProfessorPassion;
                professorStat3.m_ProfessorHealth = nowProfessor.ProgrammingProfessor[i].m_ProfessorHealth;
                professorStat3.m_ProfessorPower = nowProfessor.ProgrammingProfessor[i].m_ProfessorPower;

                for (int j = 0; j < nowProfessor.ProgrammingProfessor[i].professorSkills.Count; j++)
                {
                    professorStat3.m_ProfessorSkills.Add(nowProfessor.ProgrammingProfessor[i].professorSkills[j]);
                }

                // ���⼭ ������ ���� �����͸� �� �־��ִ� ���̴�.
                //Professor3.GetComponent<Instructor>().Initialize(professorStat3);
                Professor3.GetComponent<Instructor>().Initialize(m_NowPlayerProfessor.ProgrammingProfessor[i]);
            }
        }

        Debug.Log("���� 3D �ʿ� ����� ĳ���Ϳ� �����͸� �ִ� �۾�");


        // for (int i = 0; i < m_NowPlayerProfessor.AllProfessor.Count; i++)
        // {
        //     ProfessorStat temp = new ProfessorStat();
        //     // nowProfessor.ProgrammingProfessor = m_NowPlayerProfessor.ProgrammingProfessor;
        // 
        //     temp.m_ProfessorType = m_NowPlayerProfessor.AllProfessor[i].m_ProfessorType;
        //     temp.m_ProfessorNameValue = m_NowPlayerProfessor.AllProfessor[i].m_ProfessorNameValue;
        //     temp.m_ProfessorSet = m_NowPlayerProfessor.AllProfessor[i].m_ProfessorSet;
        //     temp.m_ProfessorPay = m_NowPlayerProfessor.AllProfessor[i].m_ProfessorPay;
        // 
        //     temp.m_ProfessorPassion = m_NowPlayerProfessor.AllProfessor[i].m_ProfessorPassion;
        //     temp.m_ProfessorHealth = m_NowPlayerProfessor.AllProfessor[i].m_ProfessorHealth;
        //     temp.m_ProfessorPower = m_NowPlayerProfessor.AllProfessor[i].m_ProfessorPower;
        // 
        //     for (int j = 0; j < m_NowPlayerProfessor.AllProfessor[i].professorSkills.Count; j++)
        //     {
        //         temp.m_ProfessorSkills.Add(m_NowPlayerProfessor.AllProfessor[i].professorSkills[j]);
        //     }
        // 
        //     nowProfessor.AllProfessor.Add(temp); // Mang �� ������ �� ��� �������� ������
        //     // AllProfessor.Add(temp);
        // }
    }
}