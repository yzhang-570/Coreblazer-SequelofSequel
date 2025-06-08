using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Memory/Memory Inventory")]
public class MemoryData : ScriptableObject
{
    public List<Memory> NonStaticAllMemories;
    static List<Memory> AllMemories;

    [SerializeField]
    public static List<Memory> MemoryList = new List<Memory>();
    public void Convert()
    {
        AllMemories = new List<Memory>();
        foreach (Memory m in NonStaticAllMemories)
        {
            AllMemories.Add(m);
        }
        MemoryList.Clear(); //reset player inventory on play
    }
    public static bool IsValidMemory(string memoryName)
    {
        //Debug.Log("Checking: " + memoryName);
        foreach (Memory m in AllMemories)
        {
            //Debug.Log(m.memoryName);
            if (memoryName == m.memoryName)
            {
                return true;
            }
        }
        return false;
    }

    public static void AddMemory(string memoryName) => MemoryList.Add(FindMemory(memoryName));
    public static void RemoveMemory(string memoryName) => MemoryList.Remove(FindMemory(memoryName));

    public static Memory FindMemory(string memoryName)
    {
        int i = 0;
        while (AllMemories[i].memoryName != memoryName)
        {
            i++;
        }
        return AllMemories[i];
    }

    public static string GetMemoryType(string m)
    {
        return FindMemory(m).memoryType.ToString();
    }
}
