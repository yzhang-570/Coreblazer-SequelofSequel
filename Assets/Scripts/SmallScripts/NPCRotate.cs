using UnityEngine;

public class NPCTurnToFace : MonoBehaviour
{
    private bool turnToFace;
    [SerializeField] GameObject myModel;
    [SerializeField] GameObject player;
    [SerializeField] float rotationSpeed;

    private void Start()
    {
        turnToFace = false;
    }

    void Update()
    {
        if(turnToFace == true)
        {
            Vector3 direction = player.transform.position - myModel.transform.position;
            Vector3 directionOnGround = new Vector3(direction.x, 0, direction.z);
            if(direction != Vector3.zero) //if not already facing player
            {
                Quaternion current = myModel.transform.rotation;
                Quaternion rotation = Quaternion.LookRotation(directionOnGround, Vector3.up);
                myModel.transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player" || other.gameObject.name == "ActualPlayerModel")
        {
            turnToFace = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        turnToFace = false;
    }
}
