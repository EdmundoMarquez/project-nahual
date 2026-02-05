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
            _weapon.Fire();
        }

        public void Stop() => _playerInput.ShootPressed -= OnFire;
    }
}