using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JasonGrab : MonoBehaviour
{
    public Vector3 ObjPos;

    private void OnMouseDown()
    {
        ObjPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    }

    private void OnMouseDrag()
    {
        Vector3 ScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ObjPos.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(ScreenPoint);

        transform.position = curPosition;
    }
}