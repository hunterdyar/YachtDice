using SODefinitions;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIBinding
{
	//todo: look at ScoreCategoryVisualBinding. THe listView is the way to go, as it already has the features we want.
	public class DiceCollectionBinding : MonoBehaviour
	{
		private UIDocument UIDoc;
		public VisualTreeAsset SingleDice;
		public string parentVisualElement;
		public DiceCollection DiceCollection;
		private VisualElement parentElement;

		private UIBindObservableToChildren<Dice> _binder;
		private void Awake()
		{
			UIDoc = GetComponent<UIDocument>();
			parentElement = UIDoc.rootVisualElement.Q(parentVisualElement);
			_binder = new UIBindObservableToChildren<Dice>(parentElement, DiceCollection.Dice, SingleDice);
			_binder.OnChildCreated += OnChildCreated;
		}

		private void OnChildCreated(Dice dice, VisualElement diceElement)
		{
			//manual binding.
			var d = diceElement.Q<DiceVisualElement>();
			if (d != null)
			{
				d.SetDice(dice);
			}
			else
			{
				Debug.LogError("Unable to case SingleDice visual asset to DiceVisualElement. Ensure DiceVisualElement is root object of asset.");
			}
		}

		private void OnDisable()
		{
			DiceCollection.OnDiceChanged += OnDiceChanged;
		}

		private void OnDiceChanged(DiceCollection diceCollection)
		{
			//binder is handling this for us.
			//clear children and recreate them, because it's quick and easy.
			//later, we will want to map a dictionary and then loop through looking for what's unchanged.
			//or get more specific events for add/remove.
			//whatever is most convenient for sorting.
			
			
			//clear and redraw visual elements?
			// SingleDiceDoc.rootVisualElement
		}
	}
}