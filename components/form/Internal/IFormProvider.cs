using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.Internal
{
    public interface IFormProvider
    {
        internal void AddForm(IForm form);
        internal void RemoveForm(IForm form);
    }
}
