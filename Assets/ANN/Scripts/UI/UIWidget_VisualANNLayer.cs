using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ANN
{
    public class UIWidget_VisualANNLayer : MonoBehaviour
    {
        // Serializable fields

        [SerializeField]
        private RectTransform m_LayerRoot = null;
        [SerializeField]
        private UIWidget_VisualANNNeuron m_VisualANNNeuronPrefab = null;

        // Fields

        private List<UIWidget_VisualANNNeuron> m_VisualANNNeurons = new List<UIWidget_VisualANNNeuron>();

        // LOGIC

        public void Clear()
        {
            // Destory visual layers.

            for (int neuronIndex = 0; neuronIndex < m_VisualANNNeurons.Count; ++neuronIndex)
            {
                UIWidget_VisualANNNeuron visualNeuron = m_VisualANNNeurons[neuronIndex];

                if (visualNeuron == null) continue;

                visualNeuron.Clear();

                Destroy(visualNeuron.gameObject);
            }

            m_VisualANNNeurons.Clear();
        }

        public void Visualize(Layer i_ANNLayer)
        {
            Clear();

            if (i_ANNLayer == null) return;

            for (int neuronIndex = 0; neuronIndex < i_ANNLayer.neuronCount; ++neuronIndex)
            {
                Neuron ANNNeuron = i_ANNLayer.GetNeuron(neuronIndex);
                Internal_AddNeuron(ANNNeuron);
            }
        }

        // INTERNALS

        private void Internal_AddNeuron(Neuron i_Neuron)
        {
            if (i_Neuron == null || m_VisualANNNeuronPrefab == null) return;

            UIWidget_VisualANNNeuron newVisualNeuron = Instantiate<UIWidget_VisualANNNeuron>(m_VisualANNNeuronPrefab);
            Transform parent = (m_LayerRoot != null) ? m_LayerRoot : transform;
            newVisualNeuron.transform.SetParent(parent, false);

            newVisualNeuron.Visualize(i_Neuron);

            m_VisualANNNeurons.Add(newVisualNeuron);
        }
    }
}