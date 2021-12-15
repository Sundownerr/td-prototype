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
    public class TowerPlacer : ListenerSystem
    {
        private enum State
        {
            Waiting, Replacing, BadPosition,
        }
        [SerializeField, Variable_R] private Variable pointerDown;
        [SerializeField, Variable_R] private Variable pointerUp;
        [SerializeField, Variable_R] private CellObjectVariable selectedTower;
        [SerializeField, Variable_R] private CellVariable highlightedCell;
        [SerializeField, Variable_R] private CellVariable selectedCell;
        [SerializeField, Variable_R] private CellVariable previousUsedCell;
        [SerializeField, Variable_R] private GameObjectVariable replaceLineGO;
        [SerializeField, Tweakable] private Vector3 lineOffset;
        [SerializeField, Tweakable] private UnityEvent onStartPlacing;
        [SerializeField, Tweakable] private UnityEvent onEndPlacingSuccess;
        [SerializeField, Tweakable] private UnityEvent onEndPlacingCanceled;

        private readonly Subject<int> finishedReplacing = new Subject<int>();
        private readonly Subject<int> canceledReplacing = new Subject<int>();
        private LineRenderer endPointLine;

        private State state;
        private Transform SelectedTowerRoot => selectedTower.Value.Reference.transform;

        public override void Initialize()
        {
            base.Initialize();

            replaceLineGO.Changed.Select(x => x.Current)
                .Subscribe(x =>
                {
                    endPointLine = x.GetComponent<LineRenderer>();
                    endPointLine.gameObject.SetActive(false);
                });


            HideLine();

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

            previousUsedCell.SetCell(selectedTower.CellObject.Cell);
            previousUsedCell.Publish();

            ShowLine();

            onStartPlacing?.Invoke();

            Observable.EveryUpdate().TakeUntil(finishedReplacing.Merge(canceledReplacing))
                .Subscribe(_ =>
                {
                    SelectedTowerRoot.position = Vector3.Lerp(SelectedTowerRoot.position,
                                                              GetTargetPosition() + lineOffset,
                                                              Time.deltaTime * 25f);

                    endPointLine.SetPosition(1, SelectedTowerRoot.position + lineOffset);
                });

            pointerDown.Published.Take(1)
                .TakeUntil(canceledReplacing)
                .Subscribe(_ =>
                {
                    finishedReplacing.OnNext(1);
                });

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