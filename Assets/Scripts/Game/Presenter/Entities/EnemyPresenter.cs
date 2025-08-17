
using System.ComponentModel;
using System;
using TowerDefense.Model;
using UnityEngine;
using System.Collections;

namespace TowerDefense.Presenter
{
    public class EnemyPresenter : PresenterBase<EnemyModel>
    {
        HexPositionConverter HexPositionConverter = new HexPositionConverter();

        [SerializeField] 
        private Animator _animator;

        public bool IsActive { get; private set; } = true;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        protected override void Property_Changed(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EnemyModel.NextPosition))
            {
                StartCoroutine(MoveAnimation());
                var nextPos = HexPositionConverter.ConvertCoordinateToVector3(Model.NextPosition);
                transform.LookAt(nextPos);
            }
        }

        IEnumerator MoveAnimation()
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = HexPositionConverter.ConvertCoordinateToVector3(Model.NextPosition);

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * 0.5f; //bad but i dunno how to make them slower another way
                Vector3 pos = Vector3.Lerp(startPosition, endPosition, t);
                transform.position = pos;
                yield return null;
            }

            transform.position = endPosition;
            Model.CurrentPosition = Model.NextPosition;
        }

        protected override void Destroy_Self(object sender, EventArgs e)
        {
            IsActive = false;
            gameObject.SetActive(false);
        }

        public void Attack_Goal(object sender, EventArgs e)
        {
            _animator.SetTrigger("Attack");
        }
    }
}
