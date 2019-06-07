using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pong
{
    public class HumanBrainComponent : BrainComponent
    {
        // Fields

        private float m_VerticalInput = 0f;

        // BrainComponent's interface

        protected override void OnStep()
        {
            base.OnStep();

            m_VerticalInput = Input.GetAxis("Vertical");
        }

        protected override void OnFixedStep()
        {
            base.OnFixedStep();

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
    }
}