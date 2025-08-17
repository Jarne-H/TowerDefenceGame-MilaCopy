using System;
using System.Collections;
using System.ComponentModel;
using TowerDefense.Model;
using UnityEngine;

namespace TowerDefense.Presenter
{
    public class TowerPresenter : PresenterBase<TowerModel>
    {
        [SerializeField]
        private Material _attackMaterial;

        protected override void Destroy_Self(object sender, EventArgs e)
        {
            Destroy(gameObject);
        }

        protected override void Property_Changed(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TowerModel.ShotsFired))
            {
                StartCoroutine(ShootAnimation());
            }
        }

        IEnumerator ShootAnimation()
        {
            var renderer = GetComponentInChildren<MeshRenderer>();
            var startMaterial = renderer.material;

            //change material
            renderer.material = _attackMaterial;
            yield return new WaitForSeconds(.5f);

            //change back
            renderer.material = startMaterial;
        }
    }
}
