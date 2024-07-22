using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamedArrayAttribute : PropertyAttribute
{
    public readonly string[] names;
    public NamedArrayAttribute(Type enumType) => names = Enum.GetNames(enumType);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
