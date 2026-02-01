using ProjectNahual.Weapons;
using ProjectNahual.Utils;
using UnityEngine;

public class Melee : BaseWeapon, IWeapon
{
    [SerializeField] private ParticleSystem hitEffect = null;
    WeaponLogic logic;
    private void Awake() => logic = new WeaponLogic(damage);
    public override void Fire()
    {
        // base.Fire()
        hitEffect.Play();
        var target = Registry<IDamageable>.Get(GetClosest);
        if (target == null) return;
        logic.Fire(target);
    }
}
