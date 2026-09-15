using UnityEngine;

public class RaycastProjectile : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 30f;
    public float lifetime = 3f;

    [Header("Raycast Settings")]
    public LayerMask hitLayers;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
        Destroy(gameObject, lifetime); // Safety net to clean up missed bullets
    }

    void Update()
    {
        // 1. Calculate how far the bullet should move this frame
        float movementStep = speed * Time.deltaTime;
        Vector3 direction = transform.forward;
        // 2. Raycast from the last frame's position to where the bullet is going to be
        if (Physics.Raycast(lastPosition, direction, out RaycastHit hit, movementStep, hitLayers))
        {
            HandleCollision(hit);
            return; // Stop moving if we hit something
        }

        // 3. Move the bullet forward and update the tracking position
        lastPosition = transform.position;
        transform.Translate(direction * movementStep, Space.World);

    }

    void HandleCollision(RaycastHit hit)
    {
        
        // Place your damage logic here (e.g., hit.collider.GetComponent<Health>().TakeDamage();)
        Debug.Log("Bullet hit: " + hit.collider.name);
        // Instantiate hit particles here if you have them

        Destroy(gameObject); // Destroy the bullet instantly on impact

    }
    
}           
