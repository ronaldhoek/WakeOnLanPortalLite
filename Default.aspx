<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" 
    CodeBehind="Default.aspx.cs" Inherits="WakeOnLanLite.Default" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Overzicht beschikbare computers</h2>
    <p>
        &nbsp;Zie hier het overzicht van computers binnen het netwerk, welke wakker gemaakt kunnen worden.
    </p>
    <asp:ScriptManager ID="ScriptManager1" runat="server"/>
    <asp:UpdatePanel ID="UpdatePanelComputers" runat="server">
        <ContentTemplate>            
            <asp:GridView ID="gvComputers" runat="server" AutoGenerateColumns="False" 
                DataSourceID="odsComputers" onrowcommand="gvComputers_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Computer" SortExpression="Name" >
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Netbios" HeaderText="Naam" SortExpression="Netbios">
                    <HeaderStyle Width="200px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MAC" HeaderText="MAC-adres" SortExpression="MAC" >
                        <HeaderStyle Width="150px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastPingResult" HeaderText="Pingtijd" 
                        ReadOnly="True" SortExpression="LastPingResult" DataFormatString="{0:d}ms " >
                        <ItemStyle HorizontalAlign="Right" Width="65px" />
                    </asp:BoundField>
                    <asp:CheckBoxField DataField="Online" HeaderText="Online" ReadOnly="True" 
                        SortExpression="Online" >
                        <HeaderStyle Width="50px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:CheckBoxField>
                    <asp:ButtonField CommandName="action" DataTextField="ActionText" HeaderText="Actie" 
                        InsertVisible="False" >
                        <HeaderStyle Width="65px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:ButtonField>
                    <asp:ButtonField CommandName="rdpdownload" HeaderText="RDP" 
                        InsertVisible="False" Text="Download" />
                </Columns>
            </asp:GridView>
            <br />
            <asp:CheckBox ID="cbAutoRefresh" runat="server" 
                Checked="True" oncheckedchanged="cbAutoRefresh_CheckedChanged" 
                Text="Automatisch versen" />&nbsp;
            <asp:Button ID="btnRefresh" runat="server" onclick="btnRefreshClick" 
                Text="Nu verversen" />
            &nbsp;Laatst ververst: <asp:Label ID="lblRefreshtime" runat="server" Text="1:00:00"></asp:Label>
            <br />
            &nbsp;&nbsp;
            <asp:Label ID="lblErrors" runat="server" Text="-fouten-" ForeColor="Red" 
                Visible="False"></asp:Label>
            <asp:Label ID="lblMessage" runat="server" ForeColor="#000099" Text="-message-"></asp:Label>
            <asp:Timer ID="tmrRefreshComputers" runat="server" Interval="5000" 
                ontick="tmrRefreshComputers_Tick">
            </asp:Timer>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvComputers" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="odsComputers" runat="server" 
        SelectMethod="getMachines" TypeName="MachineList" 
        onselected="odsComputers_Selected" />
    <br />
</asp:Content>
