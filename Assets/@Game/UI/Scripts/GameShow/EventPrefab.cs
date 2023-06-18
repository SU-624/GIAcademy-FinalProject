using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_DDaY;
    [SerializeField] private TextMeshProUGUI m_ActivityText;
    [SerializeField] private Image m_TimeAnimation;

    public TextMeshProUGUI DDAyText { get { return m_DDaY; } set { m_DDaY = value; } }
    public TextMeshProUGUI ActivityText { get { return m_ActivityText; } set { m_ActivityText = value; } }
    public Image TimeAnimation { get { return m_TimeAnimation; } set { m_TimeAnimation = value; } }
    private Coroutine TypingTexteffect;
}
