using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;

public class IWLabel : Label
{
#if UNITY_EDITOR
    static int SkipFrame = 5;
#else
    static int SkipFrame = 2;
#endif

    float CalculatedHeight = 0;
    float CalculatedWidth = 0;
    public new class UxmlFactory : UxmlFactory<IWLabel, UxmlTraits>
    {

    }
    public new class UxmlTraits : Label.UxmlTraits
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
            var ate = ve as IWLabel;
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

    public IWLabel()
    {

    }

    public async void RunScaleByLater(int t)
    {
        for (int i = 0; i < t; i++)
            await Task.Yield();
        CalculateImageSize(10, 10, resolvedStyle.width, resolvedStyle.height, ScaleImageBy);

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
            style.fontSize = resolvedStyle.fontSize * resolveWidth / resolvedHeight;
        }
        else if (ScaleImageBy == ScaleMode.HEIGHT)
        {
            style.fontSize = resolvedStyle.fontSize* resolvedHeight / resolveWidth;
        }
    }

    public void CalculatePositionOverride(float width, float height, float horizontal, float vertical)
    {
        float widthvalue = scaleMode == ScaleMode.HEIGHT ? width : resolvedStyle.width;

        float heightvalue = scaleMode == ScaleMode.WIDTH ? height : resolvedStyle.height;

        if (HorizontalCheck || verticalcheck)
            style.position = Position.Absolute;
        else
            style.position = resolvedStyle.position;

        if (HorizontalCheck)
            style.left = (parent.resolvedStyle.width - widthvalue - resolvedStyle.marginLeft - resolvedStyle.marginRight) * horizontal;
        if (VerticalCheck)
            style.top = (parent.resolvedStyle.height - heightvalue - resolvedStyle.marginTop - resolvedStyle.marginBottom) * vertical;
    }
}
