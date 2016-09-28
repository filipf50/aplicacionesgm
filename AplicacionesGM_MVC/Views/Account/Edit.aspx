<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.AccountEditViewModel>" %>
<%@ Import Namespace="System.Globalization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	User Details: <% =Html.Encode(Model.DisplayName) %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<form id="form1" runat="server">

	<link href='<% =Url.Content("~/Content/MvcMembership.css") %>' rel="stylesheet" type="text/css" />

    <h2>Datos del Usuario: <% =Html.Encode(Model.DisplayName) %> [<% =Model.Status %>]</h2>

	<h3>Cuenta de Usuario</h3>
	<div class="mvcMembership-account">
		<fieldset>
            <dl>
			    <dt>Nombre de usuario:</dt>
				    <dd><% =Html.Encode(Model.User.UserName) %></dd>
			    <% if(Model.User.LastActivityDate == Model.User.CreationDate){ %>
			    <dt>Última actividad:</dt>
				    <dd><em>Never</em></dd>
			    <dt>Último acceso:</dt>
				    <dd><em>Never</em></dd>
			    <% }else{ %>
			    <dt>Última actividad:</dt>
				    <dd><% =Model.User.LastActivityDate.ToString("dd MMMM yyyy H:mm:ss tt", CultureInfo.CurrentCulture)%></dd>
			    <dt>Último acceso:</dt>
				    <dd><% =Model.User.LastLoginDate.ToString("dd MMMM yyyy H:mm:ss tt", CultureInfo.CurrentCulture)%></dd>
			    <% } %>
			    <dt>Fecha de creación:</dt>
				    <dd><% =Model.User.CreationDate.ToString("dd MMMM yyyy H:mm:ss tt", CultureInfo.CurrentCulture)%></dd>
		    </dl>

		    <% using(Html.BeginForm("ChangeApproval", "Account", new{ userName = Model.User.UserName })){ %>
			    <% =Html.Hidden("isApproved", !Model.User.IsApproved) %>
			    <input type="submit" value='<% =(Model.User.IsApproved ? "Desaprobar" : "Aprobar") %> Cuenta' />
		    <% } %>
        </fieldset>
	</div>
    
    <h3>Código de Agente en QS</h3>
	<div class="mvcMembership-emailAndComments">
        <%  using (Html.BeginForm("GuardarAgenteQS", "Account", new { userName = Model.User.UserName }))
            {
                %>
		<fieldset>
			<p>
                <dl>
                    <dt><label for="IDAgenteQSMV">Cód. Agente MV:</label></dt>
				    <dd><%: Html.DropDownList("IDAgenteQSMV", (SelectList)ViewData["AgentesMV"], "--Seleccione un valor--", new { @class = "textbox30" })%></dd>
                    <dt><label for="IDAgenteQSHMA">Cód. Agente HMA:</label></dt>
				    <dd><%: Html.DropDownList("IDAgenteQSHMA", (SelectList)ViewData["AgentesHMA"], "--Seleccione un valor--", new { @class = "textbox30" })%></dd>
                    <dt><label for="IDAgenteQSECA">Cód. Agente ECA:</label></dt>
				    <dd><%: Html.DropDownList("IDAgenteQSECA", (SelectList)ViewData["AgentesECA"], "--Seleccione un valor--", new { @class = "textbox30" })%></dd>
                </dl>
			</p>
			<input type="submit" value="Guardar código de Agente" />
		</fieldset>
		<% } %>
	</div>

	<h3>Dirección de Email y observaciones</h3>
	<div class="mvcMembership-emailAndComments">
		<% using(Html.BeginForm("Edit", "Account", new{ userName = Model.User.UserName })){ %>
		<fieldset>
			<p>
				<label for="E-Mail">E-Mail:</label>
				<%: Html.TextBox("E-Mail",Model.User.Email) %>
			</p>
			<p>
				<label for="Comentarios">Comentarios:</label>
				<%: Html.TextBox("Comentarios",Model.User.Comment)%>
			</p>
			<input type="submit" value="Guardar E-Mail y Comentarios" />
		</fieldset>
		<% } %>
	</div>

	<h3>Contraseña</h3>
	<div class="mvcMembership-password">
		<fieldset>
            <% if(Model.User.IsLockedOut){ %>
			    <p>Cuenta bloqueada desde <% =Model.User.LastLockoutDate.ToString("dd MMMM yyyy H:mm:ss tt", CultureInfo.CurrentCulture)%></p>
			    <% using(Html.BeginForm("Unlock", "Account", new{ userName = Model.User.UserName })){ %>
			    <input type="submit" value="Desbloquear Cuenta" />
			    <% } %>
		    <% }else{ %>

			    <% if(Model.User.LastPasswordChangedDate == Model.User.CreationDate){ %>
			    <dl>
				    <dt>Última modificación:</dt>
				    <dd><em>Never</em></dd>
			    </dl>
			    <% }else{ %>
			    <dl>
				    <dt>Última modificación:</dt>
				    <dd><% =Model.User.LastPasswordChangedDate.ToString("dd MMMM yyyy H:mm:ss tt", CultureInfo.CurrentCulture)%></dd>
			    </dl>
			    <% } %>

			<% using(Html.BeginForm("ResetPassword", "Account", new{ userName = Model.User.UserName})){ %>
			
				<p>
					<dl>
						<dt>Pregunta para la contraseña:</dt>
						<% if(string.IsNullOrEmpty(Model.User.PasswordQuestion) || string.IsNullOrEmpty(Model.User.PasswordQuestion.Trim())){ %>
						<dd><em>No tiene pregunta defina.</em></dd>
						<% }else{ %>
						<dd><% =Html.Encode(Model.User.PasswordQuestion) %></dd>
						<% } %>
					</dl>
				</p>
				<p>
					<label for="answer">Nueva contraseña</label>
					<% =Html.TextBox("nuevaContraseña") %>
				</p>
				<input type="submit" value="Regenerar contraseña" />
			<% } %>
		<% } %>
		</fieldset>
	</div>

	<h3>Roles</h3>
	<div class="mvcMembership-userRoles">
		<fieldset>
            <ul>
			    <% foreach(var role in Model.Roles){ %>
			    <li>
				    <% if(role.Value){ %>
					    <% using(Html.BeginForm("RemoveFromRole", "Account", new{userName = Model.User.UserName, role = role.Key})){ %>
					    <input type="submit" value="Quitar" />
					    <% } %>
				    <% }else{ %>
					    <% using(Html.BeginForm("AddToRole", "Account", new{userName = Model.User.UserName, role = role.Key})){ %>
					    <input type="submit" value="Asignar" />
					    <% } %>
				    <% } %>
                    <% = role.Key.ToString() %>				
			    </li>
			    <% } %>
		    </ul>
        </fieldset>
	</div>
    <div>
        <h3>Puede ver acciones de los usuarios ...</h3>
        <fieldset>
            <%using (Html.BeginForm("GuardarSubordinados", "Account", new { userName = Model.User.UserName }))
              { %>
                <div class="editor-field checkboxlist">
                    <% AplicacionesGM_MVC.Models.CheckBoxModel chkSubordinados = (AplicacionesGM_MVC.Models.CheckBoxModel)Model.Subordinados;
                       ProfileBase profile = ProfileBase.Create(Model.User.UserName);
                       foreach (var info in chkSubordinados.lstValores)
                       {
                           bool blnChecked = false;
                           if (profile["Subordinados"].ToString().Contains(info.Key.ToString()))
                           {
                               blnChecked = true;
                           }
                           %>
                                <div>
                                    <%: Html.CheckBox("arrSubordinados", blnChecked, new { value = info.Key.ToString() })%>
                                    <%: info.Value%>
                                </div>
                    <%    } %>
                </div>
                <input type="submit" value="Guadar Subordinados" />
            <% } %>
        </fieldset>
    </div>
    <div>
        <h3>Puede ver acciones de los orígenes ...</h3>
        <fieldset>
            <%using (Html.BeginForm("GuardarOrigenes", "Account", new { userName = Model.User.UserName }))
              { %>
                <div class="editor-field checkboxlist">
                    <% AplicacionesGM_MVC.Models.CheckBoxModel chkOrigenes = (AplicacionesGM_MVC.Models.CheckBoxModel)Model.Origenes;
                       ProfileBase profile = ProfileBase.Create(Model.User.UserName);
                       foreach (var info in chkOrigenes.lstValores)
                       {
                           bool blnChecked = false;
                           if (profile["Origenes"].ToString().Contains(info.Key.ToString()))
                           {
                               blnChecked = true;
                           }
                           %>
                                <div>
                                    <%: Html.CheckBox("arrOrigenes", blnChecked, new { value = info.Key.ToString() })%>
                                    <%: info.Value%>
                                </div>
                    <%    } %>
                </div>
                <input type="submit" value="Guadar Orígenes" />
            <% } %>
        </fieldset>
    </div>
    <div>
        <h3>Puede accedera a las aplicaciones ...</h3>
        <fieldset>
            <%using (Html.BeginForm("GuardarAplicaciones", "Account", new { userName = Model.User.UserName }))
              { %>
                <div class="editor-field checkboxlist">
                    <% AplicacionesGM_MVC.Models.CheckBoxModel chkAplicaciones = (AplicacionesGM_MVC.Models.CheckBoxModel)Model.Aplicaciones;
                       ProfileBase profile = ProfileBase.Create(Model.User.UserName);
                       foreach (var info in chkAplicaciones.lstValores)
                       {
                           bool blnChecked = false;
                           if (profile["AplicacionesGM"].ToString().Contains(info.Key.ToString()))
                           {
                               blnChecked = true;
                           }
                           %>
                                <div>
                                    <%: Html.CheckBox("arrAplicacionesGM", blnChecked, new { value = info.Key.ToString() })%>
                                    <%: info.Value%>
                                </div>
                    <%    } %>
                </div>
                <input type="submit" value="Guadar Aplicaciones" />
            <% } %>
        </fieldset>
    </div>
    </form>
    

</asp:Content>

