using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform firingCenter;
    public Transform muzzle;
    public Transform firingDirectionTarget;

    public GameObject bulletPrefab;

    [Header("Settings")]
    public float fireRate = 1f; // Bullets per second
    private float nextFireTime;
    void Update()
    {
   
    }
    public void Shoot(Quaternion targetDirection)
    {
        if (bulletPrefab != null && muzzle != null)
        {
            // Spawn the bullet at the muzzle position, matching the muzzle's rotation
            GameObject newBullet = Instantiate(bulletPrefab, muzzle.position, targetDirection);
        }
    }
    public void SetWeaponDamage()
    {

    }
}
