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
    private CultureInfo _culture;
    private readonly Gtk.CheckButton _chkFilter;
    private readonly Gtk.Label _lblAmount;
    private readonly Gtk.Button _btnEdit;
    private readonly Gtk.Button _btnDelete;
    private readonly Gtk.Box _box;

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
    /// <param name="culture">The CultureInfo to use for the amount string</param>
    /// <param name="localizer">The Localizer for the app</param>
    /// <param name="filterActive">Whether or not the filter checkbutton should be active</param>
    public GroupRow(Group group, CultureInfo culture, Localizer localizer, bool filterActive)
    {
        _culture = culture;
        //Row Settings
        SetUseMarkup(false);
        //Filter Checkbox
        _chkFilter = Gtk.CheckButton.New();
        _chkFilter.AddCssClass("selection-mode");
        _chkFilter.OnToggled += FilterToggled; 
        AddPrefix(_chkFilter);
        //Amount Label
        _lblAmount = Gtk.Label.New(null);
        _lblAmount.SetValign(Gtk.Align.Center);
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
        //Box
        _box = Gtk.Box.New(Gtk.Orientation.Horizontal, 6);
        _box.Append(_lblAmount);
        _box.Append(_btnEdit);
        _box.Append(_btnDelete);
        AddSuffix(_box);
        UpdateRow(group, filterActive);
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
    /// <param name="filterActive">Whether or not the filter checkbox is active</param>
    public void UpdateRow(Group group, bool filterActive)
    {
        Id = group.Id;
        //Row Settings
        SetTitle(group.Name);
        SetSubtitle(group.Description);
        //Filter Checkbox
        _chkFilter.SetActive(filterActive);
        //Amount Label
        _lblAmount.SetLabel($"{(group.Balance >= 0 ? "+  " : "-  ")}{Math.Abs(group.Balance).ToString("C", _culture)}");
        _lblAmount.AddCssClass(group.Balance >= 0 ? "success" : "error");
        _lblAmount.AddCssClass(group.Balance >= 0 ? "denaro-income" : "denaro-expense");
        //Buttons
        _btnEdit.SetVisible(group.Id != 0);
        _btnEdit.SetSensitive(group.Id != 0);
        _btnDelete.SetVisible(group.Id != 0);
        _btnDelete.SetSensitive(group.Id != 0);
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