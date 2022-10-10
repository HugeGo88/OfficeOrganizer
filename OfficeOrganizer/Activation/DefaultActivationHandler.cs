using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using OfficeOrganizer.Contracts.Services;
using OfficeOrganizer.ViewModels;

namespace OfficeOrganizer.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return _navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        AppActivationArguments openArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
        if (openArgs.Kind == ExtendedActivationKind.Launch)
        {
            _navigationService.NavigateTo(typeof(SettingsViewModel).FullName!, args.Arguments);
        }
        else if (openArgs.Kind == ExtendedActivationKind.File)
        {
            _navigationService.NavigateTo(typeof(WriterViewModel).FullName!, args.Arguments);
        }

        await Task.CompletedTask;
    }
}
