using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class AIBrainComponent : BrainComponent
    {
        // Serializable fields

        [Header("AIBrain")]

        [SerializeField]
        private BallComponent m_Ball = null;

        // ACCESSORS

        protected BallComponent mBall
        {
            get
            {
                return m_Ball;
            }
        }
    }
}