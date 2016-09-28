<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Aplicaciones GM
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <p style="margin:auto; text-align:center; margin-top:100px;">
        Introduzca su usuario y contraseña. <%: Html.ActionLink("Registrese", "Register")%> si usted no tiene una cuenta.
    </p>
    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset class="login">
                <%: Html.ValidationSummary(true, "Acceso incorrecto. Por favor, corriga los errores y vuelva a intentarlo.") %>
        
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.UserName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.UserName) %>
                    <%: Html.ValidationMessageFor(m => m.UserName) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Password) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password) %>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.CheckBoxFor(m => m.RememberMe) %>
                    <%: Html.LabelFor(m => m.RememberMe) %>
                </div>
                
                <p>
                    <input type="submit" value="Acceder" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
