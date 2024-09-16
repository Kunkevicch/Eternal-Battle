using UnityEngine;

namespace ProjectDiva
{
    [CreateAssetMenu(fileName = "ObjectsForPooling_", menuName = "Configs/Objects For Pooling")]
    public class ObjectForPooling : ScriptableObject
    {
        [SerializeField]
        private Pool[] _pool = null;

        public Pool[] Pool => _pool;
    }
}
