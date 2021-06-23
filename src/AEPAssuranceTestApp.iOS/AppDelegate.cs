﻿/*
Copyright 2020 Adobe
All Rights Reserved.

NOTICE: Adobe permits you to use, modify, and distribute this file in
accordance with the terms of the Adobe license agreement accompanying
it. If you have received this file from a source other than Adobe,
then your use, modification, or distribution of it requires the prior
written permission of Adobe. (See LICENSE-MIT for details)
*/

using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Com.Adobe.Marketing.Mobile;

namespace AEPAssuranceTestApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            // set the wrapper type
            ACPCore.SetWrapperType(ACPMobileWrapperType.Xamarin);

            // set log level
            ACPCore.LogLevel = ACPMobileLogLevel.Verbose;

            // register SDK extensions
            ACPLifecycle.RegisterExtension();
            ACPIdentity.RegisterExtension();
            AEPAssurance.RegisterExtension();

            // start core
            ACPCore.Start(startCallback);

            // register dependency service to link interface from ACPCoreTestApp base project
            DependencyService.Register<IAEPAssuranceExtensionService, AEPAssuranceExtensionService>();

            return base.FinishedLaunching(app, options);
        }

        // Called when the application is launched and every time the app returns to the foreground.
        public override void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);
            ACPCore.LifecycleStart(null);
        }

        // Called when the application is about to enter the background, be suspended, or when the user receives an interruption such as a phone call or text.
        public override void OnResignActivation(UIApplication uiApplication)
        {
            base.OnResignActivation(uiApplication);
            ACPCore.LifecyclePause();
        }
        // Handle app open from url scheme of aepassurancetestApp://link
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            AEPAssurance.StartSession(url);
            return false;
        }

        private void startCallback()
        {
            // set launch config
            ACPCore.ConfigureWithAppID("94f571f308d5/e30a9514788b/launch-44fec1a705f1-development");
        }
    }
}
