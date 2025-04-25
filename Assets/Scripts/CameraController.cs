using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    private void LateUpdate()
    {
        Vector3 targetPosition = targetTransform.position;
        targetPosition.z = -10;
        transform.position = targetTransform.position;
    }
}
