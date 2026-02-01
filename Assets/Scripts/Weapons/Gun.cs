namespace ProjectNahual.Weapons
{
    using UnityEngine;

    public class Gun : BaseWeapon, IWeapon
    {
        [SerializeField] private GameObject bulletPrefab = null;
        [SerializeField] private Transform bulletSpawn = null;
        WeaponLogic logic;
        private void Awake() => logic = new WeaponLogic(damage);
        public override void Fire()
        {
            // base.Fire(target);
            Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            var target = GetFirstInLine();
            if (target == null) return;
            logic.Fire(target);
        }

        protected IDamageable GetFirstInLine()
        {
            Transform playerCamera = Camera.main.transform;
            IDamageable firstInLine = null;

            if(Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out RaycastHit hit, maxDistance))
            {
                Debug.DrawRay(playerCamera.position, playerCamera.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                if(hit.transform.TryGetComponent<IDamageable>(out firstInLine))
                {
                    return firstInLine;
                }
            }
            return firstInLine;
        }
    }
}