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
    public class CellObjectEvent : UnityEvent<CellObject>{ }
    
    [HideMonoScript]
    public class TowerBuilder : MonoBehaviour
    {
        [SerializeField, Variable_R] private CellVariable selectedCell;
        [SerializeField, Tweakable] private CellObjectEvent onTowerBuilded;

        public void BuildTower(TowerDataVariable towerData)
        {
            var buildPosition = selectedCell.Value.transform.position;
            
            var cellObject = InstantiateTower(towerData.Value.Prefab, buildPosition)
                .GetComponentInChildren<CellObject>();

            cellObject.UseCell(selectedCell.Cell);
         
            onTowerBuilded?.Invoke(cellObject);
        }

        private void DestroyTower(CellObject towerBehaviour)
        {
            Destroy(towerBehaviour.Reference);
        }

        public void DestroyTower(CellObjectVariable towerVariable) => DestroyTower(towerVariable.CellObject);

        private GameObject InstantiateTower(GameObject prefab, Vector3 position)
        {
           
            var tower = Instantiate(prefab, position, Quaternion.identity);

            tower.transform.parent = transform;

            return tower;
        }
    }
}