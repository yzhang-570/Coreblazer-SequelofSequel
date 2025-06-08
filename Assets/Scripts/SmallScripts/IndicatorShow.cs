using System;
using UnityEngine;

public class IndicatorShow : MonoBehaviour
{
    public GameObject memoryIndicator;
    public MemoryDisplayManager mdm;
    public Memory memoryToGive;
    private bool memoryAvailable = true;

    void Start()
    {
        memoryIndicator.SetActive(false);
    }
    void Update()
    {
        if (memoryIndicator.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log("Clicked on: " + hit.collider.name);

                    if (hit.collider.gameObject == memoryIndicator)
                    {
                        Debug.Log("Target sprite clicked!");
                        MemoryUnavailable();
                        mdm.ObtainMemory(memoryToGive.memoryName);
                    }
                }
            }
        
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && memoryAvailable)
        {
            Debug.Log("touched player");
            memoryIndicator.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && memoryAvailable)
        {
            memoryIndicator.SetActive(false);
        }
    }

    public void MemoryUnavailable()
    {
        memoryAvailable = false;
        memoryIndicator.SetActive(false);
    }
}
