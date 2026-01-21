using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private GameObject coinMesh;
    [SerializeField] private float rotationSpeed = 150f;

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}
