using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentBonusSkillObjectPool : MonoBehaviour
{
    public static StudentBonusSkillObjectPool Instance;

    [SerializeField] private GameObject BonusSkillPrefab;

    Queue<GameObject> BonusSkillQueue = new Queue<GameObject>();     // 최대 99며이의 강사를 넣어둘 풀

    const int MaxBonusSkillCount = 99;

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
        for (int i = 0; i < MaxBonusSkillCount; i++)
        {
            BonusSkillQueue.Enqueue(CreateStudentBonusSkillPrefab());
        }
    }

    private GameObject CreateStudentBonusSkillPrefab()
    {
        GameObject NowInstrucors = Instantiate(BonusSkillPrefab);
        NowInstrucors.gameObject.SetActive(false);
        NowInstrucors.transform.SetParent(transform);

        return NowInstrucors;
    }

    public static GameObject GetStudentBonusSkillPrefab(Transform setTransform)
    {
        if (Instance.BonusSkillQueue.Count > 0)
        {
            var _obj = Instance.BonusSkillQueue.Dequeue();
            _obj.transform.SetParent(setTransform);
            _obj.gameObject.SetActive(true);
            return _obj;
        }
        else
        {
            var _newObj = Instance.CreateStudentBonusSkillPrefab();
            _newObj.gameObject.SetActive(true);
            _newObj.transform.SetParent(setTransform);
            return _newObj;
        }
    }

    public static void ReturnStudentBonusSkillPrefab(GameObject BonusPrefab)
    {
        BonusPrefab.gameObject.SetActive(false);
        BonusPrefab.transform.SetParent(Instance.transform);       // object -> UI canvas 간의 이동 - 사이즈 변화문제 : 부모의 사이즈로 크기 맞춰줌
        Instance.BonusSkillQueue.Enqueue(BonusPrefab);
    }
}
