using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TestTD.Data
{
    [Serializable]
    public class Memo<T>
    {
        [SerializeField, HideLabel] private T current;
        [SerializeField, HideInEditorMode, HideLabel] private T previous;

        public Memo(T current)
        {
            this.previous = current;
            this.current = current;
        }

        public T Current
        {
            get => current;
            set
            {
                previous = current;
                current = value;
            }
        }

        public T Previous => previous;
    }

    [Serializable]
    public class IntMemo : Memo<int>
    {
        public IntMemo(int current) : base(current)
        {
        }
    }

    [Serializable]
    public class FloatMemo : Memo<float>
    {
        public FloatMemo(float current) : base(current)
        {
        }
    }
}