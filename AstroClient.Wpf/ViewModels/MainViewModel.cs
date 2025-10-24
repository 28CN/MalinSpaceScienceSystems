using AstroClient.Wpf.Models; // Use the client's Models namespace
using AstroClient.Wpf.Services;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AstroClient.Wpf.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        private readonly AstroApiClient _api = new AstroApiClient(); // API client instance.

        // Input and Output Properties for UI Binding
        public double? ObservedWavelength { get; set; }
        public double? RestWavelength { get; set; }
        private string? _velocityResult;
        public string? VelocityResult { get => _velocityResult; set { SetProperty(ref _velocityResult, value); } } 
        public RelayCommand VelocityCommand { get; }

        public double? ParallaxArcseconds { get; set; }
        private string? _distanceResult;
        public string? DistanceResult { get => _distanceResult; set { SetProperty(ref _distanceResult, value); } }
        public RelayCommand DistanceCommand { get; }

        public double? Celsius { get; set; }
        private string? _kelvinResult;
        public string? KelvinResult { get => _kelvinResult; set { SetProperty(ref _kelvinResult, value); } }
        public RelayCommand KelvinCommand { get; }

        public double? MassKg { get; set; }
        private string? _radiusResult;
        public string? RadiusResult { get => _radiusResult; set { SetProperty(ref _radiusResult, value); } }
        public RelayCommand EventHorizonCommand { get; }

        // Status Message
        private string? _status;
        public string? StatusMessage { get => _status; private set { SetProperty(ref _status, value); } } // Make setter private

        // Event for language change notification.
        public event EventHandler<string>? LanguageChanged;

        // Constructor initializes commands.
        public MainViewModel()
        {
            VelocityCommand = new RelayCommand(async () => await CalcVelocityAsync());
            DistanceCommand = new RelayCommand(async () => await CalcDistanceAsync());
            KelvinCommand = new RelayCommand(async () => await CalcKelvinAsync());
            EventHorizonCommand = new RelayCommand(async () => await CalcEventHorizonAsync());
        }

        // Helper to format results to scientific notation.
        private static string ToE6(double v) => v.ToString("E6", CultureInfo.InvariantCulture);
        // Helper to set status message to OK.
        private void SetOk() => StatusMessage = "OK";
        // set status message to an error.
        private void SetError(string msg) => StatusMessage = msg;

        // Calculation Methods

        private async Task CalcVelocityAsync()
        {
            StatusMessage = string.Empty; // Clear status.
            VelocityResult = null; // Clear previous result.

            // input validation.
            if (ObservedWavelength is null || RestWavelength is null)
            { SetError("Valid number required."); return; }
            if (ObservedWavelength.Value <= 0 || RestWavelength.Value <= 0)
            { SetError("Wavelengths must be positive numbers."); return; }

            // Create the request DTO.
            var request = new AstroDataTransfer
            {
                Type = CalculationType.Velocity,
                ObservedWavelength = ObservedWavelength.Value,
                RestWavelength = RestWavelength.Value
            };

            // Call the unified API method.
            var response = await _api.CalculateAsync(request);

            // Process the response DTO.
            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                SetError(response.ErrorMessage); // Display error from server or API client.
            }
            else if (response.VelocityMps.HasValue)
            {
                VelocityResult = ToE6(response.VelocityMps.Value); // Display result.
                SetOk();
            }
            else { SetError("Invalid response received."); } // Should not happen with current server logic
        }

        private async Task CalcDistanceAsync()
        {
            StatusMessage = string.Empty;
            DistanceResult = null;

            if (ParallaxArcseconds is null)
            { SetError("Valid number required."); return; }
            if (ParallaxArcseconds.Value <= 0)
            { SetError("Parallax must be a positive number."); return; }

            var request = new AstroDataTransfer
            {
                Type = CalculationType.Distance,
                ParallaxArcseconds = ParallaxArcseconds.Value
            };

            var response = await _api.CalculateAsync(request);

            if (!string.IsNullOrEmpty(response.ErrorMessage))
            { SetError(response.ErrorMessage); }
            else if (response.DistanceParsec.HasValue)
            { DistanceResult = ToE6(response.DistanceParsec.Value); SetOk(); }
            else { SetError("Invalid response received."); }
        }

        private async Task CalcKelvinAsync()
        {
            StatusMessage = string.Empty;
            KelvinResult = null;

            if (Celsius is null)
            { SetError("Valid number required."); return; }
            if (Celsius.Value < -273.15d)
            { SetError("Celsius must be >= -273.15."); return; }

            var request = new AstroDataTransfer
            {
                Type = CalculationType.Kelvin,
                Celsius = Celsius.Value
            };

            var response = await _api.CalculateAsync(request);

            if (!string.IsNullOrEmpty(response.ErrorMessage))
            { SetError(response.ErrorMessage); }
            else if (response.Kelvin.HasValue)
            { KelvinResult = ToE6(response.Kelvin.Value); SetOk(); }
            else { SetError("Invalid response received."); }
        }

        private async Task CalcEventHorizonAsync()
        {
            StatusMessage = string.Empty;
            RadiusResult = null;

            if (MassKg is null)
            { SetError("Valid number required."); return; }
            if (MassKg.Value <= 0)
            { SetError("Mass must be a positive number."); return; }

            var request = new AstroDataTransfer
            {
                Type = CalculationType.EventHorizon,
                MassKg = MassKg.Value
            };

            var response = await _api.CalculateAsync(request);

            if (!string.IsNullOrEmpty(response.ErrorMessage))
            { SetError(response.ErrorMessage); }
            else if (response.RadiusMeters.HasValue)
            { RadiusResult = ToE6(response.RadiusMeters.Value); SetOk(); }
            else { SetError("Invalid response received."); }
        }

        // INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        // setting properties
        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false; // Value hasn't changed.
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string _selectedLanguage = "en-GB";
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (SetProperty(ref _selectedLanguage, value))
                {
                    LanguageChanged?.Invoke(this, _selectedLanguage); // Raise event only if changed.
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
                var clamped = Math.Max(10, Math.Min(36, value)); // Clamp value between 10 and 36.
                SetProperty(ref _selectedFontSize, clamped); // Use SetProperty.
            }
        }

        private string _selectedBackgroundColor = "Default";
        public string SelectedBackgroundColor { get => _selectedBackgroundColor; set { SetProperty(ref _selectedBackgroundColor, value); } }
    }
}