namespace EndlessRoad
{
    public class EnemyCombatMelee : EnemyCombatBase
    {
        public override void Attack()
        {
            _currentWeapon.Tick(true,out bool canshoot);
        }
    }
}
