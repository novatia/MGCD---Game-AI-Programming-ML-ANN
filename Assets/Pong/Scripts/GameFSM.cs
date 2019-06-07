using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pong
{
    public class GameFSM : MonoBehaviour
    {
        // Serializable fields

        [SerializeField]
        private BallComponent m_Ball = null;
        [SerializeField]
        private BrainComponent[] m_Competitors = null;

        [SerializeField]
        private float m_ResetBallEvery = -1f;

        // Fields

        private int[] m_Goals = null;

        private bool m_MatchStarted = false;
        private bool m_MatchEnded = false;

        private float m_ResetBallTimer = 0f;

        // ACCESSORS

        public int competitorCount
        {
            get
            {
                return (m_Competitors != null) ? m_Competitors.Length : 0;
            }
        }

        // MonoBehaviour's interface

        private void Awake()
        {
            m_Goals = (m_Competitors != null && m_Competitors.Length > 0) ? new int[m_Competitors.Length] : null;
            ResetGoals();
        }
  
        private void OnEnable()
        {
            if (m_Competitors != null)
            {
                for (int index = 0; index < m_Competitors.Length; ++index)
                {
                    BrainComponent competitor = m_Competitors[index];

                    if (competitor == null)
                        continue;

                    competitor.onGoalReceivedEvent += OnGoalReceived;
                }
            }
        }

        private void OnDisable()
        {
            if (m_Competitors != null)
            {
                for (int index = 0; index < m_Competitors.Length; ++index)
                {
                    BrainComponent competitor = m_Competitors[index];

                    if (competitor == null)
                        continue;

                    competitor.onGoalReceivedEvent -= OnGoalReceived;
                }
            }
        }

        private void Start()
        {
            StartMatch();

            m_ResetBallTimer = m_ResetBallEvery;
        }

        private void Update()
        {
            if (m_ResetBallEvery > 0f)
            {
                m_ResetBallTimer = Mathf.Max(0f, m_ResetBallTimer - Time.deltaTime);
                if (m_ResetBallTimer == 0f)
                {
                    ResetBall();
                    LaunchBall();

                    m_ResetBallTimer = m_ResetBallEvery;
                }
            }
            else
            {
                m_ResetBallTimer = -1f;
            }
        }

        // LOGIC

        public BrainComponent GetCompetitor(int i_CompetitorIndex)
        {
            if (!IsValidCompetitorIndex(i_CompetitorIndex))
            {
                return null;
            }

            return m_Competitors[i_CompetitorIndex];
        }

        public int GetGoal(int i_CompetitorIndex)
        {
            if (!IsValidCompetitorIndex(i_CompetitorIndex))
            {
                return 0;
            }

            return m_Goals[i_CompetitorIndex];
        }

        public bool IsMatchRunning()
        {
            return m_MatchStarted && !m_MatchEnded;
        }

        // INTERNALS

        private void StartMatch()
        {
            if (m_MatchStarted)
                return;

            LaunchBall();

            m_MatchStarted = true;
        }

        private void EndMatch()
        {
            if (!m_MatchStarted || m_MatchEnded)
                return;

            m_MatchEnded = true;
        }

        private void SetGoal(int i_CompetitorIndex, int i_Goal)
        {
            if (!IsValidCompetitorIndex(i_CompetitorIndex))
            {
                return;
            }

            m_Goals[i_CompetitorIndex] = Mathf.Max(0, i_Goal);
        }

        private void AddGoal(int i_CompetitorIndex, int i_Goal)
        {
            if (!IsValidCompetitorIndex(i_CompetitorIndex))
            {
                return;
            }

            int goal = m_Goals[i_CompetitorIndex];
            goal += i_Goal;
            SetGoal(i_CompetitorIndex, goal);
        }

        private void ResetGoals()
        {
            if (m_Goals != null)
            {
                for (int index = 0; index < m_Goals.Length; ++index)
                {
                    SetGoal(index, 0);
                }
            }
        }

        private void ResetBall()
        {
            if (m_Ball != null)
            {
                m_Ball.ResetBall();
            }
        }

        private void LaunchBall()
        {
            if (m_Ball != null)
            {
                m_Ball.LaunchBall();
            }
        }

        private bool IsValidCompetitorIndex(int i_Index)
        {
            if (m_Competitors == null || m_Competitors.Length == 0)
            {
                return false;
            }

            return (i_Index >= 0 && i_Index < m_Competitors.Length);
        }

        private void OnGoalReceived(BrainComponent i_Competitor)
        {
            if (i_Competitor == null)
                return;

            for (int index = 0; index < competitorCount; ++index)
            {
                if (GetCompetitor(index) != i_Competitor)
                {
                    AddGoal(index, 1);
                }
            }

            ResetBall();

            LaunchBall();
        }
    }
}