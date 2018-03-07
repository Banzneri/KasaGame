using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeBehaviour : MonoBehaviour
{

    #region Variables

    // Collider of Edge
    [SerializeField]
    private BoxCollider _Collider;
  
    // Enables or disables Collider
    public void EnableCollider(bool enable)
    {
        _Collider.enabled = enable;
    }


    [Header("Edge")]

    // Vector that goes alongside Edge
    [SerializeField]
    private Vector3 _EdgeDirection;
    // Returns EdgeDirection
    public Vector3 EdgeDirection
    {
        get { return _EdgeDirection; }
    }

    // Vector that goes up
    [SerializeField]
    private Vector3 _UpDirection;
    // Returns UpDirection
    public Vector3 UpDirection
    {
        get { return _UpDirection; }
    }

    // Vector that is perpendicular to _EdgeDirection
    [SerializeField]
    private Vector3 _FacingDirection;
    // Returns FacingDirection
    public Vector3 FacingDirection
    {
        get { return _FacingDirection; }
    }

    #endregion

    // Use this for initialization
    void Start()
    {

        // if _Collider not found, find it
        if (_Collider == null)
        {
            _Collider = GetComponent<BoxCollider>();

            // if _Collider is still not found, report error
            if (_Collider == null)
            {
                Debug.LogError("Collider not found");
            }
        }
    }


    #region Information about Edge

    // Returns true if Edge is suitable for climbing
    public bool Climbable(float MaxGradientEdge, float MaxGradientFacing)
    {
        Vector3 RealEdgeDirection = transform.TransformDirection(EdgeDirection);
        Vector3 RealFacingDirection = transform.TransformDirection(FacingDirection);

        float EdgeDifferenceY = Mathf.Abs(RealEdgeDirection.y);
        float FacingDifferenceY = Mathf.Abs(RealFacingDirection.y);

        return EdgeDifferenceY <= MaxGradientEdge && FacingDifferenceY <= MaxGradientFacing;
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

    // Returns a point in World space coordinates, when starting from the center of Edge
    // and moving alonside the EdgeDirection for the lenght of DifferenceX
    public Vector3 PointOnEdge(float DifferenceX)
    {
        return transform.position + transform.TransformDirection(EdgeDirection) * DifferenceX;
    }

    // Transforms PlayerPosition to the local space and positions it on the Edge, returning DifferenceX from the center of Edge
    public float TransformToEdge(Vector3 PlayerPosition)
    {
        PlayerPosition.Set(PlayerPosition.x, 0, PlayerPosition.z);
        PlayerPosition = transform.InverseTransformPoint(PlayerPosition);

        float DifferenceX = 0;

       if(EdgeDirection.x != 0)
        {
            DifferenceX = PlayerPosition.x * EdgeDirection.x;
            DifferenceX = Mathf.Clamp(DifferenceX, -0.5f, 0.5f);
            DifferenceX *= GetParentScale(transform).x;
        } else if(EdgeDirection.y != 0)
        {
            DifferenceX = PlayerPosition.y * EdgeDirection.y;
            DifferenceX = Mathf.Clamp(DifferenceX, -0.5f, 0.5f);
            DifferenceX *= GetParentScale(transform).y;
        } else
        {
            DifferenceX = PlayerPosition.z * EdgeDirection.z;
            DifferenceX = Mathf.Clamp(DifferenceX, -0.5f, 0.5f);
            DifferenceX *= GetParentScale(transform).z;
        }

        return DifferenceX;
    }

    // Returns true if local position on Edge is not between -0.5 ... 0.5
    public bool IsOnLedge(Vector3 PlayerPosition)
    {
        PlayerPosition.Set(PlayerPosition.x, 0, PlayerPosition.z);
        PlayerPosition = transform.InverseTransformPoint(PlayerPosition);

        float DifferenceX = 0;

        if (EdgeDirection.x != 0)
        {
            DifferenceX = PlayerPosition.x * EdgeDirection.x;
        }
        else if (EdgeDirection.y != 0)
        {
            DifferenceX = PlayerPosition.y * EdgeDirection.y;
        }
        else
        {
            DifferenceX = PlayerPosition.z * EdgeDirection.z;
        }

        return DifferenceX <= 0.5f && DifferenceX >= -0.5f;
    }



    #endregion

}
