using System;
using Satisfy.Variables;
using TestTD.Data;
using UnityEngine;

namespace TestTD.Variables
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "Variables/Custom/Enemy Data")]
    [Serializable]
    public class EnemyDataVariable : Variable<EnemyData>
    {

    }
}
