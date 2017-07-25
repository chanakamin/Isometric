using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Isometric
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        async protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                Isometric.Common.SuspensionManager.RegisterFrame(rootFrame, "appFrame");
                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                  //    await Isometric.Common.SuspensionManager.RestoreAsync();
                    
                }
                SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;
               

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(StartPage), e.Arguments);
            }
            

           // 
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        async private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await Isometric.Common.SuspensionManager.SaveAsync();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
        #region Settings
        Popup settingsPopup;
        void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            // Add an About command
            SettingsCommand SettingCommand = new SettingsCommand("הגדרות", "הגדרות", (handler) =>
            {
                SettingsCommand cmd = (SettingsCommand)handler;
                // Create a SettingsFlyout the same dimenssions as the Popup.
                Settings _settingsFlyout = new Settings();
                // Create a Popup window which will contain our flyout.
                settingsPopup = new Popup();
                settingsPopup.Closed += settingsPopup_Closed;
                Window.Current.Activated += Current_Activated;
                settingsPopup.IsLightDismissEnabled = true;
                _settingsFlyout.Height = Window.Current.Bounds.Height;

                // Add the proper animation for the panel.
                settingsPopup.ChildTransitions = new TransitionCollection();
                settingsPopup.ChildTransitions.Add(new PaneThemeTransition()
                {
                    Edge = (SettingsPane.Edge == SettingsEdgeLocation.Left) ?
                            EdgeTransitionLocation.Right :
                            EdgeTransitionLocation.Left
                });

                // Place the SettingsFlyout inside our Popup window.
                settingsPopup.Child = _settingsFlyout;

                // Let's define the location of our Popup.
                settingsPopup.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Left ? _settingsFlyout.Width : (Window.Current.Bounds.Width - _settingsFlyout.Width));
                settingsPopup.SetValue(Canvas.TopProperty, 0);
                settingsPopup.IsOpen = true;
            });
            args.Request.ApplicationCommands.Add(SettingCommand);
            SettingsCommand AboutCommand = new SettingsCommand("אודות", "אודות", (handler) =>
            {
                SettingsCommand cmd = (SettingsCommand)handler;
                // Create a SettingsFlyout the same dimenssions as the Popup.
                About _aboutFlyout = new About();
                // Create a Popup window which will contain our flyout.
                settingsPopup = new Popup();
                settingsPopup.Closed += settingsPopup_Closed;
                Window.Current.Activated += Current_Activated;
                settingsPopup.IsLightDismissEnabled = true;
                _aboutFlyout.Height = Window.Current.Bounds.Height;

                // Add the proper animation for the panel.
                settingsPopup.ChildTransitions = new TransitionCollection();
                settingsPopup.ChildTransitions.Add(new PaneThemeTransition()
                {
                    Edge = (SettingsPane.Edge == SettingsEdgeLocation.Left) ?
                            EdgeTransitionLocation.Right :
                            EdgeTransitionLocation.Left
                });

                // Place the SettingsFlyout inside our Popup window.
                settingsPopup.Child = _aboutFlyout;

                // Let's define the location of our Popup.
                settingsPopup.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Left ? _aboutFlyout.Width : (Window.Current.Bounds.Width - _aboutFlyout.Width));
                settingsPopup.SetValue(Canvas.TopProperty, 0);
                settingsPopup.IsOpen = true;
            });
            args.Request.ApplicationCommands.Add(AboutCommand);

            // Add a Preferences command
        //    SettingsCommand Prefs = new SettingsCommand("Prefrences", "Prefrences", (handler) =>
        //    {
        //        SettingsCommand cmd = (SettingsCommand)handler;
        //        // Create a SettingsFlyout the same dimenssions as the Popup.
        //        PreferenceFlyout uc = new PreferenceFlyout();
        //        // Create a Popup window which will contain our flyout.
        //        settingsPopup = new Popup();
        //        settingsPopup.Closed += settingsPopup_Closed;
        //        Window.Current.Activated += Current_Activated;
        //        settingsPopup.IsLightDismissEnabled = true;
        //        settingsPopup.Width = uc.Width;
        //        settingsPopup.Height = Window.Current.Bounds.Height;

        //        // Add the proper animation for the panel.
        //        settingsPopup.ChildTransitions = new TransitionCollection();
        //        settingsPopup.ChildTransitions.Add(new PaneThemeTransition()
        //        {
        //            Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ?
        //                    EdgeTransitionLocation.Right :
        //                    EdgeTransitionLocation.Left
        //        });

        //        // Place the SettingsFlyout inside our Popup window.
        //        settingsPopup.Child = uc;

        //        // Let's define the location of our Popup.
        //        settingsPopup.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (Window.Current.Bounds.Width - uc.Width) : 0);
        //        settingsPopup.SetValue(Canvas.TopProperty, 0);
        //        settingsPopup.IsOpen = true;
        //    });
        //    args.Request.ApplicationCommands.Add(Prefs);
        }

        void settingsPopup_Closed(object sender, object e)
        {
            Window.Current.Activated -= Current_Activated;
        }

        void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                settingsPopup.IsOpen = false;
            }
        }
        #endregion


    }
}

//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
//using Windows.ApplicationModel;
//using Windows.ApplicationModel.Activation;
//using Windows.Foundation;
//using Windows.Foundation.Collections;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Controls.Primitives;
//using Windows.UI.Xaml.Data;
//using Windows.UI.Xaml.Input;
//using Windows.UI.Xaml.Media;
//using Windows.UI.Xaml.Navigation;

//// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

//namespace Isometric
//{
//    /// <summary>
//    /// Provides application-specific behavior to supplement the default Application class.
//    /// </summary>
//    sealed partial class App : Application
//    {
//        /// <summary>
//        /// Initializes the singleton application object.  This is the first line of authored code
//        /// executed, and as such is the logical equivalent of main() or WinMain().
//        /// </summary>
//        public App()
//        {
//            this.InitializeComponent();
//            this.Suspending += OnSuspending;
//        }

//        /// <summary>
//        /// Invoked when the application is launched normally by the end user.  Other entry points
//        /// will be used such as when the application is launched to open a specific file.
//        /// </summary>
//        /// <param name="e">Details about the launch request and process.</param>
//        protected override void OnLaunched(LaunchActivatedEventArgs e)
//        {

//#if DEBUG
//            if (System.Diagnostics.Debugger.IsAttached)
//            {
//                this.DebugSettings.EnableFrameRateCounter = true;
//            }
//#endif
//            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
//            {
//                //TODO: Load state from previously suspended application
//            }

//            // Place the frame in the current Window
//            Window.Current.Content = new StartPage();

//            // Ensure the current window is active
//            Window.Current.Activate();
//        }

//        /// <summary>
//        /// Invoked when application execution is being suspended.  Application state is saved
//        /// without knowing whether the application will be terminated or resumed with the contents
//        /// of memory still intact.
//        /// </summary>
//        /// <param name="sender">The source of the suspend request.</param>
//        /// <param name="e">Details about the suspend request.</param>
//        private void OnSuspending(object sender, SuspendingEventArgs e)
//        {
//            var deferral = e.SuspendingOperation.GetDeferral();
//            //TODO: Save application state and stop any background activity
//            deferral.Complete();
//        }
//    }
//}
