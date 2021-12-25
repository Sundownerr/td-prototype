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
using Satisfy.Bricks;

namespace TestTD.Systems
{
    [Serializable, CreateAssetMenu(fileName = "Currency Manager", menuName = "System/Currency Manager")]
    [HideMonoScript]
    public class CurrencyManager : ScriptableObjectSystem
    {
        [SerializeField, Variable_R] private IntVariable towerBuildCurrency;
        [SerializeField, Variable_R] private IntVariable towerMaxLimit;
        [SerializeField, Variable_R] private IntVariable towerCurrentLimit;
        [SerializeField, Variable_R] private FloatVariable costRefundPercent;
        [SerializeField, Tweakable] private UnityEvent onNotEnoughMoney;
        [SerializeField, Tweakable] private UnityEvent onNotEnoughLimit;
        [SerializeField] private EventListenerEmbedded<EnemyDataEvent, EnemyData> enemyEventListener;
        [SerializeField] private EventListenerEmbedded<TowerDataEvent, TowerData> towerEventListener;

        public override void Initialize()
        {
            base.Initialize();
            enemyEventListener.Initialize();
            towerEventListener.Initialize();
        }

        public void HandleTowerPurchased(TowerData data)
        {
            var parameters = data.Parameters;
            towerBuildCurrency.DecreaseBy((int)parameters.BuildCost.Value);
            towerCurrentLimit.DecreaseBy((int)parameters.TowerLimitRequirement.Value);
        }

        public void HandleTowerSold(TowerData data)
        {
            var parameters = data.Parameters;
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

        public void HandleEnemyDefeated(EnemyData data)
        {
            var gainedMoney = data.Parameters.Money.Value;

            towerBuildCurrency.IncreaseBy((int)gainedMoney);
        }
    }
}