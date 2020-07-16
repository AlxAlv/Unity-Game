namespace Assets.Scripts.Skills.Archery
{
	public class ArcherySkill : BaseSkill
	{
		protected Bow _bowToUse;

		protected ArcherySkill (Bow bowToUse) : base(bowToUse)
		{
			_bowToUse = bowToUse;
		}

		public override bool IsBase()
		{
			return false;
		}
	}
}
