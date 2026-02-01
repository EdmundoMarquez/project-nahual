using UnityEngine;

namespace ProjectNahual.Weapons
{
    public interface IWeapon
    {
        void Fire();
    }

    public interface IDamageable
    {
        void TakeDamage(int amount);
    }
}