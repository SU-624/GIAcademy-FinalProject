using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankScene1 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Title;

    public void ChangeTitle(string _title)
    {
        m_Title.text = _title;
    }
}
