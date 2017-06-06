<%@ Page Language="C#" AutoEventWireup="true" Title="AMP filter" CodeBehind="Default.aspx.cs"
    Theme="Default" Inherits="CMSModules_AcceleratedMobilePages_Default"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="server">
    <div class="form-horizontal">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="LabelEnableAmp" runat="server" EnableViewState="false"
                    ResourceString="AcceleratedMobilePages.EditChkEnable" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSCheckBox runat="server" ID="chkEnableAmpFilter" AutoPostBack="true" OnCheckedChanged="chkEnableAmpFilter_OnCheckedChanged" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="LabelDefaultCss" runat="server" EnableViewState="false"
                    ResourceString="AcceleratedMobilePages.EditChkDefaultCss" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSCheckBox runat="server" ID="chkDefaultCss" AutoPostBack="true" OnCheckedChanged="chkDefaultCss_OnCheckedChanged" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="labelSelectCss" runat="server" EnableViewState="false"
                    ResourceString="AcceleratedMobilePages.EditCtrlCss" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <cms:FormControl runat="server" ID="selectStyleSheet" FormControlName="AcceleratedMobilePages.StylesheetSelector" />
            </div>
        </div>
    </div>
</asp:Content>