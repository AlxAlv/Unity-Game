using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUnlockerManager : Singleton<SkillUnlockerManager>
{
	[SerializeField] public SkillUnlocker _poisonArrowUnlocker;
	[SerializeField] public SkillUnlocker _chargedShotUnlocker;
	[SerializeField] public SkillUnlocker _frozenDaggersUnlocker;
	[SerializeField] public SkillUnlocker _boulderTossUnlocker;
	[SerializeField] public SkillUnlocker _healUnlocker;
	[SerializeField] public SkillUnlocker _skyfallSkillUnlocker;
    [SerializeField] public Animator _animator;

    private void Update()
    {
	    if (Input.GetKeyDown(KeyCode.Numlock) && Application.isEditor)
	    {
		    _poisonArrowUnlocker.IsUnlocked = true;
		    _chargedShotUnlocker.IsUnlocked = true;
		    _frozenDaggersUnlocker.IsUnlocked = true;
		    _boulderTossUnlocker.IsUnlocked = true;
		    _healUnlocker.IsUnlocked = true;
		    _skyfallSkillUnlocker.IsUnlocked = true;
	    }
    }

    public void UnlockPoisonArrow()
    {
        if (SpendGold(1000))
			_poisonArrowUnlocker.IsUnlocked = true;
    }

    public void UnlockChargedShot()
    {
	    if (SpendGold(1000))
            _chargedShotUnlocker.IsUnlocked = true;
    }

    public void UnlockFrozenDaggers()
    {
	    if (SpendGold(1000))
            _frozenDaggersUnlocker.IsUnlocked = true;
    }

    public void UnlockBoulderToss()
    {
	    if (SpendGold(1000))
            _boulderTossUnlocker.IsUnlocked = true;
    }

    public void UnlockHeal()
    {
	    if (SpendGold(1000))
            _healUnlocker.IsUnlocked = true;
    }

    public void UnlockSkyFall()
    {
	    if (SpendGold(1000))
            _skyfallSkillUnlocker.IsUnlocked = true;
    }

    public void OpenShop()
    {
	    _animator.SetBool("IsOpen", true);
    }

    public void CloseShop()
    {
	    _animator.SetBool("IsOpen", false);
    }

    private bool SpendGold(int goldAmount)
    {
	    if (CoinManager.Instance.RemoveCoins(goldAmount))
		    return true;

	    DialogManager.Instance.InstantSystemMessage("Not Enough Coins!");
	    return false;
    }
}
