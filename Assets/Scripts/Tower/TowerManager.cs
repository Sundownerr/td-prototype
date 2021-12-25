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
using Satisfy.Bricks;

namespace TestTD
{
    [HideMonoScript]
    public class TowerManager : MonoBehaviour
    {
        [SerializeField, Variable_R] private TowerDataEvent towerBuildRequest;
        [SerializeField, Variable_R] private CellVariable selectedCell;
        [SerializeField, Variable_R] private SelectableEvent cellReleased;
        [SerializeField, Editor_R] private CurrencyManager currencyManager;
        [SerializeField, Tweakable] private UnityEvent<TowerData> onTowerSold;

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
            towerBuildRequest.Raise(lastBuildTowerData);
        }

        public void SellTower(CellObjectVariable tower)
        {
            var data = towers[tower.CellObject];

            towers.Remove(tower.CellObject);

            cellReleased.Raise(tower.CellObject.Cell);

            onTowerSold?.Invoke(data);
        }

        public void HandleBuildedTower(CellObject tower)
        {
            towers.Add(tower, lastBuildTowerData);
            lastBuildTowerData = null;
        }
    }
}