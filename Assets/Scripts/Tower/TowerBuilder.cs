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
using Satisfy.Managers;
using System.Linq;
using TestTD.Data;
using Satisfy.Bricks;

namespace TestTD
{
    [Serializable, CreateAssetMenu(fileName = "Tower Builder", menuName = "System/Tower Builder")]
    [HideMonoScript]
    public class TowerBuilder : ScriptableObjectSystem
    {
        [SerializeField, Variable_R] private GameObjectVariable towerParent;
        [SerializeField, Variable_R] private CellVariable selectedCell;
        [SerializeField, Variable_R] private CellObjectVariable selectedTower;
        [SerializeField, Tweakable] private UnityEvent<CellObject> onTowerBuilded;
        [SerializeField] private EventListenerEmbedded<TowerDataEvent, TowerData> towerDataListener;
        [SerializeField] private BaseListener listener;

        public override void Initialize()
        {
            listener.Initialize();
            towerDataListener.Initialize();
        }

        [Button, Debugging]
        public void BuildTower(TowerData towerData)
        {
            var buildPosition = selectedCell.Value.transform.position;

            var tower = Instantiate(towerData.Prefab,
                                    buildPosition,
                                    Quaternion.identity,
                                    towerParent.Value.transform);

            var cellObject = tower.GetComponentInChildren<CellObject>();

            cellObject.UseCell(selectedCell.Cell);

            onTowerBuilded?.Invoke(cellObject);
        }

        [Button, Debugging]
        public void DestroyTower(CellObject towerBehaviour)
        {
            Destroy(towerBehaviour.Reference);
        }

        [Button, Debugging]
        public void DestroyTower(CellObjectVariable towerBehaviour)
        {
            DestroyTower(towerBehaviour.CellObject);
        }
    }
}