using System;
using System.Collections;
using System.Collections.Generic;
using Cainos.PixelArtTopDown_Basic;
using Systems.Node;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomMovementPath : Path<TopDownCharacterController, IMovementAction>
{
    public TopDownCharacterController player;
    private List<IMovementAction> actionsAvailable;
    private void Start()
    {
        actionsAvailable = new List<IMovementAction>();
        for (int i = 0; i < 10; i++)
        {
            actionsAvailable.Add(new MoveUp());
            actionsAvailable.Add(new MoveDown());
            actionsAvailable.Add(new MoveLeft());
            actionsAvailable.Add(new MoveRight());
        }
        
        CreateNodes(player);

        StartCoroutine(ExploreNode(paths[0], ActionCoroutine));
    }
    protected override void CreateNodes(TopDownCharacterController originObject)
    {
        var originAction = new Node<TopDownCharacterController, IMovementAction>(originObject);
        var randomIndex = Random.Range(0, actionsAvailable.Count);
        originAction.SetValue(actionsAvailable[randomIndex]);
        RandomBranchingNodes(originAction, 1, 1, 10, 15);
    }
    

    protected override Node<TopDownCharacterController, IMovementAction> TryGetNode()
    {
        var randomIndex = Random.Range(0, actionsAvailable.Count);
        return new Node<TopDownCharacterController, IMovementAction>(player, actionsAvailable[randomIndex]);
    }

    protected override bool NodesAvailable()
    {
        return actionsAvailable.Count > 0;
    }

    protected override bool CheckNodeAvailability(Node<TopDownCharacterController, IMovementAction> node)
    {
        return true;
    }

    protected override void SetNodeAvailability(Node<TopDownCharacterController, IMovementAction> node)
    {
        actionsAvailable.Remove(node.GetValue());
    }

    IEnumerator<object> ActionCoroutine(object result)
    {
        var action = (Node<TopDownCharacterController, IMovementAction>) result;
        Debug.Log(action.GetValue());
        action.GetValue().SetPlayer(action.GetObject());
        action.GetValue().Run();
        yield return new WaitForSeconds(1f);
    }
}
