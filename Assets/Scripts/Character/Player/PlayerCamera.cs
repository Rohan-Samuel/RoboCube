using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public PlayerManager player;
    public Camera cameraObject;
    [SerializeField] Transform cameraPivotTransform;
    
    
    //Change these, to tweak camera performance
    [Header("Camera Settings")]
    private float cameraSmoothSpeed = 1;  //Bigger the number, the longer for the camera to reach its position during movement
    [SerializeField] float leftAndRightRotationSpeed = 220;
    [SerializeField] float upAndDownRotationSpeed = 220;
    [SerializeField] float minimumPivot = -30; //Lowest point tou are able to look down
    [SerializeField] float maximumPivot = 60;  //Highest point you are able to look up
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;

    //Display camera values
    [Header("Camera values")] 
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition;
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    private float cameraZPosition;
    private float targetCameraZPostion;
        
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            HandleFollowTarget();
            HandleRotations();
            HandleCollisions();
        }
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleRotations()
    {
        //if locked on is a thing, force rotation
        //else rotate regularly

        //normal rotations
        //Rotate Left and Right Based on horizontal movement of the right joystick      
        leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
        //Rotate Up and Down based on the vertical movement of the right joystick
        upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) *Time.deltaTime;
        //Clam the up and down look angle between a min and max value
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        //Rotate this gameObject left and right
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        //Rotate this gameObject up and down
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions()
    {
        targetCameraZPostion = cameraZPosition;
        RaycastHit hit;
        //direction for collsion check
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        //check if there an object in front of our desired direction
        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPostion), collideWithLayers))
        {
            // if there is, we get out distance from it
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            // we then equate out target z position to the following
            targetCameraZPostion = -(distanceFromHitObject - cameraCollisionRadius);
        }

        // if you target position is less than out collision radius, we subtract out collision radius (making it snap back)
        if (Mathf.Abs(targetCameraZPostion) < cameraCollisionRadius)
        {
            targetCameraZPostion = -cameraCollisionRadius;
        }

        //we the napply our final position using a lerp over a time of 0.2f
        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPostion, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }
}
