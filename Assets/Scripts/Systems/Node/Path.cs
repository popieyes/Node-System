using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Systems.Node
{
    public abstract class Path<T,U> : MonoBehaviour
    {
        protected List<Node<T, U>> paths = new ();

        protected virtual void CreatePath() {}

        protected virtual void CreateNodes(T originObject){}
        protected virtual void CreateNodes(Node<T, U> originNode){}

        protected virtual void RandomBranchingNodes(Node<T,U> originNode, int minConnectionsPerRow, int maxConnectionsPerRow, 
            int minConnections, int maxConnections, int maxIterations = 6, bool addToCollection = true, bool allowRepeatedNodes = false)
        {
            var currentNode = originNode;
            int totalConnectionCount = Random.Range(minConnections, maxConnections);
            
            var connectionsAdded = 0;
               
            while (connectionsAdded < totalConnectionCount && currentNode != null && NodesAvailable())
            {
                int connectionsPerRow = Random.Range(minConnectionsPerRow, maxConnectionsPerRow);
                var connections = new Node<T, U>[connectionsPerRow];
                int rowConnectionsCount = 0;
                int iteration = 0;
                while (rowConnectionsCount < connectionsPerRow 
                       && connectionsAdded < totalConnectionCount && iteration < maxIterations)
                {
                    iteration++;
                    
                    Node<T, U> potentialNode = TryGetNode();
                    
                    if (!allowRepeatedNodes)
                    {
                        if(!CheckNodeAvailability(potentialNode)) continue;
                    }
                    
                    if (potentialNode != null)
                    {
                        NodeSetup(potentialNode, originNode);
                        SetNodeAvailability(potentialNode);
                        connections[rowConnectionsCount] = potentialNode;
                        connectionsAdded++;
                    }
                    rowConnectionsCount++;
                }
                currentNode.SetConnections(connections);
                currentNode = connections[Random.Range(0, connections.Length)];
            }
            if(addToCollection) paths.Add(originNode);
        }

        protected abstract Node<T, U> TryGetNode();
        protected abstract bool NodesAvailable();
        protected abstract bool CheckNodeAvailability(Node<T,U> node);
        protected abstract void SetNodeAvailability(Node<T, U> node);

        protected virtual void NodeSetup(Node<T, U> potentialNode, Node<T, U> originNode)
        {
            potentialNode.parent = originNode;
        }
        /// <summary>
        /// Explores all paths in paths list.
        /// </summary>
        /// <param name="action">Action to execute on origin node or object resultant from function.</param>
        /// <param name="function"> Function that returns a specified object.</param>
        ///  /// <param name="executeActionInOrigin"> Whether the action should be executed in the origin node or not. </param>
        protected void ExplorePaths( Action<object> action, bool executeActionInOrigin = true, Func<T,object> function = null)
        {
            foreach (var path in paths)
            {
                ExploreNode(path, action, executeActionInOrigin, function);
            }
        }
        /// <summary>
        /// Explores all nodes connected to the origin node.
        /// </summary>
        /// <param name="node"> Origin node to explore connections from.</param>
        /// <param name="action"> Action to execute on origin node or object resultant from function.</param>
        /// <param name="function"> Function that returns a specified object.</param>
        /// <param name="executeActionInOrigin"> Whether the action should be executed in the origin node or not. </param>
        protected void ExploreNode(Node<T,U> node, Action<object> action, bool executeActionInOrigin = true, Func<T,object> function = null)
        {
            if(node == null) return;
            
            if (executeActionInOrigin || !node.IsOriginNode)
            {
                object result = node;
                if (function != null)
                {
                    result = function(node.GetObject());
                }
                action(result);
            }
      

            if (node.nodes == null) return;
            foreach (var connection in node.nodes)
            {
                if (connection == null) continue;
                
                ExploreNode(connection,action, executeActionInOrigin,function);
            }
        }
        
        /// <summary>
        /// Explores all nodes connected to the origin node. Executes actions on nodes and waits until the action
        /// is finished before jumping to the next node.
        /// </summary>
        /// <param name="node"> Origin node to explore connections from.</param>
        /// <param name="action"> Action to execute on origin node or object resultant from function.</param>
        /// <param name="function"> Function that returns a specified object.</param>
        /// <param name="executeActionInOrigin"> Whether the action should be executed in the origin node or not. </param>
        protected IEnumerator ExploreNode(Node<T,U> node, Func<object, IEnumerator> action, bool executeActionInOrigin = true, Func<T,IEnumerator<object>> function = null)
        {
            if(node == null) yield break;
            
            if (executeActionInOrigin || !node.IsOriginNode)
            {
                object result = node;
                if (function != null)
                {
                    IEnumerator<object> functionCoroutine = function(node.GetObject());
                    while (functionCoroutine.MoveNext())
                    {
                        result = functionCoroutine.Current;
                        yield return null;
                    }
                }
                IEnumerator actionCoroutine = action(result);
                while (actionCoroutine.MoveNext())
                {
                    yield return actionCoroutine.Current;
                }
            }
      

            if (node.nodes == null) yield break;
            foreach (var connection in node.nodes)
            {
                if (connection == null) continue;
                IEnumerator connectionCoroutine = ExploreNode(connection, action, executeActionInOrigin, function);
                while (connectionCoroutine.MoveNext())
                {
                    yield return connectionCoroutine.Current;
                }
            }
        }
        
        /// <summary>
        /// Recalculates the paths from the origin nodes list.
        /// </summary>
        /// <param name="excludedNode"> Node excluded from recalculation.</param>
        protected void RecalculatePaths(Node<T, U> excludedNode)
        {
            foreach (var path in paths)
            {
                if(path == excludedNode) continue;
                CreateNodes(path);
            }
        }
        /// <summary>
        /// Recalculates connections from the origin node.
        /// </summary>
        /// <param name="originNode"></param>
        protected void RecalculatePath(Node<T, U> originNode) => CreateNodes(originNode);
    }
}
