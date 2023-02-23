using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// 2023. 02. 14 
/// 
/// 원본 Json 파일을 로드 할 데이터들의 모음인 클래스
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

    public List<SaveEventData> OriginalEventListData = new List<SaveEventData>();           // 원본 이벤트 데이터
    public List<StudentSaveData> OriginalStudentData = new List<StudentSaveData>();         // 원본 전체학생 데이터
    public List<MailSaveData> OriginalMailData = new List<MailSaveData>();                  // 원본 전체메일 데이터
    public List<TeacherSaveData> OriginalTeacherhData = new List<TeacherSaveData>();   // 원본 전체교수 데이터

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //Convert 한 제이슨 데이터를 이제 필요한 곳에 넣어주기 -> 각 스크립트에서

}
