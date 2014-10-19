using System;
using System.Collections.Generic;
using System.Linq;

namespace Azmyth
{
    public class StateMachine<TState> where TState : struct
    {
        private TState m_gameState;
        private Stack<TState> m_stateHistory = new Stack<TState>();

        public TState State
        {
            get 
            {
                return m_gameState;
            }
        }

        public TState LastState
        {
            get
            {
                TState state = default(TState);

                if (m_stateHistory.Count != 0)
                {
                    state = m_stateHistory.Peek();
                }

                return state;
            }
        }

        public void SetState(TState newState)
        {
            m_stateHistory.Push(m_gameState);

            m_gameState = newState;
        }

        public void PrevState()
        {
            if (m_stateHistory.Count > 0)
            {
                m_gameState = m_stateHistory.Pop();
            }
        }
    }
}
