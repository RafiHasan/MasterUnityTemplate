using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;

public class IWVisualElement : VisualElement
{
#if UNITY_EDITOR
    static int SkipFrame = 5;
#else
    static int SkipFrame = 2;
#endif

    float CalculatedHeight = 0;
    float CalculatedWidth = 0;
    public new class UxmlFactory : UxmlFactory<IWVisualElement, UxmlTraits>
    {

    }
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIWEnumAttributeDescription<ScaleMode> ScaleBy = new UxmlIWEnumAttributeDescription<ScaleMode> { name = "scale-image-by", defaultValue = ScaleMode.NONE };

        UxmlBoolAttributeDescription HorizontalCheck = new UxmlBoolAttributeDescription { name = "HorizontalCheck", defaultValue = false };
        UxmlFloatAttributeDescription Horizontal = new UxmlFloatAttributeDescription { name = "Horizontal", defaultValue = 0.5f };
        UxmlBoolAttributeDescription VerticalCheck = new UxmlBoolAttributeDescription { name = "VerticalCheck", defaultValue = false };
        UxmlFloatAttributeDescription Vertical = new UxmlFloatAttributeDescription { name = "Vertical", defaultValue = 0.5f };
        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public UxmlTraits()
        {
            canHaveAnyAttribute = true;
        }
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var ate = ve as IWVisualElement;
            ate.ScaleImageBy = ScaleBy.GetValueFromBag(bag, cc);
            ate.Horizontal = Horizontal.GetValueFromBag(bag, cc);
            ate.Vertical = Vertical.GetValueFromBag(bag, cc);
            ate.HorizontalCheck = HorizontalCheck.GetValueFromBag(bag, cc);
            ate.VerticalCheck = VerticalCheck.GetValueFromBag(bag, cc);
            ate.RunPositionOverrideLater(SkipFrame);
        }
    }
    ScaleMode scaleMode;
    public ScaleMode ScaleImageBy
    {
        get => scaleMode;
        set
        {
            scaleMode = value;
            MarkDirtyRepaint();
            RunScaleByLater(SkipFrame);
        }
    }
    bool horizontalcheck;
    public bool HorizontalCheck
    {
        get => horizontalcheck;
        set
        {
            horizontalcheck = value;
            MarkDirtyRepaint();
            RunPositionOverrideLater(SkipFrame);
        }
    }

    float horizontal;
    public float Horizontal
    {
        get => horizontal;
        set
        {
            horizontal = value;
            MarkDirtyRepaint();
            RunPositionOverrideLater(SkipFrame);
        }
    }

    bool verticalcheck;
    public bool VerticalCheck
    {
        get => verticalcheck;
        set
        {
            verticalcheck = value;
            MarkDirtyRepaint();
            RunPositionOverrideLater(SkipFrame);
        }
    }

    float vertical;
    public float Vertical
    {
        get => vertical;
        set
        {
            vertical = value;
            MarkDirtyRepaint();
            RunPositionOverrideLater(SkipFrame);
        }
    }

    public IWVisualElement()
    {

    }

    public async void RunScaleByLater(int t)
    {
        for (int i = 0; i < t; i++)
            await Task.Yield();
        Background background = resolvedStyle.backgroundImage;
        if (background != null)
        {
            if (background.renderTexture != null)
                CalculateImageSize(background.renderTexture.width, background.renderTexture.height, resolvedStyle.width, resolvedStyle.height, ScaleImageBy);
            else if (background.texture != null)
                CalculateImageSize(background.texture.width, background.texture.height, resolvedStyle.width, resolvedStyle.height, ScaleImageBy);
            else if (background.sprite != null)
                CalculateImageSize(background.sprite.texture.width, background.sprite.texture.height, resolvedStyle.width, resolvedStyle.height, ScaleImageBy);
        }

    }

    public async void RunPositionOverrideLater(int t)
    {
        for (int i = 0; i < t; i++)
            await Task.Yield();
        CalculatePositionOverride(style.width.value.value, style.height.value.value, Horizontal, Vertical);
    }


    public void CalculateImageSize(float width, float height, float resolveWidth, float resolvedHeight, ScaleMode mode)
    {
        if (ScaleImageBy == ScaleMode.WIDTH)
        {
            float calculatedheight = height * resolveWidth / width;
            style.height = calculatedheight;
            DoMeasure(width, MeasureMode.Exactly, calculatedheight, MeasureMode.Exactly);
        }
        else if (ScaleImageBy == ScaleMode.HEIGHT)
        {
            float calculatedwidth = width * resolvedHeight / height;
            style.width = calculatedwidth;
            DoMeasure(calculatedwidth, MeasureMode.Exactly, height, MeasureMode.Exactly);
        }
    }

    public void CalculatePositionOverride(float width, float height, float horizontal, float vertical)
    {
        //Debug.Log(width);
        //Debug.Log(height);

        float widthvalue = scaleMode == ScaleMode.HEIGHT ? width : resolvedStyle.width;

        float heightvalue = scaleMode == ScaleMode.WIDTH ? height : resolvedStyle.height;

        int align = (int)parent.resolvedStyle.alignItems - 1;
        int justify = (int)parent.resolvedStyle.justifyContent;

        if (parent.resolvedStyle.flexDirection == FlexDirection.Row)
        {
            //horizontal -= justify * 0.5f;
            //vertical -= align * 0.5f;
        }
        else if (parent.resolvedStyle.flexDirection == FlexDirection.RowReverse)
        {
            //horizontal -= justify * 0.5f;
            //vertical -= align * 0.5f;
        }
        else if (parent.resolvedStyle.flexDirection == FlexDirection.Column)
        {
            //vertical -= justify * 0.5f;
            //horizontal -= align * 0.5f;
        }
        else if (parent.resolvedStyle.flexDirection == FlexDirection.ColumnReverse)
        {
            //vertical -= justify * 0.5f;
            // horizontal -= align * 0.5f;
        }

        Debug.Log(name + " " + style.top + "  " + (parent.resolvedStyle.width / 2 - widthvalue / 2) + "    " + ((parent.resolvedStyle.width - widthvalue - resolvedStyle.marginLeft - resolvedStyle.marginRight) * horizontal) + " " + widthvalue);

        if (HorizontalCheck && parent.resolvedStyle.flexDirection == FlexDirection.Row)
            style.left = -resolvedStyle.left + (parent.resolvedStyle.width - widthvalue - resolvedStyle.marginLeft - resolvedStyle.marginRight) * horizontal;
        else if (HorizontalCheck && parent.resolvedStyle.flexDirection == FlexDirection.RowReverse)
            style.left = -resolvedStyle.left + (parent.resolvedStyle.width - widthvalue - resolvedStyle.marginLeft - resolvedStyle.marginRight) * horizontal;
        else if (HorizontalCheck)
            style.left = (parent.resolvedStyle.width - widthvalue - resolvedStyle.marginLeft - resolvedStyle.marginRight) * horizontal;



        if (VerticalCheck && parent.resolvedStyle.flexDirection == FlexDirection.Column)
            style.top = -resolvedStyle.top + (parent.resolvedStyle.height - heightvalue - resolvedStyle.marginTop - resolvedStyle.marginBottom) * vertical;
        else if (VerticalCheck && parent.resolvedStyle.flexDirection == FlexDirection.ColumnReverse)
            style.bottom = -resolvedStyle.bottom + (parent.resolvedStyle.height - heightvalue - resolvedStyle.marginTop - resolvedStyle.marginBottom) * vertical;
        else if (VerticalCheck)
            style.top = (parent.resolvedStyle.height - heightvalue - resolvedStyle.marginTop - resolvedStyle.marginBottom) * vertical;
    }
}
