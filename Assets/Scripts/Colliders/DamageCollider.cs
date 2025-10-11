using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Damage")]
    public float physicalDamage = 0;

    [Header("Contact Point")]
    protected Vector3 contactPoint;

    [Header("Characters Damaged")]
    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

    private void OnTriggerEnter(Collider other)
    {
       CharacterManager damageTarget = other.GetComponent<CharacterManager>();

        if (damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            //Check if we can damage the target

            //Check if target is blocking

            //Check if target is invulnerable

            //Damage
            DamageTarget(damageTarget);
        }

    }

    protected virtual void DamageTarget(CharacterManager damageTarget)
    {
       //We only apply damage once per attack
       if (charactersDamaged.Contains(damageTarget))
              return;

       charactersDamaged.Add(damageTarget);

        TakeHealthDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeHealthDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.contactPoint = contactPoint;

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

    }

}
