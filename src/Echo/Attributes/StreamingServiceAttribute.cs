using System;

namespace Echo.Attributes
{
    /// <summary>
    /// Attribute for a streaming service type contract (Interface).
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class StreamingServiceAttribute : Attribute
    {
    }
}
