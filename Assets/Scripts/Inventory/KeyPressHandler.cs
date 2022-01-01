using System;
using System.Collections.Generic;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace TestTD.Utility
{
    public class KeyPressHandler : MonoBehaviour
    {
        [Serializable]
        public class KeyAction
        {
            [HorizontalGroup("1")]
            [HideLabel]
            public KeyCode key;
            [BoxGroup("1/2", false), LabelText("Press")]
            public UnityEvent downAction;
            [BoxGroup("1/2", false), LabelText("Release")]
            public UnityEvent upAction;
        }

        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false)]
        [SerializeField, Tweakable]
        private List<KeyAction> keyActions;

        private void Start()
        {
            var update = Observable.EveryUpdate().Where(_ => enabled && gameObject.activeSelf);

            keyActions.ForEach(x =>
            {
                update.Where(_ => Input.GetKeyDown(x.key))
                    .Subscribe(_ => { x.downAction?.Invoke(); }).AddTo(this);

                update.Where(_ => Input.GetKeyUp(x.key))
                    .Subscribe(_ => { x.upAction?.Invoke(); }).AddTo(this);
            });
        }
    }
}