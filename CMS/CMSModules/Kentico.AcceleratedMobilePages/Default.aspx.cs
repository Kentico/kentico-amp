using System;
using System.Collections.ObjectModel;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using CMS.UIControls;
using Kentico.AcceleratedMobilePages;

[SaveAction(0)]
[UIElement(ModuleName.CONTENT, "Properties.AmpFilter")]

public partial class CMSModules_AcceleratedMobilePages_Default : CMSPropertiesPage
{
    /// <summary>
    /// Handling initialization
    /// </summary>
    /// <param name="e">Event arguments</param>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        DocumentManager.OnSaveData += DocumentManager_OnSaveData;

        // Non-versioned data are modified
        DocumentManager.UseDocumentHelper = false;
        DocumentManager.HandleWorkflow = false;
    }


    /// <summary>
    /// Handling page reload
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Init(object sender, EventArgs e)
    {
        // Show stylesheets only for current site
        selectStyleSheet.Properties = new Collection<Property> { new Property { Name = "ObjectSiteName", Value = SiteContext.CurrentSiteName } };
    }


    /// <summary>
    /// Handling page reload
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {      
        EditedObject = Node;

        // Check modify permissions
        if (!DocumentUIHelper.CheckDocumentPermissions(Node, PermissionsEnum.Modify))
        {
            DocumentManager.DocumentInfo = String.Format(GetString("cmsdesk.notauthorizedtoeditdocument"), Node.NodeAliasPath);

            // Disable save button
            CurrentMaster.HeaderActions.Enabled = false;
        }

        if ((Node != null) && !URLHelper.IsPostback())
        {
            ReloadData();
        } else
        {
            ShowControls(chkEnableAmpFilter.Checked);
        }
    }


    /// <summary>
    /// Enable / Disable widgets.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    protected void chkDefaultCss_OnCheckedChanged(object sender, EventArgs e)
    {
        bool useDefaultChecked = chkDefaultCss.Checked;
        selectStyleSheet.Visible = (!useDefaultChecked);
        labelSelectCss.Visible = (!useDefaultChecked);
        if (useDefaultChecked)
        {
            selectStyleSheet.Value = 0;
        }
    }


    /// <summary>
    /// Show / Hide widgets.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    protected void chkEnableAmpFilter_OnCheckedChanged(object sender, EventArgs e)
    {
        ShowControls(chkEnableAmpFilter.Checked);
    }

    /// <summary>
    /// Show or hide controls.
    /// </summary>
    /// <param name="visibility">visible or not</param>
    private void ShowControls(bool visibility=true)
    {
        chkDefaultCss.Visible = visibility;
        LabelDefaultCss.Visible = visibility;
        if (visibility)
        {
            bool useDefaultChecked = chkDefaultCss.Checked;
            selectStyleSheet.Visible = !useDefaultChecked;
            labelSelectCss.Visible = !useDefaultChecked;
        } else
        {
            selectStyleSheet.Visible = false;
            labelSelectCss.Visible = false;
        }
    }


    /// <summary>
    /// Reload data from node to controls.
    /// </summary>
    private ObjectQuery<AmpFilterInfo> GetAmpFilterInfoForGuid(String nodeGuid)
    {
        ObjectQuery<AmpFilterInfo> q = AmpFilterInfoProvider.GetAmpFilters().WhereEquals("PageNodeGuid", nodeGuid)
                                                                            .WhereEquals("SiteID", SiteContext.CurrentSiteID);
        return q;
    }

    /// <summary>
    /// Reload data from node to controls.
    /// </summary>
    private void ReloadData()
    {
        // Sets the checkbox checked if AMP filter is enabled for current node GUID
        ObjectQuery<AmpFilterInfo> q = GetAmpFilterInfoForGuid(Node.NodeGUID.ToString());
        chkEnableAmpFilter.Checked = (q.Count != 0);

        // Set controls
        if (q.Count != 0)
        {
            ShowControls(true);
            chkDefaultCss.Checked = q.FirstObject.UseDefaultStylesheet;
            selectStyleSheet.Value = q.FirstObject.StylesheetID.ToString();
            bool useDefaultChecked = chkDefaultCss.Checked;
            selectStyleSheet.Visible = !useDefaultChecked;
            labelSelectCss.Visible = !useDefaultChecked;
        } else
        {
            ShowControls(false);
            chkDefaultCss.Checked = true;
            selectStyleSheet.Visible = false;
            labelSelectCss.Visible = false;
        }
    }


    /// <summary>
    /// Creates or removes a record in database for current page according to state of amp filter (enabled/disabled)
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    protected void DocumentManager_OnSaveData(object sender, DocumentManagerEventArgs e)
    {
        if (Node != null)
        {
            string nodeGuid = Node.NodeGUID.ToString();
            ObjectQuery<AmpFilterInfo> q = GetAmpFilterInfoForGuid(nodeGuid);

            if (chkEnableAmpFilter.Checked)
            {
                AmpFilterInfo ampInfo;
                
                // First check if any record exists for this page
                if (q.Count == 0)
                {
                    // Insert new record to AmpFilterInfo table
                    ampInfo = new AmpFilterInfo();
                } else
                {
                    // Update existing record
                    ampInfo = q.FirstObject;
                }

                // Update object properties
                ampInfo.PageNodeGuid = nodeGuid;
                ampInfo.SiteID = SiteContext.CurrentSiteID;
                ampInfo.UseDefaultStylesheet = chkDefaultCss.Checked;
                ampInfo.StylesheetID = ValidationHelper.GetInteger(selectStyleSheet.Value, 0);
                AmpFilterInfoProvider.SetAmpFilterInfo(ampInfo);
            } else
            {
                // First check if any record exists for this page
                if (q.Count != 0)
                {
                    // Remove record from AmpFilterInfo table
                    AmpFilterInfoProvider.DeleteAmpFilterInfo(q.FirstObject);
                }
            }
        }
    }
}