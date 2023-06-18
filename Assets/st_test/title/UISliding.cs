using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISliding : MonoBehaviour
{
    public RectTransform imageTransform;  // 이동할 UI 이미지의 RectTransform 컴포넌트
    public float speed = 100f;  // 이동 속도
    public Vector2 direction = new Vector2(1f, 1f);  // 이동 방향 (x, y)

    private void Update()
    {
        Vector3 currentPosition = imageTransform.localPosition;
        currentPosition.x += direction.x * speed * Time.deltaTime;
        currentPosition.y += direction.y * speed * Time.deltaTime;
        imageTransform.localPosition = currentPosition;
    }
}
