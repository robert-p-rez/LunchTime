using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Intergraph.UX.WPF.Toolkit;
using Telerik.Windows.Controls;
using LunchTime;

namespace LunchTimeGui
{
    public class MainWindowViewModel : ViewModelBase
    {

        public ObservableCollection<RadTabItem> Panes { get; set; }
        public RadTabItem CurrentDay { get; set; }
        public string Name { get { return System.Security.Principal.WindowsIdentity.GetCurrent().Name.Substring(8);} }

        public MainWindowViewModel()
        {
            this.IsDarkTheme = false;
            this.ProductFamily = ProductFamily.PVElite;
            Panes = new ObservableCollection<RadTabItem>();
            WordDocumentParser parser = new WordDocumentParser();
            parser.ReadFile();
            Collection<DayOfWeek> weekdays = new Collection<DayOfWeek>();
            weekdays.Add(DayOfWeek.Monday);
            weekdays.Add(DayOfWeek.Tuesday);
            weekdays.Add(DayOfWeek.Wednesday);
            weekdays.Add(DayOfWeek.Thursday);
            weekdays.Add(DayOfWeek.Friday);
            foreach (var dayOfWeek in weekdays)
            {
                int newLineCounter = -1;
                RadTabItem day = new RadTabItem();
                TextBox test = new TextBox();
                
                foreach (var line in parser.GetDaysMenu(dayOfWeek))
                {
                    if (line != dayOfWeek.ToString().ToUpper())
                    {
                        day.Content += Tabs(newLineCounter) + line + "\n";
                    }
                    newLineCounter++;
                    if (newLineCounter == 3)
                    {
                        day.Content += "\n";
                        newLineCounter = 0;
                    }
                }
                day.Header = dayOfWeek.ToString();
                
                Panes.Add(day);

                if (DateTime.Now.DayOfWeek == dayOfWeek)
                {
                    CurrentDay = day;
                }
            }
        }

        private bool isDarkTheme;
        public bool IsDarkTheme
        {
            get
            {
                return isDarkTheme;
            }
            set
            {
                if (isDarkTheme != value)
                {
                    isDarkTheme = value;
                    OnPropertyChanged(() => this.IsDarkTheme);
                    SapphirePalette.LoadPreset(isDarkTheme ? ThemeVariation.Dark : ThemeVariation.Light);
                }
            }
        }

        private ProductFamily productFamily;
        public ProductFamily ProductFamily
        {
            get
            {
                return productFamily;
            }
            set
            {
                if (productFamily != value)
                {
                    productFamily = value;
                    OnPropertyChanged(() => this.ProductFamily);
                    SapphireBrandingPalette.LoadPreset(this.IsDarkTheme ? ThemeVariation.Dark : ThemeVariation.Light, productFamily);
                }
            }
        }

        private string Tabs(int number)
        {
            string output = "";
            for (int i = 0; i < number; i++)
            {
                output += "    ";
            }
            return output;
        }
    }
}
