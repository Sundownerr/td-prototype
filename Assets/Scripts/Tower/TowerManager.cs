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
        [SerializeField, Variable_R] private FloatParameterSO towerCost;
        [SerializeField, Variable_R] private FloatParameterSO towerElementLevelRequirement;
        [SerializeField, Variable_R] private TowerDataVariable towerBuildRequest;
        [SerializeField, Variable_R] private TowerElementListSO towerElements;
        [SerializeField, Tweakable] private UnityEvent onTowerSold;
        
        private readonly Dictionary<TowerBehaviour, TowerData> towers = new Dictionary<TowerBehaviour, TowerData>();
        private TowerData lastBuildTowerData;

        public void TryBuildTower(TowerData data)
        {
            if (!CheckCanBuild(data))
                return;

            lastBuildTowerData = data;
            towerBuildRequest.SetValueAndPublish(lastBuildTowerData);
        }

        public void SellTower(TowerVariable tower)
        {
            towers.Remove(tower.Behaviour);
            
            Destroy(tower.Value.Reference);
            onTowerSold?.Invoke();
        }

        public void HandleBuildedTower(TowerVariable tower)
        {
            towers.Add(tower.Behaviour, lastBuildTowerData);
        }

        private bool CheckCanBuild(TowerData data)
        {
            var cost = data.GetParameterValue(towerCost);
            var elementLevelRequirement = data.GetParameterValue(towerElementLevelRequirement);
            var elementLevel = towerElements.List.Find(x => data.Element == x).Level;
            
            return cost <= currency.Value &&
                   elementLevelRequirement <= elementLevel;
        }
    }
}