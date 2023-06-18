using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5f;
    public bool loop = true;

    private int currentWaypointIndex;
    //public Vector3 rotationValue;

    private void Start()
    {
        currentWaypointIndex = 0;
        transform.position = waypoints[currentWaypointIndex].position;

        //������ ȸ���� ����
        //transform.rotation = Quaternion.Euler(rotationValue);
    }

    private void Update()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentWaypointIndex++;
            }

            // ������Ʈ�� �Ӹ� ���� ����
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
            }
        }
        else if (loop)
        {
            currentWaypointIndex = 0;
            transform.position = waypoints[currentWaypointIndex].position;
        }
    }
}