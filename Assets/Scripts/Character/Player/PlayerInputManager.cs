using UnityEngine;


public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerManager player;

    PlayerControls playerControls;


    [Header("CAMERA MOVEMENT INPUT")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    [Header("LOCK ON INPUT")]
    [SerializeField] bool lockOn_Input;

    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontal_Input;
    public float moveAmount;

    [Header("PLAYER ACTION INPUT")]
    [SerializeField] bool dodge_Input = false;
    [SerializeField] bool sprint_Input = false;
    [SerializeField] bool jump_Input = false;
    [SerializeField] bool R1_Input = false;

    



    //May not work, check Ep 2 in case.
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
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();

            //for mouse movement (not perfect)
            //playerControls.PlayerCamera.Movement1.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Dodge.performed += i => dodge_Input = true;
            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
            playerControls.PlayerActions.R1.performed += i => R1_Input = true;

            //Lock ON
            playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;

            //Holding the input sets the bool to true
            playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
            //realsing the input changes the bool to false
            playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;

        }

        playerControls.Enable();
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleSprintInput();
        HandleJumpInput();
        HandleR1Input();
        HandleLockOnInput();

    }

    //Lock On

    private void HandleLockOnInput()
    {
        if (lockOn_Input)
        {
            lockOn_Input = false;
            //Attempt to Lock on
        }
    }

    //Movement

    private void HandlePlayerMovementInput()
    {
        verticalInput = movementInput.y;
        horizontal_Input = movementInput.x;

        //Returns the absolute number
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontal_Input));


        //Clamp the values at 0, 0.5, or 1
        if (moveAmount <= 0.5 && moveAmount > 0 )
        {
            moveAmount = 0.5f;

        }
        else if (moveAmount > 0.5 && moveAmount <= 1 )
        {
            moveAmount = 1;
        }

        //We pass 0 on horizontal because we are not locked on
        //we may use horizontal on locking on (if strafing)

        if (player == null)
            return;
        
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerManager.isSprinting);
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    //ACTION

    private void HandleDodgeInput()
    {
        if (dodge_Input)
        {
            dodge_Input = false;

            //future: do dothing if menu or ui is open (maybe)

            player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }

    private void HandleSprintInput()
    {
        if (sprint_Input)
        {
            player.playerLocomotionManager.HandleSprinting();
        }

        else
        {
            player.playerManager.isSprinting = false;
        }
    }

    private void HandleJumpInput()
    {
        if (jump_Input)
        {
            jump_Input = false;

            //Disable while UI is open

            //attempt to jump
            player.playerLocomotionManager.AttemptToPerformJump();
        }
    }

    private void HandleR1Input()
    {
        if (R1_Input)
        {
            R1_Input = false;

            //Attack
            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentWeapon.r1Action, player.playerInventoryManager.currentWeapon);
        }
    }
}

