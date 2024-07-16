
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public class MoveUp : IMovementAction
{
    public TopDownCharacterController player;
    public void SetPlayer(TopDownCharacterController player)
    {
        this.player = player;
    }
    public void Run()
    {
        player.dir = Vector2.up;
        player.animator.SetInteger("Direction", 1);
    }
}
