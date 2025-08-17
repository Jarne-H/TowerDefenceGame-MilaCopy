using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TowerDefense.Model
{
    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler DestroySelf;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnDestroySelf()
        {
            DestroySelf?.Invoke(this, EventArgs.Empty);
        }
    }
}
