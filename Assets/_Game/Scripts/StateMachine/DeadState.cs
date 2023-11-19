using UnityEngine;

public class DeadState : IState
{
    public void OnEnter(Bot bot)
    {
        bot.ForceControlBotAnimation(BotState.Dead);
    }

    public void OnExecute(Bot bot)
    {
    }

    public void OnExit(Bot bot)
    {

    }
}
