using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	private int _currentScore;
	private int _highScore;

	public int CountDefeatedGhost;

	public int CurrentScore { get => _currentScore; }
	public int HighScore { get => _highScore; }

	public event Action<int> OnScoreChanged;
	public event Action<int> OnHighScoreChanged;

	private void Awake()
	{
		_highScore = PlayerPrefs.GetInt("high-score", 0);
	}

	private void Start()
	{
		CountDefeatedGhost = 1;

		var allCollectibles = FindObjectsOfType<Collectible>();
		foreach (var collectible in allCollectibles)
		{
			collectible.OnCollected += Collectible_OnCollected;
		}

		var allGhostAI = FindObjectsOfType<GhostAI>();
		foreach (var ghost in allGhostAI)
		{
			ghost.OnDefeatedGhost += Ghost_OnDefeatedGhost;
		}

	}

	private void Ghost_OnDefeatedGhost(int count)
	{
		switch (count)
		{
			default:
			case 1:
				_currentScore += 200;
				OnScoreChanged?.Invoke(_currentScore);
				break;
			case 2:
				_currentScore += 400;
				OnScoreChanged?.Invoke(_currentScore);
				break;
			case 3:
				_currentScore += 600;
				OnScoreChanged?.Invoke(_currentScore);
				break;
			case 4:
				_currentScore += 800;
				OnScoreChanged?.Invoke(_currentScore);
				break;
		}
	}

	private void Collectible_OnCollected(int score, Collectible collectible)
	{
		_currentScore += score;
		OnScoreChanged?.Invoke(_currentScore);

		if (_currentScore >= _highScore)
		{
			_highScore = _currentScore;
			OnHighScoreChanged?.Invoke(_highScore);
		}

		collectible.OnCollected -= Collectible_OnCollected;
	}

	private void OnDestroy()
	{
		PlayerPrefs.SetInt("high-score", _highScore);
	}

}
