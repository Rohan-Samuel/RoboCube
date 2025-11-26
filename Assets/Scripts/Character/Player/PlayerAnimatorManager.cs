
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager player;
    MultiAimConstraint headAim;

    public float headRotationSpeed = 1f;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
       
    }

    private void Start()
    {
        headAim = PlayerManager.instance.GetComponentInChildren<MultiAimConstraint>();
    }

    private void Update()
    {
        RotationDamping();
    }

    private void OnAnimatorMove()
    {
        if (player.applyRootMotion)
        {
            Vector3 velocity = player.animator.deltaPosition;
            player.characterController.Move(velocity);
            player.transform.rotation *= player.animator.deltaRotation;

        }
    }

    private void RotationDamping()
    {
        Vector3 directionToTarget = headAim.data.sourceObjects[0].transform.position - player.transform.position;
        float angleToTarget = Vector3.Angle(player.transform.forward, directionToTarget);


        Rig rig = headAim.GetComponentInParent<Rig>();

        if (angleToTarget > 140f)
        {
            rig.weight = Mathf.Lerp(rig.weight, 0, Time.deltaTime * headRotationSpeed);
            
        }
        else if (angleToTarget > 110f)
        {
            rig.weight = Mathf.Lerp(rig.weight, .7f, Time.deltaTime * headRotationSpeed);

        }
        else
        {
            rig.weight = Mathf.Lerp(rig.weight, 1, Time.deltaTime * headRotationSpeed);
        }




    }
}
