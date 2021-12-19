using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using Satisfy.Utility;

namespace TestTD.Data
{
    [Serializable]
    public class Level
    {
        [SerializeField, LabelWidth(60), InlineProperty, HideInEditorMode] private Memo<int> value;
        [SerializeField, LabelWidth(60), InlineProperty, HideInEditorMode] private Memo<float> exp;
        [SerializeField, LabelWidth(110)] private LevelingData levelingData;

        public IObservable<float> ExpChanged => exp.Changed;
        public IObservable<float> ExpIncreased => ExpChanged.Where(x => x > exp.Previous);
        public IObservable<float> ExpDecreased => ExpChanged.Where(x => x < exp.Previous);

        public IObservable<int> Changed => value.Changed;
        public IObservable<int> Increased => Changed.Where(x => x > exp.Previous);
        public IObservable<int> Decreased => Changed.Where(x => x < exp.Previous);

        public Level(LevelingData levelingData)
        {
            this.value = new Memo<int>(0);
            this.exp = new Memo<float>(0);
            this.levelingData = levelingData;
        }

        public Level()
        {
            this.value = new Memo<int>(0);
            this.exp = new Memo<float>(0);
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

        }

        private void ChangeExp(float offset)
        {
            exp.Current += offset;

            var expToLevelUp = levelingData.GetExpForLevel(value.Current);

            if (exp.Current >= expToLevelUp)
            {
                value.Current = Mathf.Min(value.Current + 1, levelingData.MaxLevel);
                exp.Current = exp.Current - expToLevelUp;

                return;
            }

            if (exp.Current < 0)
            {
                value.Current = Mathf.Max(value.Current - 1, 0);
                exp.Current = levelingData.GetExpForLevel(value.Current) + exp.Current;
            }
        }
    }
}