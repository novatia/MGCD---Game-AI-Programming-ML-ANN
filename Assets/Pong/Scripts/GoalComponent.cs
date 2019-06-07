using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pong
{
    public class GoalComponent : MonoBehaviour
    {
        // Serializable fields

        [SerializeField]
        private string m_BallTag = "Ball";

        // Fields

        private event Action m_OnGoalEvent = null;

        // ACCESSORS

        public event Action onGoalEvent
        {
            add { m_OnGoalEvent += value; }
            remove { m_OnGoalEvent -= value; }
        }

        // MonoBehaviour's interface

        private void OnCollisionEnter2D(Collision2D i_Collision)
        {
            if (i_Collision.gameObject.tag == m_BallTag)
            {
                if (m_OnGoalEvent != null)
                {
                    m_OnGoalEvent();
                }
            }
        }
    }
}