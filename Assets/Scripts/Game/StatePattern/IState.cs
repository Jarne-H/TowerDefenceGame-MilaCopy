

namespace StatePattern
{
    public interface IState
    {
        void OnEnter();
        void Update(float deltaTime);
        void OnExit();
    }
}
