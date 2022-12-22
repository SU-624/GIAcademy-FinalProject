using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 모든 애들을 들고 있어야 한다고?
public class Json
{
    // Json 형식으로 데이터 변환
    public void TranslateToJson()
    {
        string str = JsonUtility.ToJson(PlayerInfo.Instance);

        Debug.Log("To Json : " + str);
    }

    // Json 형식의 파일을 데이터로 변환

    // Json  파일로 저장
    // 

}
