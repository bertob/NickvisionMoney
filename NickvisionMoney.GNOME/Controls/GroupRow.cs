using NickvisionMoney.Shared.Controls;
using NickvisionMoney.Shared.Helpers;
using NickvisionMoney.Shared.Models;
using System;
using System.Globalization;

namespace NickvisionMoney.GNOME.Controls;

/// <summary>
/// A row for displaying a group
/// </summary>
public partial class GroupRow : Adw.ActionRow, IGroupRowControl
{
    private CultureInfo _cultureAmount;
    private CultureInfo _cultureDate;
    private readonly Gtk.CheckButton _chkFilter;
    private readonly Gtk.Label _lblAmount;
    private readonly Gtk.Button _btnEdit;
    private readonly Gtk.Button _btnDelete;
    private readonly Gtk.Box _boxButtons;
    private readonly Gtk.FlowBox _flowBox;

    /// <summary>
    /// The Id of the Group the row represents
    /// </summary>
    public uint Id { get; private set; }

    /// <summary>
    /// Occurs when the filter checkbox is changed on the row
    /// </summary>
    public event EventHandler<(uint Id, bool Filter)>? FilterChanged;
    /// <summary>
    /// Occurs when the edit button on the row is clicked
    /// </summary>
    public event EventHandler<uint>? EditTriggered;
    /// <summary>
    /// Occurs when the delete button on the row is clicked
    /// </summary>
    public event EventHandler<uint>? DeleteTriggered;

    /// <summary>
    /// Constructs a group row 
    /// </summary>
    /// <param name="group">The Group to display</param>
    /// <param name="cultureAmount">The CultureInfo to use for the amount string</param>
    /// <param name="cultureDate">The CultureInfo to use for the date string</param>
    /// <param name="localizer">The Localizer for the app</param>
    /// <param name="filterActive">Whether or not the filter checkbutton should be active</param>
    public GroupRow(Group group, CultureInfo cultureAmount, CultureInfo cultureDate, Localizer localizer, bool filterActive)
    {
        _cultureAmount = cultureAmount;
        _cultureDate = cultureDate;
        //Row Settings
        SetUseMarkup(false);
        SetTitleLines(1);
        SetSubtitleLines(2);
        //Filter Checkbox
        _chkFilter = Gtk.CheckButton.New();
        _chkFilter.SetValign(Gtk.Align.Center);
        _chkFilter.AddCssClass("selection-mode");
        _chkFilter.OnToggled += FilterToggled;
        AddPrefix(_chkFilter);
        //Amount Label
        _lblAmount = Gtk.Label.New(null);
        _lblAmount.SetValign(Gtk.Align.Center);
        _lblAmount.SetMarginEnd(6);
        //Edit Button
        _btnEdit = Gtk.Button.NewFromIconName("document-edit-symbolic");
        _btnEdit.SetValign(Gtk.Align.Center);
        _btnEdit.AddCssClass("flat");
        _btnEdit.SetTooltipText(localizer["Edit", "GroupRow"]);
        _btnEdit.OnClicked += Edit;
        SetActivatableWidget(_btnEdit);
        //Delete Button
        _btnDelete = Gtk.Button.NewFromIconName("user-trash-symbolic");
        _btnDelete.SetValign(Gtk.Align.Center);
        _btnDelete.AddCssClass("flat");
        _btnDelete.SetTooltipText(localizer["Delete", "GroupRow"]);
        _btnDelete.OnClicked += Delete;
        //Buttons Box
        _boxButtons = Gtk.Box.New(Gtk.Orientation.Horizontal, 6);
        _boxButtons.SetHalign(Gtk.Align.End);
        _boxButtons.Append(_btnEdit);
        _boxButtons.Append(_btnDelete);
        //Suffix FlowBox
        _flowBox = Gtk.FlowBox.New();
        _flowBox.SetOrientation(Gtk.Orientation.Vertical);
        _flowBox.SetSelectionMode(Gtk.SelectionMode.None);
        _flowBox.Append(_lblAmount);
        _flowBox.Append(_boxButtons);
        AddSuffix(_flowBox);
        UpdateRow(group, cultureAmount, cultureDate, filterActive);
    }

    /// <summary>
    /// Whether or not the filter checkbox is checked
    /// </summary>
    public bool FilterChecked
    {
        get => _chkFilter.GetActive();

        set => _chkFilter.SetActive(value);
    }

    /// <summary>
    /// Updates the row with the new model
    /// </summary>
    /// <param name="group">The new Group model</param>
    /// <param name="cultureAmount">The culture to use for displaying amount strings</param>
    /// <param name="cultureDate">The culture to use for displaying date strings</param>
    /// <param name="filterActive">Whether or not the filter checkbox is active</param>
    public void UpdateRow(Group group, CultureInfo cultureAmount, CultureInfo cultureDate, bool filterActive)
    {
        Id = group.Id;
        _cultureAmount = cultureAmount;
        _cultureDate = cultureDate;
        //Row Settings
        SetTitle(group.Name);
        SetSubtitle(group.Description);
        //Filter Checkbox
        _chkFilter.SetActive(filterActive);
        //Amount Label
        _lblAmount.SetLabel($"{(group.Balance >= 0 ? "+  " : "-  ")}{Math.Abs(group.Balance).ToString("C", _cultureAmount)}");
        _lblAmount.AddCssClass(group.Balance >= 0 ? "denaro-income" : "denaro-expense");
        if (group.Id == 0)
        {
            _btnEdit.SetVisible(false);
            _btnDelete.SetVisible(false);
            _flowBox.SetValign(Gtk.Align.Center);
        }
    }

    /// <summary>
    /// Occurs when the filter checkbutton is toggled
    /// </summary>
    /// <param name="sender">Gtk.CheckButton</param>
    /// <param name="e">EventArgs</param>
    private void FilterToggled(Gtk.CheckButton sender, EventArgs e) => FilterChanged?.Invoke(this, (Id, _chkFilter.GetActive()));

    /// <summary>
    /// Occurs when the edit button is clicked
    /// </summary>
    /// <param name="sender">Gtk.Button</param>
    /// <param name="e">EventArgs</param>
    private void Edit(Gtk.Button sender, EventArgs e) => EditTriggered?.Invoke(this, Id);

    /// <summary>
    /// Occurs when the delete button is clicked
    /// </summary>
    /// <param name="sender">Gtk.Button</param>
    /// <param name="e">EventArgs</param>
    private void Delete(Gtk.Button sender, EventArgs e) => DeleteTriggered?.Invoke(this, Id);
}