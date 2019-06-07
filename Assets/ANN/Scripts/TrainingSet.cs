using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ANN
{
    [Serializable]
    public class TrainingSetEntry
    {
        // Serializable fields

        [SerializeField]
        private string m_EntryName = "";
        [SerializeField]
        private List<float> m_Inputs = null;
        [SerializeField]
        private List<float> m_Outputs = null;

        // ACCESSORS

        public string entryName
        {
            get
            {
                return m_EntryName;
            }
        }

        public int inputCount
        {
            get
            {
                return m_Inputs.Count;
            }
        }

        public int outputCount
        {
            get
            {
                return m_Outputs.Count;
            }
        }

        // LOGIC

        public float GetInput(int i_Index)
        {
            if (i_Index < 0 || i_Index >= m_Inputs.Count)
            {
                return 0f;
            }

            return m_Inputs[i_Index];
        }

        public float GetOutput(int i_Index)
        {
            if (i_Index < 0 || i_Index >= m_Outputs.Count)
            {
                return 0f;
            }

            return m_Outputs[i_Index];
        }

        public void AddInput(float i_Input)
        {
            m_Inputs.Add(i_Input);
        }

        public void AddOutput(float i_Output)
        {
            m_Outputs.Add(i_Output);
        }

        // CTOR

        public TrainingSetEntry(string i_EntryName)
        {
            m_Inputs = new List<float>();
            m_Outputs = new List<float>();

            m_EntryName = i_EntryName;
        }
    }

    [Serializable]
    public class TrainingSet
    {
        // Serializable fields

        [SerializeField]
        private List<TrainingSetEntry> m_Entries = null;

        // ACCESSORS

        public int entryCount
        {
            get
            {
                return m_Entries.Count;
            }
        }

        public int entryInputSize
        {
            get
            {
                TrainingSetEntry firstEntry = GetEntry(0);
                return (firstEntry != null) ? firstEntry.inputCount : 0;
            }
        }

        // LOGIC

        public void Clear()
        {
            m_Entries.Clear();
        }

        public void AddEntry(TrainingSetEntry i_Entry)
        {
            bool checkInputSize = (m_Entries.Count > 0);

            if (checkInputSize)
            {
                if (i_Entry.inputCount != entryInputSize)
                {
                    return;
                }
            }

            m_Entries.Add(i_Entry);
        }

        public TrainingSetEntry GetEntry(int i_Index)
        {
            if (i_Index < 0 || i_Index >= m_Entries.Count)
            {
                return null;
            }

            return m_Entries[i_Index];
        }

        // CTOR

        public TrainingSet()
        {
            m_Entries = new List<TrainingSetEntry>();
        }
    }
}