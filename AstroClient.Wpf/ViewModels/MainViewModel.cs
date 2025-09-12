using Astro.Contracts.Dtos;
using AstroClient.Wpf.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AstroClient.Wpf.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        private readonly AstroApiClient _api = new();

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

        public MainViewModel()
        {
            VelocityCommand = new RelayCommand(async () => await CalcVelocityAsync());
            DistanceCommand = new RelayCommand(async () => await CalcDistanceAsync());
            KelvinCommand = new RelayCommand(async () => await CalcKelvinAsync());
            EventHorizonCommand = new RelayCommand(async () => await CalcEventHorizonAsync());
        }

        private static string ToE6(double v) => v.ToString("E6", CultureInfo.InvariantCulture);
        private void SetOk() => StatusMessage = "OK";
        private void SetError(string msg) => StatusMessage = msg;

        private async Task CalcVelocityAsync()
        {
            StatusMessage = string.Empty;
            try
            {
                if (ObservedWavelength is null || RestWavelength is null)
                    throw new ArgumentException("Inputs required.");

                var res = await _api.PostVelocityAsync(new VelocityRequest
                {
                    ObservedWavelength = ObservedWavelength.Value,
                    RestWavelength = RestWavelength.Value
                });

                if (res is null) throw new HttpRequestException("Empty response.");
                VelocityResult = ToE6(res.VelocityMps);
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

                var res = await _api.PostDistanceAsync(new DistanceRequest
                {
                    ParallaxArcseconds = ParallaxArcseconds.Value
                });

                if (res is null) throw new HttpRequestException("Empty response.");
                DistanceResult = ToE6(res.DistanceParsec);
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

                var res = await _api.PostKelvinAsync(new KelvinRequest
                {
                    Celsius = Celsius.Value
                });

                if (res is null) throw new HttpRequestException("Empty response.");
                KelvinResult = ToE6(res.Kelvin);
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

                var res = await _api.PostEventHorizonAsync(new EventHorizonRequest
                {
                    MassKg = MassKg.Value
                });

                if (res is null) throw new HttpRequestException("Empty response.");
                RadiusResult = ToE6(res.RadiusMeters);
                SetOk();
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Settings
        private string _selectedLanguage = "en-GB";
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set { _selectedLanguage = value; OnPropertyChanged(); }
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
            set { _selectedFontSize = value; OnPropertyChanged(); }
        }

        private string _selectedBackgroundColor = "Default";
        public string SelectedBackgroundColor
        {
            get => _selectedBackgroundColor;
            set { _selectedBackgroundColor = value; OnPropertyChanged(); }
        }


    }
}
