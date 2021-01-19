using UnityEngine;

public class EntityRun : EntityComponent
{
    private float _staminaTimer = 0.0f;
    private float _staminaDuration = 0.3f;
    private float _staminaToUse = 1.0f;
    
    protected override void HandleInput()
    {

        if (IsRunning())
        {
            if (_staminaTimer < _staminaDuration)
            {
                _staminaTimer += Time.deltaTime;
            }
            else
            {
                _staminaTimer = 0.0f;
                
                if (!_stamina.UseStamina(_staminaToUse))
                {
                    StopRunning();
                }
            }
        }

		// Note: This code can be uncommented out to add the running mechanic again
		if (Input.GetKey(KeyCode.X) && !IsRunning() && (m_movement.m_moveSpeed != 0))
		{
			Run();
		}
		else if (Input.GetKeyUp(KeyCode.X))
		{
			StopRunning();
		}
	}

    private void Run()
    {
        if (_stamina.UseStamina(_staminaToUse))
        {
            _staminaTimer = 0.0f;
            m_movement.RunMovementModifier = (1.5f);
        }
    }

    private void StopRunning()
    {
        m_movement.RunMovementModifier = 1.0f;
    }

    private bool IsRunning()
    {
        return ((m_movement.RunMovementModifier != 1) && (m_movement.m_moveSpeed != 0));
    }
}
