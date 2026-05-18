using UnityEngine;
using ProjectNahual.Utils;
using ProjectNahual.Weapons;
using ProjectNahual.FSM;

namespace ProjectNahual.Enemies
{
    public class TestPunchbag : MonoBehaviour, IDamageable
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
            rigid.isKinematic = false;
            rigid.AddForce(transform.up * knockbackIntensity, ForceMode.Impulse);
            // rigid.isKinematic = true;
            
            Debug.Log("Enemy health: " + health);

            if(health <= 0)
            {
                DestroyImmediate(gameObject);
            }
        }
    }
}