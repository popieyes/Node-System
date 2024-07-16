
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public class MoveLeft : IMovementAction
{
    public TopDownCharacterController player;
    public void SetPlayer(TopDownCharacterController player)
    {
        this.player = player;
    }
    public void Run()
    {
        player.dir = Vector2.left;
        player.animator.SetInteger("Direction", 3);
    }
}
