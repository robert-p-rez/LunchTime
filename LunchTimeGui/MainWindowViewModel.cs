using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;
using LunchTime;

namespace LunchTimeGui
{
    internal class MainWindowViewModel
    {

        public ObservableCollection<RadTabItem> Panes { get; set; }
        public RadTabItem CurrentDay { get; set; }

        public MainWindowViewModel()
        {
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

        void day_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
           Application.Current.Shutdown();
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
