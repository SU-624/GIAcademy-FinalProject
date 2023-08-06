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
/// ���� UI ��ũ��Ʈ���� ���� �� UI �Ŵ���
///
/// woodpie9 2023.06.13
/// MonoBehaviour�� ���� AllInOneData�� �Լ��� �����ϱ� ���� UI�� ����ϴ� �Լ�
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
        
        // �ӽ÷� Firebasebinder ���� �־�����.
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
