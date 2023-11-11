using UnityEngine;

public class MoveState : IState
{
    private float _executeDuration;
    private float _executeTime;

    public void OnEnter(Bot bot)
    {
        bot.ForceControlBotAnimation(BotState.Move);
        _executeTime = 0f;
        _executeDuration = Random.Range(2f, 5f);
    }

    public void OnExecute(Bot bot)
    {
        if (_executeTime >= _executeDuration)
        {
            IState state = Random.Range(0, 2) == 0 ? new IdleState() : new AttackState();
            bot.SetState(state);
        }
        _executeTime += Time.fixedDeltaTime;
    }

    public void OnExit(Bot bot)
    {

    }
}
