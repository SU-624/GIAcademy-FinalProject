using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventPanel : MonoBehaviour
{
    [SerializeField] private Button m_EventButton;
    [SerializeField] private GameObject m_ScrollViewObj;
    [SerializeField] private Transform m_EventTransForm;

    public Transform EventTransform { get { return m_EventTransForm; } set { m_EventTransForm = value; } }
    public GameObject ScorllViewObj { get { return m_ScrollViewObj; } set { m_ScrollViewObj = value; } }

}
