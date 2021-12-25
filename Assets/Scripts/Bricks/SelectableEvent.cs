using System;
using UnityEngine;
using Sirenix.OdinInspector;
using TestTD;

namespace Satisfy.Bricks
{
    [HideMonoScript]
    [Serializable, CreateAssetMenu(fileName = "Selectable", menuName = "Bricks/Event/Selectable")]
    public class SelectableEvent : Event<Selectable>
    {
        public void Raise(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<Selectable>(out var selectable))
            {
                Raise(selectable);
            }
        }
    }
}