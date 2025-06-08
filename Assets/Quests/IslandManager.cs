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
        }

        finalCutsceneCollider.SetActive(false);
    }
    public void CheckCurrentIslandStatus()
    {
        if (islandNumber < allQuestsList.Count)
        {
            List<string> currentIslandQuests = allQuestsList[islandNumber].getQuestList();
            int numCompleteQuests = countCompleteQuests(currentIslandQuests);
            TextMeshProUGUI progressText = questProgressText.GetComponent<TextMeshProUGUI>();

            if (numCompleteQuests == currentIslandQuests.Count)
            {
                //Debug.Log(numCompleteQuests + "/" + currentIslandQuests.Count + " quests complete");
                //Debug.Log("Island" + islandNumber + " quests complete");
                questProgressUI.SetActive(false);

                GameObject platformParent = allPlatformsList[islandNumber];
                //play platform animation
                StartCoroutine(startPlatformRisingAnimation(platformParent));

                islandNumber += 1;
                if(islandNumber == 4)
                {
                    //Debug.Log("final trigger activated");
                    finalCutsceneCollider.SetActive(true);
                }
            }
            else
            {
                progressText.text = "Interact with NPCs (" + numCompleteQuests + "/" + currentIslandQuests.Count + ")";
                //Debug.Log(numCompleteQuests + "/" + currentIslandQuests.Count + " quests complete");
            }
        }
    }

    private IEnumerator startPlatformRisingAnimation(GameObject platformParent)
    {
        allCamerasList[islandNumber].SetActive(true);
        playerCamera.SetActive(false);
        AudioManager.Instance.PlaySFX("rock");
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

    public int countCompleteQuests(List<string> questsToCheck)
    {
        int numComplete = 0;
        foreach (string questName in questsToCheck)
        {
            if (completedQuests.Contains(questName))
            {
                numComplete += 1;
            }
        }
        return numComplete;
    }

    public void toggleQuestUI()
    {
        if (islandNumber < allQuestsList.Count)
        {
            List<string> currentIslandQuests = allQuestsList[islandNumber].getQuestList();
            int numCompleteQuests = countCompleteQuests(currentIslandQuests);
            TextMeshProUGUI progressText = questProgressText.GetComponent<TextMeshProUGUI>();

            if (numCompleteQuests != currentIslandQuests.Count)
            {
                questProgressUI.SetActive(true);
                progressText.text = "Interact with NPCs (" + numCompleteQuests + "/" + currentIslandQuests.Count + ")";
            }
            else
            {
                questProgressUI.SetActive(false);
            }
        }
    }

}
