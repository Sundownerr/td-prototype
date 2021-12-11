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
    [HideMonoScript]	
    public class CurrencyManager : MonoBehaviour
    {
        [SerializeField, Variable_R] private FloatParameterSO towerCost;
        [SerializeField, Variable_R] private IntVariable towerBuildCurrency;
        [SerializeField, Variable_R] private FloatVariable costRefundPercent;

        private float GetTowerCost(TowerData data) => data.GetValue(towerCost);

        public void HandleTowerSold(TowerDataVariable dataVariable) =>
            HandleTowerSold(dataVariable.Value);
        
        private void HandleTowerSold(TowerData data)
        {
            var costRefund = GetTowerCost(data) * costRefundPercent.Value;

            towerBuildCurrency.IncreaseBy((int) costRefund);
        }

        public void HandleTowerBuild(TowerDataVariable dataVariable) =>
            HandleTowerBuild(dataVariable.Value);
        
        private void HandleTowerBuild(TowerData data)
        {
            towerBuildCurrency.DecreaseBy((int) GetTowerCost(data));
        }

        public bool CheckCanBuy(TowerData data)
        {
            return GetTowerCost(data) <= towerBuildCurrency.Value;
        }
    }
}