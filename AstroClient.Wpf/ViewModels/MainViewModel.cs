using Astro.Contracts.Dtos;
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
        private readonly AstroApiClient _api = new AstroApiClient(); 

        // Velocity
        public double? ObservedWavelength { get; set; }
        public double? RestWavelength { get; set; }
        private string? _velocityResult;
        public string? VelocityResult { get => _velocityResult; set { _velocityResult = value; OnPropertyChanged(); } }
        public RelayCommand VelocityCommand { get; }

        // Distance
        public double? ParallaxArcseconds { get; set; }
        private string? _distanceResult;
        public string? DistanceResult { get => _distanceResult; set { _distanceResult = value; OnPropertyChanged(); } }
        public RelayCommand DistanceCommand { get; }

        // Kelvin
        public double? Celsius { get; set; }
        private string? _kelvinResult;
        public string? KelvinResult { get => _kelvinResult; set { _kelvinResult = value; OnPropertyChanged(); } }
        public RelayCommand KelvinCommand { get; }

        // Event horizon
        public double? MassKg { get; set; }
        private string? _radiusResult;
        public string? RadiusResult { get => _radiusResult; set { _radiusResult = value; OnPropertyChanged(); } }
        public RelayCommand EventHorizonCommand { get; }

        // Status
        private string? _status;
        public string? StatusMessage { get => _status; set { _status = value; OnPropertyChanged(); } }

        // New event specifically for language changes.
        public event EventHandler<string>? LanguageChanged;

        public MainViewModel()
        {
            // Initialize the calculation commands.
            VelocityCommand = new RelayCommand(async () => await CalcVelocityAsync());
            DistanceCommand = new RelayCommand(async () => await CalcDistanceAsync());
            KelvinCommand = new RelayCommand(async () => await CalcKelvinAsync());
            EventHorizonCommand = new RelayCommand(async () => await CalcEventHorizonAsync());
        }

        // format results in E6 format
        private static string ToE6(double v) => v.ToString("E6", CultureInfo.InvariantCulture); // use invariant for consistent decimal point (".")
        private void SetOk() => StatusMessage = "OK";
        private void SetError(string msg) => StatusMessage = msg;

        private async Task CalcVelocityAsync()
        {
            StatusMessage = string.Empty; //clear status message
            try
            {
                if (ObservedWavelength is null || RestWavelength is null)
                    throw new ArgumentException("Inputs required.");

                // call the api to get calculation result.
                var value = await _api.ComputeVelocityAsync(new VelocityRequest
                {
                    ObservedWavelength = ObservedWavelength.Value,
                    RestWavelength = RestWavelength.Value
                });

                // display
                VelocityResult = ToE6(value);
                SetOk();
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
            }
        }

        private async Task CalcDistanceAsync()
        {
            StatusMessage = string.Empty;
            try
            {
                if (ParallaxArcseconds is null)
                    throw new ArgumentException("Input required.");

                var value = await _api.ComputeDistanceAsync(new DistanceRequest
                {
                    ParallaxArcseconds = ParallaxArcseconds.Value
                });
                DistanceResult = ToE6(value);
                SetOk();
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
            }
        }

        private async Task CalcKelvinAsync()
        {
            StatusMessage = string.Empty;
            try
            {
                if (Celsius is null)
                    throw new ArgumentException("Input required.");

                var value = await _api.ComputeKelvinAsync(new KelvinRequest
                {
                    Celsius = Celsius.Value
                });
                KelvinResult = ToE6(value);
                SetOk();
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
            }
        }

        private async Task CalcEventHorizonAsync()
        {
            StatusMessage = string.Empty;
            try
            {
                if (MassKg is null)
                    throw new ArgumentException("Input required.");

                var value = await _api.ComputeEventHorizonRadiusAsync(new EventHorizonRequest
                {
                    MassKg = MassKg.Value
                });
                RadiusResult = ToE6(value);
                SetOk();
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
            }
        }

        // to notify UI of property changes
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Settings
        private string _selectedLanguage = "en-GB";
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set 
            {
                if (_selectedLanguage == value) return;
                _selectedLanguage = value;
                OnPropertyChanged();

                // When the language changes, invoke the new event with the new culture name.
                LanguageChanged?.Invoke(this, _selectedLanguage);
            }
        }

        private string _selectedTheme = "light";
        public string SelectedTheme
        {
            get => _selectedTheme;
            set { _selectedTheme = value; OnPropertyChanged(); }
        }

        private string _selectedFontFamily = "Segoe UI";
        public string SelectedFontFamily
        {
            get => _selectedFontFamily;
            set { _selectedFontFamily = value; OnPropertyChanged(); }
        }

        private double _selectedFontSize = 12;
        public double SelectedFontSize
        {
            get => _selectedFontSize;
            set
            {
                var clamped = Math.Max(10, Math.Min(36, value)); // set boundaries 10 - 36
                if (Math.Abs(_selectedFontSize - clamped) < 0.1) return; // if font size not change, do nothing
                _selectedFontSize = clamped;
                OnPropertyChanged();
            }
        }

        private string _selectedBackgroundColor = "Default";
        public string SelectedBackgroundColor
        {
            get => _selectedBackgroundColor;
            set { _selectedBackgroundColor = value; OnPropertyChanged(); }
        }


    }
}
