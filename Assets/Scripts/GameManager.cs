using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private enum GameStates
	{
		Starting,
		Playing,
		LifeLost,
		GameOver,
		Victory
	}

	public float StartupTime;

	public float LifeLostTimer;

	private GhostAI[] _allGhosts;
	private CharacterMotor _pacmanMotor;

	private GameStates _gameStates;
	private int _victoryCount;

	private float _lifeLostTimer;
	private bool _isGameOver;

	public event Action OnGameStarted;
	public event Action OnGameVictory;
	public event Action OnGameOver;


	private void Start()
	{
		var allColletibles = FindObjectsOfType<Collectible>();

		_victoryCount = 0;
		foreach (var collectible in allColletibles)
		{
			_victoryCount++;
			collectible.OnCollected += Collectible_OnCollected;
		}

		var pacman = GameObject.FindWithTag("Player");
		_pacmanMotor = pacman.GetComponent<CharacterMotor>();
		_allGhosts = FindObjectsOfType<GhostAI>();
		StopAllCharacters();

		pacman.GetComponent<Life>().OnLifeRemoved += PacMan_OnLifeRemoved;

		_gameStates = GameStates.Starting;
	}

	private void PacMan_OnLifeRemoved(int remainingLives)
	{
		StopAllCharacters();
		_lifeLostTimer = LifeLostTimer;
		_gameStates = GameStates.LifeLost;

		_isGameOver = remainingLives <= 0;
	}

	private void Collectible_OnCollected(int _, Collectible collectible)
	{
		_victoryCount--;

		if (_victoryCount <= 0)
		{

			_gameStates = GameStates.Victory;
			StopAllCharacters();
			OnGameVictory?.Invoke();

		}

		collectible.OnCollected -= Collectible_OnCollected;
	}

	// Update is called once per frame
	void Update()
	{
		switch (_gameStates)
		{
			case GameStates.Starting:
				StartupTime -= Time.deltaTime;

				if (StartupTime <= 0)
				{
					_gameStates = GameStates.Playing;
					StartAllCharacters();

					OnGameStarted?.Invoke();
				}
				break;
			case GameStates.LifeLost:
				_lifeLostTimer -= Time.deltaTime;

				if (_lifeLostTimer <= 0)
				{
					if (_isGameOver)
					{
						_gameStates = GameStates.GameOver;
						OnGameOver?.Invoke();
					}
					else
					{
						ResetAllCharacters();
						_gameStates = GameStates.Playing;

					}
				}
				break;
			case GameStates.GameOver:
			case GameStates.Victory:
				if (Input.anyKey)
				{
					SceneManager.LoadScene(0);
				}
				break;
		}
	}

	private void ResetAllCharacters()
	{
		_pacmanMotor.ResetPosition();

		foreach (var ghost in _allGhosts)
		{
			ghost.Reset();
		}

		StartAllCharacters();
	}

	private void StartAllCharacters()
	{
		_pacmanMotor.enabled = true;

		foreach (var ghost in _allGhosts)
		{
			ghost.StartMoving();
		}
	}

	private void StopAllCharacters()
	{
		_pacmanMotor.enabled = false;

		foreach (var ghost in _allGhosts)
		{
			ghost.StopMoving();
		}
	}
}
