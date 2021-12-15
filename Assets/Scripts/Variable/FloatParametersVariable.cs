using System;
using System.Collections.Generic;
using Satisfy.Variables;
using UnityEngine;

namespace TestTD.Data
{
    [CreateAssetMenu(fileName = "FloatParametersVariable", menuName = "Variables/Custom/Float Parameters Variable")]
    [Serializable]
    public class FloatParametersVariable : ListSO<FloatParameter>
    {

    }
}