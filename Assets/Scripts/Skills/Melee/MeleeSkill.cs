using UnityEngine;

namespace Assets.Scripts.Skills.Melee
{
	public class MeleeSkill : BaseSkill
	{
		protected Sword _swordToUse;
		protected string _meleeFxPath;

		protected MeleeSkill(Sword swordToUse) : base(swordToUse)
		{
			_swordToUse = swordToUse;
		}

		protected override void Execute()
		{
			base.Execute();
			_swordToUse.UseWeapon();

			if (_meleeFxPath.Length > 0)
				SoundManager.Instance.Playsound(_meleeFxPath);
		}

		protected override void UpdateDamage()
		{
			_swordToUse.SkillDamage = _damageAmount;
			_swordToUse.StunTime = _stunTime;
			_swordToUse.KnockbackAmount = _knockBackAmount;
			base.UpdateDamage();
		}

		public override bool IsBase()
		{
			return false;
		}
	}
}
