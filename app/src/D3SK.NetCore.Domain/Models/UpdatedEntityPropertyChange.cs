using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Domain.Models
{
    public class UpdatedEntityPropertyChange
    {
        public string PropertyName { get; }

        public object NewValue { get; }

        public object OldValue { get; }


        public UpdatedEntityPropertyChange(string propertyName, object newValue, object oldValue)
        {
            PropertyName = propertyName.NotNull(nameof(propertyName));
            NewValue = newValue;
            OldValue = oldValue;
        }
    }
}
