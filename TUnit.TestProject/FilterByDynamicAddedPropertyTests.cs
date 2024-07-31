﻿using System.Diagnostics;
using TUnit.Core;
using TUnit.Core.Interfaces;

namespace TUnit.TestProject;

[MyDynamicallyAddedProperty]
public class FilterByDynamicAddedPropertyTests
{
    [Test]
    public void Test1()
    {
    }
    
    public class MyDynamicallyAddedPropertyAttribute : Attribute, IOnTestDiscoveryAttribute
    {
        public void OnTestDiscovery(DiscoveredTestContext discoveredTestContext)
        {
            discoveredTestContext.AddProperty("MyKey", "MyDynamicallyAddedValue");
        }
    }
}