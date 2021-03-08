using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Domain.Entities
{
    public class DomainEntityReference : DomainEntityReference<int>
    {
        public DomainEntityReference()
        {
        }

        public DomainEntityReference(int id, string name, object extendedData)
            : base(id, name, extendedData)
        {
        }
    }

    public class DomainEntityReference<TKey> : EntityReference<TKey>, IDomainEntityReference<TKey>
    {
        [NotMapped]
        public new object ExtendedData
        {
            get => ExtendedDataJson.IsNotEmpty() ? JsonHelper.Deserialize(ExtendedDataJson) : null;
            set => ExtendedDataJson = JsonHelper.Serialize(value);
        }

        [JsonIgnore]
        public string ExtendedDataJson { get; private set; }

        public DomainEntityReference()
        {
        }

        public DomainEntityReference(TKey id, string name, object extendedData)
            : base(id, name, extendedData)
        {
            ExtendedData = extendedData;
        }
    }
}
