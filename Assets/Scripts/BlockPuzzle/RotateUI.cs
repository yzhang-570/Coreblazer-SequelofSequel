using UnityEngine;

public class RotateUI : MonoBehaviour
{
    public float rotationSpeed; // Degrees per second

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
