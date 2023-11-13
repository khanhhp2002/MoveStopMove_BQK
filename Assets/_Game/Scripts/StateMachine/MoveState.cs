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
        if (bot.Target is not null)
        {
            bot.SetState(new AttackState());
            return;
        }
        if (_executeTime >= _executeDuration)
        {
            IState state = Random.Range(0, 3) == 0 ? new IdleState() : new MoveState();
            bot.SetState(state);
        }
        _executeTime += Time.fixedDeltaTime;
    }

    public void OnExit(Bot bot)
    {
        bot.SetMoveDirection();
    }
}
