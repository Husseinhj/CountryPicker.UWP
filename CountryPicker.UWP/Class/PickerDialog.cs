using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using CountryPicker.UWP.Class.Helper;
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
            Init();
        }

        /// <summary>
        /// Contractor with select country
        /// </summary>
        /// <param name="countryName"></param>
        public PickerDialog(string countryName)
        {
            CountryName = countryName;
            
            Init();
        }

        /// <summary>
        /// Set county picker events for fire class events
        /// </summary>
        private void SetEvents()
        {
            CountryPickerPage.SelectedCountry += delegate (object sender, CountryModel selected)
            {
                CountryName = selected.Name;

                if (SelectedCountry != null) SelectedCountry.Invoke(sender, selected);

                Hide();
            };

            CountryPickerPage.BackButtonClicked += delegate (object sender)
            {
                BackButtonClicked?.Invoke(this);

                Hide();
            };
        }

        private CountryPickerPage _countryPage;
        private Frame _frame;
        private void Init()
        {
            SetEvents();

            if (!UniversalHelper.UniversalClass.IsPhone())
            {
                _dialog = new ContentDialog()
                {
                    VerticalContentAlignment = VerticalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    Style = Style
                };

                _frame = new Frame();

                _dialog.SizeChanged += (sender, args) =>
                {
                    var mobile = UniversalHelper.UniversalClass.IsMobileWithResponsiveDesign();

                    if (_countryPage != null)
                    {
                        if (mobile)
                        {
                            _countryPage.Width = args.NewSize.Width-60;
                            _countryPage.Height = args.NewSize.Height-80;
                        }
                        else
                        {
                            _countryPage.Width = 360;
                            _countryPage.Height = 600;
                        }
                    }
                };

                _dialog.Content = _frame;
            }
        }

        /// <summary>
        /// Show picker dialog
        /// </summary>
        public async Task ShowAsync()
        {
            ChangeColor();

            if (_dialog != null)
            {
                _countryPage = new CountryPickerPage(CountryName);
                _countryPage.InitializeProperties(this);
                _frame.Content = _countryPage;
                await _dialog.ShowAsync();
            }
            else
            {
                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame != null)
                    rootFrame.Navigate(typeof(CountryPickerPage), this, new SuppressNavigationTransitionInfo());
            }
        }

        /// <summary>
        /// Hide Picker dialog
        /// </summary>
        public void Hide()
        {
            if (_dialog != null) _dialog.Hide();
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
                if (UniversalHelper.UniversalClass.IsPhone())
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
    }
}
