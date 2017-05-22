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
    class CountryModel : INotifyPropertyChanged
    {
        #region Properties

        private string _name;

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

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get => _id;
            set
            {
                _id = value;

                OnPropertyChanged();
            }
        }

        private string _flag;

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

        private Visibility _selected;

        public Visibility Selected
        {
            get => _selected;
            set => _selected = value;
        }

        #endregion

        public CountryModel(string id,string name,string code, string group,bool isSelected = false)
        {
            this.Id = id;
            this.Name = name;
            this.Code = code;
            this.Group = group;
            this.Selected = isSelected ? Visibility.Visible : Visibility.Collapsed;

            this.Flag = String.Format("Assets/CountriesFlag/{0}.png", id);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static ObservableCollection<GroupInfoList> GetCountries()
        {
            var json = LoadCountriesJsonFile();

            var countriesCollection =
                JsonConvert.DeserializeObject<List<CountryModel>>(json);

            var col = new ObservableCollection<CountryModel>(countriesCollection);

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

            groups.Move(groups.Count - 1, 0);

            return groups;
        }

        public static ObservableCollection<GroupInfoList> GetCountries(string countryName)
        {
            var json = LoadCountriesJsonFile();

            var countriesCollection =
                JsonConvert.DeserializeObject<List<CountryModel>>(json);

            var col = new ObservableCollection<CountryModel>(countriesCollection.Where(x => x.Name.Contains(countryName)));

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
           

            return groups;
        }

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
