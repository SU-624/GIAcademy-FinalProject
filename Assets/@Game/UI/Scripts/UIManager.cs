using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


/// <summary>
/// Mang 2022.10.12
/// 
/// 여러 UI 스크립트들을 관리 할 UI 매니저
///
/// woodpie9 2023.06.13
/// MonoBehaviour가 없는 AllInOneData의 함수에 접근하기 위한 UI를 사용하는 함수
/// </summary>
public class UIManager : MonoBehaviour
{
    public EventScheduleSystem eventScheduleSystem;
    public MailManagement mailManager;
    public GameJam gameJamManager;
    public GameShow gameShowManager;
    public GameObject SpeedBtn;
    
    public void CollectDataBtn()
    {
        AllInOneData.Instance.CollectAllGameData();
        
        // 임시로 Firebasebinder 에도 넣어주자.
        eventScheduleSystem.CollectSuddenEvent();
        mailManager.CollectMailList();
        gameJamManager.CollectGameJamData();
        gameShowManager.CollectGameShowData();
    }
    
    public void LocalSaveBtn()
    {
        CollectDataBtn();
        Json.Instance.SaveDataInLocal();
    }

    public void LocalLoadBtn()
    {    
        
        
    }

    public void GoTitleBtn()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void SpeedBtnOn()
    {
        SpeedBtn.SetActive(true);
    }
}
