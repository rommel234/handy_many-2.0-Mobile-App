using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Handyman
{
    public class SignEvent:EventArgs
    {
        private string nName;
        private string nSurname;
        private string nUsername;
        private string nPassword;
        private string nEmail;

        public string name
        {
           get { return nName; }
            set { nName = value; }
        }

        public string surname
        {
            get { return nSurname; }
            set { nSurname = value; }
        }

        public string username
        {
            get { return nUsername; }
            set { nUsername = value; }
        }

        public string password
        {
            get { return nPassword; }
            set { nPassword = value; }
        }

        public string email
        {
            get { return nEmail; }
            set { nEmail = value; }
        }

        public SignEvent(string name,string surname,string username,string password,string email): base()
        {
            name = name;
            surname = surname;
            username = username;
            password = password;
            email = email;

        }
    }
    class dialogRegister : DialogFragment
    {
        private EditText etRegName;
        private EditText etRegSurname;
        private EditText etRegUsername;
        private EditText etRegPassword;
        private EditText etRegEmail;
        private Button btSignUp;

        public event EventHandler<SignEvent> mSignEvent;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
             base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.DialogActivity, container, false);

            etRegName = view.FindViewById<EditText>(Resource.Id.etRegName);
            etRegSurname = view.FindViewById<EditText>(Resource.Id.etRegSurname);
            etRegUsername = view.FindViewById<EditText>(Resource.Id.etRegUsername);
            etRegPassword = view.FindViewById<EditText>(Resource.Id.etRegPassword);
            etRegEmail = view.FindViewById<EditText>(Resource.Id.etRegEmail);
            btSignUp = view.FindViewById<Button>(Resource.Id.btSignUp);

            btSignUp.Click += BtSignUp_Click;
            return view;
        }

        private void BtSignUp_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            mSignEvent.Invoke(this, new SignEvent(etRegName.Text, etRegSurname.Text, etRegUsername.Text, etRegPassword.Text, etRegEmail.Text));
            this.Dismiss();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
    }
}