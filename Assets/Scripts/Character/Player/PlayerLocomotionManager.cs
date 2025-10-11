using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;

    //taking values from InputManager
    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement Settings")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float sprintingSpeed = 5;
    [SerializeField] float rotationSpeed = 15;
    [SerializeField] int sprintingOverheatCost = 2;

    [Header("Jump")]
    private Vector3 jumpDirection;
    [SerializeField] float jumpHeight = 20;
    [SerializeField] private int jumpOverheatCost = 5;
    [SerializeField] float jumpForwardSpeed = 5;
    [SerializeField] float freeFallSpeed = 1;
    [SerializeField] float freeFallRotationSpeed = 2;

    [Header("Dodge")]
    private Vector3 dashDirection;
    [SerializeField] private int dodgeOverheatCost = 5;
    

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>(); 
    }
    public void HandleAllMovement()
    {
        //Grounded Movement
        HandleGroundedMovement();
        HandleRotation();

        //Aerial Movement
        HandleJumpingMovement();
        HandleFreeFallMovement();
    }

    private void GetVerticalAndHorizontalInputs()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;

        // CLAMP THE MOVEMENTS for animation
    }

    private void HandleGroundedMovement()
    {
        if (!player.canMove)
            return;

        GetVerticalAndHorizontalInputs();
        //Movement Direction is based on our cameras facing perspective and movement inputs
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (player.playerManager.isSprinting)
        {
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else
        {
            if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                //Run
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                //walk
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }

        
    }

    private void HandleJumpingMovement()
    {
        if (player.isJumping)
        {
            player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
        }
    }

    private void HandleFreeFallMovement()
    {
        if (!player.isGrounded)
        {
            Vector3 freeFallDirection;

            freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
            freeFallDirection += PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
            freeFallDirection.y = 0;

            player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);


            //For Aerial Rotation
            Vector3 freeFallRotation = Vector3.zero;

            freeFallRotation = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            freeFallRotation += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
            freeFallRotation.Normalize();
            freeFallRotation.y = 0;

            if ( freeFallRotation == Vector3.zero)
            {
                freeFallRotation = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(freeFallRotation);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, freeFallRotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
            
        }
    }

    private void HandleRotation()
    {
        if (!player.canRotate)
            return;

        Vector3 targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    public void HandleSprinting()
    {
     

        if (player.isPerformingAction) 
        {
            player.playerManager.isSprinting = false;
        }

        if (player.currentOverheating >= player.maxOverheating)
        {
            player.isSprinting = false;
            return;
        }

        //if overheated, set sprinting to false

        //if moving, set sprint to true
        if (moveAmount >= 0.5)
        {
            player.playerManager.isSprinting = true;
        }
        //if slow or stationary, set it to false
        else
        {
            player.playerManager.isSprinting = false;
        }

        if (player.isSprinting) 
        { 
            player.currentOverheating += sprintingOverheatCost * Time.deltaTime;
            //Debug.Log(player.currentOverheating);
        }
    }

    public void AttemptToPerformDodge()
    {
        if (player.isPerformingAction)
            return;

        if (player.isJumping)
            return;

        if (player.currentOverheating >= player.maxOverheating)
            return;
        

        //if we are moving while dodging, dash in that direction
        if (PlayerInputManager.instance.moveAmount > 0)
        {
            dashDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            dashDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;

            dashDirection.y = 0;
            dashDirection.Normalize();

            Quaternion playerRotation = Quaternion.LookRotation(dashDirection);
            player.transform.rotation = playerRotation;

            player.playerAnimatorManager.PlayTargetActionAnimation("DashForward", true, true);
        }
        // else dash forward
        else
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("DashForward", true, true);
        }

        player.currentOverheating += dodgeOverheatCost;
    }

    public void AttemptToPerformJump()
    {
        if (player.isPerformingAction)
            return;

        if (player.currentOverheating >= player.maxOverheating)
            return;

        if (player.isJumping)
            return;

        if (!player.isGrounded)
            return;

        player.playerAnimatorManager.PlayTargetActionAnimation("Jump3_Start", false);

        player.isJumping = true;

        player.currentOverheating += jumpOverheatCost;

        jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
        jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
        jumpDirection.y = 0;

        if (jumpDirection != Vector3.zero)
        {
            //If sprinting
            if (player.playerManager.isSprinting)
            {
                jumpDirection *= 1;
            }
            //If running
            else if (PlayerInputManager.instance.moveAmount > 0.5)
            {
                jumpDirection *= 0.5f;
            }
            //If walking
            else if (PlayerInputManager.instance.moveAmount < 0.5)
            {
                jumpDirection *= 0.25f;
            }
        }
        
        
    }

    public void ApplyJumpingVelocity()
    {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
    }
}
