using System;
using System.ComponentModel;
using TowerDefense.Model;
using UnityEngine;

namespace TowerDefense.Presenter
{
    public abstract class PresenterBase<TModel> : MonoBehaviour where TModel : ModelBase
    {
        private TModel _model;

        public virtual TModel Model
        {

            get { return _model; }
            set
            {
                if (_model == value) return;
                if (_model != null)
                {
                    _model.PropertyChanged -= Property_Changed;
                    _model.DestroySelf -= Destroy_Self;
                }
                _model = value;
                if (_model != null)
                {
                    _model.PropertyChanged += Property_Changed;
                    _model.DestroySelf += Destroy_Self;
                }
            }
        }

        protected abstract void Property_Changed(object sender, PropertyChangedEventArgs e);

        protected abstract void Destroy_Self(object sender, EventArgs e);
    }
}
