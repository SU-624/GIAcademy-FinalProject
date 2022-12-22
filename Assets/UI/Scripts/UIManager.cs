using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// Mang 2022.10.12
/// 
/// 여러 UI 스크립트들을 관리 할 UI 매니저
/// </summary>
public class UIManager : MonoBehaviour
{
    private static UIManager m_Instance = null;       // Manager 변수는 싱글톤으로 사용

    // 싱글톤으로 만든 변수를 안전하게 생성하고 사용하기 위한 초기화(?) 방법
    private void Awake()
    {
        // Instance 가 존재한다면 gameObject 제거
        if (m_Instance != null)
        {
            return;
        }

        // 시작될 때 인스턴스 초기화, 씬을 넘어갈때도 유지되기 위한 처리
        m_Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // public 프로퍼티로 선언해서 외부에서 private 메버변수에만 접근 가능하도록
    public static UIManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                return null;
            }
            return m_Instance;
        }
    }

    //public void PopUpUI()
    //{
    //    if (Input.GetKeyDown("1"))
    //    {

    //    }
    //}
}
