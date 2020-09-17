using System;

namespace D3SK.NetCore.Common.Stores
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = true)]
    public class UpdateStrategyAttribute : Attribute
    {
        public string Property { get; set; }

        public bool NullOnAdd { get; set; } = false;

        public bool EnableUpdating { get; set; } = true;

        public string CrossDomainConstraint { get; set; }
    }
}
