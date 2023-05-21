using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace banking_system.forms
{
    public partial class SignUpFormBankingData : Form
    {
        public SignUpFormBankingData()
        {
            InitializeComponent();
        }

        private void HandleCardNumberTextBoxClickEvent(object sender, EventArgs e)
        {
            TextBoxService.AdjustTextBoxOnMouseClick(this.cardNumberTextBox, "1234 5678 9012 3456");
        }

        private void HandleCardNumberTextBoxTypingEvent(object sender, KeyPressEventArgs e)
        {
            TextBoxService.AdjustTextBoxOnTyping(this.cardNumberTextBox);
            string trimmedCardNumber = Regex.Replace(this.cardNumberTextBox.Text, @"\s", string.Empty);
            if (!this.cardNumberTextBox.Text.Equals("") && e.KeyChar != (char)8 && trimmedCardNumber.Length % 4 == 0)
            {
                this.cardNumberTextBox.Text += " ";
                this.cardNumberTextBox.Select(this.cardNumberTextBox.Text.Length, 0);
            }
        }

        private void HandleCardNumberTextBoxCursorExitedEvent(object sender, EventArgs e)
        {
            TextBoxService.AdjustTextOnCursorExited(this.cardNumberTextBox, "1234 5678 9012 3456");
        }

        private void HandleCardOwnerTextBoxClickEvent(object sender, EventArgs e)
        {
            TextBoxService.AdjustTextBoxOnMouseClick(this.cardOwnerTextBox, "Ex. John Doe");
        }

        private void HandleCardOwnerTextBoxCursorExitedEvent(object sender, EventArgs e)
        {
            TextBoxService.AdjustTextOnCursorExited(this.cardOwnerTextBox, "Ex. John Doe");
        }

        private void HandleCardOwnerTextBoxTypingEvent(object sender, KeyPressEventArgs e)
        {
            TextBoxService.AdjustTextBoxOnTyping(this.cardOwnerTextBox);
        }

        private void HandleExpiryDateTextBoxCursorExitedEvent(object sender, EventArgs e)
        {
            TextBoxService.AdjustTextOnCursorExited(this.expiryDateTextBox, "01/19");
        }

        private void HandleExpiryDateTextBoxClickEvent(object sender, EventArgs e)
        {
            TextBoxService.AdjustTextBoxOnMouseClick(this.expiryDateTextBox, "01/19");
        }

        private void HandleExpiryDateTextBoxTypingEvent(object sender, KeyPressEventArgs e)
        {
            TextBoxService.AdjustTextBoxOnTyping(this.expiryDateTextBox);
        }

        private void HandleSecurityCodeTextBoxClickEvent(object sender, EventArgs e)
        {
            TextBoxService.AdjustTextBoxOnMouseClick(this.securityCodeTextBox, "***");
        }

        private void HandleSecurityCodeTextBoxCursorExitedEvent(object sender, EventArgs e)
        {
            TextBoxService.AdjustTextOnCursorExited(this.securityCodeTextBox, "***");
        }

        private void HandleSecurityCodeTextBoxTypingEvent(object sender, KeyPressEventArgs e)
        {
            TextBoxService.AdjustTextBoxOnTyping(this.securityCodeTextBox);
        }

        private void HandleRegisterButton(object sender, EventArgs e)
        {
            string cardNumber = this.cardNumberTextBox.Text;
            string cardOwner = this.cardOwnerTextBox.Text;
            string expiryDate = this.expiryDateTextBox.Text;
            string securityCode = this.securityCodeTextBox.Text;

            service.ValidationService validationService = new service.ValidationService();

            string cardNumberValidationResult = validationService.IsCardNumberValid(cardNumber);
            string cardOwnerValidationResult = validationService.IsCardOwnerValid(cardOwner);
            string expiryDateValidationResult = validationService.IsExpiryDateValid(expiryDate);
            string securityCodeValidationResult = validationService.IsSecurityNumberValid(securityCode);

            bool isRegistrationValid = cardNumberValidationResult.Equals("")
                && cardOwnerValidationResult.Equals("")
                && expiryDateValidationResult.Equals("")
                && securityCodeValidationResult.Equals("");


            int formWidth = this.Width / 2;

            if (!isRegistrationValid)
            {
                this.errorLabel.Text = utils.ErrorConstants.REGISTRATION_UNSUCCESSFUL + "\n";
                if (!cardNumberValidationResult.Equals(""))
                {
                    this.errorLabel.Text += cardNumberValidationResult;
                } else if (!cardOwnerValidationResult.Equals(""))
                {
                    this.errorLabel.Text += cardOwnerValidationResult;
                } else if (!expiryDateValidationResult.Equals(""))
                {
                    this.errorLabel.Text += expiryDateValidationResult;
                } else if (!expiryDateValidationResult.Equals(""))
                {
                    this.errorLabel.Text += securityCodeValidationResult;
                }

                this.errorLabel.Visible = true;

                int errorLabelCenter = this.errorLabel.Size.Width / 2;
                int errorLabelHeight = this.errorLabel.Size.Height;

            } else
            {
                SignInForm signInForm = new SignInForm();
                signInForm.Show();
                this.Hide();
            }
        }

        
    }
}
