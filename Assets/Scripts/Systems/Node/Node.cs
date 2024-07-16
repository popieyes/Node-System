using System.Collections;

namespace Systems.Node
{
    public class Node<T,U> 
    {
        public Node<T,U>[] nodes;
        public Node<T, U> parent;

        public bool IsOriginNode => parent == null;
        
        private T referencingObject;
        private U valueStored;
        
        public Node() { }
        public Node(Node<T,U>[] connections, T refObject, U value)
        {
            nodes = connections;
            referencingObject = refObject;
            valueStored = value;
        }

        public Node(Node<T,U> connection, T refObject)
        {
            nodes = new[] { connection };
            referencingObject = refObject;
        }

        public Node(T refObject, U value)
        {
            referencingObject = refObject;
            valueStored = value;
        }
        public Node(T refObject) { referencingObject = refObject; }

        public Node(Node<T,U> connection) { nodes = new[] { connection };}

        public Node(Node<T,U>[] connections) { nodes = connections;}

        public void SetConnections(Node<T,U>[] connections)
        {
            nodes = connections;
            
        }

        public void SetConnection(Node<T,U> connection)
        {
            nodes = new[] { connection };
        }

        public void SetValue(U value)
        {
            valueStored = value;
            
        }
        public U GetValue() => valueStored;

        public T GetObject() => referencingObject;
        public void SetObject(T refObject) => referencingObject = refObject;
    }
}

