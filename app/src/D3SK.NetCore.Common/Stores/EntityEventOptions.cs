using System.Collections.Generic;

namespace D3SK.NetCore.Common.Stores
{
    public abstract class UpdateStrategyOptions
    {
    }

    public class AddEntityOptions : UpdateStrategyOptions
    {
        public bool SetNullOnAdd { get; set; } = true;
    }

    public class DeleteEntityOptions : UpdateStrategyOptions
    {
    }

    public class UpdateEntityOptions : UpdateStrategyOptions
    {
        public IList<string> PropertiesToUpdate { get; set; }

        public bool UpdateNestedEntitiesAndCollections { get; set; }
    }
    
    public class UpdateCollectionOptions : UpdateStrategyOptions
    {
    }
}
