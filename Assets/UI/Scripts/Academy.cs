using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// 
/// </summary>
public class Academy : MonoBehaviour
{
    public TMP_InputField m_Academy;
    public TMP_InputField m_Director;

    private string m_AcademyName;
    private string m_DirectorName;

    public TextMeshProUGUI m_PopupNotice;                       // 안내문구 띄워줄 변수

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
    #endregion

    // 아카데미이름 & 원장 이름 세팅
    public void SetAcademyData()
    {
        if (m_Academy.text != "" && m_Director.text != "")
        {

            MyAcademy = m_Academy.text;
            MyDirector = m_Director.text;

            Debug.Log("AcademyName : " + MyAcademy);
            Debug.Log("DirectorName : " + MyDirector);

            PlayerInfo.Instance.m_AcademyName = MyAcademy;
            PlayerInfo.Instance.m_DirectorName = MyDirector;

            string strFile = Application.dataPath + "\\Json\\PlayerInfo_NewtonJson.json";
           FileInfo fileInfo = new FileInfo(strFile);

            Debug.Log(strFile);

            // 파일 있는지 확인 있으면 true 없으면 false
            if (fileInfo.Exists == true)
            {
                Debug.Log(" 파일 있으니까 덮어쓰기 ");
                // 딕셔너리도 가능한 NewtonJson 사용하자
                // "\\Json\\NewtonJson.json" -> 파일 이름은 각각의 정보에 맞는 이름으로
                //File.WriteAllText(Application.dataPath + "\\Json\\PlayerInfo_NewtonJson.json", JsonConvert.SerializeObject(PlayerInfo.Instance));
                Debug.Log(PlayerInfo.Instance);

            }
            else
            {
                Debug.Log(" 파일 없으니까 생성 ");
                //File.WriteAllText(Application.dataPath + "\\Json\\PlayerInfo_NewtonJson.json", JsonConvert.SerializeObject(PlayerInfo.Instance));
                Debug.Log(PlayerInfo.Instance);

            }
            MoveSceneManager.m_Instance.MoveToInGameScene();
        }
        else
        {
            m_PopupNotice.text = "둘 다 입력해주세요";

            // '비밀번호가 같지 않습니다' 팝업창 띄우기
            m_PopupNotice.gameObject.SetActive(true);

            m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();    // 팝업창 띄우기 
        }
    }
}
