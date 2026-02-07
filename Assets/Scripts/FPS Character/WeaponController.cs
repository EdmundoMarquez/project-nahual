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
        private bool _enabled;

        public void Init(IPlayerInput playerInput, IWeapon weapon)
        {
            _weapon = weapon;
            _playerInput = playerInput;

            _playerInput.ShootPressed += OnFire;
            _enabled = true;
        }

        private void OnDisable()
        {
            if(_playerInput == null) return;
            _playerInput.ShootPressed -= OnFire;
        }

        private void OnFire()
        {
            if(!_enabled) return;
            _weapon.Fire();
        }

        public void Stop()
        {
            _enabled = false;
            _playerInput.ShootPressed -= OnFire;
        }
        
        public void SetState(bool state) => _enabled = state;
    }
}