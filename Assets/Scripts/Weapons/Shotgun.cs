using UnityEngine;
using System.Collections.Generic;
using ProjectNahual.Utils;

namespace ProjectNahual.Weapons
{
    public class Shotgun : BaseWeapon, IWeapon
    {
        [SerializeField] private float impactRadius = 4f;
        [SerializeField] private ParticleSystem muzzleFlash = null;
        WeaponLogic logic;
        private void Awake() => logic = new WeaponLogic(damage);
        public override void Fire()
        {
            // base.Fire(target);
            muzzleFlash.Play();
            List<IDamageable> targets = GetTargetsInsideRange();

            if(targets.Count < 1) return;

            foreach(var target in targets)
                logic.Fire(target);
        }

        protected List<IDamageable> GetTargetsInsideRange()
        {
            Transform playerCamera = Camera.main.transform;
            List<IDamageable> targetsInsideRange = new List<IDamageable>();
            RaycastHit[] results = new RaycastHit[8];
            int count = Physics.SphereCastNonAlloc(playerCamera.position, impactRadius / 2, playerCamera.TransformDirection(Vector3.forward), results, damageableLayerMask);
            Debug.Log(count);

            if(count < 1) return targetsInsideRange;
            for (int i = 0; i < count; i++)
            {
                Debug.DrawLine(playerCamera.position, playerCamera.TransformDirection(Vector3.forward) * results[i].distance, Color.yellow);
                if(results[i].transform.TryGetComponent<IDamageable>(out var possibleTarget))
                {
                    targetsInsideRange.Add(possibleTarget);
                }
            }
            return targetsInsideRange;
        }
    }
}
