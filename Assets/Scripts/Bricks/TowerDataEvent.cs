using System;
using UnityEngine;
using Sirenix.OdinInspector;
using TestTD.Data;

namespace Satisfy.Bricks
{
    [HideMonoScript]
    [Serializable, CreateAssetMenu(fileName = "TowerData", menuName = "Bricks/Event/TowerData")]
    public class TowerDataEvent : Event<TowerData> { }
}