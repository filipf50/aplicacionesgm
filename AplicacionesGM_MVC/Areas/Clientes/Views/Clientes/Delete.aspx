<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_Clientes>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Borra cliente
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Borrar cliente</h2>

    <h3>¿Está seguro de querer borrar este cliente?</h3>
    <fieldset>
            <legend>Cliente</legend>
            <div class="display-label">Empresas</div>
            <div class="display-field">
                <% AplicacionesGM_MVC.Models.CheckBoxModel chkEmpresas = (AplicacionesGM_MVC.Models.CheckBoxModel)ViewData["chkEmpresas"];
                   string strEmpresas = "";
                    foreach (var info in chkEmpresas.lstValores)
                    {
                        if (Model.Empresas != null)
                        {
                            if (Model.Empresas.Split(',').Contains(info.Key.ToString()))
                            {
                                if (strEmpresas.Length == 0)
                                {
                                    strEmpresas += info.Value;
                                }
                                else
                                {
                                    strEmpresas += ", " + info.Value;
                                }
                            }
                        }
                        %>
                <% } %>
                <%: strEmpresas %>
            </div>
            <div class="display-label">C.I.F / N.I.F</div>
            <div class="display-field"><%: Model.NIF %></div>
            <div class="display-label">Nombre</div>
            <div class="display-field"><%: Model.Nombre %></div>
            <div class="display-label">Domicilio</div>
            <div class="display-field">
                <% SelectList provincias = (SelectList)ViewData["Provincias"]; 
                   SelectList paises = (SelectList)ViewData["Paises"];%>
                <%: Model.TipoDeVia + " " + Model.Domicilio + ", " + String.Format("{0:0.##}", Model.Numero) + " " + Model.Piso + " " + Model.CP + " (" + Model.Municipio + ") " + ((Model.IDProvinciaQS == null) ? "" : provincias.Where(p => p.Value == Model.IDProvinciaQS.ToString()).First().Text) + " " + ((Model.IDPaisQS == null) ? "" : paises.Where(p => p.Value == Model.IDPaisQS.ToString()).First().Text) %></div>
        </fieldset>

 <% using (Html.BeginForm()) { %>
        <p>
		    <%: Html.ActionLink("Volver a la lista", "Index") %> |
            <input type="submit" value="Borrar" />		    
        </p>
    <% } %>
</asp:Content>
