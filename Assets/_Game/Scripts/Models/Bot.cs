using UnityEngine;

public class Bot : CharacterBase
{
    private NavigationIndicator _navigationIndicator;

    /// <summary>
    /// FixedUpdate is called once per frame.
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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
    protected void NavigationIndicatorControl()
    {
        Vector2 botPos = Camera.main.WorldToScreenPoint(transform.position);
        if (botPos.x < -10f || botPos.x > Screen.width + 10f || botPos.y < -10f || botPos.y > Screen.height + 10f) // Out of screen
        {
            Vector2 botClampPos = ClampPositionWithScreenSize(botPos);
            if (_navigationIndicator is null)
            {
                _navigationIndicator = NavigationIndicatorManager.Instance.Spawn(botClampPos);
            }
            else
            {
                _navigationIndicator.transform.position = botClampPos;
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

    /// <summary>
    /// Clamps the bot's position with the screen size.
    /// </summary>
    /// <param name="botPosition"></param>
    /// <returns></returns>
    private Vector2 ClampPositionWithScreenSize(Vector2 botPosition)
    {
        Vector2 playerPos = Camera.main.WorldToScreenPoint(GameplayManager.Instance.Player.transform.position);
        Vector2 screenPos = Vector2.Lerp(botPosition, playerPos, 0.1f);
        return new Vector2(Mathf.Clamp(screenPos.x, 0, Screen.width), Mathf.Clamp(screenPos.y, 0, Screen.height));
    }
}
