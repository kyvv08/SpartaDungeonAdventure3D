using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayerMask;
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

    private Rigidbody rigidbody;

    private InputActionAsset playerInputAsset;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
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

        playerInputAsset["Look"].started += OnLookInput;
        playerInputAsset["Look"].canceled += lamda =>{mouseDelta = Vector2.zero;};
}

    private void OnDisable()
    {
        playerInputAsset["Move"].performed -= OnMoveInput;
        playerInputAsset["Move"].canceled -= OnMoveInput;
        
        playerInputAsset["Jump"].started-= OnJumpInput;
        
        playerInputAsset["Look"].started -= OnLookInput;
        playerInputAsset["Look"].canceled -= lamda =>{mouseDelta = Vector2.zero;};
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
        if(context.phase == InputActionPhase.Started && IsGrounded())
        {
            GetComponent<Rigidbody>().AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }
    
    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = GetComponent<Rigidbody>().velocity.y;

        GetComponent<Rigidbody>().velocity = dir;
    }
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
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
}
