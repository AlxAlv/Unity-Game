using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _combatMusic;
    [SerializeField] private AudioSource _arenaMusic;
    [SerializeField] private float _combatDuration = 8.0f;
    
    private AudioSource _audioSource;
    private float _timer = 0.0f;

    private bool _isInCombat = false;
    private bool _isInArena = false;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        // Play Music
        PlayMusic("Audio/Music/Tutorial");

        AudioClip musicToPlay = Resources.Load<AudioClip>("Audio/Music/CombatMusic");
        _combatMusic.clip = musicToPlay;
        _combatMusic.volume = .2f;

        AudioClip arenaMusicToPlay = Resources.Load<AudioClip>("Audio/Music/ArenaMusic");
        _arenaMusic.clip = arenaMusicToPlay;
        _arenaMusic.volume = .2f;
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
                _combatMusic.Play();
            }

            SetHealthBars(true);
        }
        else if (_isInCombat)
        {
            _isInCombat = false;
            _combatMusic.Stop();

            if (!_isInArena)
	            _backgroundMusic.Play();
            else
	            _arenaMusic.Play();

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
        _backgroundMusic.volume = .2f;

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
            child.gameObject.SetActive(isActive);
            SetChildrenToActive(child, isActive);
        }
    }
}
