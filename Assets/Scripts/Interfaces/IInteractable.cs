public partial interface IInteractable
{   
    public enum InteractionType
    {
        Quick,
        Continuous
    }
    InteractionType CurrentInteractionType { get; }
    public void Interact(InteractionType interactionType)
    {
        switch (interactionType)
        {
            case InteractionType.Quick:
                QuickInteract();
                break;
            case InteractionType.Continuous:
                ContinuousInteract();
                break;
             
            default: 
                QuickInteract();
                break;
        }
    }
    public void QuickInteract()
    {
    }
    public void ContinuousInteract() 
    {
    }
}
