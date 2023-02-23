using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using Newtonsoft.Json;

using Conditiondata.Runtime;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class Academy : MonoBehaviour
{
    public TMP_InputField m_Academy;
    public TMP_InputField m_Director;

    private string m_AcademyName;
    private string m_DirectorName;
    private int m_Money;

    public TextMeshProUGUI m_PopupNotice;                       // �ȳ����� ����� ����
    public GameObject m_SekOKButton;
    public GameObject m_Loading_Panel;

    #region AcademyData
    public string MyAcademy
    {
        get { return m_AcademyName; }
        private set { m_AcademyName = value; }
    }
    public string MyDirector
    {
        get { return m_DirectorName; }
        private set { m_DirectorName = value; }
    }
    public int MyMoney
    {
        get { return m_Money; }
        private set { m_Money = value; }
    }
    #endregion

    // ��ư�� ��ī���̸�, ����� üũ �� ������ �ϴ� �Լ� �޾Ƶα�
    private void Start()
    {
        m_SekOKButton.GetComponent<Button>().onClick.AddListener(SetAcademyData);
    }

    // ��ī�����̸� & ���� �̸� ���� ( ���� Title�� - SetOK ��ư ������ ����)
    public void SetAcademyData()
    {
        if (m_Academy.text != "" && m_Director.text != "")
        {
            MyAcademy = m_Academy.text;
            MyDirector = m_Director.text;

            Debug.Log("AcademyName : " + MyAcademy);
            Debug.Log("DirectorName : " + MyDirector);
            PlayerInfo.Instance.m_AcademyName = MyAcademy;
            PlayerInfo.Instance.m_PrincipalName = MyDirector;
            ///////////////////////


            StudentSaveData data = new StudentSaveData();
            data.studentStat = new StudentStat();

            data.studentStat.m_StudentName = "��â��";
            data.studentStat.m_StudentType = StudentType.Programming;

            StudentSaveData data1 = new StudentSaveData();
            data1.studentStat = new StudentStat();

            data1.studentStat.m_StudentName = "���翵";
            data1.studentStat.m_StudentType = StudentType.Art;

            StudentSaveData data2 = new StudentSaveData();
            data2.studentStat = new StudentStat();

            data2.studentStat.m_StudentName = "������";
            data2.studentStat.m_StudentType = StudentType.GameDesigner;

            // � �α������� üũ�� �� ������ ������ �̸��� �ٲ�� �Ѵ�

            AllInOneData.Instance.player.m_playerID = PlayerInfo.Instance.m_PlayerID;
            AllInOneData.Instance.player.m_AcademyName = MyAcademy;
            AllInOneData.Instance.player.m_PrincipalName = MyDirector;

            AllInOneData.Instance.studentData.Add(data);
            AllInOneData.Instance.studentData.Add(data1);
            AllInOneData.Instance.studentData.Add(data2);

            SceneChangeAfterCheckData();
        }
        else
        {
            m_PopupNotice.text = "�� �� �Է����ּ���";

            // '��й�ȣ�� ���� �ʽ��ϴ�' �˾�â ����
            m_PopupNotice.gameObject.SetActive(true);

            m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();    // �˾�â ���� 
        }
    }

    public void SceneChangeAfterCheckData()
    {
        // ������ ������ üũ ��
        if(AllInOneData.Instance.player.m_AcademyName != "" && AllInOneData.Instance.player.m_PrincipalName != "")
        {
            // �ε��� ����
            m_Loading_Panel.SetActive(true);
            // ������
            MoveSceneManager.m_Instance.MoveToInGameScene();
        }   
    }
}
