using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Satisfy.Variables;
using TestTD.Entities;

namespace TestTD.Data
{
    [Serializable, CreateAssetMenu(fileName = "Enemy List", menuName = "Lists/Custom/Enemy List")]
    [HideMonoScript]
    public class EnemyListSO : ListSO<EnemyBehaviour>
    {

    }
}