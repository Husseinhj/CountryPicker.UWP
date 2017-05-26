using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace CountryPicker.UWP.Class.Models
{
    public abstract class InitializeModel
    {
        /// <summary>
        /// Show header color in titlebar (Windows desktop) and Statusbar (Windows mobile)
        /// </summary>
        public bool IsUseColorInStatusBarOrTitleBar { get; set; } = true;

        /// <summary>
        /// Country name
        /// </summary>
        public string CountryName { get; set; } = "ایران";

        /// <summary>
        /// Show picker header
        /// </summary>
        public bool ShowHeader { get; set; } = true;

        /// <summary>
        /// Text of header
        /// </summary>
        public string Header { get; set; } = "Countries";

        /// <summary>
        /// Searchbar placeholder
        /// </summary>
        public string SearchBoxPlaceHolder { get; set; } = "جستجو";

        /// <summary>
        /// Header background
        /// </summary>
        public Brush HeaderBackground { get; set; } = new SolidColorBrush(Color.FromArgb(255,2,169,79));

        /// <summary>
        /// Searchbar FontFamily
        /// </summary>
        public FontFamily SearchBoxFontFamily { get; set; } = new FontFamily("ms-appx:///CountryPicker.UWP/Fonts/IRAN Sans.ttf#IRANSans");

        /// <summary>
        /// Header FontFamily
        /// </summary>
        public FontFamily HeaderFontFamily { get; set; } = new FontFamily("ms-appx:///CountryPicker.UWP/Fonts/IRAN Sans.ttf#IRANSans");

        /// <summary>
        /// Show back button or not
        /// </summary>
        public bool ShowBackButton { get; set; } = true;

        /// <summary>
        /// Back button title
        /// </summary>
        public string BackButtonText { get; set; } = "";


        protected InitializeModel(string countryName, string header, string searchBoxPlaceHolder, bool showBackButton, bool showHeader, Brush headerBackground, FontFamily searchBoxFontFamily, FontFamily headerFontFamily, string backButtonText)
        {
            Header = header;
            SearchBoxPlaceHolder = searchBoxPlaceHolder;
            ShowBackButton = showBackButton;
            ShowHeader = showHeader;
            HeaderBackground = headerBackground;
            SearchBoxFontFamily = searchBoxFontFamily;
            HeaderFontFamily = headerFontFamily;
            BackButtonText = backButtonText;
            CountryName = countryName;
        }

        protected InitializeModel(string countryName, string header, string searchBoxPlaceHolder, bool showBackButton, bool showHeader)
        {
            Header = header;
            SearchBoxPlaceHolder = searchBoxPlaceHolder;
            ShowBackButton = showBackButton;
            ShowHeader = showHeader;
            CountryName = countryName;
        }

        protected InitializeModel()
        {
        }
    }
}
