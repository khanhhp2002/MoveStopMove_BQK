using UnityEngine;

public class AttackState : IState
{
    private float _executeDuration;
    private float _executeTime;
    public void OnEnter(Bot bot)
    {
        bot.ForceControlBotAnimation(BotState.Attack);
        _executeTime = 0f;
        _executeDuration = Random.Range(4f, 6f);
    }

    public void OnExecute(Bot bot)
    {
        if (bot.Target is null || _executeTime >= _executeDuration)
        {
            bot.IsIgnoreAttack = true;
            bot.SetState(new MoveState());
        }

        _executeTime += Time.fixedDeltaTime;
    }

    public void OnExit(Bot bot)
    {

    }
}
