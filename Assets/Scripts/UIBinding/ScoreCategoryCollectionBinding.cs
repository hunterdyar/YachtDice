using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class ScoreCategoryCollectionBinding : MonoBehaviour
{
    [FormerlySerializedAs("_Collection")] public ScoreCategoryCollection _collection;
    [SerializeField] private VisualTreeAsset _scoreCategoryTemplate;
    private UIDocument _gameplayDocument;
    private ListView _categoryList;

    private void OnEnable()
    {
       _collection.OnCategoriesChanged += OnCategoriesChanged; 
    }

    private void OnDisable()
    {
        _collection.OnCategoriesChanged -= OnCategoriesChanged;
    }

    private void OnCategoriesChanged()
    {
        if (_categoryList != null)
        {
             _categoryList.RefreshItems();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameplayDocument = GetComponent<UIDocument>();
        _categoryList = _gameplayDocument.rootVisualElement.Q<ListView>();
        InitiateList();
    }

    private void InitiateList()
    {
        _categoryList.itemsSource = _collection.Categories;
        _categoryList.makeItem = () =>
        {
            var sc = _scoreCategoryTemplate.Instantiate();
            var score = sc.contentContainer as ScoreCategoryVisualElement;
            //_categoryList.Add(sc);
            return sc;
        };

         _categoryList.bindItem = (e, i) =>
        {
            var sc = e.Q<ScoreCategoryVisualElement>();
            sc.Bind(_collection.Categories[i]);
        };
         
        _categoryList.selectionType = SelectionType.None;
    }
    
}
