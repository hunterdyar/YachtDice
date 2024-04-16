using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace SODefinitions
{
	[CreateAssetMenu(fileName = "DiceCollection", menuName = "Dice/Dice Collection", order = 0)]
	public class DiceCollection : ScriptableObject
	{
		public Action<DiceCollection> OnDiceChanged;
		public List<Dice> DiceList => _dice.ToList();
		public ObservableCollection<Dice> Dice => _dice;
		private readonly ObservableCollection<Dice> _dice = new ObservableCollection<Dice>();
		public Action<Dice> OnDiceSelected;
		private IEnumerable<IGrouping<int,Dice>> _groupsCache;
		public void AddDice(Dice newDice)
		{
			if (!_dice.Contains(newDice))
			{
				newDice.OnSelected += OnDiceSelected;
				_dice.Add(newDice);
				RecalculateGroups();
				OnDiceChanged?.Invoke(this);
			}
		}

		public void RemoveDice(Dice dice)
		{
			if(_dice.Contains(dice))
			{
				dice.OnSelected -= OnDiceSelected;
				_dice.Remove(dice);
				RecalculateGroups();	
				OnDiceChanged?.Invoke(this);
			}
		}

		public void Clear()
		{
			foreach (var die in _dice)
			{
				die.OnSelected -= OnDiceSelected;
			}
			_dice.Clear();
			RecalculateGroups();
			OnDiceChanged.Invoke(this);
		}

		private void DiceSelected(Dice dice)
		{
			//if safety...
			if (!_dice.Contains(dice))
			{
				dice.OnSelected -= DiceSelected;
				Debug.LogWarning("Dice Select callback that should have been unsubscribed from.");
				return;
			}	
			//bubble up.
			OnDiceSelected?.Invoke(dice);
		}

		public void RecalculateGroups()
		{
			_groupsCache = _dice.GroupBy(x => x.UpFace().GetValue());
		}

		public IEnumerable<IGrouping<int,Dice>> GetGroups()
		{
			//we want to recalculate this when any dice inside the group gets rolled, or when added, removed, and cleared.
			//we don't want to recalculate every single time - or it will likely get recalculated multiple times during scoring one set, for different scorecategories.
			//later tho, the solution is known but adding a listener to each dice roll is annoying.
			RecalculateGroups();
			return _groupsCache;
		}

		public IEnumerable<Dice> GetSortedLowToHigh()
		{
			//todo: for caching.
			return Dice.OrderBy(d => d.UpFace().GetValue());
		}
	}
}