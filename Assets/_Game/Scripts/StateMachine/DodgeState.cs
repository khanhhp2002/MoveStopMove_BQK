using UnityEngine;

public class DodgeState : IState
{
    private float _executeDuration;
    private float _executeTime;
    public void OnEnter(Bot bot)
    {
        bot.ForceControlBotAnimation(BotState.Move);
        _executeTime = 0f;
        _executeDuration = .75f;
    }

    public void OnExecute(Bot bot)
    {
        if (_executeTime >= _executeDuration)
        {
            if (bot.Target is not null && !bot.IsIgnoreAttack) // Chance to attack
            {
                bot.SetState(new AttackState());
            }
            else // Chance to idle or move
            {
                IState state = Random.Range(0, 2) == 0 ? new IdleState() : new MoveState();
                bot.SetState(state);
            }
        }

        _executeTime += Time.fixedDeltaTime;
    }

    public void OnExit(Bot bot)
    {

    }
}
