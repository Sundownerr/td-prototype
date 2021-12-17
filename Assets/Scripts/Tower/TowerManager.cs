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
        [SerializeField, Variable_R] private CellVariable selectedCell;
        [SerializeField, Editor_R] private CurrencyManager currencyManager;
        [SerializeField, Tweakable] private TowerDataEvent onTowerSold;

        private readonly Dictionary<CellObject, TowerData> towers = new Dictionary<CellObject, TowerData>();
        private TowerData lastBuildTowerData;

        public void TryBuildTower(TowerData data)
        {
            if (selectedCell.Value == null)
                return;

            if (selectedCell.Cell.IsUsed)
                return;

            if (!currencyManager.CheckCanBuy(data.Parameters))
                return;

            lastBuildTowerData = data;
            towerBuildRequest.SetValueAndPublish(lastBuildTowerData);
        }

        public void SellTower(CellObjectVariable tower)
        {
            var data = towers[tower.CellObject];

            towers.Remove(tower.CellObject);
            onTowerSold?.Invoke(data);
        }

        public void HandleBuildedTower(CellObjectVariable tower)
        {
            towers.Add(tower.CellObject, lastBuildTowerData);
            lastBuildTowerData = null;
        }
    }
}