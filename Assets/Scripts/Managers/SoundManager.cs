using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _combatMusic;
    [SerializeField] private AudioSource _arenaMusic;
    [SerializeField] private AudioSource _dungeonMusic;
    [SerializeField] private AudioSource _dungeonMasterMusic;
    [SerializeField] private float _combatDuration = 8.0f;
    
    private AudioSource _audioSource;
    private float _timer = 0.0f;

    private bool _isInCombat = false;
    private bool _isInArena = false;
    private bool _isInDungeon = false;
    private bool _isFightingDungeonMaster = false;

    public bool IsInCombat => _isInCombat;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        // Play Music
        PlayMusic("Audio/Music/Tutorial");

        AudioClip musicToPlay = Resources.Load<AudioClip>("Audio/Music/CombatMusic");
        _combatMusic.clip = musicToPlay;
        _combatMusic.volume = (0.5f);

        AudioClip arenaMusicToPlay = Resources.Load<AudioClip>("Audio/Music/ArenaMusic");
        _arenaMusic.clip = arenaMusicToPlay;
        _arenaMusic.volume = (0.5f);

        AudioClip dungeonMusicToPlay = Resources.Load<AudioClip>("Audio/Music/DungeonMusic");
        _dungeonMusic.clip = dungeonMusicToPlay;
        _dungeonMusic.volume = (0.5f);

        AudioClip dungeonMasterMusicToPlay = Resources.Load<AudioClip>("Audio/Music/DungeonMasterMusic");
        _dungeonMasterMusic.clip = dungeonMasterMusicToPlay;
        _dungeonMasterMusic.volume = (0.5f);
    }

    private void Update()
    {
        if ((Time.time < _timer))
        {
            if (!_isInCombat)
            {
                _isInCombat = true;
                _backgroundMusic.Pause();
	            _arenaMusic.Pause();
	            _dungeonMusic.Pause();
	            _dungeonMasterMusic.Pause();
                _combatMusic.Play();
            }

            SetHealthBars(true);
        }
        else if (_isInCombat)
        {
            _isInCombat = false;
            _combatMusic.Stop();

            if (_isFightingDungeonMaster)
	            _dungeonMasterMusic.Play();
            else if (_isInDungeon)
	            _dungeonMusic.Play();
            else if (_isInArena)
	            _arenaMusic.Play();
            else
	            _backgroundMusic.Play();

            SetHealthBars(false);
        }
    }

    public void Playsound(string soundPath)
    {
        AudioClip clipToPlay = Resources.Load<AudioClip>(soundPath);
        _audioSource.PlayOneShot(clipToPlay);
    }

    public void PlayMusic(string songPath)
    {
        AudioClip musicToPlay = Resources.Load<AudioClip>(songPath);
        _backgroundMusic.clip = musicToPlay;
        _backgroundMusic.volume = (0.5f);

        _backgroundMusic.Play();
    }

    public void UpdateCombatTimer()
    {
        _timer = Time.time + _combatDuration;
    }

    public void SetArenaStatus(bool isInArena)
    {
	    if (_isInArena && !isInArena)
	    {
		    _isInCombat = false;

            _arenaMusic.Stop();
		    _backgroundMusic.Play();
        }
        else if (!_isInArena && isInArena)
	    {
		    _isInCombat = false;

		    _backgroundMusic.Stop();
		    _arenaMusic.Play();
	    }

        _isInArena = isInArena;
    }

    public void SetDungeonStatus(bool isInDungeon)
    {
	    if (_isInDungeon && !isInDungeon)
	    {
		    _isInCombat = false;

		    _dungeonMusic.Stop();
		    _backgroundMusic.Play();
	    }
	    else if (!_isInDungeon && isInDungeon)
	    {
		    _isInCombat = false;

		    _backgroundMusic.Stop();
		    _dungeonMusic.Play();
	    }

	    _isInDungeon = isInDungeon;
    }

    public void SetDungeonMasterStatus(bool isFightingDungeonMaster)
    {
	    if (_isFightingDungeonMaster && !isFightingDungeonMaster)
	    {
		    _isInCombat = false;

		    _dungeonMasterMusic.Stop();
		    _backgroundMusic.Play();
	    }
	    else if (!_isFightingDungeonMaster && isFightingDungeonMaster)
	    {
		    _isInCombat = false;

		    _dungeonMusic.Stop();
		    _backgroundMusic.Stop();
		    _combatMusic.Stop();
		    _arenaMusic.Stop();
		    _dungeonMasterMusic.Play();
	    }

	    _isFightingDungeonMaster = isFightingDungeonMaster;
    }

    void SetHealthBars(bool isActive)
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("HealthBar");

        foreach (var healthbar in list)
        {
            SetChildrenToActive(healthbar.transform, isActive);
        }
    }

    void SetChildrenToActive(Transform parent, bool isActive)
    {
        foreach (Transform child in parent.transform)
        {
	        if (!child.gameObject.activeSelf && isActive)
	        {
		        UIBarOpen.Instance.OpenUpBar(parent.gameObject);
            }

            child.gameObject.SetActive(isActive);
            SetChildrenToActive(child, isActive);
        }
    }
}
