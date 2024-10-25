using System;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "ImpactConfig", menuName = "Configs/Impact")]
    public class ImpactConfig : ScriptableObject
    {
        public List<ImpactKeyPair> impactDictionary;

        [Serializable]
        public struct ImpactKeyPair
        {
            public ImpactType type;
            public ImpactEffect effect;
        }
    }
}
