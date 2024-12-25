using System;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class RewardPresenter : IInitializable, IDisposable
    {
        private readonly RewardObserver _rewardObserver;
        private readonly TextView _textViewReward;

        public RewardPresenter(RewardObserver rewardObserver, TextView textViewReward)
        {
            _rewardObserver = rewardObserver;
            _textViewReward = textViewReward;
        }

        public void Initialize() => _rewardObserver.RewardChanged += OnRewardChanged;

        public void Dispose() => _rewardObserver.RewardChanged += OnRewardChanged;

        private void OnRewardChanged(int newReward) => _textViewReward.TextChange(newReward.ToString());
    }
}
