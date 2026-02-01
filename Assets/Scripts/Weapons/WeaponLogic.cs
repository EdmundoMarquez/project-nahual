namespace ProjectNahual.Weapons
{
    public class WeaponLogic
    {
        readonly int damage;
        public WeaponLogic(int damage) => this.damage = damage;
        public void Fire(IDamageable target) => target.TakeDamage(damage);
    }
}
