using System;
using UnityEngine;

public class Bot : CharacterBase, IPoolable<Bot>
{
    [SerializeField] private float _navigationIndicatorRange;
    private NavigationIndicator _navigationIndicator;

    private Action<Bot> _returnAction;

    /// <summary>
    /// FixedUpdate is called once per frame.
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Input.GetKeyDown(KeyCode.M))
        {
            NavigationIndicatorControl();
        }
        NavigationIndicatorControl();
    }

    /// <summary>
    /// Detects when the player collides with another collider.
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    /// <summary>
    /// Controls the indicator that shows the bot's position when it is out of the screen.
    /// </summary>
    private void NavigationIndicatorControl()
    {
        Vector2 botPos = Camera.main.WorldToScreenPoint(transform.position);
        if (botPos.x < -10f || botPos.x > Screen.width + 10f || botPos.y < -10f || botPos.y > Screen.height + 10f) // Out of screen
        {
            Vector2 playerPos = Camera.main.WorldToScreenPoint(GameplayManager.Instance.Player.transform.position);
            Vector2 direction = (botPos - playerPos).normalized;
            Vector2 navigationIndicatorPos = direction * _navigationIndicatorRange + playerPos;

            if (_navigationIndicator is null)
            {
                _navigationIndicator = NavigationIndicatorManager.Instance.Spawn(navigationIndicatorPos);
                _navigationIndicator.SetPoint(_killCount);
            }
            else
            {
                _navigationIndicator.transform.position = navigationIndicatorPos;
                _navigationIndicator.LookAt(direction);
            }
        }
        else // In screen
        {
            if (_navigationIndicator is not null)
            {
                _navigationIndicator.ReturnToPool();
                _navigationIndicator = null;
            }
        }
    }

    private void SetPoint()
    {
        if (_navigationIndicator is not null)
            _navigationIndicator.SetPoint(_killCount);
    }

    /// <summary>
    /// Initialize the bot return action.
    /// </summary>
    /// <param name="returnAction"></param>
    public void Initialize(Action<Bot> returnAction)
    {
        _returnAction = returnAction;
    }

    /// <summary>
    /// Returns the bot to the pool.
    /// </summary>
    public void ReturnToPool()
    {
        _returnAction?.Invoke(this);
    }
}
