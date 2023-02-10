using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameObjectPool : MonoBehaviour
{
    public static InGameObjectPool Instance;

    [SerializeField] private List<GameObject> m_StudentPrefab = new List<GameObject>();
    [SerializeField] private GameObject m_PoolingChatBoxPrefab;

    private static Transform m_PoolingChatTransform;
    
    public List<int> m_ModelID = new List<int>();
    
    Queue<GameObject> m_PoolingStudentQueue = new Queue<GameObject>();
    Queue<GameObject> m_PoolingChatQueue = new Queue<GameObject>();

    private void Awake()
    {
        m_PoolingChatTransform = GameObject.Find("ChatObjectPool").transform;
        Instance = this;
        CreateStudent();
        Initalize(40);
    }

    void Initalize(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            m_PoolingChatQueue.Enqueue(CreateChatBox(m_PoolingChatTransform));
        }
    }

    private void CreateStudent()
    {
        List<int> _indexList = new List<int>();
        int _currentNum = Random.Range(0, m_StudentPrefab.Count);

        for (int i = 0; i < m_StudentPrefab.Count;)
        {
            if (_indexList.Contains(_currentNum))
            {
                _currentNum = Random.Range(0, m_StudentPrefab.Count);
            }
            else
            {
                _indexList.Add(_currentNum);
                i++;
            }
        }

        for (int i = 0; i < m_StudentPrefab.Count; i++)
        {
            GameObject _newStudent = Instantiate(m_StudentPrefab[_indexList[i]]);
            m_ModelID.Add(_indexList[i]);
            _newStudent.gameObject.SetActive(false);
            _newStudent.transform.SetParent(transform);
            m_PoolingStudentQueue.Enqueue(_newStudent);

        }
    }

    private GameObject CreateChatBox(Transform _transform)
    {
        GameObject _newChatBox = Instantiate(m_PoolingChatBoxPrefab);
        _newChatBox.gameObject.SetActive(false);
        _newChatBox.transform.SetParent(_transform);

        return _newChatBox;
    }

    public static GameObject GetStudentObject(Transform _transform)
    {
        if (Instance.m_PoolingStudentQueue.Count > 0)
        {
            var _obj = Instance.m_PoolingStudentQueue.Dequeue();
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

    public static void ReturnStudentObject(GameObject _student)
    {
        _student.gameObject.SetActive(false);
        _student.transform.SetParent(Instance.transform);
        Instance.m_PoolingStudentQueue.Enqueue(_student);
    }

    public static void ReturnChatObject(GameObject _chat)
    {
        _chat.gameObject.SetActive(false);
        _chat.transform.SetParent(Instance.transform);
        Instance.m_PoolingChatQueue.Enqueue(_chat);
    }
}
