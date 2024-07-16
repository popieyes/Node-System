using System.Collections;
using System.Collections.Generic;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public class MoveDown : IMovementAction
{
    public TopDownCharacterController player;

    public void SetPlayer(TopDownCharacterController player)
    {
        this.player = player;
    }

    public void Run()
    {
        player.dir = Vector2.down;
        player.animator.SetInteger("Direction", 0);
    }
}
