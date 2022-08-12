using UnityEngine;

public enum GhostType
{
	Blinky,
	Pink,
	Inky,
	Clyde
}

public class GhostView : MonoBehaviour
{
	public CharacterMotor CharacterMotor;
	public GhostAI GhostAI;
	public Animator Animator;
	public GhostType GhostType;

	// Start is called before the first frame update
	void Start()
	{
		Animator.SetInteger("GhostType", (int)GhostType);
		CharacterMotor.OnDirectionCharged += CharacterMotor_OnDirectionCharged;
		GhostAI.OnGhostStateChange += GhostAI_OnGhostStateChange;
	}

	private void GhostAI_OnGhostStateChange(GhostState ghostState)
	{
		Animator.SetInteger("State", (int)ghostState);
	}

	private void CharacterMotor_OnDirectionCharged(Direction direction)
	{
		Animator.SetInteger("Direction", (int)direction - 1);
	}
}
