namespace Assets.Scripts.Skills.Magic
{
	public class MagicSkill : BaseSkill
	{
		protected Staff _staffToUse;

		protected MagicSkill(Staff staffToUse) : base(staffToUse)
		{
			_staffToUse = staffToUse;
		}

		public override bool IsBase()
		{
			return false;
		}
	}
}
