﻿using banking_system.models;
using banking_system.service;
using banking_system.utils;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace banking_system.forms
{
    public partial class DepositForm : Form
    {
        public string Username { get; set; }

        public Transaction Transaction { get; set; }
        public DepositForm()
        {
            InitializeComponent();
        }

        private void HandleDepositButtonClick(object sender, EventArgs e)
        {
            string amount = this.amountTextBox.Text;
            string cardOwner = this.cardOwnerTextBox.Text;
            string cardNumber = this.cardNumberTextBox.Text;

            ValidationService validationService = new ValidationService();

            string amountValidationResult = validationService.ValidateDepositAmount(amount);
            string cardOwnerValidationResult = validationService.IsCardOwnerValid(cardOwner);
            string cardNumberValidationResult = validationService.IsCardNumberValid(cardNumber);

            bool isRegistrationValid = cardNumberValidationResult.Equals("") &&
                cardNumberValidationResult.Equals("") &&
                amountValidationResult.Equals("");

            if (!isRegistrationValid)
            {
                this.errorLabel.Text = ErrorConstants.DEPOSIT_ERROR + "\n";
                if (!amountValidationResult.Equals(""))
                {
                    this.errorLabel.Text += amountValidationResult;
                }
                else if (!cardNumberValidationResult.Equals(""))
                {
                    this.errorLabel.Text += cardNumberValidationResult;
                }
                else if (!cardOwnerValidationResult.Equals(""))
                {
                    this.errorLabel.Text += cardOwnerValidationResult;
                }
                LabelService.CenterLabel(this, this.errorLabel);
                this.errorLabel.Visible = true;
            }
            else
            {
                UserService userService = new UserService();
                User user = userService.GetUserByUsername(Username);

                BankingDataService bankingDataService = new BankingDataService();
                BankingData bankingData = new BankingData(
                    cardNumber,
                    cardOwner,
                    user.UserId
                );
                BankingData retrievedBankingData = bankingDataService.GetBankingDataByCardOwnerCardNumberUserId(bankingData);

                if (retrievedBankingData.BankingDataId < 0)
                {
                    this.errorLabel.Text = ErrorConstants.DEPOSIT_ERROR + "\n" + ErrorConstants.CARD_WAS_NOT_FOUND;
                    LabelService.CenterLabel(this, this.errorLabel);
                    this.errorLabel.Visible = true;

                } else {
                    Transaction transaction = new Transaction(
                        "Deposit",
                        double.Parse(amount, System.Globalization.CultureInfo.InvariantCulture),
                        cardOwner,
                        cardNumber,
                        Username,
                        DateTime.Now.ToString()
                    );
                    
                    TransactionService transactionService = new TransactionService();
                    

                    int success = transactionService.SaveTransaction(transaction);
                    if (success == 0)
                    {
                        this.errorLabel.Text = ErrorConstants.DEPOSIT_ERROR + "\n" + ErrorConstants.TRANSACTION_ERROR;
                        LabelService.CenterLabel(this, this.errorLabel);
                        this.errorLabel.Visible = true;
                    }
                    else
                    {
                        FileService.SaveToCsv("D:\\Dox\\Projects\\banking-system\\files\\transactions.csv", transaction.ToString());
                        CardService cardService = new CardService();
                        CardData cardData = new CardData();
                        cardData.CardOwner = transaction.CardOwner;
                        cardData.CardNumber = transaction.CardNumber;
                        cardData.Sold = transaction.Sold;
                        cardService.UpdateCardByCardOwnerAndCardNumber(cardData);
                        Transaction = transaction;

                        UserMenu userMenu = new UserMenu();
                        userMenu.username = Username;
                        userMenu.Show();
                        this.Hide();
                    }
                }
            }
        }

        private void HandleBackButtonClick(object sender, EventArgs e)
        {
            UserMenu userMenu = new UserMenu();
            userMenu.username = Username;
            userMenu.Show();
            this.Hide();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("DEPOSIT", new Font("Arial", 20, FontStyle.Bold), Brushes.Black, new Point(350, 10));
            e.Graphics.DrawString("Deposit value: " + Transaction.Sold, new Font("Arial", 20, FontStyle.Bold), Brushes.Black, new Point(10, 100));
            e.Graphics.DrawString("Card number: " + Transaction.CardNumber, new Font("Arial", 20, FontStyle.Bold), Brushes.Black, new Point(10, 150));
            e.Graphics.DrawString("Card owner: " + Transaction.CardOwner, new Font("Arial", 20, FontStyle.Bold), Brushes.Black, new Point(10, 200));
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string amount = this.amountTextBox.Text;
            string cardOwner = this.cardOwnerTextBox.Text;
            string cardNumber = this.cardNumberTextBox.Text;

            if (amount.Equals("") ||
                cardOwner.Equals("") ||
                cardNumber.Equals(""))
            {
                this.errorLabel.Text = ErrorConstants.EMPTY_FIELD;
                LabelService.CenterLabel(this, this.errorLabel);
                this.errorLabel.Visible = true;
            }
            else
            {
                Transaction transaction = new Transaction(
                    "Deposit",
                    double.Parse(amount, System.Globalization.CultureInfo.InvariantCulture),
                    cardOwner,
                    cardNumber,
                    Username,
                    DateTime.Now.ToString()
                );
                Transaction = transaction;

                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDocument1;
                DialogResult result = printDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }
            
        }

        private void saveAsCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string amount = this.amountTextBox.Text;
            string cardOwner = this.cardOwnerTextBox.Text;
            string cardNumber = this.cardNumberTextBox.Text;

            if (amount.Equals("") ||
                cardOwner.Equals("") ||
                cardNumber.Equals(""))
            {
                this.errorLabel.Text = ErrorConstants.EMPTY_FIELD;
                LabelService.CenterLabel(this, this.errorLabel);
                this.errorLabel.Visible = true;
            }
            else
            {
                Transaction transaction = new Transaction(
                    "Deposit",
                    double.Parse(amount, System.Globalization.CultureInfo.InvariantCulture),
                    cardOwner,
                    cardNumber,
                    Username,
                    DateTime.Now.ToString()
                );

                FileService.SaveToCsv("D:\\Dox\\Projects\\banking-system\\files\\transactions.csv", transaction.ToString());
            }
        }
    }
}
