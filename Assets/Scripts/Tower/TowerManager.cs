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

namespace TestTD
{
    public class TowerManager : MonoBehaviour
    {
        [SerializeField, Variable_R] private IntVariable currency;
        [SerializeField, Variable_R] private FloatParameterSO towerCostParameter;
        [SerializeField, Variable_R] private TowerDataVariable towerBuildRequest;
        // [SerializeField, Variable_R] CellVariable selectedCell;
        [SerializeField, Tweakable] private UnityEvent onTowerSold;

        public void TryBuildTower(TowerData towerData)
        {
            // if (selectedCell.Value == null)
            //     return;

            // if (selectedCell.Cell.IsUsed)
            //     return;

            var towerCost = towerData.GetParameterValue(towerCostParameter);

            if (!CheckCanBuy(towerCost))
                return;

            var towerBehaviour = towerData.Prefab.GetComponentInChildren<TowerBehaviour>();
            towerBuildRequest.SetValueAndPublish(towerData);
        }

        public void SellTower(TowerVariable tower)
        {
            Destroy(tower.Value.Reference);
            onTowerSold?.Invoke();
        }

        private bool CheckCanBuy(float cost)
        {
            return cost <= currency.Value;
        }
    }
}