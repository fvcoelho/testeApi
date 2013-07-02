<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default"  %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to ASP.NET!
    </h2>
    <p>
        To learn more about ASP.NET visit <a href="http://www.asp.net" title="ASP.NET Website">www.asp.net</a>.
    </p>
    <p>
        <asp:TextBox ID="txtCidade" runat="server" TextMode="MultiLine" Width="564px"></asp:TextBox>
    </p>
    <p>
        <asp:Button ID="BtnAdd" runat="server" onclick="Add_Click" Text="Add Cidade" />
    </p>
<p>
        <asp:Button ID="BtnUpdate" runat="server" Text="Update Cidade" 
            onclick="BtnUpdate_Click" />
    </p>
    <p>
        ID
        <asp:TextBox ID="txtUpdateId" runat="server"></asp:TextBox>
    </p>
<p>
        <asp:Button ID="BtnDelete" runat="server" Text="Delete Cidade" />
    </p>
    <p>
        ID 
        <asp:TextBox ID="txtDeleteId" runat="server"></asp:TextBox>
    </p>
<p>
        <asp:Button ID="BtnGet" runat="server" Text="Get Cidade" />
    </p>
    <p>
        ID 
        <asp:TextBox ID="txtGetId" runat="server"></asp:TextBox>
    </p>
    <p>
        You can also find <a href="http://go.microsoft.com/fwlink/?LinkID=152368&amp;clcid=0x409"
            title="MSDN ASP.NET Docs">documentation on ASP.NET at MSDNN</a>.
    </p>
</asp:Content>
