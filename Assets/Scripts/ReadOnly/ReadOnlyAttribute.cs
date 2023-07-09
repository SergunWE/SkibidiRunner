using System;
using UnityEngine;

namespace SkibidiRunner.ReadOnly
{
    /// <summary>
    /// Read Only attribute.
    /// Attribute is use only to mark ReadOnly properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
}