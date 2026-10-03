using System;
using EpoxyFloorManager.Models;

namespace EpoxyFloorManager.Services;

public class AuthService
{
    public User? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser != null;
    
    public event Action? OnChange;

    public void Login(User user)
    {
        CurrentUser = user;
        NotifyStateChanged();
    }

    public void Logout()
    {
        CurrentUser = null;
        NotifyStateChanged();
    }

    public void UpdateCurrentUser(User user)
    {
        if (CurrentUser != null && CurrentUser.Id == user.Id)
        {
            CurrentUser = user;
            NotifyStateChanged();
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
