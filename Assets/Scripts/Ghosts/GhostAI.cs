using System;
using UnityEngine;

public enum GhostState
{
	Active,
	Vulnerable,
	VulnerabilityEnding,
	Defeated
}

[RequireComponent(typeof(GhostMove))]
public class GhostAI : MonoBehaviour
{
	public float VulnerabilityEndingTime;

	private GhostMove _ghostMove;
	private Transform _pacman;
	private GhostState _ghostState;
	private ScoreManager scoreManager;
	private float _vulnerabilityTimer;
	private bool _leaveHouse;

	public event Action<GhostState> OnGhostStateChange;
	public event Action<int> OnDefeatedGhost;

	public void Reset()
	{
		_ghostMove.CharacterMotor.ResetPosition();
		_ghostState = GhostState.Active;
		OnGhostStateChange?.Invoke(_ghostState);
		_leaveHouse = false;

	}

	public void StartMoving()
	{
		_ghostMove.CharacterMotor.enabled = true;
	}

	public void StopMoving()
	{
		_ghostMove.CharacterMotor.enabled = false;
	}

	public void SetVulnerable(float duration)
	{
		_vulnerabilityTimer = duration;
		_ghostState = GhostState.Vulnerable;
		OnGhostStateChange?.Invoke(_ghostState);
		_ghostMove.AllowReverseDirection();
	}

	public void Recover()
	{
		_ghostMove.CharacterMotor.CollideWithGates(true);
		_ghostState = GhostState.Active;
		OnGhostStateChange?.Invoke(_ghostState);
		_leaveHouse = false;
	}

	public void LeaveHouse()
	{
		_ghostMove.CharacterMotor.CollideWithGates(false);
		_leaveHouse = true;
	}

	private void Start()
	{
		_ghostMove = GetComponent<GhostMove>();
		_ghostMove.OnUpdateMoveTarget += _ghostMove_OnUpdateMoveTarget;

		_pacman = GameObject.FindWithTag("Player").transform;
		scoreManager = FindObjectOfType<ScoreManager>();

		_ghostState = GhostState.Active;
		_leaveHouse = false;
	}

	private void Update()
	{
		switch (_ghostState)
		{
			case GhostState.Vulnerable:
				_vulnerabilityTimer -= Time.deltaTime;

				if (_vulnerabilityTimer <= VulnerabilityEndingTime)
				{
					_ghostState = GhostState.VulnerabilityEnding;
					OnGhostStateChange?.Invoke(_ghostState);
				}
				break;
			case GhostState.VulnerabilityEnding:
				_vulnerabilityTimer -= Time.deltaTime;
				if (_vulnerabilityTimer <= 0)
				{
					_ghostState = GhostState.Active;
					OnGhostStateChange?.Invoke(_ghostState);
					scoreManager.CountDefeatedGhost = 1;
				}
				break;
		}
	}

	private void _ghostMove_OnUpdateMoveTarget()
	{
		switch (_ghostState)
		{
			case GhostState.Active:
				if (_leaveHouse)
				{
					if (transform.position == new Vector3(0, 3, 0))
					{
						_leaveHouse = false;
						_ghostMove.CharacterMotor.CollideWithGates(true);
						_ghostMove.SetTargetMoveLocation(_pacman.position);
					}
					else
					{
						_ghostMove.SetTargetMoveLocation(new Vector3(0, 3, 0));
					}
				}
				else
				{
					_ghostMove.SetTargetMoveLocation(_pacman.position);
				}
				break;
			case GhostState.Vulnerable:
			case GhostState.VulnerabilityEnding:
				_ghostMove.SetTargetMoveLocation((transform.position - _pacman.position) * 2);
				break;
			case GhostState.Defeated:
				_ghostMove.SetTargetMoveLocation(Vector3.zero);
				break;
		}


	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		switch (_ghostState)
		{
			case GhostState.Active:
				if (other.CompareTag("Player"))
				{
					other.GetComponent<Life>().RemoveLife();
				}
				break;
			case GhostState.Vulnerable:
			case GhostState.VulnerabilityEnding:
				if (other.CompareTag("Player"))
				{
					if (scoreManager.CountDefeatedGhost > 4)
					{
						scoreManager.CountDefeatedGhost = 1;
					}
					_ghostMove.CharacterMotor.CollideWithGates(false);
					_ghostState = GhostState.Defeated;
					OnGhostStateChange?.Invoke(_ghostState);
					OnDefeatedGhost?.Invoke(scoreManager.CountDefeatedGhost);
					scoreManager.CountDefeatedGhost++;
				}
				break;
		}
	}
}
