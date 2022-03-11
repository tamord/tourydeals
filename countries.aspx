<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="countries.aspx.cs" Inherits="MyTouristBook.countries" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p class="text-center">
        <br />
        </p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        Deals</p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        <asp:DropDownList ID="DropDownListCountry3" runat="server" Height="30px" Width="309px">
        </asp:DropDownList>
    </p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        <asp:TextBox ID="TextBox4" runat="server" Height="240px" TextMode="MultiLine" Width="454px"></asp:TextBox>
    </p>
    <p class="text-center">
        <asp:Button ID="Button5" runat="server" OnClick="submit4_click" Text="Submit" />
    </p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        Welcome Thread</p>
    <p class="text-center">
        Countries</p>
    <p class="text-center">
        <asp:DropDownList ID="DropDownListCountry2" runat="server" Height="30px" Width="309px">
        </asp:DropDownList>
    </p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        <asp:TextBox ID="TextBox3" runat="server" Height="240px" TextMode="MultiLine" Width="454px"></asp:TextBox>
    </p>
    <p class="text-center">
        <asp:Button ID="Button4" runat="server" OnClick="submit3_click" Text="Submit" />
    </p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        Cities</p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        <asp:DropDownList ID="DropDownListCountry" runat="server" Height="30px" Width="309px">
        </asp:DropDownList>
    </p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        <asp:TextBox ID="TextBox2" runat="server" Height="240px" TextMode="MultiLine" Width="454px"></asp:TextBox>
    </p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        <asp:Button ID="Button2" runat="server" OnClick="submit2_click" Text="Submit" />
&nbsp;&nbsp;
        <asp:Button ID="Button3" runat="server" OnClick="clear_click" Text="Clear" />
    </p>
    <p class="text-center">
        &nbsp;</p>
    <p class="text-center">
        Countries</p>
    <p class="text-center">
        <asp:TextBox ID="TextBox1" runat="server" Height="240px" TextMode="MultiLine" Width="454px"></asp:TextBox>
    </p>
    <p>
    </p>
    <p class="text-center">
        <asp:Button ID="Button1" runat="server" OnClick="submit_click" Text="Submit" />
    </p>
    <p>
    </p>
    <p>
    </p>
</asp:Content>
