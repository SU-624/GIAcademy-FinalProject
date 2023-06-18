using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.Timeline;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckSelecteClass : MonoBehaviour
{
    [SerializeField] private PopOffUI m_PopOffCheckClassPanel;
    [SerializeField] private PopUpUI m_PopUpCheckClassPanel;
    [SerializeField] private Button m_1WeekButton;
    [SerializeField] private Button m_2WeekButton;
    [SerializeField] private Button m_StartClassButton;
    [SerializeField] private GameObject m_HideButton;
    [SerializeField] private TextMeshProUGUI m_CurrentMoney;
    [SerializeField] private Transform m_CompleteClassPrefabParent;
    [SerializeField] private GameObject m_CompleteClassPrefabParentObj;
    [SerializeField] private TextMeshProUGUI m_TotalClassMoney;

    [SerializeField] private TextMeshProUGUI m_1WeekGMClassName;
    [SerializeField] private TextMeshProUGUI m_2WeekGMClassName;
    [SerializeField] private TextMeshProUGUI m_1WeekGMClassMoney;
    [SerializeField] private TextMeshProUGUI m_2WeekGMClassMoney;

    [SerializeField] private TextMeshProUGUI m_1WeekArtClassName;
    [SerializeField] private TextMeshProUGUI m_2WeekArtClassName;
    [SerializeField] private TextMeshProUGUI m_1WeekArtClassMoney;
    [SerializeField] private TextMeshProUGUI m_2WeekArtClassMoney;

    [SerializeField] private TextMeshProUGUI m_1WeekProgrammingClassName;
    [SerializeField] private TextMeshProUGUI m_2WeekProgrammingClassName;
    [SerializeField] private TextMeshProUGUI m_1WeekProgrammingClassMoney;
    [SerializeField] private TextMeshProUGUI m_2WeekProgrammingClassMoney;

    public Transform CompleteClassPrefabParent { get { return m_CompleteClassPrefabParent; } }
    public Button _1WeekButton { get { return m_1WeekButton; } set { m_1WeekButton = value; } }
    public Button _2WeekButton { get { return m_2WeekButton; } set { m_2WeekButton = value; } }
    public Button _StartButton { get { return m_StartClassButton; } set { m_StartClassButton = value; } }

    private void Start()
    {
        m_1WeekButton.Select();
    }

    public void SetTotalMoney(string _money)
    {
        m_TotalClassMoney.text = _money;
    }

    public void SetCurrentMoney(string _money)
    {
        m_CurrentMoney.text = _money;
    }

    public void PopUpCheckClassPanel()
    {
        m_PopUpCheckClassPanel.TurnOnUI();
    }

    public void PopOffCheckClassPanel()
    {
        m_PopOffCheckClassPanel.TurnOffUI();
    }

    public void SetGMData(string _1WeekName, string _2WeekName, string _1WeekMoney, string _2WeekMoney)
    {
        m_1WeekGMClassName.text = _1WeekName;
        m_2WeekGMClassName.text = _2WeekName;

        m_1WeekGMClassMoney.text = _1WeekMoney;
        m_2WeekGMClassMoney.text = _2WeekMoney;
    }

    public void SetArtData(string _1WeekName, string _2WeekName, string _1WeekMoney, string _2WeekMoney)
    {
        m_1WeekArtClassName.text = _1WeekName;
        m_2WeekArtClassName.text = _2WeekName;

        m_1WeekArtClassMoney.text = _1WeekMoney;
        m_2WeekArtClassMoney.text = _2WeekMoney;
    }

    public void SetProgrammingData(string _1WeekName, string _2WeekName, string _1WeekMoney, string _2WeekMoney)
    {
        m_1WeekProgrammingClassName.text = _1WeekName;
        m_2WeekProgrammingClassName.text = _2WeekName;

        m_1WeekProgrammingClassMoney.text = _1WeekMoney;
        m_2WeekProgrammingClassMoney.text = _2WeekMoney;
    }

    public void DestroyCheckClassObject()
    {
        Transform[] _childCount = m_CompleteClassPrefabParentObj.GetComponentsInChildren<Transform>();

        if (_childCount != null)
        {
            for (int i = 0; i < _childCount.Length; i++)
            {
                if (_childCount[i] != m_CompleteClassPrefabParentObj.transform)
                {
                    Destroy(_childCount[i].gameObject);
                }
            }
        }
    }
}
