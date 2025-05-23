using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float minMoveSpeed;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask climbLayerMask;
    private Vector2 curMovementInput;
    
    [Header("Look")]
    [SerializeField] private Transform cameraContainer;
    [SerializeField] private float minXLook;
    [SerializeField] private float maxXLook;
    [SerializeField] private float lookSensitivity;
    private float camCurXRot;
    
    const string cameraContainerName = "CameraContainer";
    
    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody rigidBody;

    private InputActionAsset playerInputAsset;
    
    Coroutine speedCoroutine = null;
    Coroutine jumpCoroutine = null;
    
    private bool isOnMovingFlatform = false;
    private MovingPlatform curPlatform = null;
    
    //----extrajump
    private int maxJumpCount = 1;
    private int curJumpCount = 0;
    
    //-----stamina
    [SerializeField] private float useStamina = 5f;
    
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        cameraContainer = transform.Find(cameraContainerName).transform;
        playerInputAsset = GetComponent<PlayerInput>().actions;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        playerInputAsset["Move"].performed += OnMoveInput;
        playerInputAsset["Move"].canceled += OnMoveInput;

        playerInputAsset["Jump"].started += OnJumpInput;
        //playerInputAsset["Jump"].performed += OnJumpInput;

        playerInputAsset["Look"].started += OnLookInput;
        playerInputAsset["Look"].canceled += OnLookInput;

        playerInputAsset["Interact"].started += OnInteractInput;
    }

    private void OnDisable()
    {
        playerInputAsset["Move"].performed -= OnMoveInput;
        playerInputAsset["Move"].canceled -= OnMoveInput;
        
        playerInputAsset["Jump"].started-= OnJumpInput;
        //playerInputAsset["Jump"].performed -= OnJumpInput;
        
        playerInputAsset["Look"].started -= OnLookInput;
        playerInputAsset["Look"].canceled -= OnLookInput;

        playerInputAsset["Interact"].started -= OnInteractInput;
    }
    
    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }
    
    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        curMovementInput = context.ReadValue<Vector2>();
    }
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            curJumpCount = 0;
        }
        if(context.phase == InputActionPhase.Started && curJumpCount < maxJumpCount)
        {
            ++curJumpCount;
            rigidBody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            CharacterManager.Instance.Player.playerCondition.UseStamina(useStamina);
        }
    }
    
    private void Move()
    {
        Vector3 dir;
        if (IsClimbable())
        {
            dir = transform.up * curMovementInput.y;
            dir *= moveSpeed;
            dir.x = dir.z = rigidBody.velocity.x;
        }
        else
        {
            dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
            dir *= moveSpeed;
            dir.y = rigidBody.velocity.y;
        }

        if (isOnMovingFlatform && curPlatform != null)
        {
            dir += curPlatform.velocity;
            if(curPlatform.IsMovingY())
            {
                dir.y = curPlatform.velocity.y;
            }
        }
        rigidBody.velocity = dir;
    }
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsClimbable()
    {
        Ray[] rays = new Ray[]
        {
            new Ray(transform.position + (transform.up * 0.5f) + (-transform.forward * 0.01f), transform.forward),
            new Ray(transform.position + (transform.up * 0.01f) + (-transform.forward * 0.01f), transform.forward)
        };

        for(int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.3f, climbLayerMask))
            {
                return true;
            }
        }

        return false;
    }
    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for(int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
    //----------- 속도 변화
    public void AddSpeed(float value, float time)
    {
        if (speedCoroutine != null)
        {
            StopCoroutine(speedCoroutine);
            speedCoroutine = null;
        }
        speedCoroutine = StartCoroutine(ChangeSpeed(value, time));
    }

    IEnumerator ChangeSpeed(float value, float time)
    {
        moveSpeed += value;
        yield return new WaitForSeconds(time);
        moveSpeed -= value;
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        GetComponent<Interaction>().Interact();
    }
    //------------발판 밟음 여부
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("MovingPlatform"))
        {
            isOnMovingFlatform = true;
            curPlatform = collision.collider.GetComponent<MovingPlatform>();
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if(collision.collider.CompareTag("MovingPlatform"))
        {
            isOnMovingFlatform = false;
            curPlatform = null;
        }
    }
    //-------------추가 점프
    public void EnableExtraJump(int value, float time)
    {
        if (jumpCoroutine != null)
        {
            StopCoroutine(jumpCoroutine);
        }

        jumpCoroutine = StartCoroutine(ChangeMaxJumpCount(value, time));
    }

    IEnumerator ChangeMaxJumpCount(int value, float time)
    {
        maxJumpCount += value;
        yield return new WaitForSeconds(time);
        maxJumpCount -= value;
    }
}
