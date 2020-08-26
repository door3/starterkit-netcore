using System.Collections.Generic;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Domain.Entities
{
    public class DomainAddress : CompositeEntityBase, IAddressEntity
    {
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string StateOrProvince { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public DomainAddress()
        {
        }

        public DomainAddress(string address1, string address2, string city, string stateOrProvince, string postalCode,
            string country)
        {
            AddressLine1 = address1;
            AddressLine2 = address2;
            City = city;
            StateOrProvince = stateOrProvince;
            PostalCode = postalCode;
            Country = country;
        }

        public override IEnumerable<object> GetUniqueValues()
        {
            yield return AddressLine1;
            yield return AddressLine2;
            yield return City;
            yield return StateOrProvince;
            yield return PostalCode;
            yield return Country;
        }
    }
}
