<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.RegisterModel>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Crear nuevo usuario
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Crear nuevo usuario</h2>
    <p>
        Rellene el siguient formulario para crear su cuenta de usuario 
    </p>
    <p>
        El password debe de tener un mínimo de <%: ViewData["PasswordLength"] %> caracteres
    </p>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "El usuario no ha podido crearse correctamente. Por favor, corriga las incidencias y vuelva a intentarlo") %>
        <div>
            <fieldset>
                <legend>Información de usuario</legend>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.UserName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.UserName) %>
                    <%: Html.ValidationMessageFor(m => m.UserName) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Email) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Email) %>
                    <%: Html.ValidationMessageFor(m => m.Email) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Password) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password) %>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.ConfirmPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.ConfirmPassword) %>
                    <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </div>
                
                <p>
                    <input type="submit" value="Guardar" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
