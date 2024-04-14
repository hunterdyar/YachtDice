
using DefaultNamespace;
using UIBinding;

using UnityEngine;
using UnityEngine.UIElements;

public class ScoreCategoryCollectionBinding : MonoBehaviour
{
    public ScoreCategoryCollection _Collection;
    [SerializeField] private VisualTreeAsset _scoreCategoryTemplate;
    private UIDocument _gameplayDocument;
    private ListView _categoryList;
    

    // Start is called before the first frame update
    void Start()
    {
        _gameplayDocument = GetComponent<UIDocument>();
        _categoryList = _gameplayDocument.rootVisualElement.Q<ListView>();
        InitiateList();
    }

    private void InitiateList()
    {
        _categoryList.itemsSource = _Collection.Catgories;
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
            sc.Bind(_Collection.Catgories[i]);
        };
         
        _categoryList.selectionType = SelectionType.None;
    }
    
}
