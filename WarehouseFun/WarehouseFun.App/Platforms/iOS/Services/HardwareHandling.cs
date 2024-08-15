using WarehouseFun.App.Base;

namespace WarehouseFun.App.Services
{
    public partial class HardwareHandling : IHardwareHandling
    {
        public event Action<string>? DataScanned;
        public event Action? TriggerPressed;
    }
}
