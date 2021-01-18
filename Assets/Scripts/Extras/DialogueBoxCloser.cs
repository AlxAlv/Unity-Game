using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxCloser : MonoBehaviour
{
	[SerializeField] public Animator _animator;

	public void CloseWindow()
	{
		_animator.SetBool("IsOpen", false);
	}
}
