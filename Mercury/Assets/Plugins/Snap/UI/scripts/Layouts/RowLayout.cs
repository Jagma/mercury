using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RowLayout : Layout
{
    private float currentLayoutX = 0;
    private float currentLayoutY = 0;

    private Vector2 anchorMin = new Vector2(0, 1);
    private Vector2 anchorMax = new Vector2(0, 1);
    private Vector2 pivot = new Vector2(0, 1);

    private RectTransform panelRectTransform;
	private float stretchItemWidth;
	private float maxItemWidth;
	private float maxItemHeight;
    private float minWidth;
    private float totalBorderWidth;
    private int itemCount = 0;

    public RectOffset border=new RectOffset();
	public int spacing=0;

    public bool matchWidths = false;
    public bool matchHeights = false;
	
	public bool resizeContainerWidth = false;
	public bool resizeContainerHeight = false;
	
    override public void DoLayout()
    {
        CalculateAnchors();
        for (int t = 0; t < transform.childCount; t++)
        {
            GameObject nextItem = transform.GetChild(t).gameObject;
            RectTransform rectTransform = GetRectTransform(nextItem);
            if (rectTransform != null)
            {
                LayoutRectTransform(rectTransform);
            }
        }

    }

    private void CalculateAnchors()
    {
        panelRectTransform = GetComponent<RectTransform>();
	    Vector2 minSize=CalculateMinSize();
	    
	    if (resizeContainerWidth && resizeContainerHeight){
		    panelRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,minSize.x);
		    panelRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,minSize.y);
	    } else if (resizeContainerWidth && !resizeContainerHeight){
		    panelRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,minSize.x);
	    } else if (!resizeContainerWidth && resizeContainerHeight){
		    panelRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,minSize.y);
	    }
	    
        if (horizontalAlignment == HorizontalAlignment.Left || horizontalAlignment == HorizontalAlignment.Fit)
        {
            anchorMin = new Vector2(0, 1);
            anchorMax = new Vector2(0, 1);
            pivot = new Vector2(0, 1);
            currentLayoutX = 0;
        }
        else if (horizontalAlignment == HorizontalAlignment.Center)
        {
            anchorMin = new Vector2(.5f, 1);
            anchorMax = new Vector2(.5f, 1);
            pivot = new Vector2(0, 1);
            currentLayoutX = -(minWidth + totalBorderWidth) / 2;
        }
        else if (horizontalAlignment == HorizontalAlignment.Right)
        {
            anchorMin = new Vector2(1, 1);
            anchorMax = new Vector2(1, 1);
            pivot = new Vector2(0, 1);
            currentLayoutX = -border.right - minWidth;
        }

        if (verticleAlignment == VerticleAlignment.Top)
        {
            anchorMin = new Vector2(anchorMin.x, 1);
            anchorMax = new Vector2(anchorMax.x, 1);
            pivot = new Vector2(pivot.x, 1);
            currentLayoutY = -border.top;
        }
        else if (verticleAlignment == VerticleAlignment.Fit)
        {
            anchorMin = new Vector2(anchorMin.x, 1);
            anchorMax = new Vector2(anchorMax.x, 1);
            pivot = new Vector2(pivot.x, 1);
            currentLayoutY = -border.top;
        }
        else if (verticleAlignment == VerticleAlignment.Center)
        {
            anchorMin = new Vector2(anchorMin.x, .5f);
            anchorMax = new Vector2(anchorMax.x, .5f);
            pivot = new Vector2(pivot.x, .5f);
        }
        else if (verticleAlignment == VerticleAlignment.Bottom)
        {
            anchorMin = new Vector2(anchorMin.x, 0);
            anchorMax = new Vector2(anchorMax.x, 0);
            pivot = new Vector2(pivot.x, 0f);
        }
    }

	public override Vector2 CalculateMinSize()
    {
	    float matchWidth=0;
	    minWidth = 0;
        maxItemWidth = 0;
        maxItemHeight = 0;
		totalBorderWidth = border.left + border.right - spacing;
        itemCount = 0;
        for (int t = 0; t < transform.childCount; t++)
        {
            GameObject nextItem = transform.GetChild(t).gameObject;
	        Layout childLayout=nextItem.GetComponent<Layout>();
	        if (childLayout!=null){
		        LayoutRule layoutRule=nextItem.GetComponent<LayoutRule>();
		        Vector2 childMinSize=childLayout.CalculateMinSize();
		        RectOffset localBorder;
		        if (layoutRule!=null){
			        localBorder=layoutRule.border;
			        if (!layoutRule.stretchWidth){
			        	matchWidth+=childMinSize.x+layoutRule.border.horizontal;
			        }else{
			        	matchWidth+=maxItemWidth;
			        }
		        }else{
			        localBorder=new RectOffset();
			        matchWidth+=maxItemWidth;
		        }
		        
		        itemCount++;
		        minWidth += childMinSize.x+localBorder.horizontal;
		        totalBorderWidth += spacing;
		        
		        if (maxItemWidth < childMinSize.x+localBorder.horizontal)
		        {
			        maxItemWidth = childMinSize.x+localBorder.horizontal;
		        }
		        if (maxItemHeight < childMinSize.y+localBorder.vertical)
		        {
			        maxItemHeight = childMinSize.y+localBorder.vertical;
		        }
	        }else{
		        RectTransform rectTransform = GetRectTransform(nextItem);
	            if (rectTransform != null)
	            {
		            LayoutRule layoutRule=nextItem.GetComponent<LayoutRule>();
		            RectOffset localBorder;
		            if (layoutRule!=null){
			            localBorder=layoutRule.border;
			            if (!layoutRule.stretchWidth){
				            matchWidth+=rectTransform.rect.width+localBorder.horizontal;
			            }else{
				            matchWidth+=maxItemWidth;
			            }
		            }else{
			            localBorder=new RectOffset();
			            matchWidth+=maxItemWidth;
		            }
		            
		            itemCount++;
		            minWidth += rectTransform.rect.width+localBorder.horizontal;
		            totalBorderWidth += spacing;
		            
		            if (maxItemWidth < rectTransform.rect.width+localBorder.horizontal)
		            {
			            maxItemWidth = rectTransform.rect.width+localBorder.horizontal;
		            }
		            if (maxItemHeight < rectTransform.rect.height+localBorder.vertical)
		            {
			            maxItemHeight = rectTransform.rect.height+localBorder.vertical;
		            }
	            }
	        }
        }
        if (matchWidths)
        {
	        minWidth = matchWidth;//itemCount * maxItemWidth;
        }
	    return new Vector2(minWidth+totalBorderWidth,maxItemHeight+border.vertical);
    }

    private void LayoutRectTransform(RectTransform rectTransform)
    {
        float width = rectTransform.rect.width;
	    float height = rectTransform.rect.height;
	    
	    LayoutRule layoutRule=rectTransform.GetComponent<LayoutRule>();
	    
	    RectOffset localBorder;
	    if (layoutRule!=null){
	    	localBorder=layoutRule.border;
	    }else{
	    	localBorder=new RectOffset();
	    }    
	    
        // Set location
	    rectTransform.anchoredPosition = new Vector2(currentLayoutX + border.left+ localBorder.left, currentLayoutY-localBorder.top);
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.pivot = pivot;
        // We replace width and height as they are messed up if the component was in stretch mdoe before
        rectTransform.sizeDelta = new Vector2(width, height);

        // Set width if need be
        if (horizontalAlignment == HorizontalAlignment.Fit && matchWidths)
        {
	        rectTransform.sizeDelta = new Vector2((panelRectTransform.rect.width - totalBorderWidth) / itemCount, rectTransform.rect.height-localBorder.vertical);
        }
        else if (horizontalAlignment == HorizontalAlignment.Fit)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width / minWidth * (panelRectTransform.rect.width - totalBorderWidth), rectTransform.rect.height-localBorder.vertical);
        }
        else if (matchWidths)
        {
	        rectTransform.sizeDelta = new Vector2(maxItemWidth, rectTransform.rect.height-localBorder.vertical);
        }

        // Set width if need be
	    if (verticleAlignment == VerticleAlignment.Fit || (layoutRule!=null && layoutRule.stretchHeight))
        {
		    rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, panelRectTransform.rect.height - border.vertical - localBorder.vertical);
        }
        else if (matchHeights)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, maxItemHeight);
        }

        // Adjust height
	    currentLayoutX += rectTransform.rect.width + spacing + localBorder.horizontal;
    }
}
