/*
 *	Astral Crossbow
		- Cost: 80
		- Travel time: hitscan
		- Damage per hit: high
		- Firing speed: slow
		- Range: Very long
		- Hitbox size / shape: straight line, basically a very skinny retangle
		- HP: 
		- Effect: A crossbow that fires a magic beam of light at enemies. It can also target enemies through tiles with collision. In other words, it can shoot through walls.
					good for long range
 */

public class Crossbow : Summon
{
	protected static int cost = 80;

	public override void Start()
	{
		base.Start();

		cooldown = 0.5f;
		summonRadar.setRadius(5.0f);
	}

	public override int getCost() { return cost; }
}
