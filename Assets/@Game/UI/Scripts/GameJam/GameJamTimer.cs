using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameJamTimer : MonoBehaviour
{
    [SerializeField] private GameObject m_TimerObj;
    [SerializeField] private TextMeshProUGUI m_MinText;
    [SerializeField] private TextMeshProUGUI m_SecondText;
    [SerializeField] private TextMeshProUGUI m_TimerText;

    public void StartGameJam(string _min, string _sec)
    {
        m_MinText.text = _min;
        m_SecondText.text = _sec;
    }

    public void ChangeText(string _min, string _sec)
    {
        m_TimerText.text = string.Format("{0}:{1}", _min, _sec);
    }

    public void ChangeImageColor(Color _newColor)
    {
        m_TimerObj.GetComponent<Image>().color = _newColor;
        m_TimerText.color = _newColor;
    }

    public void SetActiveSelf(bool _isTrue = true)
    {
        m_TimerObj.SetActive(_isTrue);
    }
}
