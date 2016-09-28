<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.AccountIndexViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Administración de Usuarios
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Administración de Usuarios</h2>
    <div id="iconos">
        <a href="<%: @Url.Action("Create","Account") %>"><img class="imgLink" title="Crear Nuevo Usuario" alt="NuevoUsuario" longdesc="Nuevo Usuario" src="../../Content/images/iconos/nuevo_registro.png"/></a>
    </div> 
    <div class="mvcMembership-allUsers">
    <% if(Model.Users.Count > 0){ %>
		<table>
            <tr class="textaling-center">
                <th>Usuario</th>
                <th>Email</th>
                <th>Estado</th>
                <th>Alias</th>
                <th></th>
            </tr>
			<% foreach(MembershipUser user in Model.Users){ %>
			<tr>
				<td><%: Html.Encode(user.UserName) %></td>
				<td><a href="mailto:<% =Html.Encode(user.Email) %>"><%: Html.Encode(user.Email) %></a></td>
				<td><% if(user.IsOnline){ %>
					    <span class="isOnline">Online</span>
				    <% }else{ %>
					    <span class="isOffline">Offline for
						    <%
							    var offlineSince = (DateTime.Now - user.LastActivityDate);
							    if (offlineSince.TotalSeconds <= 60) Response.Write("1 minute.");
							    else if(offlineSince.TotalMinutes < 60) Response.Write(Math.Floor(offlineSince.TotalMinutes) + " minutes.");
							    else if (offlineSince.TotalMinutes < 120) Response.Write("1 hour.");
							    else if (offlineSince.TotalHours < 24) Response.Write(Math.Floor(offlineSince.TotalHours) + " hours.");
							    else if (offlineSince.TotalHours < 48) Response.Write("1 day.");
							    else Response.Write(Math.Floor(offlineSince.TotalDays) + " days.");
						    %>
					    </span>
				    <% } %>
                </td>
                <td>
				    <% if(!string.IsNullOrEmpty(user.Comment)){ %>
					    <span class="comment"><% =Html.Encode(user.Comment) %></span>
				    <% } %>
                </td>
                <td><%: Html.ActionLink("Modificar", "Edit", new{ userName = user.UserName}) %></td>
			</tr>
			<% } %>
		</table>
     <% } %>
     </div>

</asp:Content>
