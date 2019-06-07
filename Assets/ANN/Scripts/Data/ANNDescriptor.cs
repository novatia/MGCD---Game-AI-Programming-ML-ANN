using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ANN
{
    [Serializable]
    public class ANNDescriptor
    {
        // Fields

        [SerializeField]
        private InputLayerDescriptor m_FirstLayerDescriptor = null;
        [SerializeField]
        private bool m_IsSingleLayer = false;
        [SerializeField]
        private List<HiddenLayerDescriptor> m_HiddenLayerDescriptors = null;
        [SerializeField]
        private OutputLayerDescriptor m_OutputLayerDescriptor = null;

        [SerializeField]
        private float m_Alpha = 0f;

        // ACCESSORS

        public InputLayerDescriptor firstLayerDescriptor
        {
            get
            {
                return m_FirstLayerDescriptor;
            }
        }

        public bool isSingleLayer
        {
            get
            {
                return m_IsSingleLayer;
            }
        }

        public int hiddenLayerCount
        {
            get
            {
                if (m_HiddenLayerDescriptors == null)
                {
                    return 0;
                }

                return m_HiddenLayerDescriptors.Count;
            }
        }

        public OutputLayerDescriptor outputLayerDescriptor
        {
            get
            {
                return m_OutputLayerDescriptor;
            }
        }

        public float alpha
        {
            get
            {
                return m_Alpha;
            }
        }

        // LOGIC

        public HiddenLayerDescriptor GetHiddenLayerDescriptor(int i_Index)
        {
            if (i_Index < 0 || i_Index >= m_HiddenLayerDescriptors.Count)
            {
                return null;
            }

            return m_HiddenLayerDescriptors[i_Index];
        }

        // CTOR

        public ANNDescriptor()
        {

        }
    }
}