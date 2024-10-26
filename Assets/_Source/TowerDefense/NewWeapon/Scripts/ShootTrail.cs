using UnityEngine;

namespace EndlessRoad
{
    public class ShootTrail : MonoBehaviour
    {
        private TrailRenderer _trailRenderer;

        private void Awake()
        {
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        public TrailRenderer Trail => _trailRenderer;
    }
}
