using UnityEngine;

public class GameUI : MonoBehaviour
{
	public GameObject ReadyMenssage;
	public GameObject GameOverMenssage;
	public AudioSource AudioSource;
	public AudioClip BenginInMusic;
	public GameManager _gameManager;
	// Start is called before the first frame update
	void Start()
	{
		_gameManager = FindObjectOfType<GameManager>();
		_gameManager.OnGameStarted += GameManager_OnGameStarted;
		_gameManager.OnGameOver += GameManager_OnGameOver;
		AudioSource.PlayOneShot(BenginInMusic);

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
