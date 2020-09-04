using System.Collections.Generic;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Domain.Entities
{
    public class DomainShortName : CompositeEntityBase, IShortNameEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DomainShortName()
        {
        }

        public DomainShortName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        
        public override IEnumerable<object> GetUniqueValues()
        {
            yield return FirstName;
            yield return LastName;
        }
    }

    public class DomainName : DomainShortName, INameEntity
    {
        public string MiddleName { get; set; }

        public DomainName()
        {
        }

        public DomainName(string firstName, string lastName, string middleName = null) : base(firstName, lastName)
        {
            MiddleName = middleName;
        }

        public override IEnumerable<object> GetUniqueValues()
        {
            yield return FirstName;
            yield return MiddleName;
            yield return LastName;
        }
    }
}
