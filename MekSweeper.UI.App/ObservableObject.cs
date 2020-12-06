using System;
using System.ComponentModel;
using System.Diagnostics;

namespace MekSweeper.UI.App
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName); 
                handler(this, e);
            }
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real, 
            // public, instance property on this object. 
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;
                throw new Exception(msg);
            }
        }

        protected void SetProperty<T>(string propertyName, ref T propertyValue, ref T newValue, params string[] auxiliaryPropertyNames)
        {
            if (propertyValue == null && newValue == null)
            {
                return;
            }

            if (propertyValue != null && newValue != null && propertyValue.Equals(newValue))
            {
                return;
            }

            propertyValue = newValue;
            OnPropertyChanged(propertyName);
            foreach (string auxiliaryPropertyName in auxiliaryPropertyNames ?? new string[0])
            {
                OnPropertyChanged(auxiliaryPropertyName);
            }
        }
    }
}
