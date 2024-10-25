namespace EndlessRoad
{
    public interface IDamageable : IImpactable
    {
        public void ApplyDamage(int damage);
    }
}
