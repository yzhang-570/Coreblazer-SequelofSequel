//using UnityEngine;
//using System.Collections.Generic;

//[CreateAssetMenu(fileName = "Checkpoints", menuName = "Scriptable Objects/Checkpoints")]
//public class Checkpoints : ScriptableObject
//{
//    public List<GameObject> checkpointsList;
//    public GameObject test;
//}

using UnityEngine;

[CreateAssetMenu(fileName = "TestSO", menuName = "Test/TestSO")]
public class TestSO : ScriptableObject
{
    [SerializeField]
    private GameObject test;
}