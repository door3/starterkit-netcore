using System.Collections.Generic;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Domain.Entities
{
    public class DomainPhone : CompositeEntityBase, IPhoneEntity
    {
        public string Number { get; set; }

        public DomainPhone()
        {
        }

        public DomainPhone(string number)
        {
            Number = number;
        }

        public override IEnumerable<object> GetUniqueValues()
        {
            yield return Number;
        }
    }
}
