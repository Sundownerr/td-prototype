using System;
using UnityEngine;
using Sirenix.OdinInspector;
using TestTD.Data;

namespace Satisfy.Bricks
{
    [HideMonoScript]
    [Serializable, CreateAssetMenu(fileName = "EnemyData", menuName = "Bricks/Event/EnemyData")]
    public class EnemyDataEvent : Event<EnemyData> { }
}