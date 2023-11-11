using UnityEngine;

public class DeadState : IState
{
    private float _executeDuration;
    private float _executeTime;

    public void OnEnter(Bot bot)
    {
        bot.ForceControlBotAnimation(BotState.Dead);
        _executeTime = 0f;
        _executeDuration = Random.Range(2f, 5f);
    }

    public void OnExecute(Bot bot)
    {
        if (_executeTime >= _executeDuration)
        {
            bot.ReturnToPool();
        }
        _executeTime += Time.fixedDeltaTime;
    }

    public void OnExit(Bot bot)
    {

    }
}
