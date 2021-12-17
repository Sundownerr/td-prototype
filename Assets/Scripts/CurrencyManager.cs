using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using UniRx;
using Satisfy.Variables;
using Satisfy.Attributes;
using Satisfy.Managers;
using TestTD.Data;
using TestTD.Variables;

namespace TestTD.Systems
{
    [Serializable, CreateAssetMenu(fileName = "Currency Manager", menuName = "System/Currency Manager")]
    [HideMonoScript]
    public class CurrencyManager : ListenerSystem
    {
        [SerializeField, Variable_R] private FloatParameterSO towerCost;
        [SerializeField, Variable_R] private FloatParameterSO towerLimitRequirement;
        [SerializeField, Variable_R] private IntVariable towerBuildCurrency;
        [SerializeField, Variable_R] private IntVariable towerMaxLimit;
        [SerializeField, Variable_R] private IntVariable towerCurrentLimit;
        [SerializeField, Variable_R] private FloatVariable costRefundPercent;

        [SerializeField, Tweakable] private UnityEvent onNotEnoughMoney;
        [SerializeField, Tweakable] private UnityEvent onNotEnoughLimit;

        public override void Initialize()
        {
            base.Initialize();
        }

        public void HandleTowerBuild(TowerDataVariable dataVariable) =>
            HandleTowerBuild(dataVariable.Value.Parameters);

        private void HandleTowerBuild(TowerParameters parameters)
        {
            towerBuildCurrency.DecreaseBy((int)parameters.BuildCost.Value);
            towerCurrentLimit.DecreaseBy((int)parameters.TowerLimitRequirement.Value);
        }

        public void HandleTowerSold(TowerDataVariable dataVariable) =>
            HandleTowerSold(dataVariable.Value.Parameters);

        private void HandleTowerSold(TowerParameters parameters)
        {
            towerBuildCurrency.IncreaseBy((int)(parameters.BuildCost.Value * costRefundPercent.Value));
            towerCurrentLimit.IncreaseBy((int)parameters.TowerLimitRequirement.Value);
        }

        public bool CheckCanBuy(TowerParameters parameters)
        {
            var haveMoney = parameters.BuildCost.Value <= towerBuildCurrency.Value;

            if (!haveMoney)
            {
                onNotEnoughMoney?.Invoke();
                return false;
            }

            var limitRequirement = parameters.TowerLimitRequirement.Value;
            var haveLimit = towerCurrentLimit.Value - limitRequirement >= 0;

            if (!haveLimit)
            {
                onNotEnoughLimit?.Invoke();
                return false;
            }

            return true;
        }
    }
}