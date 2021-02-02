using System.Collections.Generic;
using ActionSample.Components.Unit;
using ActionSample.Domain.EntityManager;
using ActionSample.Signals;
using UnityEngine;
using Zenject;
using System.Linq;

namespace ActionSample.Components.Ui
{

    public class StatusCardOrganizer : MonoBehaviour
    {
        [SerializeField]
        private GameObject cardPrefab;
        private Dictionary<int, StatusCard> cards;

        [Inject]
        private SignalBus signalBus;

        void Awake()
        {
            cards = new Dictionary<int, StatusCard>();
            signalBus.Subscribe<UnitBornSignal>(OnUnitBorn);
            signalBus.Subscribe<UnitKilledSignal>(OnUnitKilled);
        }

        void FixedUpdate()
        {
        }


        public void OnUnitBorn(UnitBornSignal signal)
        {
            Add(signal.Unit);
        }

        public void OnUnitKilled(UnitKilledSignal signal)
        {
            int key = signal.Unit.GetHashCode();
            cards[key].Close();
        }

        public void OnStatusCardClosed(StatusCard.StatusCardCloseEvent e)
        {
            cards.Remove(e.Unit.GetHashCode());
            RearrangeCards();
        }

        public void Add(IUnit unit)
        {
            var cardObject = Instantiate(cardPrefab);
            cardObject.transform.SetParent(transform);

            var card = cardObject.GetComponent<StatusCard>();
            card.Init(unit, CalcuratePosition(cards.Count));
            cards.Add(unit.GetHashCode(), card);
            card.CloseEvent.AddListener(OnStatusCardClosed);
        }

        private Vector2 CalcuratePosition(int index)
        {
            return new Vector2((110 * index) + 10.0f, 0);
        }

        public void RearrangeCards()
        {
            int index = 0;
            foreach (var kvp in cards)
            {
                kvp.Value.SetDestination(CalcuratePosition(index));
                index++;
            }
        }
    }

}
