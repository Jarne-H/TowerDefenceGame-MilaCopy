using Codice.CM.Common;
using System;
using System.Collections;
using System.ComponentModel;
using TowerDefense.Model;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace TowerDefense.Presenter
{
    public class GoalPresenter : PresenterBase<GoalModel>
    {
        protected override void Destroy_Self(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void Property_Changed(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GoalModel.TakeDamage))
            {
                StartCoroutine(DamageAnimation());
                Debug.Log($"Goal damaged! Health: {Model.Health}");
            }
        }

        IEnumerator DamageAnimation()
        {
            var speed = .2f; //how long it takes (*2 here because it's done twice)
            var scaleFactor = 0.9f;

            Vector3 originalScale = transform.localScale;
            Vector3 newScale = originalScale * scaleFactor;
            float startTime = Time.time;

            while (Time.time <= startTime + speed)
            {
                
                transform.localScale = Vector3.Lerp(originalScale, newScale, (Time.time - startTime) / speed);
                yield return null;
            }

            startTime = Time.time;
            while (Time.time <= startTime + speed)
            {

                transform.localScale = Vector3.Lerp(newScale, originalScale, (Time.time - startTime) / speed);
                yield return null;
            }
        }
    }
}
