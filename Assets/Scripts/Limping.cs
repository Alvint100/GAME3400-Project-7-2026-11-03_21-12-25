
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Limping : MonoBehaviour
{
    private CharacterController playerController;
    private Transform cameraBob;
    private Transform playerCamera;

    [Header("Walking")]
    [SerializeField] float stepFrequency = 2f;
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float strongStepSpeed = 1f;
    [SerializeField] float weakStepSpeed = -1f;

    [Header("Camera Turning")]
    [SerializeField] float lookSensitivityX = 1f;
    [SerializeField] float lookSensitivityY = 1f;
    [SerializeField] float minYLookAngle = -90f;
    [SerializeField] float maxYLookAngle = 90f;
    private float verticalRotation;
    
    [Header("Camera Bob")]
    [SerializeField] float strongStepBob = 0.08f;
    [SerializeField] float weakStepBob = 0.025f;
    [SerializeField] float sideBob = 0.04f;
    [SerializeField] float cameraTilt = 3f;

    private Vector3 startPos;
    private Quaternion startRot;
    private float bobTimer;

    void Start()
    {
        playerController = GetComponent<CharacterController>();
        cameraBob = transform.GetChild(0).transform;
        playerCamera = cameraBob.GetChild(0).transform;
        
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
        startPos = cameraBob.localPosition;
        startRot = cameraBob.localRotation;
    }

    void Update()
    {
        Vector3 moveDir = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
        if (moveDir.magnitude >= 0.1f) {
            playerController.Move(moveDir.normalized * ((normalSpeed + UpdateLimp()) * Time.deltaTime));   
        }

        UpdateLook();
    }

    void UpdateLook()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * lookSensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * lookSensitivityY;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, minYLookAngle, maxYLookAngle);

        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    float UpdateLimp()
    {
        bobTimer += Time.deltaTime * stepFrequency;
        
        float walkCycle = Mathf.Repeat(bobTimer, 2f * Mathf.PI);
        bool strongStep = walkCycle < Mathf.PI;
        float stepProgress = (walkCycle % Mathf.PI) / Mathf.PI;

        float bobStrength = strongStep ? strongStepBob : weakStepBob;
        float speedOffset = strongStep ? strongStepSpeed : weakStepSpeed;

        float verticalBob = Mathf.Sin(stepProgress * Mathf.PI) * bobStrength;
        float sidewaysBob = Mathf.Cos(bobTimer) * sideBob;

        cameraBob.localPosition = startPos + new Vector3(sidewaysBob, -verticalBob, 0f);

        float tilt = Mathf.Sin(bobTimer) * cameraTilt;

        cameraBob.localRotation = startRot * Quaternion.Euler(0f, 0f, tilt);
        
        return speedOffset;
    }
}
