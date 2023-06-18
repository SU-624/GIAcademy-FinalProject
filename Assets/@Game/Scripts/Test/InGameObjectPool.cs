using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameObjectPool : MonoBehaviour
{
    public static InGameObjectPool Instance;

    [SerializeField] private List<GameObject> m_FemaleStudentPrefab = new List<GameObject>();
    [SerializeField] private List<GameObject> m_MaleStudentPrefab = new List<GameObject>();
    [SerializeField] private GameObject m_PoolingChatBoxPrefab;

    private static Transform m_PoolingChatTransform;
    
    public List<int> m_ModelID = new List<int>();
    
    Queue<GameObject> m_PoolingFemaleStudentQueue = new Queue<GameObject>();
    Queue<GameObject> m_PoolingMaleStudentQueue = new Queue<GameObject>();
    Queue<GameObject> m_PoolingChatQueue = new Queue<GameObject>();

    private void Awake()
    {
        m_PoolingChatTransform = GameObject.Find("ChatObjectPool").transform;
        Instance = this;
        CreateFemaleStudent();
        CreateMaleStudent();
        Initalize(40);
    }

    void Initalize(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            m_PoolingChatQueue.Enqueue(CreateChatBox(m_PoolingChatTransform));
        }
    }

    private void CreateFemaleStudent()
    {
        List<int> _indexList = new List<int>();
        int _currentNum = Random.Range(0, m_FemaleStudentPrefab.Count);

        for (int i = 0; i < m_FemaleStudentPrefab.Count;)
        {
            if (_indexList.Contains(_currentNum))
            {
                _currentNum = Random.Range(0, m_FemaleStudentPrefab.Count);
            }
            else
            {
                _indexList.Add(_currentNum);
                i++;
            }
        }

        for (int i = 0; i < m_FemaleStudentPrefab.Count; i++)
        {
            GameObject _newStudent = Instantiate(m_FemaleStudentPrefab[_indexList[i]]);
            _newStudent.name = m_FemaleStudentPrefab[_indexList[i]].name;
            m_ModelID.Add(_indexList[i]);
            _newStudent.gameObject.SetActive(false);
            _newStudent.transform.SetParent(transform);
            m_PoolingFemaleStudentQueue.Enqueue(_newStudent);

        }
    }

    private void CreateMaleStudent()
    {
        List<int> _indexList = new List<int>();
        int _currentNum = Random.Range(0, m_MaleStudentPrefab.Count);

        for (int i = 0; i < m_MaleStudentPrefab.Count;)
        {
            if (_indexList.Contains(_currentNum))
            {
                _currentNum = Random.Range(0, m_MaleStudentPrefab.Count);
            }
            else
            {
                _indexList.Add(_currentNum);
                i++;
            }
        }

        for (int i = 0; i < m_MaleStudentPrefab.Count; i++)
        {
            GameObject _newStudent = Instantiate(m_MaleStudentPrefab[_indexList[i]]);
            _newStudent.name = m_MaleStudentPrefab[_indexList[i]].name;
            m_ModelID.Add(_indexList[i]);
            _newStudent.gameObject.SetActive(false);
            _newStudent.transform.SetParent(transform);
            m_PoolingMaleStudentQueue.Enqueue(_newStudent);

        }
    }

    private GameObject CreateChatBox(Transform _transform)
    {
        GameObject _newChatBox = Instantiate(m_PoolingChatBoxPrefab);
        _newChatBox.gameObject.SetActive(false);
        _newChatBox.transform.SetParent(_transform);

        return _newChatBox;
    }

    public static GameObject GetFemaleStudentObject(Transform _transform)
    {
        if (Instance.m_PoolingFemaleStudentQueue.Count > 0)
        {
            var _obj = Instance.m_PoolingFemaleStudentQueue.Dequeue();
            _obj.transform.SetParent(_transform);
            _obj.gameObject.SetActive(true);
            return _obj;
        }
        else
        {
            Debug.Log("풀이 비었습니다.");
            return null;
        }
    }

    public static GameObject GetMaleStudentObject(Transform _transform)
    {
        if (Instance.m_PoolingMaleStudentQueue.Count > 0)
        {
            var _obj = Instance.m_PoolingMaleStudentQueue.Dequeue();
            _obj.transform.SetParent(_transform);
            _obj.gameObject.SetActive(true);
            return _obj;
        }
        else
        {
            Debug.Log("풀이 비었습니다.");
            return null;
        }
    }

    public static GameObject GetChatObject(Transform _transform)
    {
        if (Instance.m_PoolingChatQueue.Count > 0)
        {
            var _obj = Instance.m_PoolingChatQueue.Dequeue();
            _obj.transform.SetParent(_transform);
            _obj.gameObject.SetActive(true);
            return _obj;
        }
        else
        {
            var _newObj = Instance.CreateChatBox(m_PoolingChatTransform);
            _newObj.gameObject.SetActive(true);
            _newObj.transform.SetParent(_transform);
            return _newObj;
        }
    }

    public static void ReturnFemaleStudentObject(GameObject _student)
    {
        _student.gameObject.SetActive(false);
        _student.transform.SetParent(Instance.transform);
        Instance.m_PoolingFemaleStudentQueue.Enqueue(_student);
    }

    public static void ReturnMaleStudentObject(GameObject _student)
    {
        _student.gameObject.SetActive(false);
        _student.transform.SetParent(Instance.transform);
        Instance.m_PoolingMaleStudentQueue.Enqueue(_student);
    }

    public static void ReturnChatObject(GameObject _chat)
    {
        _chat.gameObject.SetActive(false);
        _chat.transform.SetParent(Instance.transform);
        Instance.m_PoolingChatQueue.Enqueue(_chat);
    }
}
