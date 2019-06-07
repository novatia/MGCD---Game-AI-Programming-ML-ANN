using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ANN
{
    public class UIWidget_VisualANNNeuronLabel : MonoBehaviour
    {
        // Fields

        private Text m_Label = null;

        // MonoBehaviour's interface

        private void Awake()
        {
            m_Label = GetComponent<Text>();
        }

        // LOGIC

        public void Clear()
        {
            SetLabel("");
        }

        public void SetLabel(string i_LabelContent)
        {
            if (m_Label != null)
            {
                m_Label.text = i_LabelContent;
            }
        }
    }
}
