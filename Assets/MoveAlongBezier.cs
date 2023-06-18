using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SanyoniLib.Bezier;

public class MoveAlongBezier : MonoBehaviour
{
    [SerializeField] private Bezier m_Bezier;
    [SerializeField] private float m_Duration = 4f;

    private void Update()
    {
        float _delta = (Time.time % m_Duration) / m_Duration;
        BezierResult _point = m_Bezier.GetPoint(_delta);
        transform.position = _point.GetPosition();
        transform.rotation = Quaternion.LookRotation(_point.GetDirection(), Vector3.forward);
    }
}
