using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.Lang;

namespace Handyman
{
    [Activity(Label = "Handyman", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
       // int count = 1;
        private Button btregister;
        private ProgressBar prBar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            btregister = FindViewById<Button>(Resource.Id.btregister);
            prBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);

            btregister.Click += (object sender, EventArgs args) =>
             {
                 //pop up the dialog
                 FragmentTransaction transaction = FragmentManager.BeginTransaction();
                 dialogRegister registerDialog = new dialogRegister();
                 registerDialog.Show(transaction,"dialog fragment");
                 registerDialog.mSignEvent += registerDialog_mSignEvent;
                 

             };

            // Get our button from the layout resource,
            // and attach an event to it

        }

        private void registerDialog_mSignEvent(object sender, SignEvent e)
        {
            // throw new NotImplementedException();
            prBar.Visibility = ViewStates.Visible;
            Thread thread = new Thread(ActeLikeArequest);
            thread.Start();
            string userPassword = e.password;
            string username = e.username;
        }
        private void ActeLikeArequest()
        {
            Thread.Sleep(3000);
            RunOnUiThread(() => { prBar.Visibility = ViewStates.Invisible; });
        }
    }
}

