using UnityEngine;

public class IdleState : IState
{
    private float _executeDuration;
    private float _executeTime;

    public void OnEnter(Bot bot)
    {
        bot.ForceControlBotAnimation(BotState.Idle);
        _executeTime = 0f;
        _executeDuration = Random.Range(2f, 5f);
    }

    public void OnExecute(Bot bot)
    {
        // if find other character then change state to attack
        if (bot.Target is not null && !bot.IsIgnoreAttack)
        {
            bot.SetState(new AttackState());
        }

        if (_executeTime >= _executeDuration)
        {
            bot.SetState(new MoveState());
        }

        _executeTime += Time.fixedDeltaTime;

    }

    public void OnExit(Bot bot)
    {

    }
}
