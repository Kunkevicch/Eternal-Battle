using System;

namespace EndlessRoad
{
    public class EventBus
    {
        public event Action<int> EnemyDied;

        public void RaiseEnemyDied(int id) => EnemyDied?.Invoke(id);
    }
}
