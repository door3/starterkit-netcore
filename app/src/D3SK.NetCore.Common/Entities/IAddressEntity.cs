using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public interface IAddressEntity : ICompositeEntity
    {
        string AddressLine1 { get; set; }

        string AddressLine2 { get; set; }

        string City { get; set; }

        string StateOrProvince { get; set; }

        string PostalCode { get; set; }

        string Country { get; set; }
    }
}
