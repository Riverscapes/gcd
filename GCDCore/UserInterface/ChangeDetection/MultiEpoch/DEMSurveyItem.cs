using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GCDCore.UserInterface.ChangeDetection.MultiEpoch
{
    public class DEMSurveyItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; //from https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/raise-change-notifications--bindingsource

        private bool _IsActive = true;

        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                Console.WriteLine(DEMName + " IsActive change to " + value);
                _IsActive = value;
                NotifyPropertyChanged();
            }
        }
        public readonly DEMSurvey DEM;
        public readonly ErrorSurface ErrorSurf;

        public string DEMName { get { return DEM.Name; } }
        public string ErrorName { get { return ErrorSurf.Name; } }

        public readonly naru.ui.SortableBindingList<ErrorSurface> ErrorSurfaces;

        public DEMSurveyItem(DEMSurvey dem, ErrorSurface err)
        {
            DEM = dem;
            ErrorSurf = err;

            ErrorSurfaces = new naru.ui.SortableBindingList<ErrorSurface>();
            foreach(ErrorSurface es in dem.ErrorSurfaces)
            {
                ErrorSurfaces.Add(es);
            }
            //IsActive = false;
        }

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
