using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ANN
{
    public class ANN
    {
        // STATIC

        private static string s_DUMP_NUMBER_FORMAT = "F4";

        public static string FloatToString(float i_Float)
        {
            return i_Float.ToString(s_DUMP_NUMBER_FORMAT);
        }

        // Fields

        private int m_ANNInputCount = 0;
        private int m_ANNOutputCount = 0;

        private float m_Alpha = 0f;

        private List<Layer> m_Layers = null;

        // ACCESSORS

        public int ANNInputCount
        {
            get
            {
                return m_ANNInputCount;
            }
        }

        public int ANNOutputCount
        {
            get
            {
                return m_ANNOutputCount;
            }
        }

        public int layerCount
        {
            get
            {
                return m_Layers.Count;
            }
        }

        public int hiddenLayerCount
        {
            get
            {
                return Mathf.Max(0, m_Layers.Count - 2);
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

        public Layer GetLayer(int i_Index)
        {
            if (!IsValidLayerIndex(i_Index))
            {
                return null;
            }

            return m_Layers[i_Index];
        }

        public float[] Run(float[] i_Inputs)
        {
            float[] outputs = null;
            TryRun(i_Inputs, out outputs);
            return outputs;
        }

        public bool TryRun(float[] i_Inputs, out float[] o_Outputs)
        {
            o_Outputs = null;

            if (i_Inputs == null || i_Inputs.Length < m_ANNInputCount)
            {
                return false;
            }

            float[] prevLayerOutputs = i_Inputs;
            float[] currentLayerOutputs = null;
            for (int layerIndex = 0; layerIndex < m_Layers.Count; ++layerIndex)
            {
                Layer layer = m_Layers[layerIndex];

                bool layerExecutionSuccess = layer.TryRun(prevLayerOutputs, out currentLayerOutputs);
                
                if (!layerExecutionSuccess)
                {
                    return false;
                }

                prevLayerOutputs = currentLayerOutputs;
                currentLayerOutputs = null;
            }

            o_Outputs = prevLayerOutputs;

            return true;
        }

        public void Dump(StreamWriter i_Writer, bool i_Inline)
        {
            if (i_Writer == null)
                return;

            if (i_Inline)
            {
                Internal_DumpInline(i_Writer);
            }
            else
            {
                Internal_Dump(i_Writer);
            }
        }

        // INTERNALS

        private bool IsValidLayerIndex(int i_Index)
        {
            return (i_Index >= 0 && i_Index < m_Layers.Count);
        }

        private void Internal_Dump(StreamWriter i_Writer)
        {
            if (i_Writer == null)
                return;

            // ANN - Common.

            i_Writer.WriteLine("ANN DATA");

            // Alpha.

            i_Writer.WriteLine("Alpha: " + FloatToString(alpha));

            i_Writer.WriteLine("");
            i_Writer.WriteLine("=================================================");

            // Layers.

            for (int index = 0; index < layerCount; ++index)
            {
                Layer layer = GetLayer(index);

                if (layer == null) continue;

                i_Writer.WriteLine("");
                i_Writer.WriteLine("LAYERS: Layer " + (index + 1).ToString());

                layer.Dump(i_Writer, false);

                i_Writer.WriteLine("");
                i_Writer.WriteLine("--------------------------------------------");
            }
        }

        private void Internal_DumpInline(StreamWriter i_Writer)
        {
            // Alpha.

            i_Writer.Write(FloatToString(alpha));

            // Layers.

            for (int index = 0; index < layerCount; ++index)
            {
                Layer layer = GetLayer(index);

                if (layer == null) continue;

                i_Writer.Write(",");
                layer.Dump(i_Writer, true);
            }
        }

        // CTOR

        public ANN(ANNDescriptor i_Descriptor)
        {
            m_Layers = new List<Layer>();

            // Create first layer.

            InputLayerDescriptor firstLayerDescriptor = i_Descriptor.firstLayerDescriptor;

            m_ANNInputCount = firstLayerDescriptor.inputCount;
            Layer firstLayer = new Layer(firstLayerDescriptor.neuronCount, m_ANNInputCount, firstLayerDescriptor.neuronActivationFunction);
            m_Layers.Add(firstLayer);

            // If this is a multi layer ANN, create other layers.

            if (!i_Descriptor.isSingleLayer)
            {
                // Create hiddens.

                for (int index = 0; index < i_Descriptor.hiddenLayerCount; ++index)
                {
                    HiddenLayerDescriptor hiddenLayerDescriptor = i_Descriptor.GetHiddenLayerDescriptor(index);
                    HiddenLayerDescriptor prevHiddenLayerDescriptor = i_Descriptor.GetHiddenLayerDescriptor(index - 1);

                    int layerNeuronCount = hiddenLayerDescriptor.neuronCount;
                    int layerInputCount = (index == 0) ? firstLayerDescriptor.neuronCount : prevHiddenLayerDescriptor.neuronCount;

                    Layer layer = new Layer(layerNeuronCount, layerInputCount, hiddenLayerDescriptor.neuronActivationFunction);
                    m_Layers.Add(layer);
                }

                // Create last layer.

                OutputLayerDescriptor lastLayerDescriptor = i_Descriptor.outputLayerDescriptor;
                LayerDescriptor prevLayerDescriptor = (m_Layers.Count - 1 > 0) ? (LayerDescriptor)i_Descriptor.GetHiddenLayerDescriptor(i_Descriptor.hiddenLayerCount - 1) : (LayerDescriptor)firstLayerDescriptor;

                m_ANNOutputCount = lastLayerDescriptor.neuronCount;
                int lastLayerInputCount = prevLayerDescriptor.neuronCount;

                Layer lastLayer = new Layer(m_ANNOutputCount, lastLayerInputCount, lastLayerDescriptor.neuronActivationFunction);
                m_Layers.Add(lastLayer);
            }

            // Init alpha.

            m_Alpha = i_Descriptor.alpha;
        }
    }
}