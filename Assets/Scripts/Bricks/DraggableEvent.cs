using System;
using Sirenix.OdinInspector;
using TestTD.UI;
using UnityEngine;

namespace Satisfy.Bricks
{
    [HideMonoScript]
    [Serializable, CreateAssetMenu(fileName = "GameObject", menuName = "Bricks/Event/GameObject")]
    public class DraggableEvent : Event<DraggableUIElement>
    {
    }
}