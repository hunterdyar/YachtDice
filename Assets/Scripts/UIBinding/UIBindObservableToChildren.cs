using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Unity.VisualScripting.FullSerializer.Internal.Converters;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIBinding
{
	public class UIBindObservableToChildren<T>
	{
		private ObservableCollection<T> _collection;
		private VisualTreeAsset _childAsset;
		private VisualElement _parent;
		public Action <T, VisualElement> OnChildCreated;
		private Dictionary<T, VisualElement> children = new Dictionary<T, VisualElement>();
		
		public UIBindObservableToChildren(VisualElement parent, ObservableCollection<T> collection, VisualTreeAsset childAsset)
		{
			_collection = collection;
			_childAsset = childAsset;
			_parent = parent;
			_collection.CollectionChanged += CollectionOnCollectionChanged;
			ForceSync();
		}
		

		//delete all and recreate.
		private void ForceSync()
		{
			children.Clear();
			
			if (_parent == null)
			{
				return;
			}
			if (_parent.childCount > 0)
            {
            	for (int i = _parent.childCount - 1; i >= 0; i--)
	            {
            		_parent.RemoveAt(i);
            	}
            }
            //we are just deleting all of the things, but we could find the elements by dice, or vise-versa, or whatever, with a dictionary.

            foreach (var item in _collection)
            {
				CreateChildElement(item);	
            }
		}

		private void CreateChildElement(T item)
		{
			if (children.ContainsKey(item))
			{
				if (children[item] == null)
				{
					//hmmmmm
					Debug.LogWarning("huh");
					children.Remove(item);
					//then recreate below? i guess?
				}
				else
				{
					Debug.LogWarning($"item already exists in {_parent.name}");
					//skip
					return;
				}
			}
			
			var c = _childAsset.Instantiate();
			_parent.Add(c);
			children.Add(item, c);
			OnChildCreated?.Invoke(item, c);
		}

		private void CollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
			{
				foreach (var obj in e.OldItems)
				{
					if (obj is T item)
					{
						_parent.Remove(children[item]);
						children.Remove(item);
					}
				}
			}

			if (e.NewItems != null)
			{
				foreach (var obj in e.NewItems)
				{
					if (obj is T item)
					{
						CreateChildElement(item);
					}
				}
			}

			return;
			
			//lol it's FINE.
			//todo: the whole, you know, point...
			//ForceSync();	
		}
	}
}