using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Staff : Weapon
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
        if (owner.EntityType == Entity.EntityTypes.AI)
        {
            EnemySkill.Add(new EnemyIceBolt(this));
            EnemySkill.Add(new EnemyFireBolt(this));
            EnemySkill.Add(new LightningBolt(this));
            EnemySkill.Add(new BoulderToss(this));

            foreach (var skill in EnemySkill)
                skill.SetOwner(owner);
        }

        base.SetOwner(owner);
    }

    public override bool IsMagicWeapon()
    {
        return true;
    }
}
