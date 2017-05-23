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
using CountryPicker.UWP.Class.Helper;
using CountryPicker.UWP.Class.Models;

namespace CountryPicker.UWP.Class
{
    public class PickerDialog
    {
        public event CountryPickerPage.SelectedCountryEventHandler SelectedCountry;

        private ContentDialog _dialog;

        private string _countryName;
        public string CountryName
        {
            get => _countryName;
            set => _countryName = value;
        }

        private Style _style;

        public Style Style
        {
            get => _style;
            set => _style = value;
        }

        public PickerDialog()
        {
            Init();
        }

        public PickerDialog(string countryName)
        {
            _countryName = countryName;
            Init();
        }

        private void Init()
        {
            CountryPickerPage.SelectedCountry += delegate(object sender, CountryModel selected)
            {
                _countryName = selected.Name;

                if (SelectedCountry != null) SelectedCountry.Invoke(sender, selected);

                Hide();
            };
            if (!UniversalHelper.UniversalClass.IsPhone())
            {
                _dialog = new ContentDialog()
                {
                    VerticalContentAlignment = VerticalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    Style = Style
                };

                Frame frame = new Frame();
                var countryPage = new CountryPickerPage(_countryName);


                frame.Content = countryPage;

                _dialog.SizeChanged += (sender, args) =>
                {
                    var mobile = UniversalHelper.UniversalClass.IsMobileWithResponsiveDesign();

                    if (mobile)
                    {
                        countryPage.Width = args.NewSize.Width-60;
                        countryPage.Height = args.NewSize.Height-80;
                    }
                    else
                    {
                        countryPage.Width = 360;
                        countryPage.Height = 600;
                    }
                };

                _dialog.Content = frame;
            }
        }

        public async void Show()
        {
            if (_dialog != null)
                await _dialog.ShowAsync();
            else
            {
                ChangeColor();
                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame != null) rootFrame.Navigate(typeof(CountryPickerPage), _countryName);
            }
        }

        public void Hide()
        {
            if (_dialog != null) _dialog.Hide();
            else
            {
                Frame rootFrame = Window.Current.Content as Frame;

                if (rootFrame != null) rootFrame.GoBack();
            }
        }

        /// <summary>
        /// Change Title bar in Windows 10 and change Status bar in Windows Phone device color.
        /// </summary>
        public static void ChangeColor()
        {
            if (UniversalHelper.UniversalClass.IsPhone())
            {
                StatusBar.GetForCurrentView().BackgroundOpacity = 1;
                StatusBar.GetForCurrentView().BackgroundColor = Color.FromArgb(1,2, 169, 79);
                StatusBar.GetForCurrentView().ForegroundColor = Colors.White;
            }
        }
    }
}
