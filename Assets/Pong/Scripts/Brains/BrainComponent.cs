using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pong
{
    public class BrainComponent : MonoBehaviour
    {
        // Serializable fields

        [Header("Brain")]

        [SerializeField]
        private bool m_MovementAllowed = true;
        [SerializeField]
        private float m_MaxSpeed = 15f;
        [SerializeField]
        private Vector2 m_YRange = new Vector2(-4.25f, 4.25f);
        [SerializeField]
        private GoalComponent m_GoalToDefend = null;

        [SerializeField]
        private bool m_DrawGUI = false;
        [SerializeField]
        private bool m_DrawGizmos = false;

        // Fields

        private event Action<BrainComponent> m_OnGoalReceivedEvent = null;

        private Collider2D m_Collider2D = null;
        private Rigidbody2D m_Body2D = null;

        // ACCESSORS

        protected Collider2D mCollider2D
        {
            get
            {
                return m_Collider2D;
            }
        }

        protected Rigidbody2D mBody2D
        {
            get
            {
                return m_Body2D;
            }
        }

        protected Vector2 mYRange
        {
            get
            {
                return m_YRange;
            }
        }

        public bool movementAllowed
        {
            get
            {
                return m_MovementAllowed;
            }
            set
            {
                if (m_MovementAllowed != value)
                {
                    ChangeMovementAllowed(value);
                }
            }
        }

        public float maxSpeed
        {
            get
            {
                return m_MaxSpeed;
            }
            set
            {
                m_MaxSpeed = Mathf.Max(0f, value);
            }
        }

        public GoalComponent goalToDefend
        {
            get
            {
                return m_GoalToDefend;
            }
        }

        public event Action<BrainComponent> onGoalReceivedEvent
        {
            add { m_OnGoalReceivedEvent += value; }
            remove { m_OnGoalReceivedEvent -= value; }
        }

        // MonoBehaviour's interface

        private void OnGUI()
        {
            if (m_DrawGUI)
            {
                DrawGUI();
            }
        }

        private void OnDrawGizmos()
        {
            if (m_DrawGizmos)
            {
                DrawGizmos();
            }
        }

        private void Awake()
        {
            m_Collider2D = GetComponent<Collider2D>();
            m_Body2D = GetComponent<Rigidbody2D>();

            Setup();
        }

        private void OnEnable()
        {
            if (m_GoalToDefend != null)
            {
                m_GoalToDefend.onGoalEvent += OnGoalEvent;
            }
        }

        private void OnDisable()
        {
            if (m_GoalToDefend != null)
            {
                m_GoalToDefend.onGoalEvent -= OnGoalEvent;
            }
        }

        private void Start()
        {
            Begin();
        }

        private void Update()
        {
            if (m_MovementAllowed)
            {
                OnStep();
            }
        }

        private void FixedUpdate()
        {
            if (m_MovementAllowed)
            {
                OnFixedStep();
            }
        }
        
        // INTERNALS

        private void ChangeMovementAllowed(bool i_MovementAllowed)
        {
            m_MovementAllowed = i_MovementAllowed;

            if (m_Body2D != null)
            {
                m_Body2D.velocity = Vector2.zero;
            }

            OnMovementAllowedChanged();
        }

        private void OnGoalEvent()
        {
            if (m_OnGoalReceivedEvent != null)
            {
                m_OnGoalReceivedEvent(this);
            }
        }

        // Virtuals

        protected virtual void DrawGUI() { }
        protected virtual void DrawGizmos() { }

        protected virtual void Setup() { }
        protected virtual void Begin() { }
        protected virtual void OnStep() { }
        protected virtual void OnFixedStep() { }

        protected virtual void OnMovementAllowedChanged() { }
    }
}