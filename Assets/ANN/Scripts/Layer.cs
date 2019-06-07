using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ANN
{
    public class Layer
    {
        // Fields

        private int m_OutputCount = 0;
        private int m_InputCount = 0;

        private List<Neuron> m_Neurons = null;

        // ACCESSORS

        public int outputCount
        {
            get
            {
                return m_OutputCount;
            }
        }

        public int neuronCount
        {
            get
            {
                return outputCount;
            }
        }

        public int inputCount
        {
            get
            {
                return m_InputCount;
            }
        }

        // LOGIC

        public Neuron GetNeuron(int i_Index)
        {
            if (!IsValidNeuronIndex(i_Index))
            {
                return null;
            }

            return m_Neurons[i_Index];
        }

        public bool TryRun(float[] i_Inputs, out float[] o_Outputs)
        {
            o_Outputs = null;

            if (i_Inputs == null || i_Inputs.Length < m_InputCount)
            {
                return false;
            }

            o_Outputs = new float[m_OutputCount];

            for (int index = 0; index < m_OutputCount; ++index)
            {
                Neuron neuron = m_Neurons[index];

                float neuronOutput;
                bool neuronExecutionSuccess = neuron.TryRun(i_Inputs, out neuronOutput);

                if (!neuronExecutionSuccess)
                {
                    o_Outputs = null;
                    return false;
                }

                o_Outputs[index] = neuronOutput;
            }

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

        private bool IsValidNeuronIndex(int i_Index)
        {
            return (i_Index >= 0 && i_Index < m_Neurons.Count);
        }

        private void Internal_Dump(StreamWriter i_Writer)
        {
            if (i_Writer == null)
                return;

            // Neurons.

            for (int index = 0; index < neuronCount; ++index)
            {
                Neuron neuron = GetNeuron(index);

                if (neuron == null) continue;

                i_Writer.WriteLine("");
                i_Writer.WriteLine("Neuron " + (index + 1).ToString());

                neuron.Dump(i_Writer, false);
            }
        }

        private void Internal_DumpInline(StreamWriter i_Writer)
        {
            if (i_Writer == null)
                return;

            // Neurons.

            for (int index = 0; index < neuronCount; ++index)
            {
                Neuron neuron = GetNeuron(index);

                if (neuron == null) continue;

                i_Writer.Write(",");
                neuron.Dump(i_Writer, true);
            }
        }

        // CTOR

        public Layer(int i_OutputCount, int i_InputCount, ActivationFunction i_ActivationFunction)
        {
            // Cache neuron count & input count.

            m_OutputCount = Mathf.Max(1, i_OutputCount);
            m_InputCount = Mathf.Max(1, i_InputCount);

            // Initialize neurons.

            m_Neurons = new List<Neuron>();
            for (int index = 0; index < m_OutputCount; ++index)
            {
                Neuron neuron = new Neuron(m_InputCount);
                neuron.activationFunction = i_ActivationFunction;

                m_Neurons.Add(neuron);
            }
        }
    }
}