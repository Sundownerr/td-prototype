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
using TestTD.Variables;
using TestTD.Data;
using TestTD.Systems;

namespace TestTD
{
    [Serializable]
    public class TowerDataEvent : UnityEvent<TowerData> { }
    
    [HideMonoScript]
    public class TowerManager : MonoBehaviour
    {
        [SerializeField, Variable_R] private TowerDataVariable towerBuildRequest;
        [SerializeField, Editor_R] private CurrencyManager currencyManager;
        [SerializeField, Tweakable] private TowerDataEvent onTowerSold;
        
        private readonly Dictionary<TowerBehaviour, TowerData> towers = new Dictionary<TowerBehaviour, TowerData>();
        private TowerData lastBuildTowerData;

        public void TryBuildTower(TowerData data)
        {
            if (!currencyManager.CheckCanBuy(data))
                return;

            lastBuildTowerData = data;
            towerBuildRequest.SetValueAndPublish(lastBuildTowerData);
        }

        public void SellTower(TowerVariable tower)
        {
            var data = towers[tower.Behaviour];

            towers.Remove(tower.Behaviour);
            onTowerSold?.Invoke(data);
        }

        public void HandleBuildedTower(TowerVariable tower)
        {
            towers.Add(tower.Behaviour, lastBuildTowerData);
            lastBuildTowerData = null;
        }
    }
}