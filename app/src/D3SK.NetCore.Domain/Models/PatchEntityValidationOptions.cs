using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Domain.Models
{
    public class PatchEntityValidationOptions
    {
        public bool HasPatch { get; }

        public IList<string> PropertiesToUpdate { get; }

        public PatchEntityValidationOptions()
        {
            HasPatch = false;
        }

        public PatchEntityValidationOptions(IList<string> propertiesToUpdate)
        {
            HasPatch = propertiesToUpdate.Any();
            PropertiesToUpdate = propertiesToUpdate;
        }

        public bool HasProperty(string property) => !HasPatch || PropertiesToUpdate.ContainsIgnoreCase(property);
    }
}
