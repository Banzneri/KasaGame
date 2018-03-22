using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameBehaviour : MonoBehaviour {

    #region Variables

    // List of All Edges This Frame Holds
    [SerializeField]
    EdgeBehaviour[] _Edges;

    // Collider for the Frame
    [SerializeField]
    private BoxCollider _Collider;

    // Maximum allowed gradient in the direction of edge
    private float MaxGradientEdge;

    // Maximum allowed gradient in the direction of facing
    private float MaxGradientFacing;

    #endregion

    #region Basic Methods

    // Use this for initialization
    private void Start()
    {

        // If _Edges Is Empty, fill It With Children
        if (_Edges.Length == 0)
        {
            _Edges = new EdgeBehaviour[transform.childCount];
            for (int i = 0; i < _Edges.Length; i++)
            {
                _Edges[i] = transform.GetChild(i).GetComponent<EdgeBehaviour>();
            }

            // If There's Still No Edges, Report Error
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

    // Update scale
    private void UpdateColliderSize()
    {
        Vector3 ScaleMultiplier = GetParentScale(transform);
        float X = 5f / ScaleMultiplier.x;
        float Y = 5f / ScaleMultiplier.y;
        float Z = 5f / ScaleMultiplier.z;
        _Collider.size = new Vector3(1 + X, 1 + Y, 1 + Z);
    }

    // Updates scale and activity of every Edge
    public void UpdateEdges(float PlayerMaxGradientEdge, float PlayerMaxGradientFacing)
    {
        // Set Maximum gradients got from player
        MaxGradientEdge = PlayerMaxGradientEdge;
        MaxGradientFacing = PlayerMaxGradientFacing;

        // Go through every edge and update them
        for (int i = 0; i < _Edges.Length; i++)
        {
            UpdateEdge(_Edges[i]);
        }
    }

    // Scales Edge and enables their collider if needed
    private void UpdateEdge(EdgeBehaviour edge)
    {
        ScaleEdge(edge);

        if (edge.Climbable(MaxGradientEdge, MaxGradientFacing))
        {
           edge.EnableCollider(true);
        }
        else
        {
            edge.EnableCollider(false);
        }
    }

    // Disables collider of every Edge
    public void DisableEdges()
    {
        for (int i = 0; i < _Edges.Length; i++)
        {
            _Edges[i].EnableCollider(false);
        }
    }

    #endregion

    #region Scaling Methods

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

    // Scales Edge
    private void ScaleEdge(EdgeBehaviour edge)
    {
        // Get parent scale of Edge
        Vector3 ParentScale = GetParentScale(edge.transform);

        // Make new scale to be 0.1f units in real world
        float x = 0.1f / ParentScale.x;
        float y = 0.1f / ParentScale.y;
        float z = 0.1f / ParentScale.z;
        Vector3 NewScale = new Vector3(x, y, z);

        // Convert _EdgeDirection to be positive
        Vector3 PositiveEdgeDirection = new Vector3(Mathf.Abs(edge.EdgeDirection.x), Mathf.Abs(edge.EdgeDirection.y), Mathf.Abs(edge.EdgeDirection.z));

        // Scale EdgeDirection to be size of the side of parent
        NewScale += PositiveEdgeDirection;

        // Set new scale
        edge.transform.localScale = NewScale;
    }

    #endregion

}