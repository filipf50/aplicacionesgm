<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Aplicaciones Grupo Mora
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!--<h2><%: ViewData["Message"] %> </h2>-->
       <% ProfileBase profile = ProfileBase.Create(Page.User.Identity.Name);
          string widthClass="width-50";
          if (profile["AplicacionesGM"].ToString().Split(',').Length > 2){
               widthClass="width-30";
          }%>
       <div style="margin:auto; text-align:center; margin-top:100px;">
        <% if (profile["AplicacionesGM"].ToString().Split(',').Contains("2"))
           { %>
                <div id="Clientes" class="<%: widthClass %>">
                    <a href="<%: @Url.Action("Index","Clientes", new{area="Clientes"}) %>">&nbsp;&nbsp;&nbsp;&nbsp;<img id="imgClientes"alt="Gestión de clientes" longdesc="Gestión de Clientes" src="../../Content/images/iconos/informes.jpg"/><br />Gestión de Clientes</a>
                </div>
        <%}
        if (profile["AplicacionesGM"].ToString().Split(',').Contains("1"))
           { %>
                <div id="Reuniones" class="<%: widthClass %>">
                    <a href="<%: @Url.Action("Index","Acciones", new {area="Reuniones"})%>">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img id="imgReuniones" alt="Gestión de reuniones" longdesc="Gestión de Reuniones" src="../../Content/images/iconos/entrar_a_reunion.jpg"/><br />Gestión de Reuniones</a>
                </div> 
        <%}
        if (profile["AplicacionesGM"].ToString().Split(',').Contains("3"))
            { %>       
                <div id="Proveedores" class="<%: widthClass %>">
                    <a href="<%: @Url.Action("Index","Incidencias", new {area="Proveedores"})%>">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img id="imgProveedores" alt="Gestión de Proveedores" longdesc="Gestión de Proveedores" src="../../Content/images/iconos/proveedores.png"/><br />Gestión de Proveedores</a>
                </div>        
        <%} %>
      </div>  
</asp:Content>
