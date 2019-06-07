using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pong
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallComponent : MonoBehaviour
    {
        // Serializable fields

        [SerializeField]
        private float m_StartForce = 400f;
        [SerializeField]
        private string m_TargetTag = "Bound_Target";

        [SerializeField]
        private float m_MinSpeed = 10f;
        [SerializeField]
        private float m_MaxSpeed = 15f;

        [SerializeField]
        private AudioClip m_DefaultBounceClip = null;
        [SerializeField]
        private AudioClip m_BackwallBounceClip = null;

        // Fields

        private Vector3 m_BallStartPosition = Vector3.zero;
        private Rigidbody2D m_Body2D = null;

        private AudioSource m_AudioSourceComponent = null;

        // ACCESSORS

        public Vector2 position
        {
            get
            {
                return transform.position;
            }
        }

        public Vector2 direction
        {
            get
            {
                Vector2 v = velocity;
                if (v.sqrMagnitude > Mathf.Epsilon)
                {
                    return v.normalized;
                }

                return Vector2.up;
            }
        }

        public Vector2 velocity
        {
            get
            {
                return (m_Body2D != null) ? m_Body2D.velocity : Vector2.zero;
            }
        }

        public float minSpeed
        {
            get
            {
                return m_MinSpeed;
            }
            set
            {
                m_MinSpeed = Mathf.Min(value, m_MaxSpeed);
                ClampVelocity();
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
                m_MaxSpeed = Mathf.Max(m_MinSpeed, value);
                ClampVelocity();
            }
        }

        // MonoBehaviour's interface

        private void Awake()
        {
            m_Body2D = GetComponent<Rigidbody2D>();

            m_AudioSourceComponent = gameObject.AddComponent<AudioSource>();
            m_AudioSourceComponent.playOnAwake = false;
            m_AudioSourceComponent.loop = false;
            m_AudioSourceComponent.spatialize = false;
        }

        private void Start()
        {
            m_BallStartPosition = transform.position;

            ResetBall();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetBall();
                LaunchBall();
            }
        }

        private void FixedUpdate()
        {
            ClampVelocity();
        }

        private void OnCollisionEnter2D(Collision2D i_Collision)
        {
            if (i_Collision.gameObject.tag == m_TargetTag)
            {
                PlaySfx(m_BackwallBounceClip);
            }
            else
            {
                PlaySfx(m_DefaultBounceClip);
            }
        }

        // LOGIC

        public void ResetBall()
        {
            m_Body2D.velocity = Vector3.zero;
            transform.position = m_BallStartPosition;
        }

        public void LaunchBall()
        {
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
            m_Body2D.AddForce(randomDir * m_StartForce);
        }

        // INTERNALS

        private void ClampVelocity()
        {
            if (m_MinSpeed > 0)
            {
                Vector2 v = velocity;
                if (v.sqrMagnitude < m_MinSpeed * m_MinSpeed)
                {
                    v = direction * m_MinSpeed;
                    m_Body2D.velocity = v;
                }
            }

            if (m_MaxSpeed > 0)
            {
                Vector2 v = velocity;
                if (v.sqrMagnitude > m_MaxSpeed * m_MaxSpeed)
                {
                    v = direction * m_MaxSpeed;
                    m_Body2D.velocity = v;
                }
            }
        }

        private void PlaySfx(AudioClip i_AudioClip)
        {
            if (i_AudioClip == null)
                return;

            m_AudioSourceComponent.clip = i_AudioClip;
            m_AudioSourceComponent.Play();
        }
    }
}