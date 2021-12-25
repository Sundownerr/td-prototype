using System;
using UnityEngine;
using Sirenix.OdinInspector;
using TestTD;

namespace Satisfy.Bricks
{
    [HideMonoScript]
    [Serializable, CreateAssetMenu(fileName = "CellObject", menuName = "Bricks/Event/CellObject")]
    public class CellObjectEvent : Event<CellObject>
    {
        public void Raise(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<CellObject>(out var cellObject))
            {
                Raise(cellObject);
            }
        }
    }
}