using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float horizontal = horizontalMovement;
        float vertical = verticalMovement;
        

        if (isSprinting)
        {
            vertical = 2;
        }

        character.animator.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
        character.animator.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(string targetAnimation, 
        bool isPerformingAction, 
        bool applyRootMotion = true, 
        bool canRotate = false, bool 
        canMove = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);

        //Can be used to stop character from attempting new actions
        //turns true if busy
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;
    }
}
