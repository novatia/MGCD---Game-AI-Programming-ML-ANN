using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using ANN;

namespace Pong
{
    public struct PongANNInput
    {

    }

    public struct PongANNOutput
    {

    }

    public class ANNBrainComponent : AIBrainComponent
    {
        // Serializable fields
       
        [Header("ANN")]

        [SerializeField]
        private ANNDescriptorObject m_ANNDescriptorAsset = null;
        [SerializeField]
        private ANNDescriptor m_ANNDescriptor = null;

        [SerializeField]
        private bool m_IsTrainingMode = false;

        [Header("Game")]

        [SerializeField]
        private LayerMask m_BounceLayerMask = 0;
        [SerializeField]
        private LayerMask m_GoalLayerMask = 0;

        // Fields

        private ANN.ANN m_ANN = null;

        private PongANNOutput m_LastANNOutput;

        // ACCESSORS

        public bool isTrainingMode
        {
            get
            {
                return m_IsTrainingMode;
            }
            set
            {
                m_IsTrainingMode = value;
            }
        }

        // BrainComponent's interface

        protected override void DrawGUI()
        {
            base.DrawGUI();
        }

        protected override void DrawGizmos()
        {
            base.DrawGizmos();
        }

        protected override void Setup()
        {
            base.Setup();

            ANNDescriptor annDescriptor = (m_ANNDescriptorAsset != null) ? m_ANNDescriptorAsset.ANNDescriptor : m_ANNDescriptor;
            m_ANN = new ANN.ANN(annDescriptor);
        }

        protected override void Begin()
        {
            base.Begin();
        }

        protected override void OnStep()
        {
            base.OnStep();

            bool bSpeedUp = Input.GetKey(KeyCode.T);
            bool bSlowDown = Input.GetKey(KeyCode.S);

            Time.timeScale = (bSpeedUp == bSlowDown) ? 1f : ((bSpeedUp) ? 2f : 0.25f);
        }

        protected override void OnFixedStep()
        {
            base.OnFixedStep();
        }

        protected override void OnMovementAllowedChanged()
        {
            base.OnMovementAllowedChanged();

            if (!movementAllowed)
            {
                Time.timeScale = 1f;
            }
        }

        // INTERNALS

        private PongANNOutput Internal_RunANN(PongANNInput i_Input)
        {
            float[] inputs = new float[5];

            float[] outputs = m_ANN.Run(inputs);

            PongANNOutput output = new PongANNOutput();
            return output;
        }

        private PongANNOutput Internal_TrainANN(PongANNInput i_Input, PongANNOutput i_DesiredOutput)
        {
            float[] inputs = new float[5];

            float[] desiredOutputs = new float[1];

            float[] outputs = m_ANN.Run(inputs);
            ANNFunctionLibrary.AdjustWeights(m_ANN, desiredOutputs);

            PongANNOutput output = new PongANNOutput();
            return output;
        }
    }
}