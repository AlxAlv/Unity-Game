namespace Assets.Scripts.Skills.Archery
{
	public class ArcherySkill : BaseSkill
	{
		protected Bow _bowToUse;

		protected ArcherySkill() : base() { }

		protected ArcherySkill (Weapon bowToUse) : base(bowToUse)
		{
			_bowToUse = bowToUse as Bow;
		}

		public override bool IsBase()
		{
			return false;
		}
	}
}
