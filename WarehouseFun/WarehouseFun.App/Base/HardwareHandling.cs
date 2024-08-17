using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public partial class HardwareHandling : IHardwareHandling
    {
        private static readonly Lazy<HardwareHandling> lazy = new Lazy<HardwareHandling>(() => new HardwareHandling());
        public static HardwareHandling Instance { get { return lazy.Value; } }

        private HardwareHandling()
        {
        }

        public event Action<string>? DataScanned;
        public event Action? TriggerPressed;

        public void OnDataScanned(string data)
        {
            DataScanned?.Invoke(data);
        }

        public void OnTriggerPressed()
        {
            TriggerPressed?.Invoke();
        }
    }
}
