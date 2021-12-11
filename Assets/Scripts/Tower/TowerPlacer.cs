using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using JetBrains.Annotations;
using UniRx;
using Satisfy.Attributes;
using Satisfy.Variables;
using TestTD.Variables;

namespace TestTD
{
    public static class SourceTemplate
    {
       
        [SourceTemplate]
        public static void suba<T>(this IObservable<T> x)
        { /*$
            x.Subscribe(_ =>
            {
                 $END$
            }).AddTo(this);
            */
        }
    }
    
    public class TowerPlacer : MonoBehaviour
    {
        private enum State
        {
            Waiting, Replacing, BadPosition,
        }
        [SerializeField, Variable_R] private Variable pointerDown;
        [SerializeField, Variable_R] private Variable pointerUp;
        [SerializeField, Variable_R] private TowerVariable selectedTower;
        [SerializeField, Variable_R] private CellVariable highlightedCell;
        [SerializeField, Variable_R] private CellVariable selectedCell;
        [SerializeField, Variable_R] private CellVariable previousUsedCell;
        [SerializeField, Editor_R] private LineRenderer endPointLine;
        [SerializeField, Tweakable] private Vector3 lineOffset;
        [SerializeField, Tweakable] private UnityEvent onStartPlacing;
        [SerializeField, Tweakable] private UnityEvent onEndPlacingSuccess;
        [SerializeField, Tweakable] private UnityEvent onEndPlacingCanceled;
        
        private readonly Subject<int> finishedReplacing = new Subject<int>();
        private readonly Subject<int> canceledReplacing = new Subject<int>();

        private IObservable<long> update;
        private State state;
        private Transform SelectedTowerRoot => selectedTower.Value.Reference.transform;

        private void Start()
        {
            update = Observable.EveryUpdate().Where(_ => enabled && gameObject.activeSelf);

            endPointLine.gameObject.SetActive(false);
            HideLine();

            finishedReplacing.Subscribe(_ =>
            {
                HideLine();

                selectedTower.Behaviour.UseCell(selectedCell.Value as Cell);
                MoveTowerTo(selectedCell.Value.transform.position);

                onEndPlacingSuccess?.Invoke();
            }).AddTo(this);

            canceledReplacing.Subscribe(_ =>
            {
                HideLine();
                MoveTowerTo(selectedTower.Behaviour.Cell.transform.position);

                onEndPlacingCanceled?.Invoke();
            }).AddTo(this);

            finishedReplacing.Merge(canceledReplacing)
                .Subscribe(_ =>
                {
                    state = State.Waiting;
                }).AddTo(this);

            update.Where(_ => true).Subscribe(_ =>
            {
                
            }).AddTo(this);
        }

        public void StartReplacing()
        {
            state = State.Replacing;
            previousUsedCell.SetCell(selectedTower.Behaviour.Cell);
            previousUsedCell.Publish();

            ShowLine();

            onStartPlacing?.Invoke();

            update.TakeUntil(finishedReplacing.Merge(canceledReplacing))
                .Subscribe(_ =>
                {
                    SelectedTowerRoot.position = Vector3.Lerp(SelectedTowerRoot.position,
                                                              GetTargetPosition(),
                                                              Time.deltaTime * 25f);

                    endPointLine.SetPosition(1, SelectedTowerRoot.position);
                }).AddTo(this);

            pointerDown.Published.Take(1)
                .TakeUntil(canceledReplacing)
                .Subscribe(_ =>
                {
                    finishedReplacing.OnNext(1);
                }).AddTo(this);

            var a = new int[5];
        }

        private Vector3 GetTargetPosition()
        {
            var endPosition = SelectedTowerRoot.position;

            if (highlightedCell.Value != null)
            {
                endPosition = highlightedCell.Value.transform.position;
            }

            return endPosition;
        }

        public void CancelReplacing()
        {
            if (state == State.Waiting)
                return;

            if (selectedTower.Value == null)
                return;

            canceledReplacing.OnNext(1);
        }

        private void MoveTowerTo(Vector3 position)
        {
            SelectedTowerRoot.DOMove(position, 0.2f);
        }

        private void ShowLine()
        {
            endPointLine.SetPosition(0, SelectedTowerRoot.position);
            endPointLine.SetPosition(1, SelectedTowerRoot.position);

            endPointLine.gameObject.SetActive(true);
            DOTween.To(() => endPointLine.widthMultiplier, val => { endPointLine.widthMultiplier = val; }, 1, 0.2f);

           
        }

        private void HideLine()
        {
            DOTween.To(() => endPointLine.widthMultiplier, val => { endPointLine.widthMultiplier = val; }, 0, 0.1f)
                .OnUpdate(() =>
                {
                    endPointLine.SetPosition(1, SelectedTowerRoot.position);
                })
                .OnComplete(() =>
                {
                    endPointLine.gameObject.SetActive(false);
                });
        }
    }
}