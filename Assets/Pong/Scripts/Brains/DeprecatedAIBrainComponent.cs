using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using ANN;

namespace Pong
{
    public class DeprecatedAIBrainComponent : AIBrainComponent
    {
        // Serializable fields

        [SerializeField]
        private float m_TimeToReachMultiplier = 1f;

        [Header("Game")]

        [SerializeField]
        private string m_BottomTag = "";
        [SerializeField]
        private string m_TopTag = "";
        [SerializeField]
        private LayerMask m_BounceLayerMask = 0;
        [SerializeField]
        private LayerMask m_GoalLayerMask = 0;

        // Fields

        private float m_DistanceFromTarget = 0f;

        private float m_NextSpeed = 0f;

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
        }

        protected override void Begin()
        {
            base.Begin();

            UpdateDistanceFromTarget();
        }

        protected override void OnStep()
        {
            base.OnStep();
        }

        protected override void OnFixedStep()
        {
            base.OnFixedStep();

            // Update position or velocity.

            float speed = Mathf.Clamp(m_NextSpeed * maxSpeed, -maxSpeed, maxSpeed);

            if (mBody2D != null && !mBody2D.isKinematic)
            {
                mBody2D.velocity = Vector2.up * speed;
            }
            else
            {
                float currentY = transform.position.y;

                float yOffset = speed * Time.fixedDeltaTime;
                float nextY = currentY + yOffset;
                nextY = Mathf.Clamp(nextY, mYRange.x, mYRange.y);

                transform.position = new Vector3(transform.position.x, nextY, transform.position.z);
            }

            // Compute next frame "input".

            Vector2 myPosition = new Vector2(transform.position.x, transform.position.y);
            Vector2 ballPosition = (mBall != null) ? mBall.position : Vector2.zero;
            Vector2 ballVelocity = (mBall != null) ? mBall.velocity : Vector2.zero;
            Vector2 ballDirection = (mBall != null) ? mBall.direction : Vector2.up;
            float ballSpeed = ballVelocity.magnitude;
            float ballMaxSpeed = (mBall != null) ? mBall.maxSpeed : 0f;

            // Raycast from ball to check if is reaching the target.

            RaycastHit2D raycastHit2D = Physics2D.Raycast(ballPosition, ballDirection, 100f, m_BounceLayerMask);
            if (raycastHit2D)
            {
                // If ball will reach the top or bottom, reflect it and raycast hit.

                if (raycastHit2D.collider.gameObject.tag == m_TopTag || raycastHit2D.collider.gameObject.tag == m_BottomTag)
                {
                    ballPosition = raycastHit2D.point;
                    ballPosition.y = Mathf.Clamp(ballPosition.y, mYRange.x, mYRange.y);

                    Vector2 reflectedBallDirection = Vector2.Reflect(ballDirection, raycastHit2D.normal);
                    raycastHit2D = Physics2D.Raycast(ballPosition, reflectedBallDirection, 100f, m_GoalLayerMask);
                }

                // Ball is reaching the target, calculate intersection y, account for paddle-target shift.

                GameObject goalToDefendGo = (goalToDefend != null) ? goalToDefend.gameObject : null;

                if (raycastHit2D && raycastHit2D.collider.gameObject == goalToDefendGo)
                {
                    Vector2 b = ballPosition;
                    Vector2 t = raycastHit2D.point;
                    float x = transform.position.x;

                    float y = 0f;

                    if (Mathf.Abs(t.x - b.x) > Mathf.Epsilon)
                    {
                        y = ((x * t.y) - (x * b.y) + (b.y * t.x) - (b.x * t.y)) / (t.x - b.x);
                    }
                    else
                    {
                        y = t.y;
                    }

                    Vector2 desiredPoint = new Vector2(x, y);

                    // Calculate time to reach the desired position.

                    Vector2 distanceToTarget = (desiredPoint - ballPosition);
                    float timeToReach = (distanceToTarget.sqrMagnitude > Mathf.Epsilon && ballSpeed > Mathf.Epsilon) ? (distanceToTarget.magnitude / ballSpeed) : 0f;

                    // Calculate velocity to reach desired position in time.

                    float deltaY = desiredPoint.y - transform.position.y;
                    float desiredYVelocity = (timeToReach > Mathf.Epsilon) ? deltaY / (timeToReach * m_TimeToReachMultiplier) : 0f;
                    desiredYVelocity = Mathf.Clamp(desiredYVelocity / maxSpeed, -1f, 1f);

                    m_NextSpeed = desiredYVelocity;
                }
                else
                {
                    // Ball is not reaching the target. Just stay.

                    m_NextSpeed = 0f;
                }
            }
            else
            {
                // Ball is not reaching the target. Just stay.

                m_NextSpeed = 0f;
            }
        }

        protected override void OnMovementAllowedChanged()
        {
            base.OnMovementAllowedChanged();

            if (!movementAllowed)
            {
                m_NextSpeed = 0f;
            }
        }

        // INTERNALS

        private void UpdateDistanceFromTarget()
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, -transform.up, 100f, m_BounceLayerMask);
            if (raycastHit2D)
            {
                m_DistanceFromTarget = raycastHit2D.distance;
            }
            else
            {
                m_DistanceFromTarget = 0f;
            }
        }
    }
}