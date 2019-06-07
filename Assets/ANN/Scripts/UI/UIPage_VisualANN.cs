using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ANN
{
    public class UIPage_VisualANN : MonoBehaviour
    {
        // Serializable fields

        [SerializeField]
        private RectTransform m_ANNRoot = null;
        [SerializeField]
        private UIWidget_VisualANNLayer m_VisualANNLayerPrefab = null;

        // Fields

        private List<UIWidget_VisualANNLayer> m_VisualANNLayers = new List<UIWidget_VisualANNLayer>();

        // LOGIC

        public void Clear()
        {
            // Destory visual layers.

            for (int layerIndex = 0; layerIndex < m_VisualANNLayers.Count; ++layerIndex)
            {
                UIWidget_VisualANNLayer visualLayer = m_VisualANNLayers[layerIndex];

                if (visualLayer == null) continue;

                visualLayer.Clear();

                Destroy(visualLayer.gameObject);
            }

            m_VisualANNLayers.Clear();
        }

        public void Visualize(ANN i_ANN)
        {
            Clear();

            if (i_ANN == null) return;

            for (int layerIndex = 0; layerIndex < i_ANN.layerCount; ++layerIndex)
            {
                Layer ANNLayer = i_ANN.GetLayer(layerIndex);
                Internal_AddLayer(ANNLayer);
            }
        }

        // INTERNALS

        private void Internal_AddLayer(Layer i_ANNLayer)
        {
            if (i_ANNLayer == null || m_VisualANNLayerPrefab == null) return;

            UIWidget_VisualANNLayer newVisualLayer = Instantiate<UIWidget_VisualANNLayer>(m_VisualANNLayerPrefab);
            Transform parent = (m_ANNRoot != null) ? m_ANNRoot : transform;
            newVisualLayer.transform.SetParent(parent, false);

            newVisualLayer.Visualize(i_ANNLayer);

            m_VisualANNLayers.Add(newVisualLayer);
        }
    }
}