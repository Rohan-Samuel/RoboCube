using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SyncAimTarget : MonoBehaviour
{
    [Header("The Robot (Source)")]
    [Tooltip("The Multi-Aim Constraint on the Robot that already has a target.")]
    public MultiAimConstraint robotConstraint;

    [Header("The Follower (Destination)")]
    [Tooltip("The Multi-Aim Constraint on the other object that should copy the Robot.")]
    public MultiAimConstraint followerConstraint;

    [Header("Settings")]
    [Tooltip("Update every frame? Disable if the target only changes once.")]
    public bool updateEveryFrame = true;

    private void Start()
    {
        robotConstraint = PlayerManager.instance.GetComponentInChildren<MultiAimConstraint>();

        SyncTarget();
    }

    private void Update()
    {
        if (updateEveryFrame)
        {
            SyncTarget();
        }
    }

    private void SyncTarget()
    {
        // Safety checks
        if (robotConstraint == null || followerConstraint == null) return;
        if (robotConstraint.data.sourceObjects.Count == 0) return;

        // 1. Get the target Transform from the Robot
        // We assume the main target is at index 0 (the first item in the list)
        Transform currentTarget = robotConstraint.data.sourceObjects[0].transform;

        // If the follower already has the same target, do nothing (optimization)
        if (followerConstraint.data.sourceObjects.Count > 0 &&
            followerConstraint.data.sourceObjects[0].transform == currentTarget)
        {
            return;
        }

        // 2. Prepare the data structure for modification
        // We must copy the struct, modify it, and paste it back
        var followerData = followerConstraint.data;
        var followerSources = followerData.sourceObjects;

        // 3. Update the source object
        if (followerSources.Count == 0)
        {
            // If the follower has no slots, add one
            followerSources.Add(new WeightedTransform(currentTarget, 1f));
        }
        else
        {
            // If it has a slot, just replace the transform at index 0
            followerSources.SetTransform(0, currentTarget);
            // Optional: Ensure weight is 1
            followerSources.SetWeight(0, 1f);
        }

        // 4. Apply changes back to the constraint
        followerData.sourceObjects = followerSources;
        followerConstraint.data = followerData;

        var rigBuilder = followerConstraint.GetComponentInParent<RigBuilder>();
        if (rigBuilder != null)
        {
            rigBuilder.Build();
        }
    }
}