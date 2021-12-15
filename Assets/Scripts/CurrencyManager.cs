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

        private float GetTowerCost(TowerData data) => data.GetValue(towerCost);

        public override void Initialize()
        {
            base.Initialize();
        }

        public void HandleTowerBuild(TowerDataVariable dataVariable) =>
            HandleTowerBuild(dataVariable.Value);

        private void HandleTowerBuild(TowerData data)
        {
            towerBuildCurrency.DecreaseBy((int)GetTowerCost(data));
            towerCurrentLimit.DecreaseBy((int)data.GetValue(towerLimitRequirement));
        }

        public void HandleTowerSold(TowerDataVariable dataVariable) =>
            HandleTowerSold(dataVariable.Value);

        private void HandleTowerSold(TowerData data)
        {
            towerBuildCurrency.IncreaseBy((int)(GetTowerCost(data) * costRefundPercent.Value));
            towerCurrentLimit.IncreaseBy((int)data.GetValue(towerLimitRequirement));
        }

        public bool CheckCanBuy(TowerData data)
        {
            var haveMoney = GetTowerCost(data) <= towerBuildCurrency.Value;

            if (!haveMoney)
            {
                onNotEnoughMoney?.Invoke();
                return false;
            }

            var limitRequirement = data.GetValue(towerLimitRequirement);
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