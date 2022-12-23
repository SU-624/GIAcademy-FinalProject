using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] private GameObject m_Month1;
    [SerializeField] private GameObject m_Month2;
    [SerializeField] private GameObject m_Month3;
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
            if(m_Month1.name == "ProductManagerC_Button" || m_Month2.name == "ProductManagerC_Button" || m_Month3.name == "ProductManagerC_Button")
            {
                for(int i =0; i < ClassSchedule.Instance.m_NowPlayerClass.ProductManagerClass.Count; i++)
                {
                    GameObject _classClick1 = GameObject.Instantiate(m_Prefab, m_parent);
                    _classClick1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ClassSchedule.Instance.m_NowPlayerClass.ProductManagerClass[i].m_ClassName;
                    //_classClick1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_SelecteClass.dataBase.classDatas[i].ClassName;
                    _classClick1.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ClassSchedule.Instance.m_NowPlayerClass.ProductManagerClass[i].m_ClassSystemValue.ToString();
                    _classClick1.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = ClassSchedule.Instance.m_NowPlayerClass.ProductManagerClass[i].m_ClassContentsValue.ToString();
                    _classClick1.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = ClassSchedule.Instance.m_NowPlayerClass.ProductManagerClass[i].m_ClassBalanceValue.ToString();
                    //_classClick1.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = m_SelecteClass.dataBase.classDatas[i].ClassName;
                }
            }

            if (m_Month1.name == "ArtC_Button" || m_Month2.name == "ArtC_Button" || m_Month3.name == "ArtC_Button")
            {
                for (int i = 0; i < ClassSchedule.Instance.m_NowPlayerClass.ArtClass.Count; i++)
                {
                    GameObject _classClick2 = GameObject.Instantiate(m_Prefab, m_parent);
                    _classClick2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ClassSchedule.Instance.m_NowPlayerClass.ArtClass[i].m_ClassName;
                    //_classClick2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_SelecteClass.dataBase.classDatas[i].ClassName;
                    _classClick2.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ClassSchedule.Instance.m_NowPlayerClass.ArtClass[i].m_ClassSystemValue.ToString();
                    _classClick2.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = ClassSchedule.Instance.m_NowPlayerClass.ArtClass[i].m_ClassContentsValue.ToString();
                    _classClick2.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = ClassSchedule.Instance.m_NowPlayerClass.ArtClass[i].m_ClassBalanceValue.ToString();
                    //_classClick2.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = m_SelecteClass.dataBase.classDatas[i].ClassName;
                }
            }

            if (m_Month1.name == "ProgrammingC_Button" || m_Month2.name == "ProgrammingC_Button" || m_Month3.name == "ProgrammingC_Button")
            {
                for (int i = 0; i < ClassSchedule.Instance.m_NowPlayerClass.ProgrammingClass.Count; i++)
                {
                    GameObject _classClick3 = GameObject.Instantiate(m_Prefab, m_parent);
                    _classClick3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ClassSchedule.Instance.m_NowPlayerClass.ProgrammingClass[i].m_ClassName;
                    //_classClick3.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_SelecteClass.dataBase.classDatas[i].ClassName;
                    _classClick3.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ClassSchedule.Instance.m_NowPlayerClass.ProgrammingClass[i].m_ClassSystemValue.ToString();
                    _classClick3.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = ClassSchedule.Instance.m_NowPlayerClass.ProgrammingClass[i].m_ClassContentsValue.ToString();
                    _classClick3.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = ClassSchedule.Instance.m_NowPlayerClass.ProgrammingClass[i].m_ClassBalanceValue.ToString();
                    //_classClick3.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = m_SelecteClass.dataBase.classDatas[i].ClassName;
                }
            }
            //isCheck = false;
        }
    }
}
