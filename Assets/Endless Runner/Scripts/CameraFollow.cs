using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 6f;

    private float offsetZ;
    private float offsetY;

    void Start()
    {
        offsetZ = transform.position.z - target.position.z;
        offsetY = transform.position.y - target.position.y;
    }

    void LateUpdate()
    {
        if (!target) return;

        float desiredZ = target.position.z + offsetZ;
        float desiredY = target.position.y + offsetY;

        Vector3 newPosition = transform.position;

        newPosition.z = Mathf.Lerp(
            transform.position.z,
            desiredZ,
            followSpeed * Time.deltaTime
        );

        newPosition.y = Mathf.Lerp(
            transform.position.y,
            desiredY,
            followSpeed * Time.deltaTime
        );

        transform.position = newPosition;
    }
}
