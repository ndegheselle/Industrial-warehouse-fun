using Android.Content;

namespace WarehouseFun.App
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    public class ScanReceiver : BroadcastReceiver
    {
        private readonly MainActivity _mainActivity;
        private bool _isScannerActif = false;

        public event Action<string>? ReceptionScanner;

        // Intent Footer for our operation
        public string IntentAction { get; set; }
        public string IntentCategory { get; set; }

        private static string DATA_STRING_TAG = "com.motorolasolutions.emdk.datawedge.data_string";
        private static string LABEL_TYPE_TAG = "com.motorolasolutions.emdk.datawedge.label_type";
        private static string DECODE_DATA_TAG = "com.motorolasolutions.emdk.datawedge.decode_data";
        private static string SOURCE_TAG = "com.motorolasolutions.emdk.datawedge.source";

        private static string _INTENT_ACTION = "pickles.RECVR";
        private static string _INTENT_CATEGORY = "android.intent.category.DEFAULT";

        private static string _EXTRA_PARAM = "com.symbol.datawedge.api.SCANNER_INPUT_PLUGIN";
        private static string _ACTION_SCANNERINPUTPLUGIN = "com.symbol.datawedge.api.ACTION";
        private static string _DWAPI_STOP_PLUGIN = "DISABLE_PLUGIN";
        private static string _DWAPI_START_PLUGIN = "ENABLE_PLUGIN";

        public ScanReceiver(MainActivity mainActivity, string pIntentAction, string pIntentCategory)
        {
            _mainActivity = mainActivity;
            IntentAction = pIntentAction;
            IntentCategory = pIntentCategory;
        }

        public ScanReceiver()
        { }

        public override void OnReceive(Context? context, Intent? intent)
        {
            if (intent?.Action != null && intent.Action.Equals(IntentAction))
            {
                string string_tag = intent.GetStringExtra(DATA_STRING_TAG) ?? "";
                string label_tag = intent.GetStringExtra(LABEL_TYPE_TAG) ?? "";
                string decode_tag = intent.GetStringExtra(DECODE_DATA_TAG) ?? "";
                string source_tag = intent.GetStringExtra(SOURCE_TAG) ?? "";

                if (!string.IsNullOrEmpty(string_tag))
                {
                    ReceptionScanner?.Invoke(string_tag);
                }
            }
        }

        public void Resume()
        {
            IntentFilter scannerIntent = new IntentFilter(_INTENT_ACTION);
            scannerIntent.AddCategory(_INTENT_CATEGORY);
            _mainActivity.RegisterReceiver(this, scannerIntent);
        }
        public void Pause()
        {
            _mainActivity.UnregisterReceiver(this);
        }

        public void ActivateScanner()
        {
            if (_isScannerActif)
                return;

            var intent = new Intent();
            intent.SetAction(_ACTION_SCANNERINPUTPLUGIN);
            intent.PutExtra(_EXTRA_PARAM, _DWAPI_START_PLUGIN);
            _mainActivity.SendBroadcast(intent);
            _isScannerActif = true;
        }

        public void DeactivateScanner()
        {
            if (_isScannerActif == false)
                return;

            var intent = new Intent();
            intent.SetAction(_ACTION_SCANNERINPUTPLUGIN);
            intent.PutExtra(_EXTRA_PARAM, _DWAPI_STOP_PLUGIN);
            _mainActivity.SendBroadcast(intent);
            _isScannerActif = false;
        }
    }
}
