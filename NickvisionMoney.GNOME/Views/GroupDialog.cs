using NickvisionMoney.Shared.Controllers;

namespace NickvisionMoney.GNOME.Views;

/// <summary>
/// A dialog for managing a Group
/// </summary>
public partial class GroupDialog
{
    private bool _constructing;
    private readonly GroupDialogController _controller;
    private readonly Adw.MessageDialog _dialog;
    private readonly Adw.PreferencesGroup _grpGroup;
    private readonly Adw.EntryRow _rowName;
    private readonly Adw.EntryRow _rowDescription;

    /// <summary>
    /// Constructs a GroupDialog
    /// </summary>
    /// <param name="controller">GroupDialogController</param>
    /// <param name="parentWindow">Gtk.Window</param>
    public GroupDialog(GroupDialogController controller, Gtk.Window parentWindow)
    {
        _constructing = true;
        _controller = controller;
        //Dialog Settings
        _dialog = Adw.MessageDialog.New(parentWindow, _controller.Localizer["Group"], "");
        _dialog.SetDefaultSize(360, -1);
        _dialog.SetHideOnClose(true);
        _dialog.SetModal(true);
        _dialog.AddResponse("cancel", _controller.Localizer["Cancel"]);
        _dialog.SetCloseResponse("cancel");
        _dialog.AddResponse("ok", _controller.Localizer["OK"]);
        _dialog.SetDefaultResponse("ok");
        _dialog.SetResponseAppearance("ok", Adw.ResponseAppearance.Suggested);
        _dialog.OnResponse += (sender, e) => _controller.Accepted = e.Response == "ok";
        //Preferences Group
        _grpGroup = Adw.PreferencesGroup.New();
        //Name
        _rowName = Adw.EntryRow.New();
        _rowName.SetTitle(_controller.Localizer["Name", "Field"]);
        _rowName.SetActivatesDefault(true);
        _rowName.OnNotify += (sender, e) =>
        {
            if (e.Pspec.GetName() == "text")
            {
                if(!_constructing)
                {
                    Validate();
                }
            }
        };
        _grpGroup.Add(_rowName);
        //Description
        _rowDescription = Adw.EntryRow.New();
        _rowDescription.SetTitle(_controller.Localizer["Description", "Field"]);
        _rowDescription.SetActivatesDefault(true);
        _rowDescription.OnNotify += (sender, e) =>
        {
            if (e.Pspec.GetName() == "text")
            {
                if (!_constructing)
                {
                    Validate();
                }
            }
        };
        _grpGroup.Add(_rowDescription);
        //Layout
        _dialog.SetExtraChild(_grpGroup);
        //Load Group
        _rowName.SetText(_controller.Group.Name);
        _rowDescription.SetText(_controller.Group.Description);
        Validate();
        _constructing = false;
    }

    public event GObject.SignalHandler<Adw.MessageDialog, Adw.MessageDialog.ResponseSignalArgs> OnResponse
    {
        add
        {
            _dialog.OnResponse += value;
        }
        remove
        {
            _dialog.OnResponse -= value;
        }
    }

    /// <summary>
    /// Shows the dialog
    /// </summary>
    public void Show() => _dialog.Show();

    /// <summary>
    /// Destroys the dialog
    /// </summary>
    public void Destroy() => _dialog.Destroy();

    /// <summary>
    /// Validates the dialog's input
    /// </summary>
    private void Validate()
    {
        var checkStatus = _controller.UpdateGroup(_rowName.GetText(), _rowDescription.GetText());
        _rowName.RemoveCssClass("error");
        _rowName.SetTitle(_controller.Localizer["Name", "Field"]);
        if (checkStatus == GroupCheckStatus.Valid)
        {
            _dialog.SetResponseEnabled("ok", true);
        }
        else
        {
            if (checkStatus == GroupCheckStatus.EmptyName)
            {
                _rowName.AddCssClass("error");
                _rowName.SetTitle(_controller.Localizer["Name", "Empty"]);
            }
            else if (checkStatus == GroupCheckStatus.NameExists)
            {
                _rowName.AddCssClass("error");
                _rowName.SetTitle(_controller.Localizer["Name", "Exists"]);
            }
            _dialog.SetResponseEnabled("ok", false);
        }
    }
}