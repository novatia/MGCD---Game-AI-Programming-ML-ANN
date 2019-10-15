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
        public float y_ball;
    }

    public struct PongANNOutput
    {
        public float y_competitor;
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
            PongANNInput i_Input = new PongANNInput();
            i_Input.y_ball = mBall.transform.position.y;

            PongANNOutput i_DesiredOutput;
            i_DesiredOutput.y_competitor = mBall.transform.position.y;

            PongANNOutput i_Output;

            if (m_IsTrainingMode)
                i_Output = Internal_TrainANN(i_Input, i_DesiredOutput);
            else
                i_Output = Internal_RunANN(i_Input);


            float m_VerticalInput = i_Output.y_competitor;



            float targetSpeed = Mathf.Abs(m_VerticalInput * maxSpeed);
            Vector2 targetVelocity = Vector2.up * targetSpeed * Mathf.Sign(m_VerticalInput);

            if (mBody2D != null && !mBody2D.isKinematic)
            {
                mBody2D.velocity = targetVelocity;
            }
            else
            {
                float currentY = transform.position.y;

                float yOffset = targetSpeed * Time.fixedDeltaTime;
                float nextY = currentY + yOffset;
                nextY = Mathf.Clamp(nextY, mYRange.x, mYRange.y);

                transform.position = new Vector3(transform.position.x, nextY, transform.position.z);
            }


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
            float[] inputs = new float[1];
            inputs[0] = i_Input.y_ball;

            float[] outputs = m_ANN.Run(inputs);

            PongANNOutput output = new PongANNOutput();
            output.y_competitor = outputs[0];
            return output;
        }

        private PongANNOutput Internal_TrainANN(PongANNInput i_Input, PongANNOutput i_DesiredOutput)
        {
            float[] inputs = new float[1];

            inputs[0] = i_Input.y_ball;
            float[] desiredOutputs = new float[1];
            if (i_Input.y_ball > 0)
            {
                desiredOutputs[0] = 1;
                inputs[0] += 0.2f;
            }
            else
            {
                if (i_Input.y_ball < 0)
                {
                    desiredOutputs[0] = -1;
                    inputs[0] -= 0.2f;
                }
                else
                {
                    desiredOutputs[0] = 0;
                }
            }
            
            float[] outputs = m_ANN.Run(inputs);
            ANNFunctionLibrary.AdjustWeights(m_ANN, desiredOutputs);

            PongANNOutput output = new PongANNOutput();
            output.y_competitor = outputs[0];
            return output;
        }
    }
}