using AstroClient.Wpf.Models;
using AstroClient.Wpf.Services;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AstroClient.Wpf.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        private readonly AstroApiClient _api = new AstroApiClient();

        #region calculation inputs and results
        // Velocity calculation
        public double? ObservedWavelength { get; set; }
        public double? RestWavelength { get; set; }
        private string? _velocityResult;
        public string? VelocityResult { get => _velocityResult; set { SetProperty(ref _velocityResult, value); } }

        // Distance calculation
        public double? ParallaxArcseconds { get; set; }
        private string? _distanceResult;
        public string? DistanceResult { get => _distanceResult; set { SetProperty(ref _distanceResult, value); } }

        // Kelvin calculation
        public double? Celsius { get; set; }
        private string? _kelvinResult;
        public string? KelvinResult { get => _kelvinResult; set { SetProperty(ref _kelvinResult, value); } }

        // Event Horizon calculation
        public double? MassKg { get; set; }
        private string? _radiusResult;
        public string? RadiusResult { get => _radiusResult; set { SetProperty(ref _radiusResult, value); } }
        #endregion

        // UI state and settings
        private string? _status;
        public string? StatusMessage { get => _status; private set { SetProperty(ref _status, value); } } //stage bar text

        public event EventHandler<string>? LanguageChanged; 

        public MainViewModel()
        {
        }



        // status message helpers
        private void SetOk() => StatusMessage = "OK";
        private void SetError(string msg) => StatusMessage = msg;

        // Formats results
        private static string ToE6(double v) => v.ToString("E6", CultureInfo.InvariantCulture);

        // Validates user input, calls the API, updates the property
        public async Task CalcVelocityAsync()
        {
            StatusMessage = string.Empty;
            VelocityResult = null;

            // Error input handling
            if (ObservedWavelength is null || RestWavelength is null)
            { SetError("Valid number required."); return; }
            if (ObservedWavelength.Value <= 0 || RestWavelength.Value <= 0)
            { SetError("Wavelengths must be positive numbers."); return; }

            var request = new AstroDataTransfer
            {
                ObservedWavelength = ObservedWavelength.Value,
                RestWavelength = RestWavelength.Value
            };

            // Call the specific API method
            var response = await _api.CalculateVelocityAsync(request);

            if (!response.IsSuccess)
            {
                SetError(response.ErrorMessage ?? "Unknown error");
            }
            else if (response.Data?.VelocityMps.HasValue == true)
            {
                VelocityResult = ToE6(response.Data.VelocityMps.Value);
                SetOk();
            }
            else { SetError("Invalid response data received."); }
        }

        public async Task CalcDistanceAsync()
        {
            StatusMessage = string.Empty;
            DistanceResult = null;

            if (ParallaxArcseconds is null)
            { SetError("Valid number required."); return; }
            if (ParallaxArcseconds.Value <= 0)
            { SetError("Parallax must be a positive number."); return; }

            var request = new AstroDataTransfer
            {
                ParallaxArcseconds = ParallaxArcseconds.Value
            };

            // Call the specific API method
            var response = await _api.CalculateDistanceAsync(request);

            if (!response.IsSuccess)
            { SetError(response.ErrorMessage ?? "Unknown error"); }
            else if (response.Data?.DistanceParsec.HasValue == true)
            { DistanceResult = ToE6(response.Data.DistanceParsec.Value); SetOk(); }
            else { SetError("Invalid response data received."); }
        }

        public async Task CalcKelvinAsync()
        {
            StatusMessage = string.Empty;
            KelvinResult = null;

            if (Celsius is null)
            { SetError("Valid number required."); return; }
            if (Celsius.Value < -273.15d)
            { SetError("Celsius must be >= -273.15."); return; }

            var request = new AstroDataTransfer
            {
                Celsius = Celsius.Value
            };

            // Call the specific API method
            var response = await _api.CalculateKelvinAsync(request);

            if (!response.IsSuccess)
            { SetError(response.ErrorMessage ?? "Unknown error"); }
            else if (response.Data?.Kelvin.HasValue == true)
            { KelvinResult = ToE6(response.Data.Kelvin.Value); SetOk(); }
            else { SetError("Invalid response data received."); }
        }

        public async Task CalcEventHorizonAsync()
        {
            StatusMessage = string.Empty;
            RadiusResult = null;

            if (MassKg is null)
            { SetError("Valid number required."); return; }
            if (MassKg.Value <= 0)
            { SetError("Mass must be a positive number."); return; }

            var request = new AstroDataTransfer
            {
                MassKg = MassKg.Value
            };

            // Call the specific API method
            var response = await _api.CalculateEventHorizonAsync(request);

            if (!response.IsSuccess)
            {
                SetError(response.ErrorMessage ?? "Unknown error");
            }
            else if (response.Data?.RadiusMeters.HasValue == true)
            {
                RadiusResult = ToE6(response.Data.RadiusMeters.Value);
                SetOk();
            }
            else { SetError("Invalid response data received."); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // set property values
        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region settings properties
        private string _selectedLanguage = "en-GB";
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (SetProperty(ref _selectedLanguage, value))
                {
                    LanguageChanged?.Invoke(this, _selectedLanguage);
                }
            }
        }

        private string _selectedTheme = "light";
        public string SelectedTheme { get => _selectedTheme; set { SetProperty(ref _selectedTheme, value); } }

        private string _selectedFontFamily = "Segoe UI";
        public string SelectedFontFamily { get => _selectedFontFamily; set { SetProperty(ref _selectedFontFamily, value); } }

        private double _selectedFontSize = 12;
        public double SelectedFontSize
        {
            get => _selectedFontSize;
            set
            {
                var clamped = Math.Max(10, Math.Min(36, value));
                SetProperty(ref _selectedFontSize, clamped);
            }
        }

        private string _selectedBackgroundColor = "Default";
        public string SelectedBackgroundColor { get => _selectedBackgroundColor; set { SetProperty(ref _selectedBackgroundColor, value); } }

        #endregion
    }
}