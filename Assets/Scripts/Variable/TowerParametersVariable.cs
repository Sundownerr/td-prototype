using System;
using System.Collections.Generic;
using Satisfy.Variables;
using UnityEngine;

namespace TestTD.Data
{
    [CreateAssetMenu(fileName = "TowerParametersVariable", menuName = "Variables/Custom/Tower Parameters Variable")]
    [Serializable]
    public class TowerParametersVariable : ListSO<FloatParameter>
    {
        
    }
}