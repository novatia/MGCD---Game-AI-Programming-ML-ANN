using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ANN
{
    public class TrainingSetDescriptor : ScriptableObject
    {
        [SerializeField]
        private TrainingSet m_TrainingSet = null;

        public TrainingSet trainingSet
        {
            get
            {
                return m_TrainingSet;
            }
        }
    }
}