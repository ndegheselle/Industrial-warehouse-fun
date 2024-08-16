using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using WarehouseFun.App.Base;

namespace WarehouseFun.App
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private const Keycode TRIGGER_KEYCODE = Keycode.ButtonL1;
        private ScanReceiver _scanReceiver;
        private bool _triggerDown = false;

        private static string _INTENT_ACTION = "pickles.RECVR";
        private static string _INTENT_CATEGORY = "android.intent.category.DEFAULT";

        private static string _EXTRA_PARAM = "com.symbol.datawedge.api.SCANNER_INPUT_PLUGIN";
        private static string _ACTION_SCANNERINPUTPLUGIN = "com.symbol.datawedge.api.ACTION";
        private static string _DWAPI_STOP_PLUGIN = "DISABLE_PLUGIN";
        private static string _DWAPI_START_PLUGIN = "ENABLE_PLUGIN";

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _scanReceiver = new ScanReceiver(this, _INTENT_ACTION, _INTENT_CATEGORY);
            _scanReceiver.ActivateScanner();
            _scanReceiver.ReceptionScanner += _scanReceiver_ReceptionScanner;
        }

        private void _scanReceiver_ReceptionScanner(string obj)
        {
            HardwareHandling.Instance.OnDataScanned(obj);
        }

        protected override void OnResume()
        {
            base.OnResume();
            _scanReceiver.Resume();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _scanReceiver.Pause();
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent? e)
        {
            // Gachette du scanner
            if (keyCode == TRIGGER_KEYCODE && _triggerDown == false)
            {
                _triggerDown = true;
                HardwareHandling.Instance.OnTriggerPressed();
            }

            return base.OnKeyDown(keyCode, e);
        }

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent? e)
        {
            if (keyCode == TRIGGER_KEYCODE)
            {
                _triggerDown = false;
            }
            return base.OnKeyUp(keyCode, e);
        }
    }
}
