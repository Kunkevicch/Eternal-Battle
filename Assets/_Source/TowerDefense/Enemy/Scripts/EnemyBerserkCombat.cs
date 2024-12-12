namespace EndlessRoad
{
    public class EnemyBerserkCombat : EnemyCombat
    {
        public override void Attack()
        {
            _currentWeapon.Tick(true,out bool canshoot);
        }
    }
}
