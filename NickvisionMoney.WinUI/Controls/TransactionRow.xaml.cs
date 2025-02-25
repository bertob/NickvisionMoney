using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using NickvisionMoney.Shared.Controls;
using NickvisionMoney.Shared.Helpers;
using NickvisionMoney.Shared.Models;
using NickvisionMoney.WinUI.Helpers;
using System;
using System.Globalization;
using Windows.UI;

namespace NickvisionMoney.WinUI.Controls;

/// <summary>
/// A row to display a Transaction model
/// </summary>
public sealed partial class TransactionRow : UserControl, IModelRowControl<Transaction>
{
    private CultureInfo _culture;
    private Color _defaultColor;
    private Localizer _localizer;
    private int _repeatFrom;

    /// <summary>
    /// The Id of the Transaction the row represents
    /// </summary>
    public uint Id { get; private set; }
    /// <summary>
    /// The GridViewItem container of this row
    /// </summary>
    public GridViewItem? Container { private get; set; }

    /// <summary>
    /// Occurs when the edit button on the row is clicked 
    /// </summary>
    public event EventHandler<uint>? EditTriggered;
    /// <summary>
    /// Occurs when the delete button on the row is clicked 
    /// </summary>
    public event EventHandler<uint>? DeleteTriggered;

    /// <summary>
    /// Constructs a TransactionRow
    /// </summary>
    /// <param name="transaction">The Transaction model to represent</param>
    /// <param name="culture">The CultureInfo to use for the amount string</param>
    /// <param name="defaultColor">The default transaction color</param>
    /// <param name="localizer">The Localizer for the app</param>
    public TransactionRow(Transaction transaction, CultureInfo culture, Color defaultColor, Localizer localizer)
    {
        InitializeComponent();
        _culture = culture;
        _defaultColor = defaultColor;
        _localizer = localizer;
        _repeatFrom = 0;
        //Localize Strings
        MenuEdit.Text = _localizer["Edit", "TransactionRow"];
        MenuDelete.Text = _localizer["Delete", "TransactionRow"];
        ToolTipService.SetToolTip(BtnEdit, _localizer["Edit", "TransactionRow"]);
        ToolTipService.SetToolTip(BtnDelete, _localizer["Delete", "TransactionRow"]);
        UpdateRow(transaction);
    }

    /// <summary>
    /// Shows the row
    /// </summary>
    public void Show() => Container!.Visibility = Visibility.Visible;

    /// <summary>
    /// Hides the row
    /// </summary>
    public void Hide() => Container!.Visibility = Visibility.Collapsed;

    /// <summary>
    /// Updates the row with the new model
    /// </summary>
    /// <param name="transaction">The new Transaction model</param>
    public void UpdateRow(Transaction transaction)
    {
        Id = transaction.Id;
        _repeatFrom = transaction.RepeatFrom;
        MenuEdit.IsEnabled = _repeatFrom <= 0;
        BtnEdit.Visibility = _repeatFrom <= 0 ? Visibility.Visible : Visibility.Collapsed;
        MenuDelete.IsEnabled = _repeatFrom <= 0;
        BtnDelete.Visibility = _repeatFrom <= 0 ? Visibility.Visible : Visibility.Collapsed;
        BtnId.Content = transaction.Id;
        BtnId.Background = new SolidColorBrush(ColorHelpers.FromRGBA(transaction.RGBA) ?? _defaultColor);
        LblName.Text = transaction.Description;
        LblDescription.Text = transaction.Date.ToString("d");
        if (transaction.RepeatInterval != TransactionRepeatInterval.Never)
        {
            LblDescription.Text += $"\n{_localizer["TransactionRepeatInterval", "Field"]}: {_localizer["RepeatInterval", transaction.RepeatInterval.ToString()]}";
        }
        LblAmount.Text = $"{(transaction.Type == TransactionType.Income ? "+" : "-")}  {transaction.Amount.ToString("C", _culture)}";
        LblAmount.Foreground = transaction.Type == TransactionType.Income ? new SolidColorBrush(ActualTheme == ElementTheme.Light ? Color.FromArgb(255, 38, 162, 105) : Color.FromArgb(255, 143, 240, 164)) : new SolidColorBrush(ActualTheme == ElementTheme.Light ? Color.FromArgb(255, 192, 28, 40) : Color.FromArgb(255, 255, 123, 99));
    }

    /// <summary>
    /// Occurs when the edit button on the row is clicked 
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">RoutedEventArgs</param>
    public void Edit(object sender, RoutedEventArgs e)
    {
        if(_repeatFrom <= 0)
        {
            EditTriggered?.Invoke(this, Id);
        }
    }

    /// <summary>
    /// Occurs when the delete button on the row is clicked 
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">RoutedEventArgs</param>
    private void Delete(object sender, RoutedEventArgs e)
    {
        if(_repeatFrom <= 0)
        {
            DeleteTriggered?.Invoke(this, Id);
        }
    }
}
