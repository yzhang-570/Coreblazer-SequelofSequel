using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IslandManager : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManagerScript;

    [SerializeField] GameObject questProgressUI;
    [SerializeField] GameObject questProgressText;

    [SerializeField] QuestList StartQuestList;
    [SerializeField] QuestList Island1QuestList;
    [SerializeField] QuestList Island2QuestList;
    [SerializeField] QuestList Island3QuestList;

    [SerializeField] GameObject StartQuestPlatforms;
    [SerializeField] GameObject Island1Platforms;
    [SerializeField] GameObject Island2Platforms;
    [SerializeField] GameObject Island3Platforms;

    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject Platform1Camera;
    [SerializeField] GameObject Platform2Camera;
    [SerializeField] GameObject Platform3Camera;

    [SerializeField] GameObject finalCutsceneCollider;

    private List<QuestList> allQuestsList;
    private List<GameObject> allPlatformsList;
    private List<GameObject> allCamerasList;
    private HashSet<string> completedQuests;
    [SerializeField] int islandNumber;
    [SerializeField] List<GameObject> islandsList;

    private HashSet<int> islandAnimationFinished = new HashSet<int>();

    private float startingDistance;

    void Start()
    {
        completedQuests = dialogueManagerScript.completedQuests;
        //islandNumber = 1;
        startingDistance = 10f;

        allQuestsList = new List<QuestList>() {
            StartQuestList, //buffer list
            Island1QuestList,
            Island2QuestList,
            Island3QuestList};

        allPlatformsList = new List<GameObject>()
        {
            StartQuestPlatforms, //buffer
            Island1Platforms,
            Island2Platforms,
            Island3Platforms,
        };

        allCamerasList = new List<GameObject>()
        {
            playerCamera,
            Platform1Camera,
            Platform2Camera,
            Platform3Camera
        };

        foreach(GameObject platformsParent in allPlatformsList)
        {
            Vector3 currPosition = platformsParent.transform.position;
            platformsParent.transform.position = new Vector3(currPosition.x, currPosition.y - startingDistance, currPosition.z);
            platformsParent.SetActive(false);
        }

        finalCutsceneCollider.SetActive(false);
        CheckCurrentIslandStatus();
        questProgressUI.SetActive(true);
}
    public void CheckCurrentIslandStatus()
    {
        if (islandNumber < allQuestsList.Count)
        {
            List<string> currentIslandQuests = allQuestsList[islandNumber].getQuestList();
            int numCompleteQuests = countCompleteQuests(currentIslandQuests);
            TextMeshProUGUI progressText = questProgressText.GetComponent<TextMeshProUGUI>();

            if (numCompleteQuests == currentIslandQuests.Count && !islandAnimationFinished.Contains(islandNumber))
            {
                islandAnimationFinished.Add(islandNumber);
                if(islandNumber != 0)
                {
                    GameObject platformParent = allPlatformsList[islandNumber];
                    StartCoroutine(startPlatformRisingAnimation(platformParent));
                }

                //islandNumber += 1;
                if(islandNumber == islandsList.Count - 1)
                {
                    finalCutsceneCollider.SetActive(true);
                }
                progressText.text = "Proceed to the next island";
                //questProgressUI.SetActive(true);
            }
            else
            {
                progressText.text = "Interact with NPCs (" + numCompleteQuests + "/" + currentIslandQuests.Count + ")";
            }
        }
    }

    public void updateIslandNumber(string collisionObjectName)
    {
        int newIslandNumber = getIslandNumber(collisionObjectName);
        if(newIslandNumber != -1 && newIslandNumber > islandNumber)
        {
            islandNumber = newIslandNumber;
            CheckCurrentIslandStatus();
            Debug.Log("island number updated to " + newIslandNumber);
        }
    }

    //checks if the gameObject is a island; returns -1 if not a island;
    public int getIslandNumber(string objectName)
    {
        for(int i = 0; i < islandsList.Count; i++)
        {
            if (islandsList[i].name == objectName)
            {
                return i;
            }
        }
        return -1;
    }

    public int countCompleteQuests(List<string> questsToCheck)
    {
        int numComplete = 0;
        foreach (string questName in questsToCheck)
        {
            Debug.Log("checking for " + questName);
            if (completedQuests.Contains(questName))
            {
                Debug.Log(questName + " completed");
                numComplete += 1;
            }
        }
        return numComplete;
    }

    //public void toggleQuestUI()
    //{
    //    if (islandNumber < allQuestsList.Count)
    //    {
    //        List<string> currentIslandQuests = allQuestsList[islandNumber].getQuestList();
    //        int numCompleteQuests = countCompleteQuests(currentIslandQuests);
    //        TextMeshProUGUI progressText = questProgressText.GetComponent<TextMeshProUGUI>();

    //        if (numCompleteQuests != currentIslandQuests.Count)
    //        {
    //            questProgressUI.SetActive(true);
    //            progressText.text = "Interact with NPCs (" + numCompleteQuests + "/" + currentIslandQuests.Count + ")";
    //        }
    //        else
    //        {
    //            questProgressUI.SetActive(false);
    //        }
    //    }
    //}
    private IEnumerator startPlatformRisingAnimation(GameObject platformParent)
    {
        allCamerasList[islandNumber].SetActive(true);
        playerCamera.SetActive(false);
        AudioManager.Instance.PlaySFX("rock");
        platformParent.SetActive(true);
        foreach (Transform child in platformParent.transform)
        {
            //Debug.Log("moving " + child);
            StartCoroutine(moveVertically(child.gameObject, startingDistance, 2f));
            yield return new WaitForSeconds(0.25f);
        }
        StartCoroutine(delayExecution(4f));
        playerCamera.SetActive(true);
        if (islandNumber < allQuestsList.Count)
        {
            allCamerasList[islandNumber].SetActive(false);
        }
    }

    private IEnumerator moveVertically(GameObject propToMove, float distanceY, float duration)
    {
        float startTime = Time.time;
        Vector3 currPosition = propToMove.transform.position;
        Vector3 newPosition = new Vector3(currPosition.x, currPosition.y + distanceY, currPosition.z);
        while (Time.time - startTime < duration)
        {
            propToMove.transform.position = Vector3.Lerp(currPosition, newPosition, (Time.time - startTime) / duration);
            yield return null;
        }
        yield break;
    }

    private IEnumerator delayExecution(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
