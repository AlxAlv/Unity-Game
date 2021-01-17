public class EnemyInfo : EntityInfo
{
    public override void SetShowInfo(bool showInfo)
    {
        base.SetShowInfo(showInfo);
    }

    protected override void Update()
    {
        base.Update();

        if (_showInfo && GetComponent<LootHelper>() != null && GetComponent<Health>() != null)
        {
            _textObject.text = "Lv " + GetComponent<Exp>().CurrentLevel.ToString() + " (" + GetComponent<Health>().m_currentHealth + "/" + GetComponent<Health>().m_maxHealth + ")";
        }
    }
}
