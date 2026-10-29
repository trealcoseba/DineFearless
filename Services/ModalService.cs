using DineFearless.Models;

namespace DineFearless.Services;

public enum ActiveModal
{
    None,
    DishModification,
    DishDetail,
    ServerCardPrint,
    KitchenProtocols,
    ProfileSettings,
    SafetyLabels
}

public class ModalService
{
    public ActiveModal CurrentModal { get; private set; } = ActiveModal.None;
    public MenuItem? SelectedDish { get; private set; }

    public event Action? OnModalChanged;

    public void Open(ActiveModal modal, MenuItem? dish = null)
    {
        CurrentModal = modal;
        SelectedDish = dish;
        OnModalChanged?.Invoke();
    }

    public void Close()
    {
        CurrentModal = ActiveModal.None;
        SelectedDish = null;
        OnModalChanged?.Invoke();
    }
}