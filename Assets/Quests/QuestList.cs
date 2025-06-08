using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuestList", menuName = "QuestManager/QuestList")]
public class QuestList : ScriptableObject
{
    [SerializeField] List<string> questList;

    public List<string> getQuestList()
    {
        return questList;
    }
}
