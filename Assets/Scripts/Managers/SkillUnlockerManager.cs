using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUnlockerManager : Singleton<SkillUnlockerManager>
{
	[SerializeField] private SkillUnlocker _poisonArrowUnlocker;
	[SerializeField] private SkillUnlocker _chargedShotUnlocker;
	[SerializeField] private SkillUnlocker _frozenDaggersUnlocker;
	[SerializeField] private SkillUnlocker _boulderTossUnlocker;
	[SerializeField] private SkillUnlocker _healUnlocker;
	[SerializeField] private SkillUnlocker _skyfallSkillUnlocker;
    [SerializeField] private Animator _animator;

    private void Update()
    {
	    if (Input.GetKeyDown(KeyCode.Numlock))
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
        if (SpendGold(250))
			_poisonArrowUnlocker.IsUnlocked = true;
    }

    public void UnlockChargedShot()
    {
	    if (SpendGold(250))
            _chargedShotUnlocker.IsUnlocked = true;
    }

    public void UnlockFrozenDaggers()
    {
	    if (SpendGold(250))
            _frozenDaggersUnlocker.IsUnlocked = true;
    }

    public void UnlockBoulderToss()
    {
	    if (SpendGold(250))
            _boulderTossUnlocker.IsUnlocked = true;
    }

    public void UnlockHeal()
    {
	    if (SpendGold(250))
            _healUnlocker.IsUnlocked = true;
    }

    public void UnlockSkyFall()
    {
	    if (SpendGold(250))
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
