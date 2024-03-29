﻿using System;
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
    [Serializable]
    public class FloatParameter
    {
        [SerializeField, HideLabel]
        private float baseValue;

        private readonly List<FloatModifier> modifiers = new List<FloatModifier>();

        public void Add(FloatModifier modifier)
        {
            modifiers.Add(modifier);
        }

        public void Remove(FloatModifier modifier)
        {
            modifiers.Remove(modifier);
        }

        public float Value
        {
            get
            {
                var finalValue = baseValue;
                var totalAddedPercent = 0f;

                for (int i = 0; i < modifiers.Count; i++)
                {
                    var modifier = modifiers[i];


                    if (modifier.Type == ModifyType.Flat)
                    {
                        finalValue += modifier.Value;
                    }

                    if (modifier.Type == ModifyType.PercentAdd)
                    {
                        totalAddedPercent += modifier.Value;

                        if (i + 1 >= modifiers.Count || modifiers[i + 1].Type != ModifyType.PercentAdd)
                        {
                            finalValue *= 1 + totalAddedPercent;
                            totalAddedPercent = 0;
                        }
                    }

                    if (modifier.Type == ModifyType.PercentMultiply)
                    {
                        finalValue *= 1 + modifier.Value;
                    }
                }

                return (float)Math.Round(finalValue, 1);
            }
        }
    }
}