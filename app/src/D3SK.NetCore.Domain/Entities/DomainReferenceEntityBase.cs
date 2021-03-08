using System.ComponentModel.DataAnnotations.Schema;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Utilities;
using Newtonsoft.Json;

namespace D3SK.NetCore.Domain.Entities
{
    public abstract class DomainReferenceEntityBase : EntityBase
    {
        public DomainReferenceEntityBase(int id, string name, object extendedData = null)
        {
            Id = id;
            Name = name;
            ExtendedData = extendedData;
        }

        protected DomainReferenceEntityBase()
        {
        }

        [NotMapped]
        public object ExtendedData
        {
            get => ExtendedDataJson.IsNotEmpty() ? JsonHelper.Deserialize(ExtendedDataJson) : null;
            set => ExtendedDataJson = JsonHelper.Serialize(value);
        }
        public string Name { get; set; }
        [JsonIgnore]
        public string ExtendedDataJson { get; private set; }
    }
}
