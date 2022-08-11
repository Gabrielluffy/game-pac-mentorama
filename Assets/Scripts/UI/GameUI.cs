using UnityEngine;

public class GameUI : MonoBehaviour
{
	public GameObject ReadyMenssage;
	public GameObject GameOverMenssage;
	public AudioSource AudioSource;
	public AudioClip BenginInMusic;
	public GameManager _gameManager;
	public BlinkTileMapColor BlinkTileMap;
	// Start is called before the first frame update
	void Start()
	{
		_gameManager = FindObjectOfType<GameManager>();
		_gameManager.OnGameStarted += GameManager_OnGameStarted;
		_gameManager.OnGameOver += GameManager_OnGameOver;
		_gameManager.OnGameVictory += _gameManager_OnGameVictory;
		AudioSource.PlayOneShot(BenginInMusic);

	}

	private void _gameManager_OnGameVictory()
	{
		BlinkTileMap.enabled = true;
	}

	private void GameManager_OnGameStarted()
	{
		ReadyMenssage.SetActive(false);
	}

	private void GameManager_OnGameOver()
	{
		GameOverMenssage.SetActive(true);
	}


}
