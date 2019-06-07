using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ANN
{
    public class UIWidget_VisualANNNeuron : MonoBehaviour
    {
        // Serializable fields

        [SerializeField]
        private RectTransform m_NeuronRoot = null;
        [SerializeField]
        private UIWidget_VisualANNNeuronLabel m_VisualANNNeuronLabelPrefab = null;

        // Fields

        private List<UIWidget_VisualANNNeuronLabel> m_VisualANNNeuronLabels = new List<UIWidget_VisualANNNeuronLabel>();

        // LOGIC

        public void Clear()
        {
            // Destory labels.

            for (int labelIndex = 0; labelIndex < m_VisualANNNeuronLabels.Count; ++labelIndex)
            {
                UIWidget_VisualANNNeuronLabel label = m_VisualANNNeuronLabels[labelIndex];

                if (label == null) continue;

                label.Clear();

                Destroy(label.gameObject);
            }

            m_VisualANNNeuronLabels.Clear();
        }
        
        public void Visualize(Neuron i_ANNNeuron)
        {
            Clear();

            if (i_ANNNeuron == null) return;

            for (int weightIndex = 0; weightIndex < i_ANNNeuron.inputCount; ++weightIndex)
            {
                float weight = i_ANNNeuron.GetWeight(weightIndex);
                Internal_AddLabel("W" + weightIndex + ": " + weight.ToString("F2"));
            }

            Internal_AddLabel("Bias: " + i_ANNNeuron.bias.ToString("F2"));

            Internal_AddLabel("AF: " + i_ANNNeuron.activationFunction.ToString());
        }

        // INTERNALS

        private void Internal_AddLabel(string i_LabelContent)
        {
            if (m_NeuronRoot == null || m_VisualANNNeuronLabelPrefab == null) return;

            UIWidget_VisualANNNeuronLabel newLabel = Instantiate<UIWidget_VisualANNNeuronLabel>(m_VisualANNNeuronLabelPrefab);
            Transform parent = (m_NeuronRoot != null) ? m_NeuronRoot : transform;
            newLabel.transform.SetParent(parent, false);

            newLabel.SetLabel(i_LabelContent);

            m_VisualANNNeuronLabels.Add(newLabel);
        }
    }
}