using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    RaycastHit hit;
    bool isDragging = false;
    [HideInInspector]
    public List<UnitController> selectedUnits = new List<UnitController>();
    Vector3 mousePosition;
    public LayerMask IgnoreMe;

    private void OnGUI()
    {
        if(isDragging)
        {
            var rect = Draw.GetScreenRect(mousePosition, Input.mousePosition);
            Draw.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.1f));
            Draw.DrawScreenRectBorder(rect, 1f, Color.blue);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        IsOneSelected();
        if(Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
            var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(camRay, out hit, 10000f, ~IgnoreMe))
            {
                //Debug.Log(hit.transform.tag);
                if(hit.transform.CompareTag("Player Unit"))
                {
                    SelectUnit(hit.transform.GetComponent<UnitController>(), Input.GetKey(KeyCode.LeftShift));
                }
                else
                {
                    isDragging = true;
                }
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                DeselectUnits();

                foreach (var selectableObject in FindObjectsOfType<PlayerUnitController>())
                {
                    
                    if (isWithinSelectionBounds(selectableObject.transform))
                    {
                        SelectUnit(selectableObject.gameObject.GetComponent<UnitController>(), true);
                        
                    }
                }

                isDragging = false;
            }

        }

        if(Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
        {
            mousePosition = Input.mousePosition;
            var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(camRay, out hit, 10000f, ~IgnoreMe))
            {
                //Debug.Log(hit.transform.tag);
                if(hit.transform.CompareTag("Ground"))
                {
                    foreach (var selectableObj in selectedUnits)
                    {
                        if(selectedUnits.Count == 1)
                            selectableObj.MoveUnit(hit.point);
                        
                        else
                            selectableObj.MoveAllUnits(hit.point);
                    }
                }
                else if (hit.transform.CompareTag("EnemyUnit"))
                {
                    foreach (var selectableObj in selectedUnits)
                    {
                        selectableObj.SetNewTarget(hit.transform);
                    }
                }
                    
            }
        }
    }


    private void SelectUnit(UnitController unit, bool isMultiSelect = false)
    {
        if(!isMultiSelect)
        {
            DeselectUnits();
        }
        
        selectedUnits.Add(unit);
        if(unit != null)
        {
            unit.SetSelected(true);
            unit.ShowHealthBar();
        }
        
        
    }

    private void DeselectUnits()
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            if(selectedUnits[i] != null)
            {
                selectedUnits[i].SetSelected(false);
                selectedUnits[i].HideHealthBar();
            }
            
            

        }
        selectedUnits.Clear();
        
    }

    private bool isWithinSelectionBounds(Transform t)
    {
        if(!isDragging)
        {
            return false;
        }

        var camera = Camera.main;
        var viewportBounds = Draw.GetViewportBounds(camera, mousePosition, Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(t.position));
    }

    public bool IsOneSelected()
    {
        if(selectedUnits.Count == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
