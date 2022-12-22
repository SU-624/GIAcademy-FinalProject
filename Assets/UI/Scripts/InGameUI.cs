using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField]
    PopUpUI m_popUpInstant;     // Title -> InGame 씬으로 이동 시 제일 먼저 띄워질 팝업창
    
    public TextMeshProUGUI m_nowAcademyName;
    public TextMeshProUGUI m_nowDirectorName;

    // public TextMeshProUGUI m_nowTime;

    // Start is called before the first frame update
    void Start()
    {
        m_popUpInstant.DelayTurnOnUI();

        m_nowAcademyName.text = PlayerInfo.Instance.m_AcademyName;
        m_nowDirectorName.text = PlayerInfo.Instance.m_DirectorName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
