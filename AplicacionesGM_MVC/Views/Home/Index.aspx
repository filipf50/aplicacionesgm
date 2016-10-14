<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Aplicaciones Grupo Mora
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!--<h2><%: ViewData["Message"] %> </h2>-->
       <div style="margin:auto; text-align:center; margin-top:100px;">
        <div id="Clientes">
            <a href="<%: @Url.Action("Index","Clientes", new{area="Clientes"}) %>">&nbsp;&nbsp;&nbsp;&nbsp;<img id="imgClientes"alt="Clientes" longdesc="Gestion de Clientes" src="../../Content/images/iconos/informes.jpg"/><br />Gestión de Clientes</a>
        </div>
        <div id="Reuniones">
            <a href="<%: @Url.Action("Index","Acciones", new {area="Reuniones"})%>">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img id="imgReuniones" alt="Gestion de reuniones" longdesc="Gestion de Reuniones" src="../../Content/images/iconos/entrar_a_reunion.jpg"/><br />Gestión de Reuniones</a>
        </div>
        
      </div>  
</asp:Content>
