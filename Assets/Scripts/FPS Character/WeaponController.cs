using UnityEngine;
using ProjectNahual.Input;
using ProjectNahual.Weapons;
using ProjectNahual.Utils;

namespace ProjectNahual.FPCharacter
{
    public class WeaponController : MonoBehaviour
    {
        private IWeapon _weapon;
        private IPlayerInput _playerInput;
        IDamageable target;

        public void Init(IPlayerInput playerInput, IWeapon weapon)
        {
            _weapon = weapon;
            _playerInput = playerInput;

            _playerInput.ShootPressed += OnFire;
        }

        private void OnDisable()
        {
            _playerInput.ShootPressed -= OnFire;
        }

        private void OnFire()
        {
            target = Registry<IDamageable>.GetFirst();
            if (target == null) return;
            _weapon.Fire(target);
        }
    }
}