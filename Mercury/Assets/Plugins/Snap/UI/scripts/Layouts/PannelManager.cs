using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PannelManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    public enum ScaleType
    {
        None,
        Zoom,
        Layout
    }

    public bool dragable = true;
    public bool keepInScreen = true;

    public bool savePositionAndSize = false;
    public string saveName = "";

    public ScaleType scaleType = ScaleType.None;
    public bool scaleProportional = true;
    public float dragEdgeRange = 5f;
    public Vector2 scaleMin = new Vector2(0, 0);
    public Vector2 scaleMax = new Vector2(5000f, 5000f);


    private bool mouseActive = false;
    private Vector2 lastMousePosition = Vector2.zero;
    private Vector2 mouseDown = new Vector2(0, 0);
    private Vector2 mouseDistanceLost = new Vector2(0, 0);

    private enum DragType
    {
        None,
        Move,
        ScaleLeft,
        ScaleBottom,
        ScaleTop,
        ScaleRight,
        ScaleBottomLeft,
        ScaleBottomRight,
        ScaleTopLeft,
        ScaleTopRight
    }

    private DragType dragType = DragType.None;
    private RectTransform rt;
    private float ratio;
    private float saveWhen = 0;

    public void Start()
    {
        rt = GetComponent<RectTransform>();

        ratio = Mathf.Abs(rt.sizeDelta.x / (rt.sizeDelta.y == 0 ? 0.0001f : rt.sizeDelta.y));

        // Fix min and max if the dont match ratio.
        if (scaleProportional)
        {
            if (scaleType == ScaleType.Layout)
            {
                scaleMin = new Vector2(scaleMin.x, scaleMin.x / ratio);
                scaleMax = new Vector2(scaleMax.x, scaleMax.x / ratio);
            }
            else
            {
                scaleMin = new Vector2(scaleMin.x, scaleMin.x);
                scaleMax = new Vector2(scaleMax.x, scaleMax.x);
            }
        }

        // Load settings from prefabs
        saveName=saveName.Trim();
        if (savePositionAndSize && saveName != null && saveName.Length != 0)
        {
            SetSettingFromString(PlayerPrefs.GetString("snap.panelmanager." + saveName));
        }
    }

    public void SetSettingFromString(string settings)
    {
        if (settings != null && settings.Trim().Length > 0) {
            Debug.Log("Loading panel details for '"+saveName+"' from '"+settings+"'");
            string[] parts = settings.Split(',');
            if (parts.Length < 3)
            {
                Debug.LogWarning("Could not determine panel settings on '"+saveName+"' from '"+settings+"'");
                return;
            }

            // Match type
            if (int.Parse(parts[0]) == ((int)scaleType))
            {
                // Location
                transform.position=new Vector3(float.Parse(parts[1]),float.Parse(parts[2]),transform.position.z);
                // Scale
                if (scaleType == ScaleType.Zoom && parts.Length>=5)
                {
                    transform.localScale = new Vector3(float.Parse(parts[3]), float.Parse(parts[4]), transform.localScale.z);
                }
                else if (scaleType == ScaleType.Layout && parts.Length >= 5)
                {
                    rt.sizeDelta = new Vector2(float.Parse(parts[3]), float.Parse(parts[4]));
                }
            }
            else
            {
                Debug.LogWarning("Panel zoom type has changed on '" + saveName + "' from '" + settings + "' can not effectivly use");
            }
        }
    }

    public string GetSettingAsString()
    {
        string details = ""+((int)scaleType) + "," + transform.position.x + "," + transform.position.y;

        if (scaleType == ScaleType.Zoom)
        {
            details = details + "," + transform.localScale.x + "," + transform.localScale.y;
        }
        else if (scaleType == ScaleType.Layout)
        {
            details = details + "," + rt.sizeDelta.x + "," + rt.sizeDelta.y;
        }
        Debug.Log("Saving panel details for '" + saveName + "' as '" + details + "'");
        return details;
    }

    public void Update()
    {
        if (mouseActive)
        {
            DoDrag();
            saveWhen = Time.time + .5f;
        }
        if (saveWhen!=0 && saveWhen<Time.time){
            saveWhen = 0;
            if (savePositionAndSize && saveName != null && saveName.Length != 0)
            {
                PlayerPrefs.SetString("snap.panelmanager." + saveName,GetSettingAsString());
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartDrag();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EndDrag();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void StartDrag()
    {
        mouseActive = true;
        mouseDown = lastMousePosition = Input.mousePosition;
        mouseDistanceLost = new Vector2(0, 0);

        if (dragable)
        {
            dragType = DragType.Move; // Type is drag unless a side or corner is identified
        }
        else
        {
            dragType = DragType.None; // Type is drag unless a side or corner is identified
        }


        if (scaleType != ScaleType.None)
        {
            // Figure out type of drag

            float left = transform.position.x - (rt.pivot.x * rt.sizeDelta.x * transform.localScale.x);
            float top = transform.position.y + (rt.pivot.y * rt.sizeDelta.y * transform.localScale.y);
            float right = transform.position.x + ((1 - rt.pivot.x) * rt.sizeDelta.x * transform.localScale.x);
            float bottom = transform.position.y - ((1 - rt.pivot.y) * rt.sizeDelta.y * transform.localScale.y);

            if (lastMousePosition.x < left + dragEdgeRange ||
                lastMousePosition.x > right - dragEdgeRange ||
                lastMousePosition.y > top - dragEdgeRange ||
                lastMousePosition.y < bottom + dragEdgeRange)
            {

                // Corners only
                if (scaleProportional)
                {
                    if (lastMousePosition.x < left + rt.sizeDelta.x / 2)
                    {
                        if (lastMousePosition.y > top - rt.sizeDelta.y / 2)
                        {
                            dragType = DragType.ScaleTopLeft;
                        }
                        else
                        {
                            dragType = DragType.ScaleBottomLeft;
                        }
                    }
                    else
                    {
                        if (lastMousePosition.y > top - rt.sizeDelta.y / 2)
                        {
                            dragType = DragType.ScaleTopRight;
                        }
                        else
                        {
                            dragType = DragType.ScaleBottomRight;
                        }
                    }

                }
                else
                {
                    // Left
                    if (lastMousePosition.x < left + dragEdgeRange)
                    {
                        // Check top
                        if (lastMousePosition.y > top - dragEdgeRange)
                        {
                            dragType = DragType.ScaleTopLeft;
                        }
                        // Check bottom 
                        else if (lastMousePosition.y < bottom + dragEdgeRange)
                        {
                            dragType = DragType.ScaleBottomLeft;
                        }
                        else
                        {
                            dragType = DragType.ScaleLeft;
                        }
                    }
                    // Right
                    else if (lastMousePosition.x > right - dragEdgeRange)
                    {
                        // Check top 
                        if (lastMousePosition.y > top - dragEdgeRange)
                        {
                            dragType = DragType.ScaleTopRight;
                        }
                        // Check bottom
                        else if (lastMousePosition.y < bottom + dragEdgeRange)
                        {
                            dragType = DragType.ScaleBottomRight;
                        }
                        else
                        {
                            dragType = DragType.ScaleRight;
                        }
                    }
                    else if (lastMousePosition.y > top - dragEdgeRange)
                    {
                        dragType = DragType.ScaleTop;
                    }
                    // Check bottom
                    else if (lastMousePosition.y < bottom + dragEdgeRange)
                    {
                        dragType = DragType.ScaleBottom;
                    }
                }
            }

        }
    }

    public void EndDrag()
    {
        mouseActive = false;
        lastMousePosition = Input.mousePosition;
    }

    public void DoDrag()
    {
        Vector2 newMousePosition = Input.mousePosition;
        if (mouseActive)
        {
            switch (dragType)
            {
                case DragType.Move:
                    DoMove(newMousePosition);
                    break;
                default:
                    if (scaleProportional)
                    {
                        float xa = newMousePosition.x - mouseDown.x - mouseDistanceLost.x;
                        float ya = newMousePosition.y - mouseDown.y - mouseDistanceLost.y;
                        bool xneg = xa < 0;
                        bool yneg = ya < 0;

                        if (xneg)
                        {
                            xa = -xa;
                        }

                        if (yneg)
                        {
                            ya = -ya;
                        }

                        if (xa > ya)
                        {
                            ya = xa / ratio;
                            if (xneg)
                            {
                                xa = -xa;
                            }
                            switch (dragType)
                            {
                                case DragType.ScaleTopLeft:
                                case DragType.ScaleBottomRight:
                                    if (!xneg)
                                    {
                                        ya = -ya;
                                    }
                                    break;
                                case DragType.ScaleTopRight:
                                case DragType.ScaleBottomLeft:
                                    if (xneg)
                                    {
                                        ya = -ya;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            xa = ya * ratio;
                            if (yneg)
                            {
                                ya = -ya;
                            }
                            switch (dragType)
                            {
                                case DragType.ScaleTopLeft:
                                case DragType.ScaleBottomRight:
                                    if (!yneg)
                                    {
                                        xa = -xa;
                                    }
                                    break;
                                case DragType.ScaleTopRight:
                                case DragType.ScaleBottomLeft:
                                    if (yneg)
                                    {
                                        xa = -xa;
                                    }
                                    break;
                            }
                        }

                        newMousePosition = new Vector2(mouseDown.x + xa, mouseDown.y + ya);

                        mouseDistanceLost = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - newMousePosition;

                    }
                    DoScale(newMousePosition);
                    break;
            }
            FitOnScreen();
        }
        // TODO: Move the mouse cursor to this location
        lastMousePosition = newMousePosition;
    }

    public void DoScale(Vector2 newMousePosition)
    {
        Debug.Log("DragType=" + dragType);

        float xa = newMousePosition.x - lastMousePosition.x;
        float ya = -(newMousePosition.y - lastMousePosition.y);

        float nx = transform.position.x;
        float ny = transform.position.y;

        Vector2 size = rt.sizeDelta;
        Vector2 fixedSize = size;
        // Maintain location
        switch (dragType)
        {
            case DragType.ScaleBottomRight:
                size = new Vector2(rt.sizeDelta.x + xa, rt.sizeDelta.y + ya);
                fixedSize = scaleType == ScaleType.Layout ? new Vector2(Mathf.Min(scaleMax.x, Mathf.Max(scaleMin.x, size.x)), Mathf.Min(scaleMax.y, Mathf.Max(scaleMin.y, size.y))) : size;
                xa = fixedSize.x - rt.sizeDelta.x;
                ya = fixedSize.y - rt.sizeDelta.y;
                break;
            case DragType.ScaleBottomLeft:
                size = new Vector2(rt.sizeDelta.x - xa, rt.sizeDelta.y + ya);
                fixedSize = scaleType == ScaleType.Layout ? new Vector2(Mathf.Min(scaleMax.x, Mathf.Max(scaleMin.x, size.x)), Mathf.Min(scaleMax.y, Mathf.Max(scaleMin.y, size.y))) : size;
                xa = rt.sizeDelta.x - fixedSize.x;
                ya = fixedSize.y - rt.sizeDelta.y;
                break;
            case DragType.ScaleTopRight:
                size = new Vector2(rt.sizeDelta.x + xa, rt.sizeDelta.y - ya);
                fixedSize = scaleType == ScaleType.Layout ? new Vector2(Mathf.Min(scaleMax.x, Mathf.Max(scaleMin.x, size.x)), Mathf.Min(scaleMax.y, Mathf.Max(scaleMin.y, size.y))) : size;
                xa = fixedSize.x - rt.sizeDelta.x;
                ya = rt.sizeDelta.y - fixedSize.y;
                //ya = fixedSize.y - rt.sizeDelta.y;
                break;
            case DragType.ScaleTopLeft:
                size = new Vector2(rt.sizeDelta.x - xa, rt.sizeDelta.y - ya);
                fixedSize = scaleType == ScaleType.Layout ? new Vector2(Mathf.Min(scaleMax.x, Mathf.Max(scaleMin.x, size.x)), Mathf.Min(scaleMax.y, Mathf.Max(scaleMin.y, size.y))) : size;
                xa = rt.sizeDelta.x - fixedSize.x;
                break;
            case DragType.ScaleLeft:
                size = new Vector2(rt.sizeDelta.x - xa, rt.sizeDelta.y);
                fixedSize = scaleType == ScaleType.Layout ? new Vector2(Mathf.Min(scaleMax.x, Mathf.Max(scaleMin.x, size.x)), Mathf.Min(scaleMax.y, Mathf.Max(scaleMin.y, size.y))) : size;
                xa = rt.sizeDelta.x - fixedSize.x;
                break;
            case DragType.ScaleRight:
                size = new Vector2(rt.sizeDelta.x + xa, rt.sizeDelta.y);
                fixedSize = scaleType == ScaleType.Layout ? new Vector2(Mathf.Min(scaleMax.x, Mathf.Max(scaleMin.x, size.x)), Mathf.Min(scaleMax.y, Mathf.Max(scaleMin.y, size.y))) : size;
                xa = fixedSize.x - rt.sizeDelta.x;
                break;
            case DragType.ScaleTop:
                size = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y - ya);
                fixedSize = scaleType == ScaleType.Layout ? new Vector2(Mathf.Min(scaleMax.x, Mathf.Max(scaleMin.x, size.x)), Mathf.Min(scaleMax.y, Mathf.Max(scaleMin.y, size.y))) : size;
                ya = rt.sizeDelta.y - fixedSize.y;
                break;
            case DragType.ScaleBottom:
                size = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + ya);
                fixedSize = scaleType == ScaleType.Layout ? new Vector2(Mathf.Min(scaleMax.x, Mathf.Max(scaleMin.x, size.x)), Mathf.Min(scaleMax.y, Mathf.Max(scaleMin.y, size.y))) : size;
                ya = fixedSize.y - rt.sizeDelta.y;
                break;
        }

        if (scaleType == ScaleType.Layout)
        {
            rt.sizeDelta = fixedSize;
        }
        else
        {
            Vector2 adjust = fixedSize - rt.sizeDelta;
            if (scaleProportional)
            {
                transform.localScale = new Vector3(Mathf.Min(scaleMax.x, Mathf.Max(scaleMin.x, transform.localScale.x + adjust.x / rt.sizeDelta.x)), Mathf.Min(scaleMax.x, Mathf.Max(scaleMin.y, transform.localScale.y + adjust.y / rt.sizeDelta.y)), transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Min(scaleMax.x, Mathf.Max(scaleMin.x, transform.localScale.x + adjust.x / rt.sizeDelta.x)), Mathf.Min(scaleMax.y, Mathf.Max(scaleMin.y, transform.localScale.y + adjust.y / rt.sizeDelta.y)), transform.localScale.z);
            }
        }

        // Maintain location
        switch (dragType)
        {
            case DragType.ScaleBottomRight:
                nx = nx + xa * rt.pivot.x;
                ny = ny - ya * (1 - rt.pivot.y);
                break;
            case DragType.ScaleBottomLeft:
                nx = nx + xa * (1 - rt.pivot.x);
                ny = ny - ya * (1 - rt.pivot.y);
                break;
            case DragType.ScaleTopRight:
                nx = nx + xa * rt.pivot.x;
                ny = ny - ya * rt.pivot.y;
                break;
            case DragType.ScaleTopLeft:
                nx = nx + xa * (1 - rt.pivot.x);
                ny = ny - ya * rt.pivot.y;
                break;
            case DragType.ScaleLeft:
                nx = nx + xa * (1 - rt.pivot.x);
                break;
            case DragType.ScaleRight:
                nx = nx + xa * rt.pivot.x;
                break;
            case DragType.ScaleTop:
                ny = ny - ya * rt.pivot.y;
                break;
            case DragType.ScaleBottom:
                ny = ny - ya * (1 - rt.pivot.y);
                break;
        }

        transform.position = new Vector3(nx, ny, transform.position.z);

        if (scaleType == ScaleType.Layout)
        {
            SendMessage("InvalidateLayout", SendMessageOptions.DontRequireReceiver);
        }

    }

    public void FitOnScreen()
    {
        if (keepInScreen)
        {
            // RectTransform rt = GetComponent<RectTransform>();

            float nx = transform.position.x;
            float ny = transform.position.y;

            float left = rt.pivot.x * rt.sizeDelta.x * transform.localScale.x;
            float top = rt.pivot.y * rt.sizeDelta.y * transform.localScale.y;
            float right = Screen.width - (1 - rt.pivot.x) * rt.sizeDelta.x * transform.localScale.x;
            float bottom = Screen.height - (1 - rt.pivot.y) * rt.sizeDelta.y * transform.localScale.y;

            if (nx < left)
            {
                nx = left;
            }
            if (ny < top)
            {
                ny = top;
            }
            if (nx > right)
            {
                nx = right;
            }
            if (ny > bottom)
            {
                ny = bottom;
            }

            transform.position = new Vector3(nx, ny, transform.position.z);
        }
    }

    public void DoMove(Vector2 newMousePosition)
    {
        float npx = transform.position.x + newMousePosition.x - lastMousePosition.x;
        float npy = transform.position.y + newMousePosition.y - lastMousePosition.y;

        transform.position = new Vector3(npx, npy, transform.position.z);
    }

}
