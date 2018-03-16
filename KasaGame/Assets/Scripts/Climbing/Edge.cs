using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{

    [SerializeField]
    private bool Print;



    #region Variables

    // Collider of Edge
    [SerializeField]
    private BoxCollider _Collider;

    // Enables or disables Collider
    public void EnableCollider(bool enable)
    {
        _Collider.enabled = enable;
    }

    // Represents direction of the vector (for example Ym = (0, -1, 0)
    public enum VectorDirection
    {
        Xp, Yp, Zp, Xm, Ym, Zm
    }

    [Header("Edge")]

    // Vector that goes alongside the edge
    [SerializeField]
    private VectorDirection _SideDirection;
    public VectorDirection SideDirection
    {
        get { return _SideDirection; }
    }

    // Vector that goes up
    [SerializeField]
    private VectorDirection _UpDirection;
    public VectorDirection UpDirection
    {
        get { return _UpDirection; }
    }

    // Vector that is perpendicular to _SideDirection
    [SerializeField]
    private VectorDirection _ForwardDirection;
    public VectorDirection ForwardDirection
    {
        get { return _ForwardDirection; }
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

    #region Interaction with Edge

    // Returns true if Edge is suitable for climbing
    public bool Climbable(float MaxGradientSide, float MaxGradientForward)
    {

        // VectorDirections in world space
        Vector3 RealUpDirection = TransformToVectorInWorld(UpDirection, false);
        Vector3 RealSideDirection = TransformToVectorInWorld(SideDirection, false);
        Vector3 RealForwardDirection = TransformToVectorInWorld(ForwardDirection, false);

        bool WideEnough = SideDirectionScale() > 0.6f;
        bool SideDirectionGood = Mathf.Abs(RealSideDirection.y) <= MaxGradientSide;
        bool ForwardDirectionGood = Mathf.Abs(RealForwardDirection.y) <= MaxGradientForward;

        return WideEnough && SideDirectionGood && ForwardDirectionGood && RealUpDirection.y > 0;
    }

    // Returns VectorDirection in a vector form
    public Vector3 TransformToVector(VectorDirection vd)
    {
        switch (vd)
        {
            case VectorDirection.Xp:
                return Vector3.right;
            case VectorDirection.Yp:
                return Vector3.up;
            case VectorDirection.Zp:
                return Vector3.forward;
            case VectorDirection.Xm:
                return Vector3.left;
            case VectorDirection.Ym:
                return Vector3.down;
            case VectorDirection.Zm:
                return Vector3.back;
            default:
                return Vector3.zero;
        }
    }

    // Returns VectorDirection in world space. If straightened, Y is set to 0
    public Vector3 TransformToVectorInWorld(VectorDirection vd, bool Straighten)
    {
        if (Straighten)
        {
            Vector3 vector = transform.TransformDirection(TransformToVector(vd));
            vector = new Vector3(vector.x, 0, vector.z);
            return vector;
        }
        else
        {
            return transform.TransformDirection(TransformToVector(vd));
        }
    }

    // Returns true if given VectorDirection is Xp or Xm
    public bool VectorDirectionX(VectorDirection vd)
    {
        return vd.Equals(VectorDirection.Xp) || vd.Equals(VectorDirection.Xm);
    }

    // Returns true if given VectorDirection is Yp or Ym
    public bool VectorDirectionY(VectorDirection vd)
    {
        return vd.Equals(VectorDirection.Yp) || vd.Equals(VectorDirection.Ym);
    }

    // Returns true if given VectorDirection is Zp or Zm
    public bool VectorDirectioZ(VectorDirection vd)
    {
        return vd.Equals(VectorDirection.Zp) || vd.Equals(VectorDirection.Zm);
    }

    // Calculates scale of the SideDirection
    public float SideDirectionScale()
    {
        Vector3 RealScale = GetParentScale(transform);

        if (VectorDirectionX(SideDirection))
        {
            return RealScale.x;
        }
        else if (VectorDirectionY(SideDirection))
        {
            return RealScale.y;
        }
        else
        {
            return RealScale.z;
        }
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

    #endregion

}
