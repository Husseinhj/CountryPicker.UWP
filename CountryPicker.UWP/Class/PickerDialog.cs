using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using CountryPicker.UWP.Class.Models;

namespace CountryPicker.UWP.Class
{
    public class PickerDialog : InitializeModel
    {
        public delegate void SelectedCountryEventHandler(object sender, CountryModel selected);

        public delegate void BackButtonPressedEventHandler(object sender);

        /// <summary>
        /// Event fire when user click country
        /// </summary>
        public event SelectedCountryEventHandler SelectedCountry;

        /// <summary>
        /// Event fire when Hardware back button pressed or Header back button was clicked.
        /// </summary>
        public event BackButtonPressedEventHandler BackButtonClicked;

        private ContentDialog _dialog;

        #region Properties

        private Style _style;

        /// <summary>
        /// Style for Content dialog
        /// </summary>
        public Style Style
        {
            get => _style;
            set => _style = value;
        }

        #endregion

        public PickerDialog()
        {
            SetEvents();
        }

        /// <summary>
        /// Contractor with select country
        /// </summary>
        /// <param name="countryName"></param>
        public PickerDialog(string countryName)
        {
            CountryName = countryName;
            SetEvents();
        }

        #region Private methods

        /// <summary>
        /// Set county picker events for fire class events
        /// </summary>
        private void SetEvents()
        {
            CountryPickerPage.ClearSelectedEvents();
            CountryPickerPage.SelectedCountryEvent += CountryPickerPageOnSelectedCountry;

            CountryPickerPage.ClearBackEvents();
            CountryPickerPage.BackButtonClickedEvent += CountryPickerPageOnBackButtonClicked;
        }

        private void CountryPickerPageOnBackButtonClicked(object sender)
        {
            BackButtonClicked?.Invoke(this);
            Hide();
        }

        private void CountryPickerPageOnSelectedCountry(object o, CountryModel selected)
        {
            CountryName = selected.Name;

            if (SelectedCountry != null) SelectedCountry.Invoke(o, selected);

            Hide();
        }

        private void Init()
        {
            if (!IsPhone())
            {
                _dialog = new ContentDialog()
                {
                    VerticalContentAlignment = VerticalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    Background = new SolidColorBrush(Color.FromArgb(20,50,50,50)),
                    FullSizeDesired = true,

                };

                var frame = new Frame();

                var countryPage = new CountryPickerPage(CountryName);
                countryPage.InitializeProperties(this);

                frame.Content = countryPage;

                _dialog.Content = frame;
            }
        }

        private bool IsPhone()
        {
            return ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar");
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Show picker dialog
        /// </summary>
        public async void Show()
        {
            ChangeColor();
            Init();

            if (_dialog != null)
            {
                if (Style != null) _dialog.Style = Style;

                await _dialog.ShowAsync();
            }
            else
            {
                Frame rootFrame = Window.Current.Content as Frame;

                rootFrame?.Navigate(typeof(CountryPickerPage), this, new SuppressNavigationTransitionInfo());
            }
        }

        /// <summary>
        /// Hide Picker dialog
        /// </summary>
        public void Hide()
        {
            if (_dialog != null)
            {
                _dialog.Hide();
                _dialog = null;
            }
            else
            {
                Frame rootFrame = Window.Current.Content as Frame;

                if (rootFrame != null)
                {
                    if(rootFrame.CanGoBack)
                        rootFrame.GoBack(new SlideNavigationTransitionInfo());
                }
            }
        }

        /// <summary>
        /// Change Title bar in Windows 10 and change Status bar in Windows Phone device color.
        /// </summary>
        public void ChangeColor()
        {
            if (IsUseColorInStatusBarOrTitleBar)
            {
                if (IsPhone())
                {
                    StatusBar.GetForCurrentView().BackgroundOpacity = 1;

                    StatusBar.GetForCurrentView().BackgroundColor = (HeaderBackground as SolidColorBrush)?.Color ?? Color.FromArgb(1, 2, 169, 79);

                    StatusBar.GetForCurrentView().ForegroundColor = Colors.White;
                }
                else
                {
                    var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                    titleBar.BackgroundColor = (HeaderBackground as SolidColorBrush)?.Color ?? Color.FromArgb(1, 2, 169, 79);
                    titleBar.ButtonBackgroundColor = titleBar.BackgroundColor;
                    titleBar.ForegroundColor = Colors.White;
                }
            }
        }

        #endregion
    }
}
