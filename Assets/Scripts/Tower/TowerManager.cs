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
using TestTD.UI;
using Event = UnityEngine.Event;

namespace TestTD
{
    [HideMonoScript]
    public class TowerManager : MonoBehaviour
    {
        [SerializeField, Variable_R] private Satisfy.Bricks.Event buildSelectedTower;
        [SerializeField, Variable_R] private GameObjectEvent inventoryItemSelected;
        [SerializeField, Variable_R] private TowerDataEvent towerPurchased;
        [SerializeField, Variable_R] private SelectableEvent cellReleased;
        [SerializeField, Editor_R] private CurrencyManager currencyManager;
        [SerializeField, Tweakable] private UnityEvent<TowerData> onTowerSold;

        private readonly Dictionary<CellObject, TowerData> towers = new Dictionary<CellObject, TowerData>();
        private TowerData lastBuildTowerData;
        private TowerData nextTowerToBuild;

        private void Start()
        {
            inventoryItemSelected.Raised.Select(x => x.GetComponent<InventoryTower>())
                .Where(x =>x != null)
                .Subscribe(x =>
                {
                    nextTowerToBuild = x.Data;
                }).AddTo(this);

            buildSelectedTower.Raised.Where(_ => nextTowerToBuild != null)
                .Subscribe(_ =>
                {
                    TryBuildTower(nextTowerToBuild);
                }).AddTo(this);
        }

        public void TryBuildTower(TowerData data)
        {
            if (!currencyManager.CheckCanBuy(data.Parameters))
                return;

            lastBuildTowerData = data;
            towerPurchased.Raise(lastBuildTowerData);
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