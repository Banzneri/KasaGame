using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour {

    #region Variables

    // List of all Edges this Frame holds
    [SerializeField]
    Edge[] _Edges;

    // Collider for the Frame
    [SerializeField]
    private BoxCollider _Collider;

    #endregion

    #region Basic Methods

    // Use this for initialization
    private void Start()
    {

        // If _Edges is empty, fill it with children
        if (_Edges.Length == 0)
        {
            _Edges = new Edge[transform.childCount];
            for (int i = 0; i < _Edges.Length; i++)
            {
                _Edges[i] = transform.GetChild(i).GetComponent<Edge>();
            }

            // If there's still no Edges, report error
            if (_Edges.Length == 0)
            {
                Debug.LogError("No Edges found for this frame");
            }
        }

        // If Collider is not found, try to find it
        if (_Collider == null)
        {
            _Collider = GetComponent<BoxCollider>();
            if (_Collider == null)
            {
                Debug.LogError("Collider not found");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       UpdateColliderSize();
    }

    #endregion

    #region Update Edges and self

    // Updates Collider's size so that it is reasonable
    private void UpdateColliderSize()
    {
        Vector3 ScaleMultiplier = GetParentScale(transform);
        float X = 5f / ScaleMultiplier.x;
        float Y = 5f / ScaleMultiplier.y;
        float Z = 5f / ScaleMultiplier.z;
        _Collider.size = new Vector3(1 + X, 1 + Y, 1 + Z);
    }

    // Returns true scale of child by multiplying all scales up to the top Parent
    private Vector3 GetParentScale(Transform child)
    {
        Vector3 Scale = new Vector3(1, 1, 1);
        Transform Parent = child.parent;
        bool ParentFound = Parent != null;

        while (ParentFound)
        {
            Scale = Vector3.Scale(Scale, Parent.localScale);
            Parent = Parent.parent;
            ParentFound = Parent != null;
        }

        return Scale;
    }

    // Updates Colliders of Edges and status of said Colliders
    public void UpdateEdges(float PlayerMaxGradientSide, float PlayerMaxGradientForward)
    {
        // Go through every edge and update them
        for (int i = 0; i < _Edges.Length; i++)
        {
            ScaleEdge(_Edges[i]);
            _Edges[i].EnableCollider(_Edges[i].Climbable(PlayerMaxGradientSide, PlayerMaxGradientForward));
        }
    }

    // Scales Edge so that UpDirection and ForwardDirection is 0.1f units in real space, and SideDirection is almost as long as the Obstacle object
    private void ScaleEdge(Edge edge)
    {
        // Get parent scale of Edge
        Vector3 ParentScale = GetParentScale(edge.transform);

        // Make new scale to be 0.1f units in real space
        float x = 0.1f / ParentScale.x;
        float y = 0.1f / ParentScale.y;
        float z = 0.1f / ParentScale.z;

        // Add length to the SideDirection
        if(edge.VectorDirectionX(edge.SideDirection))
        {
            x = x * -5 + 1;
        } else if (edge.VectorDirectionY(edge.SideDirection))
        {
            y = y * -5 + 1;
        } else
        {
            z = z * -5 + 1;
        }

        // Set new scale
        edge.transform.localScale = new Vector3(x, y, z);
    }

    // Disables all Edge Colliders
    public void DisableEdges()
    {
        for(int i = 0; i < _Edges.Length; i++)
        {
            _Edges[i].EnableCollider(false);
        }
    }

    #endregion
}
