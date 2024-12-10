using System;
using System.Collections;
using UnityEngine;

namespace EndlessRoad
{
    public class BerserkRage
    {
        private bool _isBerserk;
        private bool _canRage = true;
        private float _rageDuration;
        public BerserkRage(float rageDuration)
        {
            _rageDuration = rageDuration;
        }

        public bool IsBerserk => _isBerserk;
        public bool CanRage => _canRage;

        public IEnumerator RageRoutine(Action rageEndCallback)
        {
            _canRage = false;
            _isBerserk = true;

            yield return new WaitForSeconds(_rageDuration);
            _isBerserk = false;
            rageEndCallback();
        }
    }
}