using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NZBHags
{
    public interface ISettings
    {
        bool ValidateUI();
        void Save();
        void UpdateUI();

    }
}
