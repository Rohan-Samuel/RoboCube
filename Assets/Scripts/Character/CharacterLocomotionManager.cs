using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Ground Check & Jumping")]
    [SerializeField] protected float gravityForce = -5.55f;
    [SerializeField] float groundcheckSphereRadius = 1;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] protected Vector3 yVelocity;   //Force at which the character is pulled up or down
    [SerializeField] protected float groundedVelocity = -20;  //Force at which character sticks to the ground while grounded
    [SerializeField] protected float fallStartYVelocity = -5;  //Force at which the character begins to fall when becoming ungrounded (Rises with time ungrounded)
    [SerializeField] protected float characterWidth = 0.2f; //For the groundcheck spheres
    protected bool fallingVelocityHasBeenSet = false;
    protected float inAirTimer = 0;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
        HandleGroundCheck();

        if (character.isGrounded)
        {
            //If we are not jumping or attempting to move upward
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocityHasBeenSet = false;
                yVelocity.y = groundedVelocity;
            }
        }
        else
        {
            if (!character.isJumping && !fallingVelocityHasBeenSet)
            {
                fallingVelocityHasBeenSet = true;
                yVelocity.y = fallStartYVelocity;
            }

            inAirTimer += Time.deltaTime;
            character.animator.SetFloat("InAirTimer", inAirTimer);
            yVelocity.y += gravityForce * Time.deltaTime;

         
        }

        //Always have downward force on character
        character.characterController.Move(yVelocity * Time.deltaTime);
    }

    protected void HandleGroundCheck()
    {
        Vector3 frontRight = new Vector3(characterWidth, 0, characterWidth);
        Vector3 frontLeft = new Vector3(-characterWidth, 0, characterWidth);
        Vector3 backRight = new Vector3(characterWidth, 0, -characterWidth);
        Vector3 backLeft = new Vector3(-characterWidth, 0, -characterWidth);

        character.isGrounded = Physics.CheckSphere(character.transform.position, groundcheckSphereRadius, groundLayer);
        
       /* if (!character.isGrounded)
        {
            //Check all 4 corners of the character to see if any are grounded
            if (Physics.CheckSphere(character.transform.position + frontRight, groundcheckSphereRadius, groundLayer) ||
                Physics.CheckSphere(character.transform.position + frontLeft, groundcheckSphereRadius, groundLayer) ||
                Physics.CheckSphere(character.transform.position + backRight, groundcheckSphereRadius, groundLayer) ||
                Physics.CheckSphere(character.transform.position + backLeft, groundcheckSphereRadius, groundLayer))
            {
                character.isGrounded = true;
            }
        }*/
        
    }

    protected void OnDrawGizmosSelected()
    {
        Vector3 frontRight = new Vector3(characterWidth, 0, characterWidth);
        Vector3 frontLeft = new Vector3(-characterWidth , 0, characterWidth);
        Vector3 backRight = new Vector3(characterWidth, 0, -characterWidth);
        Vector3 backLeft = new Vector3(-characterWidth, 0, -characterWidth);

        Gizmos.DrawSphere(character.transform.position + frontRight, groundcheckSphereRadius);
        Gizmos.DrawSphere(character.transform.position + frontLeft, groundcheckSphereRadius);
        Gizmos.DrawSphere(character.transform.position + backLeft, groundcheckSphereRadius);
        Gizmos.DrawSphere(character.transform.position + backRight, groundcheckSphereRadius);
    }
}
