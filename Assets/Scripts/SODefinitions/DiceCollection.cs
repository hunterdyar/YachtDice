using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
		public void AddDice(Dice newDice)
		{
			if (!_dice.Contains(newDice))
			{
				newDice.OnSelected += OnDiceSelected;
				_dice.Add(newDice);
				OnDiceChanged?.Invoke(this);
			}
		}

		public void RemoveDice(Dice dice)
		{
			if(_dice.Contains(dice))
			{
				dice.OnSelected -= OnDiceSelected;
				_dice.Remove(dice);
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
	}
}