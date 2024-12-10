using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EndlessRoad
{
    public class RagdollHelper
    {
        private readonly List<Rigidbody> _rigidBodies;

        public RagdollHelper(GameObject gameObject)
        {
            _rigidBodies = new(gameObject.GetComponentsInChildren<Rigidbody>());
        }

        public void Enable()
        {
            foreach (var rigidBody in _rigidBodies)
            {
                rigidBody.isKinematic = false;
            }
        }

        public void Disable()
        {
            foreach (var rigidBody in _rigidBodies)
            {
                rigidBody.isKinematic = true;
            }
        }

        public void AddForceInDirection(Vector3 direction)
        {
            foreach (var rigidBody in _rigidBodies)
            {
                rigidBody.AddForce(direction * 20f, ForceMode.Impulse);
            }
        }
    }
}
