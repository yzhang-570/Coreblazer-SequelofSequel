using UnityEngine;

public class IndicatorRotate : MonoBehaviour
{
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
