public class Shield : Weapon
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void SetOwner(Entity owner)
    {
        base.SetOwner(owner);
    }

    public override bool IsShield()
    {
        return true;
    }
}
