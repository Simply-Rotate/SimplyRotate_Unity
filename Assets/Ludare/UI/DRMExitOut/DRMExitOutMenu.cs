using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DRMExitOutMenu : MonoBehaviour
{
    [SerializeField]
    private UIDocument drmSignInDoc;

    public DRMCloseEvent closeEvent; 

    public void Awake()
    {
        closeEvent = new DRMCloseEvent();
    }

    public void Start()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = drmSignInDoc.rootVisualElement;

        VisualElement buttonGroup = root.Query<VisualElement>("ButtonManager");
        Button ConfirmButton = buttonGroup.Query<Button>("ConfirmButton");
        Button DenyButton = buttonGroup.Query<Button>("DenyButton");

        ConfirmButton.RegisterCallback<ClickEvent>(ConfirmSelected);
        DenyButton.RegisterCallback<ClickEvent>(DenySelected);
    }

    public void ConfirmSelected(ClickEvent Clicked)
    {
        closeEvent.Invoke(true);
        Destroy(gameObject);
    }

    public void DenySelected(ClickEvent Clicked)
    {
        closeEvent.Invoke(false);
        Destroy(gameObject);
    }
}
