using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class MemoryDisplayManager : MonoBehaviour
{

    [SerializeField] DialogueRunner dialogueRunner;
    //[SerializeField] MemoryData memoryData;
    [SerializeField] GameObject memoryObjectPrefab; //Memory object UI prefab
    [SerializeField] Transform contentPanel; //parent UI to hold memory objects
    [SerializeField] GameObject memoryMenu;
    [SerializeField] DialogueManager dialogueManagerScript;
    [SerializeField] UIInputHandler uiInputHandlerScript;
    [SerializeField] GameObject memoryGainedUI;
    List<GameObject> displayedMemoryUI;

    private void Start()
    {
        displayedMemoryUI = new List<GameObject>();
        dialogueRunner.AddCommandHandler<string, string>("take_memory", TakeMemory);
        dialogueRunner.AddFunction<string, bool>("check_has_memory_type", CheckHasMemoryType); //param: string, return: boolean
        dialogueRunner.AddCommandHandler<string>("obtain_memory", ObtainMemory);
    }

    public void RefreshUI()
    {
        //clear old UI
        foreach (GameObject memoryUI in displayedMemoryUI)
        {
            Destroy(memoryUI);
        }
        displayedMemoryUI.Clear();

        //update displayedUI - instantiate memoryPrefab, assign parent to panel
        foreach (Memory memory in MemoryData.MemoryList)
        {
            GameObject memoryUIObject = Instantiate(memoryObjectPrefab, contentPanel);

            TextMeshProUGUI nameText = memoryUIObject.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descriptionText = memoryUIObject.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
            Image memoryImage = memoryUIObject.transform.Find("MemoryImage").GetComponent<Image>();

            //update TMP title text, description text
            memoryUIObject.name = memory.memoryName; //GameObject name - used to see if memory exists?
            memoryImage.sprite = memory.memoryImage;
            nameText.text = memory.memoryName;
            descriptionText.text = memory.memoryDesc;

            //add to list of displayedUI to destroy later
            displayedMemoryUI.Add(memoryUIObject);
        }
    }

    private static readonly Regex sWhitespace = new Regex(@"\s+");
    public static string RemoveWhitespace(string input)
    {
        return sWhitespace.Replace(input, "");
    }

    public void TakeMemory(string npcName, string memoryName)
    {
        npcName = RemoveWhitespace(npcName);
        if (MemoryData.IsValidMemory(memoryName))
        {
            MemoryData.AddMemory(memoryName);
            //Debug.Log($"{memoryName} added to inventory");
            dialogueManagerScript.SetQuestComplete(npcName);

            //display and update memory gained UI
            //uiInputHandlerScript.ShowMemoryGained(memoryName);
            TextMeshProUGUI nameText = memoryGainedUI.transform.Find("MemoryName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descriptionText = memoryGainedUI.transform.Find("MemoryDesc").GetComponent<TextMeshProUGUI>();
            Image memoryImage = memoryGainedUI.transform.Find("MemoryImage").GetComponent<Image>();

            Memory memoryInfo = MemoryData.FindMemory(memoryName);
            nameText.text = memoryInfo.memoryName;
            descriptionText.text = memoryInfo.memoryDesc;
            memoryImage.sprite = memoryInfo.memoryImage;
            uiInputHandlerScript.ShowMemoryGainedUI();

            RefreshUI();
        }
        else
        {
            Debug.Log($"MEMORY NOT SUCCESSFULLY ADDED TO INVENTORY - Attemped to add memory that " +
                $"doesn't exist: {memoryName}\n" +
                $"Double check that the memory name exists in MemoryInfo dictionary " +
                $"(MemoryData.cs), and that the spelling in your YarnSpinner script matches " +
                $"it.");
        }
    }

    public void ObtainMemory(string memoryName)
    {
        MemoryData.AddMemory(memoryName);
        //Debug.Log($"{memoryName} added to inventory");

        //display and update memory gained UI
        TextMeshProUGUI nameText = memoryGainedUI.transform.Find("MemoryName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descriptionText = memoryGainedUI.transform.Find("MemoryDesc").GetComponent<TextMeshProUGUI>();
        Image memoryImage = memoryGainedUI.transform.Find("MemoryImage").GetComponent<Image>();

        Memory memoryInfo = MemoryData.FindMemory(memoryName);
        nameText.text = memoryInfo.memoryName;
        descriptionText.text = memoryInfo.memoryDesc;
        memoryImage.sprite = memoryInfo.memoryImage;
        uiInputHandlerScript.ShowMemoryGainedUI();

        RefreshUI();
    }

    public void GiveMemory(string npcName, string memoryName)
    {
        npcName = RemoveWhitespace(npcName);
        MemoryData.RemoveMemory(memoryName);
        //Debug.Log($"{memoryName} removed from inventory");
        //dialogueManagerScript.SetQuestComplete(npcName);
        //don't mark complete until both memories given
        RefreshUI();
    }

    //Dictionary<string, MemoryType> stringToType = new Dictionary<string, MemoryType>(){
    //    {"Community", MemoryType.Community},
    //    {"Joy", MemoryType.Community},
    //    {"Love", MemoryType.Community},
    //    {"Resilience", MemoryType.Community},
    //    {"Wonder", MemoryType.Community},
    //};
    public bool CheckHasMemoryType(string memoryType)
    {
        //Debug.Log("parsed memory type: " + type + " wanted memory type: " + memoryType);
        //Debug.Log($"Looking for memory type: {memoryType}");
        foreach (Memory m in MemoryData.MemoryList)
        {
            //Debug.Log($"have memory: {m.memoryName} of type {m.memoryType.ToString()}");
            if (m.memoryType.ToString() == memoryType)
            {
                //Debug.Log($"desired type found: {m.memoryName} is {memoryType}");
                return true;
            }
        }
        AudioManager.Instance.PlaySFX("negative");
        return false;
    }
}
