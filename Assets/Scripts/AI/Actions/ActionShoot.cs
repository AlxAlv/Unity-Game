using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Shoot", fileName = "ActionShoot")]
public class ActionShoot : AIAction
{
    private Vector2 _aimDirection;

    public override void Act(AIStateController controller)
    {
        DetermineAim(controller);
        ShootPlayer(controller);

        ExclamationMark.Create(controller.SkillLoadingTransform.position, controller.gameObject.GetInstanceID());
        controller.EntityMovement.RemoveFollowTarget();
    }

    private void ShootPlayer(AIStateController controller)
    {
        SoundManager.Instance.UpdateCombatTimer();

        controller.EntityMovement.StopAIMoving();

        if (controller.EntityWeapon.CurrentWeapon != null)
        {
            SetWeaponAim(controller, _aimDirection);

            // Attack
            if ((!controller.EntityWeapon.IsAnySkillLoading() || !controller.EntityWeapon.StunGuage.Stunned) && !controller.EntityWeapon.StunGuage.KnockedBack && controller.IsTimePassed())
            {
                // Create A Random Time Until Next Click
	            Debug.Log("Enemy Click");
	            controller.ResetTime(Random.Range(0.0f, 3.0f));

                controller.EntityWeapon.CurrentWeapon.TriggerEnemySkill();
            }
            else
            {
	            Debug.Log("Waiting on the next click...");
            }
        }
    }

    private void DetermineAim(AIStateController controller)
    {
        _aimDirection = controller.transform.position - controller.Target.position;
    }

    private void SetWeaponAim(AIStateController controller, Vector2 aim)
    {
        WeaponAim[] weaponAims = controller.GetWeaponAims();

        foreach (var weaponAim in weaponAims)
            weaponAim.SetAim(aim);
    }
}
