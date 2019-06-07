using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ANN
{
    public class BrainComponent : MonoBehaviour
    {
        // Serializable fields

        [SerializeField]
        private ANNDescriptorObject m_ANNDescriptorObject = null;
        [SerializeField]
        private TrainingSetDescriptor m_FixedTrainingSet = null;
        [SerializeField]
        private int m_EpochToTrain = 10000;

        [SerializeField]
        private float[] m_Inputs = null;

        // Fields

        private bool m_bInitialized = false;

        private TrainingSet m_TrainingSet = null;
        bool m_bUsingFixedTrainingSet = false;

        private ANN m_ANN = null;

        // MonoBehaviour's interface

        private void Update()
        {
            if (!m_bInitialized)
                return;

            if (Input.GetKeyDown(KeyCode.R))
            {
                Run();   
            }
        }

        // LOGIC

        public ANN GetANN()
        {
            return m_ANN;
        }

        public void Init()
        {
            if (m_bInitialized)
                return;

            m_ANN = new ANN((m_ANNDescriptorObject != null) ? m_ANNDescriptorObject.ANNDescriptor : null);
            m_TrainingSet = (m_FixedTrainingSet != null) ? m_FixedTrainingSet.trainingSet : new TrainingSet();
            m_bUsingFixedTrainingSet = (m_FixedTrainingSet != null);

            if (m_bUsingFixedTrainingSet)
            {
                Train(m_ANN, m_TrainingSet);
            }

            m_bInitialized = true;
        }

        public void Uninit()
        {
            if (!m_bInitialized)
                return;

            m_bUsingFixedTrainingSet = false;
            m_TrainingSet = null;
            m_ANN = null;

            m_bInitialized = false;
        }

        // INTERNALS

        private void Train(ANN i_ANN, TrainingSet i_TrainingSet)
        {
            if (i_ANN == null || i_TrainingSet == null)
                return;

            if (i_ANN.ANNInputCount != i_TrainingSet.entryInputSize)
                return;

            for (int epoch = 0; epoch < m_EpochToTrain; ++epoch)
            {
                for (int trainingSetEntryIndex = 0; trainingSetEntryIndex < i_TrainingSet.entryCount; ++trainingSetEntryIndex)
                {
                    TrainingSetEntry trainingSetEntry = i_TrainingSet.GetEntry(trainingSetEntryIndex);

                    if (trainingSetEntry == null)
                        continue;

                    float[] inputs = new float[trainingSetEntry.inputCount];
                    for (int inputIndex = 0; inputIndex < inputs.Length; ++inputIndex)
                    {
                        inputs[inputIndex] = trainingSetEntry.GetInput(inputIndex);
                    }

                    float[] desiredOutputs = new float[trainingSetEntry.outputCount];
                    for (int outputIndex = 0; outputIndex < desiredOutputs.Length; ++outputIndex)
                    {
                        desiredOutputs[outputIndex] = trainingSetEntry.GetOutput(outputIndex);
                    }

                    float[] outputs;
                    bool ANNRunSuccess = i_ANN.TryRun(inputs, out outputs);
                    ANNFunctionLibrary.AdjustWeights(i_ANN, desiredOutputs);
                }
            }
        }

        private void Run()
        {
            float[] outputs;
            bool bRunSuccess = m_ANN.TryRun(m_Inputs, out outputs);

            if (bRunSuccess)
            {
                string outputStr = "output = (";
                for (int outputIndex = 0; outputIndex < outputs.Length; ++outputIndex)
                {
                    outputStr += outputs[outputIndex].ToString("F2");
                    outputStr += ((outputIndex == outputs.Length - 1) ? ")" : ", ");
                }

                Debug.Log(outputStr);
            }
            else
            {
                Debug.Log("Run failed!");
            }
        }
    }
}
