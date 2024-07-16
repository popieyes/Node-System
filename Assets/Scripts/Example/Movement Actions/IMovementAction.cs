using System.Collections;
using System.Collections.Generic;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public interface IMovementAction
{
    public void SetPlayer(TopDownCharacterController player);
    public void Run();
}
