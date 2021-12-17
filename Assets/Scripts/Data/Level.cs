using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;

namespace TestTD.Data
{
    [Serializable]
    public class Level
    {
        [SerializeField, InlineProperty] private IntMemo value;
        [SerializeField, InlineProperty] private FloatMemo exp;
        [SerializeField] private LevelingData levelingData;

        public IObservable<float> ExpChanged => exp.ObserveEveryValueChanged(x => x.Current);
        public Subject<float> ExpIncreased { get; } = new Subject<float>();
        public Subject<float> ExpDecreased { get; } = new Subject<float>();

        public IObservable<int> Changed => value.ObserveEveryValueChanged(x => x.Current);
        public Subject<int> Increased { get; } = new Subject<int>();
        public Subject<int> Decreased { get; } = new Subject<int>();

        public Level(LevelingData levelingData)
        {
            this.value = new IntMemo(0);
            this.exp = new FloatMemo(0);
            this.levelingData = levelingData;
        }

        public int Value => value.Current + 1;
        public float Exp => exp.Current;
        public bool CanLevelUp => value.Current < levelingData.MaxLevel - 1;

        public void Reset()
        {
            value.Current = 0;
            exp.Current = 0;
        }

        public void Up()
        {
            if (!CanLevelUp)
                return;

            AddExp(levelingData.GetExpForLevel(value.Current));
        }

        public void Down()
        {
            if (!CanLevelUp)
                return;

            RemoveExp(-exp.Current + 0.1f);
        }

        public void AddExp(float expValue)
        {
            if (!CanLevelUp)
                return;

            if (expValue < 0)
            {
                RemoveExp(expValue);
                return;
            }

            ChangeExp(expValue);
            ExpIncreased.OnNext(expValue);
        }

        public void RemoveExp(float expValue)
        {
            if (value.Current == 0)
                return;

            if (expValue > 0)
            {
                AddExp(expValue);
                return;
            }

            ChangeExp(expValue);
            ExpDecreased.OnNext(expValue);
        }

        private void ChangeExp(float offset)
        {
            exp.Current += offset;

            var expToLevelUp = levelingData.GetExpForLevel(value.Current);

            if (exp.Current >= expToLevelUp)
            {
                value.Current = Mathf.Min(value.Current + 1, levelingData.MaxLevel);
                exp.Current = exp.Current - expToLevelUp;

                Increased.OnNext(1);
                return;
            }

            if (exp.Current < 0)
            {
                value.Current = Mathf.Max(value.Current - 1, 0);
                exp.Current = levelingData.GetExpForLevel(value.Current) + exp.Current;

                Decreased.OnNext(-1);
            }
        }
    }
}