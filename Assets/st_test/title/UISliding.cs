using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISliding : MonoBehaviour
{
    public RectTransform imageTransform;  // �̵��� UI �̹����� RectTransform ������Ʈ
    public float speed = 100f;  // �̵� �ӵ�
    public Vector2 direction = new Vector2(1f, 1f);  // �̵� ���� (x, y)

    private void Update()
    {
        Vector3 currentPosition = imageTransform.localPosition;
        currentPosition.x += direction.x * speed * Time.deltaTime;
        currentPosition.y += direction.y * speed * Time.deltaTime;
        imageTransform.localPosition = currentPosition;
    }
}
