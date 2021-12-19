using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using Satisfy.Variables;
using Satisfy.Attributes;

namespace TestTD.Data
{
    [CreateAssetMenu(fileName = "TowerElement", menuName = "Data/Tower element")]
    [HideMonoScript]
    public class TowerElement : ScriptableObject
    {
        [SerializeField] private Descriptor descriptor;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Level level;

        public Sprite Sprite => sprite;
        public Level Level => level;
        public Descriptor Descriptor => descriptor;

        public void Invest()
        {

            level.AddExp(1);
        }

        public void ResetValues()
        {
            level.Reset();
        }
    }
}