using System.ComponentModel;
using System.Linq;
using System;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Template10.Mvvm;

namespace HSDHelper.Views {
    public sealed partial class Shell : Page {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;

        public Shell() {
            Instance = this;
            InitializeComponent();
        }

        public Shell(INavigationService navigationService) : this() {
            SetNavigationService(navigationService);
        }

        public void SetNavigationService(INavigationService navigationService) {
            MyHamburgerMenu.NavigationService = navigationService;
        }
    }
}

