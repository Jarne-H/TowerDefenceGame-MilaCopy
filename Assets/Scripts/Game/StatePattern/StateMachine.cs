

namespace StatePattern
{
    public class StateMachine
    {
        private IState _currentState;

        public StateMachine(IState startState)
        {
            _currentState = startState;
            _currentState.OnEnter();
        }

        public void Update(float deltaTime)
        {
            _currentState.Update(deltaTime);
        }

        public void MoveToState(IState newState)
        {
            if (newState != _currentState)
            {
                _currentState.OnExit();
                _currentState = newState;
                _currentState.OnEnter();
            }            
        }
    }
}
