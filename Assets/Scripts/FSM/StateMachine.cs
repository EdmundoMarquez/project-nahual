using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectNahual.FSM
{
    public class StateMachine<TState> where TState : class, IState
    {
        private Dictionary<string, TState> _states;
        private TState _currentState;
        private TState _previousState;
        private string _currentStateName;
        private string _previousStateName;
        private bool _isInitialized;

        public TState CurrentState => _currentState;
        public string CurrentStateName => _currentStateName;
        public TState PreviousState => _previousState;
        public string PreviousStateName => _previousStateName;
        public bool IsInitialized => _isInitialized;

        public StateMachine()
        {
            _states = new Dictionary<string, TState>();
            _isInitialized = false;
        }

        public void AddState(string stateName, TState state)
        {
            if (string.IsNullOrEmpty(stateName))
                throw new ArgumentException("State name cannot be null or empty.", nameof(stateName));

            if (state == null)
                throw new ArgumentNullException(nameof(state), "State cannot be null.");

            if (_states.ContainsKey(stateName))
                throw new ArgumentException($"A state with the name '{stateName}' already exists.", nameof(stateName));

            _states.Add(stateName, state);
        }

        public bool RemoveState(string stateName)
        {
            if (string.IsNullOrEmpty(stateName))
                return false;

            if (_states.Remove(stateName))
            {
                // If we're removing the current state, reset to null
                if (_currentStateName == stateName)
                {
                    _currentState = null;
                    _currentStateName = null;
                }
                return true;
            }
            return false;
        }

        public TState GetState(string stateName)
        {
            _states.TryGetValue(stateName, out TState state);
            return state;
        }

        public bool HasState(string stateName)
        {
            return _states.ContainsKey(stateName);
        }

        public void SetInitialState(string stateName)
        {
            if (string.IsNullOrEmpty(stateName))
                throw new ArgumentException("Initial state name cannot be null or empty.", nameof(stateName));

            if (!_states.ContainsKey(stateName))
                throw new ArgumentException($"State '{stateName}' does not exist in the state machine.", nameof(stateName));

            _currentStateName = stateName;
            _currentState = _states[stateName];
            _isInitialized = true;

            // Initialize the initial state
            InitializeState(_currentState);
        }

        public bool ChangeState(string newStateName)
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("State machine is not initialized. Call SetInitialState first.");
                return false;
            }

            if (string.IsNullOrEmpty(newStateName))
            {
                Debug.LogWarning("Cannot transition to a null or empty state name.");
                return false;
            }

            if (!_states.ContainsKey(newStateName))
            {
                Debug.LogWarning($"State '{newStateName}' does not exist in the state machine.");
                return false;
            }

            if (newStateName == _currentStateName)
            {
                // Already in the requested state
                return true;
            }

            TState newState = _states[newStateName];

            // Exit current state
            if (_currentState != null)
            {
                ExitState(_currentState);
            }

            // Update state tracking
            _previousState = _currentState;
            _previousStateName = _currentStateName;
            _currentState = newState;
            _currentStateName = newStateName;

            // Enter new state
            EnterState(_currentState);

            return true;
        }

        public bool RevertToPreviousState()
        {
            if (string.IsNullOrEmpty(_previousStateName))
            {
                Debug.LogWarning("No previous state to revert to.");
                return false;
            }

            return ChangeState(_previousStateName);
        }

        public void Update()
        {
            if (!_isInitialized || _currentState == null)
                return;

            _currentState.Tick();
        }

        private void InitializeState(TState state)
        {
            if (state == null)
                return;

            try
            {
                state.Awake();
                state.Start();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error initializing state: {ex.Message}");
            }
        }

        private void EnterState(TState state)
        {
            if (state == null)
                return;

            try
            {
                state.Start();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error entering state: {ex.Message}");
            }
        }

        private void ExitState(TState state)
        {
            if (state == null)
                return;

            state.Stop();
        }

        public string[] GetAllStateNames()
        {
            string[] names = new string[_states.Count];
            _states.Keys.CopyTo(names, 0);
            return names;
        }

        public bool Reset()
        {
            if (!_isInitialized || string.IsNullOrEmpty(_currentStateName))
                return false;

            return ChangeState(_currentStateName);
        }

        public void Clear()
        {
            _states.Clear();
            _currentState = null;
            _previousState = null;
            _currentStateName = null;
            _previousStateName = null;
            _isInitialized = false;
        }
    }
}