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

        //프리팹 회전값 설정
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

            // 오브젝트의 머리 방향 설정
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