using ProjectNahual.Weapons;
using UnityEngine;

namespace ProjectNahual.Weapons
{
    public class BaseWeapon : MonoBehaviour, IWeapon
    {
        WeaponLogic logic;
        private void Awake() => logic = new WeaponLogic(10);
        public void Fire(IDamageable target) { logic.Fire(target); }
    }
}

