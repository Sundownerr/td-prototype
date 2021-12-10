using System;
using Satisfy.Variables;
using TestTD;
using UnityEngine;

namespace TestTD.Variables
{
    [CreateAssetMenu(fileName = "Tower", menuName = "Variables/Custom/Tower")]
    [Serializable]
    public class TowerVariable : CellObjectVariable
    {
        [SerializeField] private TowerBehaviour towerBehaviour;

        public TowerBehaviour TowerBehaviour => towerBehaviour;

        public override void SetValue(GameObject value)
        {
            if (value == null)
                return;

            SetTowerBehaviour(value.GetComponent<TowerBehaviour>());
        }

        public void SetTowerBehaviour(TowerBehaviour towerBehaviour)
        {
            this.towerBehaviour = towerBehaviour;
            SetCellObject(towerBehaviour);
        }
    }
}
