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

    public enum ModifyType
    {
        Flat, PercentAdd, PercentMultiply
    }

    public class Modifier<T>
    {
        private T value;
        private Satisfy.Variables.Variable tag;
        private ModifyType type;

        public ModifyType Type => type;
        public Satisfy.Variables.Variable Tag => tag;
        public T Value => value;
    }

    public class FloatModifier : Modifier<float> { }
}