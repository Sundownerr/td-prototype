using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TestTD.Data
{
    [Serializable]
    public static class ParameterValidator
    {
        public static void ValidateAndCorrect(
            this List<FloatParameter> parameters,
            FloatParametersVariable defaultParameters)
        {
            FillMissingParameters(parameters, defaultParameters);
            DeleteDuplicates(parameters);
            DeleteInvalidElements(parameters);
        }

        private static void DeleteInvalidElements(List<FloatParameter> parameters)
        {
            for (var i = 0; i < parameters.Count; i++)
            {
                if (parameters[i] == null || parameters[i].Data == null)
                {
                    parameters.RemoveAt(i);
                }
            }
        }

        private static void DeleteDuplicates(List<FloatParameter> parameters)
        {
            for (var i = 0; i < parameters.Count; i++)
            {
                var sameParameters = parameters.FindAll(x => x.Data == parameters[i].Data);

                if (sameParameters.Count == 1)
                    continue;

                for (var k = sameParameters.Count - 1; k >= 1; k--)
                {
                    parameters.Remove(sameParameters[k]);
                }
            }
        }

        private static void FillMissingParameters(
            List<FloatParameter> parameters,
            FloatParametersVariable defaultParameters)
        {
            var missingParameters = new List<FloatParameter>();

            foreach (var defaultParameter in defaultParameters.List)
            {
                var matchingParameter = parameters.Find(x => x.Data == defaultParameter.Data);

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