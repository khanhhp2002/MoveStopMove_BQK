using UnityEngine;

public class DeadState : IState
{
    private float _executeDuration;
    private float _executeTime;
    public void OnEnter(Bot bot)
    {
        _executeTime = 0f;
        _executeDuration = 2f;
    }

    public void OnExecute(Bot bot)
    {
        if (_executeTime >= _executeDuration)
        {
            bot.ForceControlBotAnimation(BotState.Dead);
        }
        _executeTime += Time.fixedDeltaTime;
    }

    public void OnExit(Bot bot)
    {

    }
}
