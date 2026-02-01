using UnityEngine;

namespace ProjectNahual.Weapons
{
    public interface IWeapon
    {
        void Fire(IDamageable target);
    }

    public interface IDamageable
    {
        void TakeDamage(int amount);
    }
}