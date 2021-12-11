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
        [SerializeField] private TowerBehaviour behaviour;

        public TowerBehaviour Behaviour => behaviour;

        public override void SetValue(GameObject value)
        {
            if (value == null)
                return;

            SetTowerBehaviour(value.GetComponent<TowerBehaviour>());
        }

        public void SetTowerBehaviour(TowerBehaviour towerBehaviour)
        {
            this.behaviour = towerBehaviour;
            SetCellObject(towerBehaviour);
        }
    }
}
