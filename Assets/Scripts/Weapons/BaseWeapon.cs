using System;
using System.Collections.Generic;
using ProjectNahual.Utils;
using UnityEngine;

namespace ProjectNahual.Weapons
{
    public class BaseWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] protected float maxDistance = 10f;
        [SerializeField] protected int damage = 10;
        [SerializeField] protected LayerMask damageableLayerMask;
        WeaponLogic logic;
        private void Awake() => logic = new WeaponLogic(damage);
        public virtual void Fire()
        { 
            var target = Registry<IDamageable>.Get(GetClosest);
            if(target == null) return;
            logic.Fire(target);
        }

        protected IDamageable GetClosest(IEnumerable<IDamageable> candidates)
        {
            IDamageable closest = null;
            float minDistance = float.MaxValue;

            foreach (var candidate in candidates)
            {
                if(candidate == null) continue;
                if(candidate is not Component component) continue;

                float distance = Vector3.Distance(transform.position, component.transform.position);
                if(distance > maxDistance) continue;
                if(distance < minDistance)
                {
                    minDistance = distance;
                    closest = candidate;
                }
            }
            return closest;
        }
    }
}

