using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Animator animator;
    private Transform _target;
    private Rigidbody rb;
    private bool hitCollision = false;

    void Awake() => rb = GetComponent<Rigidbody>();

    public void Init(Transform target)
    {
        _target = target;
    }

    void FixedUpdate()
    {
        // Move the projectile forward in its current direction
        rb.linearVelocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(hitCollision) return;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        animator.SetTrigger("OnCollision");
        hitCollision = true;
        
        if (!collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject, 20f);
            return;
            
        } 
        Destroy(gameObject);
    }
}
