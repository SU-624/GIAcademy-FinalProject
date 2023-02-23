using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// 2023. 02. 14 
/// 
/// ���� Json ������ �ε� �� �����͵��� ������ Ŭ����
/// </summary>
public class AllOriginalJsonData
{
    private static AllOriginalJsonData instance = null;

    public static AllOriginalJsonData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AllOriginalJsonData();
            }

            return instance;
        }

        set
        {
            instance = value;
        }
    }

    public List<SaveEventData> OriginalEventListData = new List<SaveEventData>();           // ���� �̺�Ʈ ������
    public List<StudentSaveData> OriginalStudentData = new List<StudentSaveData>();         // ���� ��ü�л� ������
    public List<MailSaveData> OriginalMailData = new List<MailSaveData>();                  // ���� ��ü���� ������
    public List<TeacherSaveData> OriginalTeacherhData = new List<TeacherSaveData>();   // ���� ��ü���� ������

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //Convert �� ���̽� �����͸� ���� �ʿ��� ���� �־��ֱ� -> �� ��ũ��Ʈ����

}
