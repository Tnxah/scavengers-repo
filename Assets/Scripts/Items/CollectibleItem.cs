public class CollectibleItem : Item, IInteractable, IHidable
{
    public bool CanInteract()
    {
        return true;
    }

    public void Hide()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    public void Unhide()
    {
        throw new System.NotImplementedException();
    }
}
