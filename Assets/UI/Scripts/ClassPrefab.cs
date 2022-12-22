using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StatData.Runtime;

/// <summary>
/// 프리팹 생성해줄 스크립트
/// </summary>
public class ClassPrefab : MonoBehaviour
{
    public GameObject m_Prefab;
    public Transform m_parent;
    [SerializeField] private ClassController m_SelecteClass;

    bool isCheck = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MakeClass()
    {
        if (isCheck == true)
        {
            for (int i = 0; i < m_SelecteClass.dataBase.classDatas.Count; i++)     // 수업의 갯수만큼 생성해주기
            {
                // 이미 만들어졌다면 더이상 생성하지 못하도록
                GameObject.Instantiate(m_Prefab, m_parent);
                m_Prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_SelecteClass.dataBase.classDatas[i].ClassName;
                //m_Prefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_SelecteClass.dataBase.classDatas[i].ClassName;
                m_Prefab.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_SelecteClass.dataBase.classDatas[i].Stats.systemValue.ToString();
                m_Prefab.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_SelecteClass.dataBase.classDatas[i].Stats.contentsValue.ToString();
                m_Prefab.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_SelecteClass.dataBase.classDatas[i].Stats.balanceValue.ToString();
                //m_Prefab.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = m_SelecteClass.dataBase.classDatas[i].ClassName;


                isCheck = false;
            }
        }
    }
}
