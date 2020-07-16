using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomReward : MonoBehaviour
{
    [Header("Settings]")]
    [SerializeField] private float _yRandomPosition = 2f;
    [SerializeField] private float _xRandomPosition = 2f;
    [SerializeField] [Range(0, 100)] private float _rewardChance = 50f;

    [Header("Rewards")]
    [SerializeField] private GameObject[] _rewards;

    private Vector3 _rewardRandomPosition;

    public void GiveReward()
    {
	    _rewardRandomPosition.x = Random.Range(-_xRandomPosition, _xRandomPosition);
	    _rewardRandomPosition.y = Random.Range(-_yRandomPosition, _yRandomPosition);

	    if ((Random.Range(0, 100)) > _rewardChance)
	    {
		    Instantiate(SelectReward(), transform.position + _rewardRandomPosition, Quaternion.identity);
	    }
    }

    private GameObject SelectReward()
    {
	    int randomRewardIndex = Random.Range(0, _rewards.Length);

	    for (int i = 0; i < _rewards.Length; i++)
	    {
		    return _rewards[randomRewardIndex];
	    }

	    return null;
    }
}
