
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public class MoveRight : IMovementAction
{
    public TopDownCharacterController player;
    public void SetPlayer(TopDownCharacterController player)
    {
        this.player = player;
    }
    public void Run()
    {
        player.dir = Vector2.right;
        player.animator.SetInteger("Direction", 2);
    }
}
