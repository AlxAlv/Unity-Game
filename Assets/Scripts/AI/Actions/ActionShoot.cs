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
            if ((!controller.EntityWeapon.IsAnySkillLoaded() || !controller.EntityWeapon.StunGuage.Stunned) && !controller.EntityWeapon.StunGuage.KnockedBack)
            {
                controller.EntityWeapon.CurrentWeapon.TriggerEnemySkill();
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
