using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;
    public WeaponItem currentWeaponBeingUsed;

    public Transform target;
    public MultiAimConstraint headAim;

    override protected void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
        //headAim = PlayerManager.instance.GetComponentInChildren<MultiAimConstraint>();
    }

    private void Update()
    {
       SetTarget();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponItemAction, WeaponItem weaponItem)
    {
        if (weaponItemAction != null && weaponItem != null)
        {
            weaponItemAction.PerformAction(weaponItem, player);
        }
    }

    public void SetTarget()
    {
        if (headAim.data.sourceObjects.Count > 0 && headAim.data.sourceObjects[0].transform == target)
        {
            return;
        }

        var headAimData = headAim.data;
        var headAimSources = headAimData.sourceObjects;

        if (headAimSources.Count == 0)
        {
            headAimSources.Add(new WeightedTransform(target, 0.5f));
        }
        else
        {
            headAimSources.SetTransform(0, target);
        }

        headAimData.sourceObjects = headAimSources;
        headAim.data = headAimData;

        var rigBuilder = headAim.GetComponentInParent<RigBuilder>();
        if (rigBuilder != null)
        {
            rigBuilder.Build();
        }

    }
}
