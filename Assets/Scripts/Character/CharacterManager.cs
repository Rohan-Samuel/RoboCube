using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Status")]
    public bool isDead = false;

    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool isSprinting = false;
    public bool isGrounded = true;
    public bool isJumping = false;

    [Header("Stats")]
    public int durability = 10;
    public int coolant = 1;

    [Header("Resources")]
    public float currentHealth;
    public int maxHealth;

    public float currentOverheating = 0;
    public int maxOverheating;


    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
    }

    protected virtual void Update()
    {
        animator.SetBool("IsGrounded", isGrounded);
    }    

    protected virtual void LateUpdate()
    {

    }

    public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        currentHealth = 0;
        isDead = true;

        //Reset Flags Here

        //If we are not grounded, play aerial death animation

        if (!manuallySelectDeathAnimation)
        {
            characterAnimatorManager.PlayTargetActionAnimation("Death", true);
        }

        //Play Death SFX

        yield return new WaitForSeconds(5);

        //Award players with runes/chips
    }

    public virtual void ReviveCharacter()
    {
       
    }



}
