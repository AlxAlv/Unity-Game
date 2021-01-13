using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MeleeWeapon
{
    [SerializeField] private float _attackDelay = 0.5f;

    private bool _isAttacking;

    public void Awake()
    {
        base.Awake();

        WeaponInfo.Damage = 1;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Start()
    {
        base.Start();
    }

    public void UseWeapon()
    {
        base.UseWeapon();
    }

    public override void SetOwner(Entity owner)
    {
        base.SetOwner(owner);
    }
}
