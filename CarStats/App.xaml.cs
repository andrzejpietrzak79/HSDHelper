using Microsoft.HockeyApp;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Template10.Controls;
using Template10.Common;
using System;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using HSDHelper.Services;
using HSDHelper.ViewModels;
using Microsoft.HockeyApp;

namespace HSDHelper
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki
    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            Microsoft.HockeyApp.HockeyClient.Current.Configure("a34bcb3a8abc442cbf0cd6ca3736b518");
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);
            var dr = new Windows.System.Display.DisplayRequest();
            dr.RequestActive();
#region app settings
            // some settings must be set in app.constructor
            AutoSuspendAllFrames = true;
            AutoRestoreAfterTerminated = true;
            AutoExtendExecutionSession = true;
#endregion
            App.Current.RequestedTheme = ApplicationTheme.Dark;

            Microsoft.HockeyApp.HockeyClient.Current.Configure("a34bcb3a8abc442cbf0cd6ca3736b518");
		}

	    private void MainPage_BackRequested(object sender, BackRequestedEventArgs e) {
		    
	    }

	    public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude);
            return new ModalDialog{DisableBackButtonWhenModal = true, Content = new Views.Shell(service), ModalContent = new Views.Busy(), };
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // TODO: add your long-running task here
            await NavigationService.NavigateAsync(typeof (Views.MainPage));
        }
    }
}