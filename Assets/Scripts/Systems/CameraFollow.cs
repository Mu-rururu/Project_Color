using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Vector3 offset;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollow: target is missing.");
            return;
        }

        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;
    }
}