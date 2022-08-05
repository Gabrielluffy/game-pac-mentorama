using UnityEngine;

[RequireComponent(typeof(GhostMove))]
public class GhostAI : MonoBehaviour
{
	private GhostMove _ghostMove;
	private Transform _pacman;
	// Start is called before the first frame update
	void Start()
	{
		_ghostMove = GetComponent<GhostMove>();
		_ghostMove.OnUpdateMoveTarget += _ghostMove_OnUpdateMoveTarget;

		_pacman = GameObject.FindWithTag("Player").transform;
	}

	private void _ghostMove_OnUpdateMoveTarget()
	{
		_ghostMove.SetTargetMoveLocation(_pacman.position);
	}


}
