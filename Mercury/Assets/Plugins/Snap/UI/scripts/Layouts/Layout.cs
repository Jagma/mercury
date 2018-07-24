using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public abstract class Layout : MonoBehaviour
{
    // This bool is used to manage the validity of the layout to prevent unessary calls to Layout
    protected bool invalidLayout = true;

    public HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Left;
    public VerticleAlignment _verticleAlignment = VerticleAlignment.Top;

    public HorizontalAlignment horizontalAlignment
    {
        get
        {
            return _horizontalAlignment;
        }
        set
        {
            _horizontalAlignment = value;
            invalidLayout = true;
        }
    }

    public VerticleAlignment verticleAlignment
    {
        get
        {
            return _verticleAlignment;
        }
        set
        {
            _verticleAlignment = value;
            invalidLayout = true;
        }
    }


    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right,
        Fit
    };

    public enum VerticleAlignment
    {
        Top,
        Center,
        Bottom,
        Fit
    };

    public void Update()
    {
        if (invalidLayout)
        {
            ValidateLayout();
        }
    }

    public void Start()
    {
        invalidLayout = true;
    }

    public void InvalidateLayout()
    {
        invalidLayout = true;
    }

	public void CLICK () { 
		Debug.Log("click");
	}

    /**
     * This will test the layout validity and if it need to be recalculated it will do so. If there is a prent layout thats also invalide it will call that first.
     */
    public void ValidateLayout()
    {
        if (invalidLayout)
        {
            // Am I a root node? If not test parent first only call if its layout has been invalidated.
            Layout parentLayout = GetParentLayout();

            if (parentLayout != null && parentLayout.invalidLayout)
            {
                Debug.Log("Parent Layout been called " + gameObject.name);
                parentLayout.ValidateLayout();
            }
            else
            {
                Debug.Log("My Layout been called " + gameObject.name);
                DoLayout();
                invalidLayout = false;
                // Layout all my children
                for (int t = 0; t < transform.childCount; t++)
                {
                    GameObject child = transform.GetChild(t).gameObject;
                    Layout childLayout = child.GetComponent<Layout>();
                    if (childLayout != null)
                    {
                        Debug.Log("Child Layout been called " + child.name);
                        childLayout.DoLayout();
                        childLayout.invalidLayout = false;
                    }

                }
            }
        }
    }

    protected RectTransform GetRectTransform(GameObject go)
    {
        ILayoutElement element = GetLayoutComponent(go);
        if (element is Text)
        {
            return ((Text)element).rectTransform;
        }
        else if (element is Image)
        {
            return ((Image)element).rectTransform;
        }
        return go.GetComponent<RectTransform>();
    }

    private ILayoutElement GetLayoutComponent(GameObject go)
    {
        ILayoutElement element = null;

        Component[] components = go.GetComponents<Component>();
        for (int t = 0; t < components.Length; t++)
        {
            Component c = components[t];
            if (c is ILayoutElement)
            {
                element = (ILayoutElement)c;
                break;
            }

        }

        return element;
    }

    protected void FixStretchMode(RectTransform rectTransform )
    {
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        rectTransform.anchorMin = new Vector2(.5f, .5f);
        rectTransform.anchorMax = new Vector2(.5f, .5f);
        rectTransform.sizeDelta = new Vector2(width, height);
    }

    /**
     * This gets the parent layout of this object. We should consider caching it, or at the very least make sure its called sparingly. DoLayout must not be called each frame.
     */
    private Layout GetParentLayout()
    {
        if (transform.parent != null)
        {
            GameObject go = transform.parent.gameObject;
            if (go != null)
            {
                Layout parentLayout = go.GetComponent<Layout>();

                return parentLayout;
            }

        }
        return null;
    }

	public abstract void DoLayout();
	public abstract Vector2 CalculateMinSize();
	

}
