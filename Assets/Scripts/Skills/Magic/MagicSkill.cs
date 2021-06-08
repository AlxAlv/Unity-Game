namespace Assets.Scripts.Skills.Magic
{
	public class MagicSkill : BaseSkill
	{
		protected Staff _staffToUse;

		protected MagicSkill() : base() { }

		protected MagicSkill(Weapon staffToUse) : base(staffToUse)
		{
			_staffToUse = staffToUse as Staff;
		}

		public override bool IsBase()
		{
			return false;
		}
	}
}
