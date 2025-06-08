using UnityEngine;
public enum MemoryType
    {
        Love,
        Community,
        Resilience,
        Joy,
        Wonder

    }
[CreateAssetMenu(menuName = "Memory/Memory Data")]
public class Memory : ScriptableObject
{
    
    public string memoryName;
    public MemoryType memoryType;
    [TextArea] public string memoryDesc;
    public Sprite memoryImage;
}
