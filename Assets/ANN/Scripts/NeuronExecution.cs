using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ANN
{
    public class NeuronExecution
    {
        // Fields

        private int m_InputCount = 0;

        private float[] m_Inputs = null;
        private float[] m_Weights = null;
        private float m_Bias = 0f;

        private float m_Output = 0f;

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

        public float output
        {
            get
            {
                return m_Output;
            }
            set
            {
                m_Output = value;
            }
        }

        // LOGIC

        public void SetInput(int i_Index, float i_Input)
        {
            if (!IsValidIndex(i_Index))
            {
                return;
            }

            m_Inputs[i_Index] = i_Input;
        }

        public void SetWeight(int i_Index, float i_Weight)
        {
            if (!IsValidIndex(i_Index))
            {
                return;
            }

            m_Weights[i_Index] = i_Weight;
        }

        public float GetInput(int i_Index)
        {
            float input;
            TryGetInput(i_Index, out input);
            return input;
        }

        public float GetWeight(int i_Index)
        {
            float weight;
            TryGetWeight(i_Index, out weight);
            return weight;
        }

        public bool TryGetInput(int i_Index, out float o_Input)
        {
            o_Input = 0f;

            if (!IsValidIndex(i_Index))
            {
                return false;
            }

            o_Input = m_Inputs[i_Index];
            return true;
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

        // INTERNALS

        private bool IsValidIndex(int i_Index)
        {
            return (i_Index >= 0 && i_Index < m_InputCount);
        }

        // CTOR

        public NeuronExecution(int i_InputCount)
        {
            // Cache input count.

            m_InputCount = Mathf.Max(1, i_InputCount);

            // Initialize inputs & weights.

            m_Inputs = new float[m_InputCount];
            m_Weights = new float[m_InputCount];
        }
    }
}