using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UxmlIWEnumAttributeDescription<T> : UxmlEnumAttributeDescription<T> where T : struct, IConvertible
{

    public UxmlIWEnumAttributeDescription()
    {
        use = Use.Optional;
        restriction = null;
    }
    public override T GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
    {
        T value =base.GetValueFromBag(bag,cc);
        Func<string, T, T> converterFunc = (s, t) =>
          {
              if (Enum.Parse(typeof(T), s)!=null)
              {
                  return (T)Enum.Parse(typeof(T), s);
              }
              
              return defaultValue;
          };
        if (TryGetValueFromBag(bag, cc, converterFunc, defaultValue, ref value))
        {                                                      
            return value;
        }
        
        return defaultValue;
    }
}