using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ANN
{
    public enum ActivationFunction
    {
        Identity,
        BinaryStep,
        Sigmoid,
        TanH,
        ReLU,
        LeakyReLU,
        SoftSign,
    }

    public class Neuron
    {
        // STATIC

        private static int s_MaxNeuronExecutionCached = 10;

        private static float s_WeightMinStartingValue = -1f;
        private static float s_WeightMaxStartingValue = 1f;
        private static float s_BiasMinStartingValue = -1f;
        private static float s_BiasMaxStartingValue = 1f;

        // Fields

        private int m_InputCount = 0;

        private List<float> m_Weights = null;
        private float m_Bias = 0f;

        private ActivationFunction m_ActivationFunction = ActivationFunction.Identity;

        private List<NeuronExecution> m_NeuronExecutionsCache = null;

        // ACCESSORS

        public int inputCount
        {
            get
            {
                return m_InputCount;
            }
        }

        public float bias
        {
            get
            {
                return m_Bias;
            }
            set
            {
                m_Bias = value;
            }
        }

        public int neuronExecutionsCachedCount
        {
            get
            {
                return m_NeuronExecutionsCache.Count;
            }
        }

        public ActivationFunction activationFunction
        {
            get
            {
                return m_ActivationFunction;
            }
            set
            {
                m_ActivationFunction = value;
            }
        }

        public NeuronExecution lastNeuronExecution
        {
            get
            {
                if (m_NeuronExecutionsCache.Count == 0)
                {
                    return null;
                }

                return m_NeuronExecutionsCache[m_NeuronExecutionsCache.Count - 1];
            }
        }

        // LOGIC

        public NeuronExecution GetNeuronExecution(int i_Index)
        {
            if (i_Index < 0 || i_Index >= m_NeuronExecutionsCache.Count)
            {
                return null;
            }

            return m_NeuronExecutionsCache[i_Index];
        }

        public float GetWeight(int i_Index)
        {
            float weight;
            TryGetWeight(i_Index, out weight);
            return weight;
        }

        public void SetWeight(int i_Index, float i_Value)
        {
            if (!IsValidIndex(i_Index))
            {
                return;
            }

            m_Weights[i_Index] = i_Value;
        }

        public bool TryGetWeight(int i_Index, out float o_Weight)
        {
            o_Weight = 0f;

            if (!IsValidIndex(i_Index))
            {
                return false;
            }

            o_Weight = m_Weights[i_Index];
            return true;
        }

        public float Run(float[] i_Inputs, bool i_CacheExecution = true)
        {
            float output;
            TryRun(i_Inputs, out output, i_CacheExecution);
            return output;
        }

        public bool TryRun(float[] i_Inputs, out float o_Output, bool i_CacheExecution = true)
        {
            o_Output = 0f;

            if (i_Inputs == null || i_Inputs.Length < m_InputCount)
            {
                return false;
            }

            NeuronExecution neuronExexecution = new NeuronExecution(m_InputCount);

            float output = 0f;

            for (int index = 0; index < m_InputCount; ++index)
            {
                float input = i_Inputs[index];
                float weight = m_Weights[index];

                neuronExexecution.SetInput(index, input);
                neuronExexecution.SetWeight(index, weight);
                
                float contribute = input * weight;
                output += contribute;
            }

            neuronExexecution.bias = m_Bias;
            output += m_Bias;

            output = ApplyActivationFunction(output);

            neuronExexecution.output = output;
            o_Output = output;

            if (i_CacheExecution)
            {
                CacheNeuronExecution(neuronExexecution);
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

        private bool IsValidIndex(int i_Index)
        {
            return (i_Index >= 0 && i_Index < m_InputCount);
        }

        private void Internal_Dump(StreamWriter i_Writer)
        {
            if (i_Writer == null)
                return;

            // Activation function.

            i_Writer.WriteLine("Activation function: " + m_ActivationFunction.ToString());

            // Weights.

            i_Writer.WriteLine("Weights");

            for (int index = 0; index < inputCount; ++index)
            {
                float weight = GetWeight(index);
                i_Writer.WriteLine("\t" + (index) + ": " + ANN.FloatToString(weight));
            }

            // Bias.

            i_Writer.WriteLine("Bias: " + ANN.FloatToString(bias));
        }

        private void Internal_DumpInline(StreamWriter i_Writer)
        {
            if (i_Writer == null)
                return;

            // Activation function.

            i_Writer.Write(m_ActivationFunction.ToString());

            // Weights.

            for (int index = 0; index < inputCount; ++index)
            {
                float weight = GetWeight(index);
                i_Writer.Write(",");
                i_Writer.Write(ANN.FloatToString(weight));
            }

            // Bias.

            i_Writer.Write(",");
            i_Writer.Write(ANN.FloatToString(bias));
        }

        private void CacheNeuronExecution(NeuronExecution i_NeuronExecution)
        {
            if (i_NeuronExecution == null)
            {
                return;
            }

            m_NeuronExecutionsCache.Add(i_NeuronExecution);

            while (m_NeuronExecutionsCache.Count > s_MaxNeuronExecutionCached)
            {
                m_NeuronExecutionsCache.RemoveAt(0);
            }
        }

        private float ApplyActivationFunction(float i_Value)
        {
            if (m_ActivationFunction == ActivationFunction.Identity) return ApplyActivationFunction_Identity(i_Value);
            if (m_ActivationFunction == ActivationFunction.BinaryStep) return ApplyActivationFunction_BinaryStep(i_Value);
            if (m_ActivationFunction == ActivationFunction.Sigmoid) return ApplyActivationFunction_Sigmoid(i_Value);
            if (m_ActivationFunction == ActivationFunction.TanH) return ApplyActivationFunction_TanH(i_Value);
            if (m_ActivationFunction == ActivationFunction.ReLU) return ApplyActivationFunction_ReLU(i_Value);
            if (m_ActivationFunction == ActivationFunction.LeakyReLU) return ApplyActivationFunction_LeakyReLU(i_Value);
            if (m_ActivationFunction == ActivationFunction.SoftSign) return ApplyActivationFunction_SoftSign(i_Value);

            return ApplyActivationFunction_Identity(i_Value);
        }

        // (https://en.wikipedia.org/wiki/Activation_function) --> Full list of activation functions.

        private float ApplyActivationFunction_Identity(float i_Value)
        {
            return i_Value;
        }

        private float ApplyActivationFunction_BinaryStep(float i_Value)
        {
            return (i_Value < 0f) ? 0f : 1f;
        }

        private float ApplyActivationFunction_Sigmoid(float i_Value)
        {
            float k = Mathf.Exp(i_Value);
            k = k / (1f + k);
            return k;
        }

        private float ApplyActivationFunction_TanH(float i_Value)
        {
            //float posExp = Mathf.Exp(i_Value);
            //float negExp = Mathf.Exp(-i_Value);

            //float value = (posExp - negExp) / (posExp + negExp);
            //return value;

            // Use a TanH(x) approximation.

            float k = Mathf.Exp(-2f * i_Value);
            return ((2f / (1f + k)) - 1f);
        }

        private float ApplyActivationFunction_ReLU(float i_Value)
        {
            float multiplier = (i_Value < 0f) ? 0f : 1f;
            return i_Value * multiplier;
        }

        private float ApplyActivationFunction_LeakyReLU(float i_Value)
        {
            float multiplier = (i_Value < 0f) ? 0.01f : 1f;
            return i_Value * multiplier;
        }

        private float ApplyActivationFunction_SoftSign(float i_Value)
        {
            float output = i_Value / (1f + Mathf.Abs(i_Value));
            return output;
        }

        // CTOR

        public Neuron(int i_InputCount)
        {
            // Cache input count.

            m_InputCount = Mathf.Max(1, i_InputCount);

            // Initialize weights.

            m_Weights = new List<float>();
            for (int weightIndex = 0; weightIndex < m_InputCount; ++weightIndex)
            {
                float randomWeight = UnityEngine.Random.Range(s_WeightMinStartingValue, s_WeightMaxStartingValue);
                m_Weights.Add(randomWeight);
            }

            // Initialize bias.

            m_Bias = UnityEngine.Random.Range(s_BiasMinStartingValue, s_BiasMaxStartingValue);

            // Initialize Neuron executions cache.

            m_NeuronExecutionsCache = new List<NeuronExecution>();
        }
    }
}