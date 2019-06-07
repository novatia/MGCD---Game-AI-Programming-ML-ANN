using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ANN
{
    public class ANNDescriptorObject : ScriptableObject
    {
        [SerializeField]
        private ANNDescriptor m_ANNDescriptor = null;

        public ANNDescriptor ANNDescriptor
        {
            get
            {
                return m_ANNDescriptor;
            }
        }
    }
}
