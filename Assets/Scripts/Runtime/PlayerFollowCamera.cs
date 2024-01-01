using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Camera -> Cinemachine -> Virtual Camera -> Follow -> Player (수정 예정)
public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothFactor = 0.5f;

    private void LateUpdate()
    {
        Vector3 targetPosition = playerTransform.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
