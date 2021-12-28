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
using Satisfy.Managers;
using Satisfy.Bricks;

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

    [Serializable, CreateAssetMenu(fileName = "Tower Placer", menuName = "System/Tower Placer")]
    [HideMonoScript]
    public class TowerPlacer : ScriptableObjectSystem
    {
        private enum State
        {
            Waiting, Replacing, BadPosition,
        }
        [SerializeField, Variable_R] private Satisfy.Bricks.Event pointerDown;
        [SerializeField, Variable_R] private Satisfy.Bricks.Event pointerUp;
        [SerializeField, Variable_R] private CellObjectVariable selectedTower;
        [SerializeField, Variable_R] private CellVariable highlightedCell;
        [SerializeField, Variable_R] private CellVariable selectedCell;
        [SerializeField, Variable_R] private GameObjectVariable replaceLineVar;
        [SerializeField, Variable_R] private Satisfy.Bricks.SelectableEvent cellReleased;
        [SerializeField, Tweakable] private Vector3 lineOffset;
        [SerializeField, Tweakable] private UnityEvent onStartPlacing;
        [SerializeField, Tweakable] private UnityEvent onEndPlacingSuccess;
        [SerializeField, Tweakable] private UnityEvent onEndPlacingCanceled;
        [SerializeField] private BaseListener listener;

        private readonly Subject<int> finishedReplacing = new Subject<int>();
        private readonly Subject<int> canceledReplacing = new Subject<int>();
        
        private State state;
        private Transform SelectedTowerRoot => selectedTower.Value.Reference.transform;
        private LineRenderer endPointLine;

        public override void Initialize()
        {
            base.Initialize();
            listener.Initialize();
            
            HideLine();

            replaceLineVar.Changed.Select(x => x.Current)
                .Subscribe(x =>
                {
                    endPointLine = x.GetComponent<LineRenderer>();
                });

            finishedReplacing.Subscribe(_ =>
            {
                HideLine();

                selectedTower.CellObject.UseCell(selectedCell.Cell);
                MoveTowerTo(selectedCell.Value.transform.position);

                onEndPlacingSuccess?.Invoke();
            });

            canceledReplacing.Subscribe(_ =>
            {
                HideLine();
                MoveTowerTo(selectedTower.CellObject.Cell.transform.position);

                onEndPlacingCanceled?.Invoke();
            });

            finishedReplacing.Merge(canceledReplacing)
                .Subscribe(_ =>
                {
                    state = State.Waiting;
                });
        }

        public void StartReplacing()
        {
            state = State.Replacing;

            cellReleased.Raise(selectedTower.CellObject.Cell);

            ShowLine();

            onStartPlacing?.Invoke();

            Observable.EveryUpdate().TakeUntil(finishedReplacing.Merge(canceledReplacing))
                .Subscribe(_ =>
                {
                    var targetPosition = GetTargetPosition() + lineOffset;
                    
                    SelectedTowerRoot.position = Vector3.Lerp(SelectedTowerRoot.position,
                                                              targetPosition,
                                                              Time.deltaTime * 25f);

                    endPointLine.SetPosition(1, targetPosition);
                });

            pointerDown.Raised.Take(1)
                .TakeUntil(canceledReplacing)
                .Subscribe(_ =>
                {
                    finishedReplacing.OnNext(1);
                });
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