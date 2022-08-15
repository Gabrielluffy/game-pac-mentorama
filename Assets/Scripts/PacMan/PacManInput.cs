using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class PacManInput : MonoBehaviour
{
	[SerializeField]
	private CharacterMotor _motor;
	void Start()
	{
		_motor.GetComponent<CharacterMotor>();
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			_motor.SetMoveDirection(Direction.Up);
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			_motor.SetMoveDirection(Direction.Left);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			_motor.SetMoveDirection(Direction.Down);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			_motor.SetMoveDirection(Direction.Right);
		}
	}

}
