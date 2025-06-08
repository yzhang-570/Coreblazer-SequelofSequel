using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;
using Unity.Cinemachine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody rb;
    [SerializeField] float weight_max14; //max is double of moveSpeed
    [SerializeField] MemoryData memoryData;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float rotationSpeed;
    [SerializeField] DialogueRunner dialogueRunner; //for detecting if dialogue is running
    [SerializeField] RespawnScript respawnScript;
    [SerializeField] float max_distance_from_ground;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject parkourCamera;
    [SerializeField] IslandManager questProgressScript;
    
    private float moveX;
    private float moveY;
    private bool isGrounded;
    private bool jumpSound;
    private bool jumpCooldownFinished;

    private PlayerInputActions inputActions;
    private Vector3 currentDirection = Vector3.forward;

    private bool movementEnabled = true;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void Start()
    {
        memoryData.Convert();
        jumpCooldownFinished = true;
    }
    private void OnEnable()
    {
        inputActions.Player.Enable(); //start listening for input
    }

    private void OnDisable()
    {
        inputActions.Player.Disable(); //stop listening
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        movementEnabled = isEnabled;
    }

    private void Update()
    {
        //disable character movement during dialogue
        if (!dialogueRunner.IsDialogueRunning && movementEnabled)
        {
            moveX = inputActions.Player.Move.ReadValue<Vector2>().x; //in Vector3 - (x, 0, 0)
            moveY = inputActions.Player.Move.ReadValue<Vector2>().y; //in Vector 3 - (0, 0, z/y)

            //movement
            weight_max14 = MemoryData.MemoryList.Count;

            //NEW MOVEMENT
            //Vector3 moveDir = (transform.forward * moveY).normalized;
            //transform.position += moveDir * (moveSpeed - weight_max14 / 2f) * Time.deltaTime;

            //OLD MOVEMENT
            transform.position += new Vector3(moveX, 0f, moveY) * (moveSpeed - weight_max14 / 2) * Time.deltaTime;

            //for physics based movement: rb.AddForce(new Vector3(moveX, 0f, moveY) * 2f * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);

            Debug.DrawRay(transform.position, -transform.up * 10f, Color.red);
            //ground check
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hits, max_distance_from_ground))
            {
                isGrounded = true; //if not far from ground
                if (jumpSound)
                {
                    AudioManager.Instance.PlaySFX("tap");
                    jumpSound = false;
                }
            }
            else
            {
                isGrounded = false;
                jumpSound = true;
            }

            //NEW ROTATION
            // If there's X input, change the look direction (horizontal input only affects direction)
            //if (Mathf.Abs(moveX) > 0.1f)
            //{
            //    // Rotate left/right on horizontal input
            //    currentDirection = Quaternion.Euler(0, moveX * rotationSpeed, 0) * currentDirection;
            //}
            //if (currentDirection != Vector3.zero)
            //{
            //    Quaternion targetRotation = Quaternion.LookRotation(currentDirection);
            //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            //}

            //OLD ROTATION
            Vector3 direction = new Vector3(moveX, 0f, moveY).normalized;
            float magnitude = new Vector3(moveX, 0f, moveY).magnitude;
            if (magnitude > 0f)
            {
                Quaternion current = transform.rotation;
                //Quaternion rotation = Quaternion.LookRotation(npcPosition.transform.position - transform.position);
                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotationSpeed);
            }


        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        respawnScript.updateCheckpoint(collision.gameObject.name);
        if(collision.gameObject.transform.parent)
        {
            if (collision.gameObject.transform.parent.name.Contains("Parkour"))
            {
                parkourCamera.SetActive(true);
                playerCamera.SetActive(false);
            }
            else if (collision.gameObject.name.Contains("Island") || 
                collision.gameObject.transform.parent && collision.gameObject.transform.parent.name.Contains("Island"))
            {
                playerCamera.SetActive(true);
                parkourCamera.SetActive(false);
                questProgressScript.toggleQuestUI();
            }
        }
    }

    private void OnJump()
    {
        if (isGrounded && jumpCooldownFinished)
        {
            StartCoroutine(runJumpCooldown());
            isGrounded = false;
            rb.AddForce(Vector3.up * (jumpForce - weight_max14/4), ForceMode.Impulse);
        }
    }

    public void faceNPC(Transform npcPosition)
    {
        //transform.LookAt(npcPosition.transform); //snappy rotate to face if u like that better :>
        StartCoroutine(rotateOverTime(npcPosition.position));
    }

    public IEnumerator rotateOverTime(Vector3 npcPosition)
    {
        float startTime = Time.time;
        float duration = 2.0f;
        while(Time.time - startTime < duration)
        {
            Quaternion current = transform.rotation;
            Vector3 npcPosLevelWithPlayer = new Vector3(npcPosition.x, transform.position.y, npcPosition.z);
            //so that player doesn't start looking into the sky or into the ground LMFAO
            Quaternion rotation = Quaternion.LookRotation(npcPosLevelWithPlayer - transform.position);
            transform.rotation = Quaternion.Slerp(current, rotation, (Time.time - startTime)/duration); //gives fraction of rotation complete
            yield return null;
        }
        yield break;
    }

    private IEnumerator runJumpCooldown()
    {
        jumpCooldownFinished = false;
        yield return new WaitForSeconds(0.1f);
        jumpCooldownFinished = true;
    }
}