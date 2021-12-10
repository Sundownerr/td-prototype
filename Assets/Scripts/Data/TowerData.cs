using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TestTD.Data
{
    [HideMonoScript]
    [CreateAssetMenu(fileName = "new-tower", menuName = "Data/Tower")]
    [Serializable]
    public class TowerData : Descriptor
    {
        [BoxGroup("TowerSettings", false)]
        [SerializeField] private TowerElement element;

        [BoxGroup("TowerSettings", false)]
        // [PreviewField(120, ObjectFieldAlignment.Right)]
        // [HideLabel]
        [SerializeField] private GameObject prefab;

        [BoxGroup("TowerSettings", false)]
        // [PreviewField(120, ObjectFieldAlignment.Right)]
        // [HideLabel]
        [SerializeField] private Sprite sprite;
        
        public GameObject Prefab => prefab;
        public Sprite Sprite => sprite;
        public TowerElement Element => element;

        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, DraggableItems = false)]
        [SerializeField]
        private List<FloatParameter> parameters = new List<FloatParameter>();

        public float GetParameterValue(FloatParameterSO data)
        {
            return parameters.Find(x => x.So == data).GetValue();
        }

        [SerializeField, BoxGroup("Debug", false)] private TowerParametersVariable defaultTowerParameters;
        
        [Button(ButtonSizes.Large), BoxGroup("Debug", false)]
        public void ValidateData()
        {
            var defaultParametersList = defaultTowerParameters.List;
            var currentParametersDescriptors = parameters.Select(x => x.So).ToList();

            FillMissingParameters(defaultParametersList);
            DeleteDuplicates();
            DeleteInvalidElements();
        }

        private void DeleteInvalidElements()
        {
            for (var i = 0; i < parameters.Count; i++)
            {
                if (parameters[i] == null || parameters[i].So == null)
                {
                    parameters.RemoveAt(i);
                }
            }
        }

        private void DeleteDuplicates()
        {
            for (var i = 0; i < parameters.Count; i++)
            {
               var sameParameters = parameters.FindAll(x => x.So == parameters[i].So);

               if (sameParameters.Count == 1)
                   continue;
               
               for (var k = sameParameters.Count - 1; k >= 1; k--)
               {
                   parameters.Remove(sameParameters[k]);
               }
            }
        }
        
        private void FillMissingParameters(List<FloatParameter> defaultParametersList)
        {
            var missingParameters = new List<FloatParameter>();

            foreach (var defaultParameter in defaultParametersList)
            {
               var matchingParameter = parameters.Find(x => x.So == defaultParameter.So);

               if (matchingParameter == null)
               {
                   missingParameters.Add(defaultParameter);
               }
            }
            
            foreach (var x in missingParameters)
            {
                parameters.Add(x);
            }
        }
    }
}