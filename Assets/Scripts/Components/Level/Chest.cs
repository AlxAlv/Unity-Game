using UnityEngine;

public class Chest : MonoBehaviour
{
	[Header("Reward Position")]
	[SerializeField] private float _yRandomPosition = 2f;
	[SerializeField] private float _xRandomPosition = 2f;

	[SerializeField] private GameObject[] _rewards;

	private Vector3 _rewardRandomPosition;
	private Animator _animator;
	private bool _canReward;
	private bool _rewardGiven;
	private readonly int _chestOpenedParamter = Animator.StringToHash("Rewarded");

	private void Start()
	{
		_rewardGiven = false;
		_canReward = false;
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{

		if (Input.GetKeyDown(KeyCode.V))
		{
			if (_canReward)
			{
				// Can reward the player
				RewardPlayer();
			}
		}
	}

	private void RewardPlayer()
	{
		if (_canReward && !_rewardGiven)
		{
			_animator.SetTrigger(_chestOpenedParamter);
			_rewardRandomPosition.x = Random.Range(-_xRandomPosition, _xRandomPosition);
			_rewardRandomPosition.y = Random.Range(-_yRandomPosition, _yRandomPosition);

			// Instantiate the reward
			Instantiate(SelectReward(), transform.position + _rewardRandomPosition, Quaternion.identity);

			_rewardGiven = true;
			SoundManager.Instance.Playsound("Audio/SoundEffects/ChestOpenFx");
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

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			_canReward = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			_canReward = false;
		}
	}
}
