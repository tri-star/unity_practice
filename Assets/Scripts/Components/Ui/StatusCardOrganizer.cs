using System.Collections.Generic;
using ActionSample.Components.Unit;
using ActionSample.Domain.EntityManager;
using ActionSample.Signals;
using UnityEngine;
using Zenject;

namespace ActionSample.Components.Ui
{

    public class StatusCardOrganizer : MonoBehaviour
    {
        [SerializeField]
        private GameObject cardPrefab;
        private List<StatusCard> cards;

        [Inject]
        private SignalBus signalBus;

        void Awake()
        {
            cards = new List<StatusCard>();
            signalBus.Subscribe<UnitBornSignal>(OnUnitBorn);
        }

        void FixedUpdate()
        {

        }


        public void OnUnitBorn(UnitBornSignal signal)
        {
            Add(signal.Unit);
        }

        public void OnAddUnit(EntityAddEvent e)
        {
            Add(e.Unit);
        }

        public void Add(IUnit unit)
        {
            var cardObject = Instantiate(cardPrefab);
            cardObject.transform.SetParent(transform);

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
