using UnityEngine;
using ProjectNahual.Utils;

namespace ProjectNahual.Weapons
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Rigidbody rigid;
        [SerializeField] private float knockbackIntensity = 5f;
        public int health = 100;

        private void Awake() { Registry<IDamageable>.TryAdd(this); }
        private void OnDestroy() { Registry<IDamageable>.Remove(this); }

        public void TakeDamage(int amount)
        {
            health -= amount;

            //Apply knockback force
            rigid.AddForce(transform.up * knockbackIntensity, ForceMode.Impulse);
            
            Debug.Log("Enemy health: " + health);

            if(health <= 0)
            {
                DestroyImmediate(gameObject);
            }
        }
    }
}