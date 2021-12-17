using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using UniRx;
using Satisfy.Variables;
using Satisfy.Attributes;

namespace TestTD.Data
{
    [Serializable, CreateAssetMenu(fileName = "Wave List", menuName = "Data/Wave List")]
    [HideMonoScript]
    public class WaveListSO : ListSO<WaveSO>
    {

    }
}