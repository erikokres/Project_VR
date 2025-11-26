using System;
using UnityEngine;

public class VerticalDragComponent : MonoBehaviour
{
    [SerializeField] private Transform dragObject;
    [SerializeField] private Transform topLimiter;
    [SerializeField] private Transform bottomLimiter;
    [SerializeField] private LayerMask rayMask;
    private float _offset;
    private bool _canDrag;

    private void Update()
    {
        Drag();
    }
    
    public void OnDragBegin()
    {
        _canDrag = true;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, rayMask))
        {
            _offset = transform.position.y - hitInfo.point.y;
        }
    }

    private void Drag()
    {
        if (!_canDrag) return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, rayMask))
        {
            float mouseY = hitInfo.point.y + _offset;
            mouseY = Math.Clamp(mouseY, bottomLimiter.position.y, topLimiter.position.y);
            transform.position = new Vector3(transform.position.x, mouseY, transform.position.z);
            dragObject.position = new Vector3(dragObject.position.x, mouseY, dragObject.position.z);
        }
    }

    public void OnDragEnd()
    {
        _canDrag = false;
    }
}