<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
        Bienvenido <b><%: Page.User.Identity.Name %></b>!
        [ <%: Html.ActionLink("Cambiar Contraseña", "ChangePassword", "Account", new { area = "" },null)%> ] |
        [ <%: Html.ActionLink("Salir", "LogOff", "Account", new { area = "" }, null)%> ]
<%
    }
    else {
%> 
        [ <%: Html.ActionLink("Entrar", "LogOn", "Account", new { area = "" }, null)%> ]
<%
    }
%>
