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
	private float _vulnerabilityTimer;

	public event Action<GhostState> OnGhostStateChange;

	public void Reset()
	{
		_ghostMove.CharacterMotor.ResetPosition();
		_ghostState = GhostState.Active;
		OnGhostStateChange?.Invoke(_ghostState);
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

	private void Start()
	{
		_ghostMove = GetComponent<GhostMove>();
		_ghostMove.OnUpdateMoveTarget += _ghostMove_OnUpdateMoveTarget;

		_pacman = GameObject.FindWithTag("Player").transform;

		_ghostState = GhostState.Active;
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
				}
				break;
		}
	}

	private void _ghostMove_OnUpdateMoveTarget()
	{
		switch (_ghostState)
		{
			case GhostState.Active:
				_ghostMove.SetTargetMoveLocation(_pacman.position);
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
					_ghostState = GhostState.Defeated;
					OnGhostStateChange?.Invoke(_ghostState);
				}
				break;
		}


	}

}
