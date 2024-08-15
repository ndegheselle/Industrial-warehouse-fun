using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseFun.App.Base
{
    public interface IHardwareHandling
    {
        event Action<string>? DataScanned;
        event Action? TriggerPressed;
    }
}
