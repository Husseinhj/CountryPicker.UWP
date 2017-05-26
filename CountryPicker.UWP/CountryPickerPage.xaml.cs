using System;
using System.Diagnostics;
using System.Reflection;
using Windows.Foundation.Metadata;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CountryPicker.UWP.Class.Models;

//Hussein.Juybari@gmail.com
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CountryPicker.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class CountryPickerPage : Page
    {
        internal  delegate void SelectedCountryEventHandler(object sender, CountryModel selected);

        internal delegate void BackButtonPressedEventHandler(object sender);

        /// <summary>
        /// Event fire when user click country
        /// </summary>
        internal static event SelectedCountryEventHandler SelectedCountry;

        /// <summary>
        /// Event fire when Hardware back button pressed or Header back button was clicked.
        /// </summary>
        internal static event BackButtonPressedEventHandler BackButtonClicked; 

        #region Properties

        private string _countryName;

        /// <summary>
        /// Select country with country name
        /// </summary>
        public string CountryName
        {
            get => _countryName;
            set
            {
                _countryName = value;
            }
        }

        /// <summary>
        /// Show picker header
        /// </summary>
        public bool ShowHeader
        {
            get { return BorderHeader.Visibility == Visibility.Visible; }
            set { BorderHeader.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// Text of header
        /// </summary>
        public string Header
        {
            get { return LblTitle.Text; }
            set { LblTitle.Text = value; }
        }

        /// <summary>
        /// Searchbar placeholder
        /// </summary>
        public string SearchBoxPlaceHolder
        {
            get { return TxtSearchBox.PlaceholderText; }
            set { TxtSearchBox.PlaceholderText = value; }
        }

        /// <summary>
        /// Header background
        /// </summary>
        public Brush HeaderBackground
        {
            get { return BorderHeader.Background; }
            set
            {
                BorderHeader.Background = value;
                TxtSearchBox.BorderBrush = value;
            }
        }

        /// <summary>
        /// Searchbar FontFamily
        /// </summary>
        public FontFamily SearchBoxFontFamily
        {
            get { return TxtSearchBox.FontFamily; }
            set { TxtSearchBox.FontFamily = value; }
        }

        /// <summary>
        /// Header FontFamily
        /// </summary>
        public FontFamily HeaderFontFamily
        {
            get { return LblTitle.FontFamily; }
            set { LblTitle.FontFamily = value; }
        }

        public bool ShowBackButton
        {
            get {return BtnBackButton.Visibility == Visibility.Visible;}
            set { BtnBackButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        public string BackButtonText
        {
            get { return BtnBackButton.Content?.ToString(); }
            set { BtnBackButton.Content = value; }
        }
        #endregion

        internal CountryPickerPage()
        {
            this.InitializeComponent();

            Loading += OnLoading;
            Loaded += OnLoaded;
        }

        internal CountryPickerPage(string countryName)
        {
            this.InitializeComponent();

            CountryName = countryName;
            
            Loading += OnLoading;
            Loaded += OnLoaded;
        }

        #region Private event methods

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                CountryListView.SelectedIndex = CountryModel.GetCountryModelIndex(_countryName);
                CountryListView.ScrollIntoView(CountryListView.SelectedItem);
            });
        }

        private void OnLoading(FrameworkElement sender, object args)
        {
            LoadData(CountryName);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is InitializeModel)
            {
                var model = e.Parameter as InitializeModel;
                InitializeProperties(model);
            }

            if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonsOnBackPressed;
            else
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            BackButtonClicked?.Invoke(this);
        }

        private void HardwareButtonsOnBackPressed(object sender, BackPressedEventArgs backPressedEventArgs)
        {
            BackButtonClicked?.Invoke(this);
        }

        private void BtnBackButton_OnClick(object sender, RoutedEventArgs e)
        {
            BackButtonClicked?.Invoke(this);
        }

        private void CountryListViewOnItemClick(object sender, ItemClickEventArgs itemClickEventArgs)
        {
            //if (CountryListView.SelectedIndex != -1)
            {
                var model = itemClickEventArgs.ClickedItem as CountryModel;
                if (SelectedCountry != null)
                    SelectedCountry.Invoke(CountryListView, model);
            }
            
        }

        private void SearchBox_OnQueryChanged(SearchBox sender, SearchBoxQueryChangedEventArgs args)
        {
            CountryVM.Source = CountryModel.GetCountries(args.QueryText);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Load data and show them
        /// </summary>
        /// <param name="countryName"></param>
        private void LoadData(string countryName = "")
        {
            CountryVM.Source = CountryModel.GetCountries();

            CountryListView.ItemClick += CountryListViewOnItemClick;
            CountryListView.IsItemClickEnabled = true;

            _countryName = countryName;
        }

        /// <summary>
        /// Initialize visual parameters.
        /// </summary>
        /// <param name="initialize">Initialize model</param>
        public void InitializeProperties(InitializeModel initialize)
        {
            if (initialize.SearchBoxFontFamily != null) SearchBoxFontFamily = initialize.SearchBoxFontFamily;
            if (!string.IsNullOrEmpty(initialize.SearchBoxPlaceHolder) ) SearchBoxPlaceHolder = initialize.SearchBoxPlaceHolder;

            if (!string.IsNullOrEmpty(initialize.Header)) Header = initialize.Header;
            ShowHeader = initialize.ShowHeader;
            if (initialize.HeaderBackground != null) HeaderBackground = initialize.HeaderBackground;
            if (initialize.HeaderFontFamily != null) HeaderFontFamily = initialize.HeaderFontFamily;

            if (!string.IsNullOrEmpty(initialize.BackButtonText)) BackButtonText = initialize.BackButtonText;
            ShowBackButton = initialize.ShowBackButton;
        }

        #endregion

    }
}
