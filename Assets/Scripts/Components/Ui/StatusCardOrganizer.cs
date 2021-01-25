using System.Collections.Generic;
using ActionSample.Components.Unit;
using UnityEngine;

namespace ActionSample.Components.Ui
{

    public class StatusCardOrganizer : MonoBehaviour
    {
        [SerializeField]
        private GameObject cardPrefab;
        private List<StatusCard> cards;

        void Awake()
        {
            cards = new List<StatusCard>();
        }

        void FixedUpdate()
        {

        }

        public void Add(IUnit unit)
        {
            var cardObject = Instantiate(cardPrefab);
            cardObject.transform.parent = transform;

            var card = cardObject.GetComponent<StatusCard>();
            card.Init(unit, CalcuratePosition());
            cards.Add(card);
        }


        private Vector2 CalcuratePosition()
        {
            var index = cards.Count;
            return new Vector2((110 * index) + 10.0f, 0);
        }
    }

}