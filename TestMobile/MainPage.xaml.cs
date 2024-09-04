using ZXing.Net.Maui;

namespace TestMobile
{
    public partial class MainPage : ContentPage
    {
        public static bool IsDetect = false;
        public MainPage()
        {
            InitializeComponent();

            CameraBarcodeReaderView.Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormat.QrCode,
                AutoRotate = false,
            };

            Torch.BackgroundColor = Color.FromRgb(79, 176, 131);
            CameraBarcodeReaderView.IsTorchOn = false;
            Prem();
        }

        private async void Prem()
        {
            PermissionStatus statusLoca = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            PermissionStatus statusCam = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (statusCam != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.Camera>();
            }
            if (statusLoca != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
        }

        private void BarcodesDetected(object? sender, BarcodeDetectionEventArgs e)
        {
            if(IsDetect) return;
            
            IsDetect = true;
            
            var results = e.Results.First();

            if (results == null) return;

            var value = results.Value;

            if (value == null) return;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new Telemetry(value));
            });
        }

        private void OnLight(object? sender, EventArgs e)
        {

            CameraBarcodeReaderView.IsTorchOn = !CameraBarcodeReaderView.IsTorchOn;

            if (!CameraBarcodeReaderView.IsTorchOn)
            {
                Torch.BackgroundColor = Color.FromRgb(79, 176, 131);
            }
            else
            {
                Torch.BackgroundColor = Color.FromRgb(201, 1, 9);
            }
        }
    }
}
