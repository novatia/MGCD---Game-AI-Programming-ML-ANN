using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ANN
{
    public class ANNTestScene : MonoBehaviour
    {
        // Serializable fields

        [SerializeField]
        private BrainComponent m_BrainComponent = null;
        [SerializeField]
        private UIPage_VisualANN m_UIPage_VisualANN = null;

        // MonoBehaviour's interface

        private void Start()
        {
            if (m_BrainComponent != null)
            {
                m_BrainComponent.Init();

                if (m_UIPage_VisualANN != null)
                {
                    m_UIPage_VisualANN.Visualize(m_BrainComponent.GetANN());
                }
            }
        }
    }
}
