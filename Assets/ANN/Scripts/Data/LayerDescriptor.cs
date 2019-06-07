using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ANN
{
    [Serializable]
    public class LayerDescriptor
    {
        // Serializable fields.

        [SerializeField]
        private int m_NeuronCount = 1;
        [SerializeField]
        private ActivationFunction m_NeuronActivationFunction = ActivationFunction.Identity;

        // ACCESSORS

        public int neuronCount
        {
            get
            {
                return Mathf.Max(1, m_NeuronCount);
            }
        }

        public ActivationFunction neuronActivationFunction
        {
            get
            {
                return m_NeuronActivationFunction;
            }
        }

        // CTOR

        public LayerDescriptor()
        {

        }
    }

    [Serializable]
    public class InputLayerDescriptor : LayerDescriptor
    {
        // Serializable fields

        [SerializeField]
        private int m_InputCount = 1;

        // ACCESSORS

        public int inputCount
        {
            get
            {
                return Mathf.Max(1, m_InputCount);
            }
        }

        // CTOR

        public InputLayerDescriptor()
            : base()
        {

        }
    }

    [Serializable]
    public class HiddenLayerDescriptor : LayerDescriptor
    {
        // CTOR

        public HiddenLayerDescriptor()
            : base()
        {

        }
    }

    [Serializable]
    public class OutputLayerDescriptor : LayerDescriptor
    {
        // CTOR

        public OutputLayerDescriptor()
            : base()
        {
        
        }
    }
}