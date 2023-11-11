/// <summary>
/// Interface for the state machine.
/// </summary>
public interface IState
{
    void OnEnter(Bot bot);
    void OnExecute(Bot bot);
    void OnExit(Bot bot);
}
