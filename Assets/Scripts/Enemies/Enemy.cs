using UnityEngine;
using ProjectNahual.Utils;
using ProjectNahual.Weapons;
using ProjectNahual.FSM;
using UnityEngine.AI;

namespace ProjectNahual.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private BaseBehaviour behaviour;
        [SerializeField] private Rigidbody rigid;
        [SerializeField] private float knockbackIntensity = 5f;
        public int health = 100;

        private void Awake() { Registry<IDamageable>.TryAdd(this); }
        private void OnDestroy() { Registry<IDamageable>.Remove(this); }

        private void Start()
        {
            behaviour.Init();
        }

        private void OnEnable()
        {
            behaviour.Reset();
        }

        private void OnDisable()
        {
            behaviour.Stop();
        }

        private void Update()
        {
            behaviour.Tick();

            // if(rigid.linearVelocity.magnitude <= 0.1f && !rigid.isKinematic)
            // {
            //     rigid.isKinematic = true;
            //     behaviour.Resume();
            // }
        }

        public void TakeDamage(int amount)
        {
            health -= amount;

            //Apply knockback force
            // behaviour.Stop();
            // rigid.isKinematic = false;
            // rigid.AddForce(transform.up * knockbackIntensity, ForceMode.Impulse);
            
            Debug.Log("Enemy health: " + health);

            if(health <= 0)
            {
                behaviour.Stop();
                // Destroy(gameObject);
            }
        }
    }
}