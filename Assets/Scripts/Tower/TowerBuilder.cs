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

namespace TestTD
{
    [Serializable]
    public class TowerBehaviourEvent : UnityEvent<TowerBehaviour>{}
    
    [HideMonoScript]
    public class TowerBuilder : MonoBehaviour
    {
        [SerializeField, Variable_R] private CellVariable selectedCell;
        [SerializeField, Tweakable] private TowerBehaviourEvent onTowerBuilded;

        public void BuildTower(TowerDataVariable towerData)
        {
            var buildPosition = selectedCell.Value.transform.position;
            
            var towerBehaviour = InstantiateTower(towerData.Value.Prefab, buildPosition)
                .GetComponentInChildren<TowerBehaviour>();

            towerBehaviour.UseCell(selectedCell.Cell);
         
            onTowerBuilded?.Invoke(towerBehaviour);
        }

        private void DestroyTower(CellObject towerBehaviour)
        {
            Destroy(towerBehaviour.Reference);
        }

        public void DestroyTower(TowerVariable towerVariable) => DestroyTower(towerVariable.CellObject);

        private GameObject InstantiateTower(GameObject prefab, Vector3 position)
        {
            var tower = Instantiate(prefab, position, Quaternion.identity);

            tower.transform.parent = transform;

            return tower;
        }
    }
}