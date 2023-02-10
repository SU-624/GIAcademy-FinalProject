using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GoogleAchievement : MonoBehaviour
{
    public GameObject MailUIButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IsMailClicked()
    {
        GameObject nowClick = EventSystem.current.currentSelectedGameObject;

        if(nowClick.name == MailUIButton.name)
        {
            Debug.Log("메일 클릭 업적 달성 테스트");


            if (GPGSBinder.Instance.UserId != null)
            {
                GPGSBinder.Instance.UnlockAchievement(GPGSIds.achievement_Mail);     //구글 업적 달성
            }

        }
    }

    public void IsSetFirstSchedule()
    {
        Debug.Log("첫 수업 세팅 되었ㄴ느냐!");


        if (GPGSBinder.Instance.UserId != null)
        {
            GPGSBinder.Instance.UnlockAchievement(GPGSIds.achievement_SetClass);     //구글 업적 달성
        }

    }

    public void GoogleAchevment()
    {
        Debug.Log("구글 업적 창 띄우기");


        if (GPGSBinder.Instance.UserId != null)
        {
            GPGSBinder.Instance.ShowAchievementUI();
        }

    }
}
