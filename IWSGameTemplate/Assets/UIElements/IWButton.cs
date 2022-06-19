using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class IWButton : Button
{
#if UNITY_EDITOR
    static int SkipFrame = 5;
#else
    static int SkipFrame = 2;
#endif

    public new class UxmlFactory : UxmlFactory<IWButton, UxmlTraits>
    {
        
    }
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIWEnumAttributeDescription<ScaleMode> ScaleBy = new UxmlIWEnumAttributeDescription<ScaleMode> { name = "scale-image-by",defaultValue=ScaleMode.NONE};
        UxmlIWEnumAttributeDescription<PositionOverRide> Horizontal = new UxmlIWEnumAttributeDescription<PositionOverRide> { name = "Horizontal", defaultValue = PositionOverRide.NONE };
        UxmlIWEnumAttributeDescription<PositionOverRide> Vertical = new UxmlIWEnumAttributeDescription<PositionOverRide> { name = "Vertical", defaultValue = PositionOverRide.NONE };

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
            var ate = ve as IWButton;
            ate.ScaleImageBy = ScaleBy.GetValueFromBag(bag, cc);
            ate.Horizontal = Horizontal.GetValueFromBag(bag, cc);
            ate.Vertical = Vertical.GetValueFromBag(bag, cc);
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

    PositionOverRide horizontal;
    public PositionOverRide Horizontal
    {
        get => horizontal;
        set
        {
            horizontal = value;
            MarkDirtyRepaint();
            RunPositionOverrideLater(SkipFrame);
        }
    }

    PositionOverRide vertical;
    public PositionOverRide Vertical
    {
        get => vertical;
        set
        {
            vertical = value;
            MarkDirtyRepaint();
            RunPositionOverrideLater(SkipFrame);
        }
    }

    public IWButton()
    {
        
    }

    public async void RunScaleByLater(int t)
    {
        for(int i=0;i<t;i++)
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


    public void CalculateImageSize(float width,float height,float resolveWidth, float resolvedHeight, ScaleMode mode)
    {
        if (ScaleImageBy == ScaleMode.WIDTH)
        {
            float calculatedheight= height * resolveWidth / width;
            style.height = calculatedheight;
            DoMeasure(width, MeasureMode.Exactly, calculatedheight, MeasureMode.Exactly);
        }
        else if(ScaleImageBy == ScaleMode.HEIGHT)
        {
            float calculatedwidth = width * resolvedHeight / height;
            style.width = calculatedwidth;
            DoMeasure(calculatedwidth, MeasureMode.Exactly, height, MeasureMode.Exactly);
        }
    }

    public void CalculatePositionOverride(float width, float height, PositionOverRide horizontal,PositionOverRide vertical)
    {
        if (horizontal == PositionOverRide.START)
        {
            style.left = width/2;
            //style.paddingRight = parent.resolvedStyle.width - width;
        }
        else if (horizontal == PositionOverRide.CENTER)
        {
            style.left = parent.resolvedStyle.width/2 - width / 2;
            //style.paddingRight = parent.resolvedStyle.width - width/2;
        }
        else if (horizontal == PositionOverRide.END)
        {
            float check= parent.resolvedStyle.width - width/2;
            style.left = check;
            //style.paddingRight = width/2;
        }

        if (vertical == PositionOverRide.START)
        {
            style.top = height/2;
            //style.paddingRight = parent.resolvedStyle.width - width;
        }
        else if (vertical == PositionOverRide.CENTER)
        {
            style.top = parent.resolvedStyle.height / 2- height/2;
            //style.paddingRight = parent.resolvedStyle.width - width/2;
        }
        else if (vertical == PositionOverRide.END)
        {
            float check = parent.resolvedStyle.height - height;
            style.top = check;
            //style.paddingRight = width/2;
        }

        style.top = style.top.value.value+ resolvedStyle.top;

        Debug.Log(style.left);
        Debug.Log(style.top);
    }
}
