using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : Singleton<ShopPanel>
{
	[SerializeField] public Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenShop()
    {
	    _animator.SetBool("IsOpen", true);
    }

    public void CloseShop()
    {
	    _animator.SetBool("IsOpen", false);
    }
}
