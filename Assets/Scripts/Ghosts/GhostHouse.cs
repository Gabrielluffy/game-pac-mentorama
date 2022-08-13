using System.Collections.Generic;
using UnityEngine;

public class GhostHouse : MonoBehaviour
{
	public int LeaveHouseInterval;

	List<GhostAI> _allGhosts;

	private float _leaveHouseTimer;

	private void Awake()
	{
		_allGhosts = new List<GhostAI>();
		_leaveHouseTimer = LeaveHouseInterval;
	}

	private void Update()
	{
		if (_allGhosts.Count > 0)
		{
			_leaveHouseTimer -= Time.deltaTime;

			if (_leaveHouseTimer <= 0)
			{
				_leaveHouseTimer += LeaveHouseInterval;
				_allGhosts[0].LeaveHouse();
				_allGhosts.RemoveAt(0);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		var ghost = other.GetComponent<GhostAI>();
		ghost.Recover();

		if (_allGhosts.Count == 0)
		{
			_leaveHouseTimer += LeaveHouseInterval;
		}

		_allGhosts.Add(ghost);
	}
}
