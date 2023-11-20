using UnityEngine;

public class AttackState : IState
{

    public void OnEnter(Bot bot)
    {
        bot.ForceControlBotAnimation(BotState.Attack);
    }

    public void OnExecute(Bot bot)
    {
        if (bot.Target is null)
        {
            bot.SetState(new MoveState());
        }
    }

    public void OnExit(Bot bot)
    {

    }
}
