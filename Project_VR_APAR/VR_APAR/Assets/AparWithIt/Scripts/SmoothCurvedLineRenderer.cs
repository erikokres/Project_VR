using UnityEngine;

public class SmoothCurvedLineRenderer : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private int numPoints = 10;
    [SerializeField] private float lineWidth = 0.2f;

    private LineRenderer _lineRenderer;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.positionCount = numPoints;
        _lineRenderer.startWidth = lineWidth;
        _lineRenderer.endWidth = lineWidth;
    }

    void LateUpdate()
    {
        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (float)(numPoints - 1);
            Vector3 position = GetPointOnBezierCurve(t);

            position = _lineRenderer.transform.InverseTransformPoint(position);

            _lineRenderer.SetPosition(i, position);
        }
    }

    private Vector3 GetPointOnBezierCurve(float t)
    {
        Vector3 p0 = startPoint.position;
        Vector3 p1 = startPoint.position + startPoint.forward * 3f;
        Vector3 p2 = endPoint.position + endPoint.forward * 3f;
        Vector3 p3 = endPoint.position;
        
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}
