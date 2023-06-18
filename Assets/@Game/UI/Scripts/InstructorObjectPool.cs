using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructorObjectPool : MonoBehaviour
{
    public static InstructorObjectPool Instance;

    [SerializeField] private GameObject EachInstructorPrefab;

    Queue<GameObject> poolingInstructorQueue = new Queue<GameObject>();     // 최대 99며이의 강사를 넣어둘 풀

    const int MaxInstructorCount = 99;

    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Initialize()
    {
        for (int i = 0; i < MaxInstructorCount; i++)
        {
            poolingInstructorQueue.Enqueue(CreateInstructorPrefab());
        }
    }

    private GameObject CreateInstructorPrefab()
    {
        GameObject NowInstrucors = Instantiate(EachInstructorPrefab);
        NowInstrucors.gameObject.SetActive(false);
        NowInstrucors.transform.SetParent(transform);

        return NowInstrucors;
    }

    public static GameObject GetInstructorPrefab(Transform setTransform)
    {
        if (Instance.poolingInstructorQueue.Count > 0)
        {
            var _obj = Instance.poolingInstructorQueue.Dequeue();
            _obj.transform.SetParent(setTransform);
            _obj.gameObject.SetActive(true);
            return _obj;
        }
        else
        {
            var _newObj = Instance.CreateInstructorPrefab();
            _newObj.gameObject.SetActive(true);
            _newObj.transform.SetParent(setTransform);
            return _newObj;
        }
    }

    public static void ReturnInstructorPrefab(GameObject instructorPrefab)
    {
        instructorPrefab.gameObject.SetActive(false);
        instructorPrefab.transform.SetParent(Instance.transform);       // object -> UI canvas 간의 이동 - 사이즈 변화문제 : 부모의 사이즈로 크기 맞춰줌
        Instance.poolingInstructorQueue.Enqueue(instructorPrefab);
    }
}
