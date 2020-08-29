using System.Collections.Generic;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Domain.Entities
{
    public class DomainName : CompositeEntityBase, INameEntity
    {
        public string Prefix { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Suffix { get; set; }

        public DomainName()
        {
        }

        public DomainName(string prefix, string firstName, string middleName, string lastName, string suffix)
        {
            Prefix = prefix;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Suffix = suffix;
        }

        public DomainName(string firstName, string lastName, string middleName = null)
        : this(null, firstName, middleName, lastName, null)
        {
        }

        public override IEnumerable<object> GetUniqueValues()
        {
            yield return Prefix;
            yield return FirstName;
            yield return MiddleName;
            yield return LastName;
            yield return Suffix;
        }
    }
}
