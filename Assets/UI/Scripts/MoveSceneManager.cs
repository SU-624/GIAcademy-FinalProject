using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 2022. 11. 22 Mang
/// 
/// 게임에서 씬 이동을 담당할 클래스
/// </summary>
public class MoveSceneManager : MonoBehaviour
{
    public static MoveSceneManager m_Instance = null;       // Manager 변수는 싱글톤으로 사용

    // 싱글톤으로 만든 변수를 안전하게 생성하고 사용하기 위한 초기화(?) 방법
    private void Awake()
    {
        // Instance 가 존재한다면 gameObject 제거
        if (m_Instance != null)
        {
            Destroy(this.gameObject);

            return;
        }

        // 시작될 때 인스턴스 초기화, 씬을 넘어갈때도 유지되기 위한 처리
        m_Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveToInGameScene()
    {
        SceneManager.LoadScene("InGameScene");
    }
}
