using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using CountryPicker.UWP.Annotations;
using CountryPicker.UWP.Data;
using Newtonsoft.Json;

namespace CountryPicker.UWP.Class.Models
{
    public class CountryModel : INotifyPropertyChanged
    {
        #region Properties

        private string _name;

        /// <summary>
        /// Country name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value; 
                OnPropertyChanged();
            }
        }

        private string _id;

        /// <summary>
        /// Country identifier
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                this.Flag = String.Format("ms-appx:///CountryPicker.UWP/Assets/CountriesFlag/{0}.png", _id);
                OnPropertyChanged();
            }
        }

        private string _flag;

        /// <summary>
        /// Country flag path
        /// </summary>
        [JsonProperty(PropertyName = "flag")]
        public string Flag
        {
            get => _flag;
            set
            {
                _flag = value;
                OnPropertyChanged();
            }
        }

        private string _code;

        /// <summary>
        /// Country code number
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged();
            }
        }

        private string _group;

        /// <summary>
        /// Country in which alphbatic group
        /// </summary>
        [JsonProperty(PropertyName = "group")]
        public string Group
        {
            get => _group;
            set
            {
                _group = value;
                OnPropertyChanged();
            }
        }

        private Visibility _selected = Visibility.Collapsed;

        /// <summary>
        /// Selected or not
        /// </summary>
        public Visibility Selected 
        {
            get => _selected;
            set => _selected = value;
        }

        /// <summary>
        /// Get collection of countries with filter
        /// </summary>
        public static ObservableCollection<GroupInfoList> FilteredCollection { get; set; }

        #endregion

        /// <summary>
        /// Create country model constractor
        /// </summary>
        /// <param name="id">Country identifier</param>
        /// <param name="name">Country name</param>
        /// <param name="code">Country code number</param>
        /// <param name="group">Country is which alpbatic group</param>
        /// <param name="isSelected">Country was selected or not</param>
        public CountryModel(string id,string name,string code, string group,bool isSelected = false)
        {
            this.Id = id;
            this.Name = name;
            this.Code = code;
            this.Group = group;
            this.Selected = isSelected ? Visibility.Visible : Visibility.Collapsed;

            this.Flag = String.Format("ms-appx:///CountryPicker.UWP/Assets/CountriesFlag/{0}.png", id);
        }

        public CountryModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Get collection of all countries.
        /// </summary>
        /// <returns>Collection</returns>
        public static ObservableCollection<GroupInfoList> GetCountries()
        {
            return GetCountries("");
        }

        /// <summary>
        /// Get collection of countries with filter country name
        /// </summary>
        /// <param name="countryName">Country name</param>
        /// <returns>Collection of countries</returns>
        public static ObservableCollection<GroupInfoList> GetCountries(string countryName)
        {
            var json = LoadCountriesJsonFile();

            var countriesCollection =
                JsonConvert.DeserializeObject<List<CountryModel>>(json);

            var col = new ObservableCollection<CountryModel>(countriesCollection.Where(x => x.Name.ToLower().Contains(countryName.ToLower())));

            ObservableCollection<GroupInfoList> groups = new ObservableCollection<GroupInfoList>();

            var query = from item in col
                group item by item.Group into g
                orderby g.Key
                select new { GroupName = g.Key, Items = g };

            foreach (var g in query)
            {
                GroupInfoList info = new GroupInfoList();
                info.Key = g.GroupName;
                foreach (var item in g.Items)
                {
                    info.Add(item);
                }
                groups.Add(info);
            }

            if (groups.Count > 0)
            {
                if (groups[groups.Count - 1].Key.ToString().Contains("ا"))
                {
                    groups.Move(groups.Count - 1, 0); 
                }
            }

            FilteredCollection = groups;

            return groups;
        }

        /// <summary>
        /// Get country model index in collection with Country name.
        /// </summary>
        /// <param name="countryName">Country name</param>
        /// <returns>Index of country in countries collection</returns>
        public static int GetCountryModelIndex(string countryName)
        {
            var json = LoadCountriesJsonFile();

            var countriesCollection =
                JsonConvert.DeserializeObject<List<CountryModel>>(json).OrderBy(x => x.Group).ToList();

            var last = countriesCollection[countriesCollection.Count - 1];
            countriesCollection.Insert(0, last);
            countriesCollection.RemoveAt(countriesCollection.Count - 1);

            for (var index = 0; index < countriesCollection.Count; index++)
            {
                var countryModel = countriesCollection[index];
                if (countryName != null && countryModel.Name.ToLower() == countryName.ToLower())
                {
                    return index;
                }
            }
            return -1;
        }

        /// <summary>
        /// Load list of countries
        /// </summary>
        /// <returns></returns>
        private static string LoadCountriesJsonFile()
        {
            try
            {
                return JsonData.Json;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
            return String.Empty;
        }

    }
}
