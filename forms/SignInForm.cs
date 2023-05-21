using banking_system.forms;
using System;
using System.Windows.Forms;

namespace banking_system {

    public partial class SignInForm : Form {
        public SignInForm() {
            InitializeComponent();
        }

        private void handleSignInButtonClick(object sender, MouseEventArgs e) 
        {
            
        }

        private void HandleUsernameTextBoxTyping(object sender, KeyPressEventArgs e) 
        {
            TextBoxService.AdjustTextBoxOnTyping(this.usernameTextBox);
        }

        private void HandleUsernameTextBoxClick(object sender, MouseEventArgs e) 
        {
            TextBoxService.AdjustTextBoxOnMouseClick(this.usernameTextBox, "Type your username...");
        }

        private void HandleUsernameTextBoxPointerLeave(object sender, EventArgs e) 
        {
            TextBoxService.AdjustTextOnCursorExited(this.usernameTextBox, "Type your username...");
        }

        private void HandlePasswordTextBoxPointerLeave(object sender, EventArgs e)
        {
            if (this.passwordTextBox.Text.Equals(""))
            {
                this.passwordTextBox.UseSystemPasswordChar = false;
            }
            TextBoxService.AdjustTextOnCursorExited(this.passwordTextBox, "Type your password...");
        }

        private void HandlePasswordTextBoxTyping(object sender, KeyPressEventArgs e)
        {
            TextBoxService.AdjustTextBoxOnTyping(this.passwordTextBox);
        }

        private void HandlePasswordTextBoxClick(object sender, MouseEventArgs e)
        {
            TextBoxService.AdjustTextBoxOnMouseClick(this.passwordTextBox, "Type your password...");
            this.passwordTextBox.UseSystemPasswordChar = true;
        }

        private void HandleSignUpButtonClick(object sender, EventArgs e)
        {
            SignUpFormUserData signUpFormUserData = new SignUpFormUserData();
            signUpFormUserData.Show();
            this.Hide();
        }
    }
}
