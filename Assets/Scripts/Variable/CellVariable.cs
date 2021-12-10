using System;
using Satisfy.Variables;
using UnityEngine;

namespace TestTD.Variables
{
    [CreateAssetMenu(fileName = "Cell", menuName = "Variables/Custom/Cell")]
    [Serializable]
    public class CellVariable : SelectableVariable
    {
        [SerializeField] private Cell cell;

        public Cell Cell => cell;

        public override void SetValue(GameObject value)
        {
            if (value == null)
                return;

            SetCell(value.GetComponent<Cell>());
        }

        public void SetCell(Cell cell)
        {
            this.cell = cell;
            SetValue(cell);
        }
    }
}
