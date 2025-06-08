using UnityEngine;
using System.Collections.Generic;
public class RespawnScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //[SerializeField] GameObject respawnsFolder;
    [SerializeField] UIInputHandler uiInputHandlerScript;
    [SerializeField] List<GameObject> checkpointTriggers; //triggers
    [SerializeField] List<GameObject> respawnPositions; //places to respawn to
    [SerializeField] int currentCheckPoint;

    private void Start()
    {
        /*
         * replace with version editable in unity editor
         * add all checkpoints in order
         * on touch - if name found in checkpoints, and index (=checkpoint #) > current checkpoint
         *      update checkpoint
         */
        currentCheckPoint = 0;
        if(checkpointTriggers.Count != respawnPositions.Count)
        {
            Debug.Log("# checkpoints and # triggers don't match; check [SerializeField]s for RespawnScript.cs in GlobalKillBlock");
        }
    }

    //checks if the gameObject is a trigger; returns -1 if not a trigger;
    private int findCheckpointNumber(string gameObjectName)
    {
        for (int i = 0; i < checkpointTriggers.Count; i += 1)
        {
            GameObject checkTriggerName = checkpointTriggers[i];
            if(checkTriggerName.gameObject.name == gameObjectName)
            {
                return i;
            }
        }
        return -1;
    }

    //note: index of respawn positions in respawnPositions corresponds to checkpoint number
    private void OnCollisionEnter(Collision collision)
    {
        GameObject player = collision.gameObject;
        Vector3 checkpointPosition = respawnPositions[currentCheckPoint].transform.position;
        Vector3 respawnPosition = new Vector3(checkpointPosition.x, checkpointPosition.y, checkpointPosition.z - 0.2f);
        player.transform.position = respawnPosition;
    }

    public void updateCheckpoint(string gameObjectName)
    {
        int newCheckpointNumber = findCheckpointNumber(gameObjectName);
        if (newCheckpointNumber != -1)
        {
            if (newCheckpointNumber > currentCheckPoint)
            {
                currentCheckPoint = newCheckpointNumber;
                uiInputHandlerScript.ShowNewSpawnPointUI();
            }
            //Debug.Log(currentCheckPoint);
        }
        else
        {
            //Debug.Log(gameObjectName + "'s checkpoint number not found in respawnModifierCheckpoints");
        }
    }
}
