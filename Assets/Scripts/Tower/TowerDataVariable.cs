using System;
using Satisfy.Variables;
using TestTD.Data;
using UnityEngine;

namespace TestTD.Variables
{
    [CreateAssetMenu(fileName = "TowerDataVariable", menuName = "Variables/Custom/Tower Data")]
    [Serializable]
    public class TowerDataVariable : Variable<TowerData>
    {

    }
}
