using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
public class ItemEditor : EditorWindow
{
    private ItemDataList_SO dataBase;//��ȡSO�ļ�
    private List<ItemDetails> itemList = new List<ItemDetails>();//ͨ�������б���������б�
    private VisualTreeAsset itemRowTemplate;
    private ListView itemListView;//����ItemListView
    private ItemDetails activeItem;//��ȡ���������
    private ScrollView itemDetailsSection;//��ȡ�Ҳ�����
    private VisualElement iconPrview;
    private Sprite defaultIcon;
    [MenuItem("M STUDIO/ItemEditor")]
    public static void ShowExample()
    {
        ItemEditor wnd = GetWindow<ItemEditor>();
        wnd.titleContent = new GUIContent("ItemEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        //VisualElement label = new Label("Hello World! From C#");
       // root.Add(label);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/UI Builder/ItemEditor.uss");
        //VisualElement labelWithStyle = new Label("Hello World! With Style");
        //  labelWithStyle.styleSheets.Add(styleSheet);
        // root.Add(labelWithStyle);

        //�ҵ�����ListView
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");
        itemDetailsSection = root.Q<ScrollView>("ItemDetails");
        iconPrview = itemDetailsSection.Q<VisualElement>("Icon");
        //�õ�Ĭ��Icon����
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/img/shop (1).jpg");
        //�õ�ģ������
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemRow Template.uxml");
        //��������

        //��ȡ����
        root.Q<Button>("AddButton").clicked += OnAddItemClick;
        root.Q<Button>("DeleteButton").clicked += OnADeleteItemClick;

        LoadDataBase();
        //ִ�л�ȡ������
        GenerateListView();
    }
    #region �����¼�
    private void OnADeleteItemClick()
    {
        itemList.Remove(activeItem);
        itemListView.Rebuild();
        itemDetailsSection.visible = false;
    }

    private void OnAddItemClick()
    {
        ItemDetails it = new ItemDetails();
        it.itemName = "New Item";
        it.ID = 1000 + itemList.Count;
        itemList.Add(it);
        itemListView.Rebuild();
    }
    #endregion

    private void LoadDataBase()
    {
       var dataArray = AssetDatabase.FindAssets("ItemDataList_SO");
        if(dataArray.Length>1)
        {
            string path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            dataBase = AssetDatabase.LoadAssetAtPath(path, typeof(ItemDataList_SO))as ItemDataList_SO;
        }
        itemList = dataBase.ItemDataList;
       EditorUtility.SetDirty(dataBase);
    }

    private void GenerateListView()
    {
        Func<VisualElement> makeItem = () => itemRowTemplate.CloneTree();

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            if (i< itemList.Count)
            {
                if (itemList[i].itemIcon != null)
                  e.Q<VisualElement>("Icon").style.backgroundImage = itemList[i].itemIcon.texture;
                e.Q<Label>("Name").text = itemList[i].itemName != null? itemList[i].itemName : "No Itme";
             //   Debug.Log(itemList[i].itemName);    
            }
        };
        itemListView.fixedItemHeight = 60;
        itemListView.itemsSource = itemList;
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;
        itemListView.onSelectionChange += OnListSectionChange;

        itemDetailsSection.visible = false;

    }

    private void OnListSectionChange(IEnumerable<object> selectedItem)
    {
        activeItem =(ItemDetails)selectedItem.First();
        itemDetailsSection.visible = true;
        GetItemDetails();
    }

    private void GetItemDetails()
    {
        itemDetailsSection.MarkDirtyRepaint();
        //ID����
        itemDetailsSection.Q<IntegerField>("ItemID").value = activeItem.ID;
        itemDetailsSection.Q<IntegerField>("ItemID").RegisterValueChangedCallback(evt =>
        {
            activeItem.ID =evt.newValue;
        });
        //���ָ���
        itemDetailsSection.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsSection.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemName = evt.newValue;
            itemListView.Rebuild();
        });
        //ö�ٸ���
        itemDetailsSection.Q<EnumField>("ItemType").Init(activeItem.itemType);
        itemDetailsSection.Q<EnumField>("ItemType").value = activeItem.itemType;
        itemDetailsSection.Q<EnumField>("ItemType").RegisterCallback<ChangeEvent<Enum>>((evt) =>
        {
            activeItem.itemType =(ItemType)evt.newValue;
        });
        //ͼƬ����
       // iconPrview.style.backgroundImage = activeItem.itemIcon.texture == null ? defaultIcon.texture : activeItem.itemIcon.texture;
        //Icon����
        itemDetailsSection.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon;
        itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon =  evt.newValue as Sprite;
            activeItem.itemIcon = newIcon;
            iconPrview.style.backgroundImage = newIcon == null?defaultIcon.texture : newIcon.texture;
            itemListView.Rebuild();
        });
        //��������
        itemDetailsSection.Q<TextField>("Description").value = activeItem.Description;
        itemDetailsSection.Q<TextField>("Description").RegisterValueChangedCallback(evt =>
        {
            activeItem.Description = evt.newValue; 
        });
        //ʹ�ð뾶����
        itemDetailsSection.Q<IntegerField>("UseRadius").value = activeItem.itemUseRadios;
        itemDetailsSection.Q<IntegerField>("UseRadius").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemUseRadios = evt.newValue;
        });
        //�Ƿ����ʹ��ĳЩ���ܣ�Boolֵ����
        itemDetailsSection.Q<Toggle>("CanPickUp").value = activeItem.canPickUp;
        itemDetailsSection.Q<Toggle>("CanPickUp").RegisterCallback<ChangeEvent<bool>>((evt) =>
        {
            activeItem.canPickUp = evt.newValue;
        });
        itemDetailsSection.Q<Toggle>("CanDropped").value = activeItem.canDropped;
        itemDetailsSection.Q<Toggle>("CanDropped").RegisterCallback<ChangeEvent<bool>>((evt) =>
        {
            activeItem.canDropped = evt.newValue;
        });
        itemDetailsSection.Q<Toggle>("CanCariied").value = activeItem.canCarried;
        itemDetailsSection.Q<Toggle>("CanCariied").RegisterCallback<ChangeEvent<bool>>((evt) =>
        {
            activeItem.canCarried = evt.newValue;
        });

        //�۸��������ٷֱȸ���
        itemDetailsSection.Q<IntegerField>("Price").value = activeItem.itemPrice;
        itemDetailsSection.Q<IntegerField>("Price").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemPrice = evt.newValue;
        });
        itemDetailsSection.Q<Slider>("SellPercentage").value = activeItem.sellPercentage;
        itemDetailsSection.Q<Slider>("SellPercentage").RegisterCallback<ChangeEvent<float>>((evt) =>
        {
            activeItem.sellPercentage = evt.newValue;
        });
    }
}