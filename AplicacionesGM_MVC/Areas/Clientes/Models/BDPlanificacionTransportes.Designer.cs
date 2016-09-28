﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace AplicacionesGM_MVC.Areas.Clientes.Models
{
    #region Contextos
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    public partial class BDPlanificacionTransportesEntities : ObjectContext
    {
        #region Constructores
    
        /// <summary>
        /// Inicializa un nuevo objeto BDPlanificacionTransportesEntities usando la cadena de conexión encontrada en la sección 'BDPlanificacionTransportesEntities' del archivo de configuración de la aplicación.
        /// </summary>
        public BDPlanificacionTransportesEntities() : base("name=BDPlanificacionTransportesEntities", "BDPlanificacionTransportesEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Inicializar un nuevo objeto BDPlanificacionTransportesEntities.
        /// </summary>
        public BDPlanificacionTransportesEntities(string connectionString) : base(connectionString, "BDPlanificacionTransportesEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Inicializar un nuevo objeto BDPlanificacionTransportesEntities.
        /// </summary>
        public BDPlanificacionTransportesEntities(EntityConnection connection) : base(connection, "BDPlanificacionTransportesEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Métodos parciales
    
        partial void OnContextCreated();
    
        #endregion
    
        #region Propiedades de ObjectSet
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        public ObjectSet<Excepciones_cliente> Excepciones_cliente
        {
            get
            {
                if ((_Excepciones_cliente == null))
                {
                    _Excepciones_cliente = base.CreateObjectSet<Excepciones_cliente>("Excepciones_cliente");
                }
                return _Excepciones_cliente;
            }
        }
        private ObjectSet<Excepciones_cliente> _Excepciones_cliente;
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        public ObjectSet<Maestro_Delegaciones> Maestro_Delegaciones
        {
            get
            {
                if ((_Maestro_Delegaciones == null))
                {
                    _Maestro_Delegaciones = base.CreateObjectSet<Maestro_Delegaciones>("Maestro_Delegaciones");
                }
                return _Maestro_Delegaciones;
            }
        }
        private ObjectSet<Maestro_Delegaciones> _Maestro_Delegaciones;

        #endregion

        #region Métodos AddTo
    
        /// <summary>
        /// Método desusado para agregar un nuevo objeto al EntitySet Excepciones_cliente. Considere la posibilidad de usar el método .Add de la propiedad ObjectSet&lt;T&gt; asociada.
        /// </summary>
        public void AddToExcepciones_cliente(Excepciones_cliente excepciones_cliente)
        {
            base.AddObject("Excepciones_cliente", excepciones_cliente);
        }
    
        /// <summary>
        /// Método desusado para agregar un nuevo objeto al EntitySet Maestro_Delegaciones. Considere la posibilidad de usar el método .Add de la propiedad ObjectSet&lt;T&gt; asociada.
        /// </summary>
        public void AddToMaestro_Delegaciones(Maestro_Delegaciones maestro_Delegaciones)
        {
            base.AddObject("Maestro_Delegaciones", maestro_Delegaciones);
        }

        #endregion

    }

    #endregion

    #region Entidades
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="PlanificacionTransportesModel", Name="Excepciones_cliente")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Excepciones_cliente : EntityObject
    {
        #region Método de generador
    
        /// <summary>
        /// Crear un nuevo objeto Excepciones_cliente.
        /// </summary>
        /// <param name="dELEGACION">Valor inicial de la propiedad DELEGACION.</param>
        /// <param name="codigoQS">Valor inicial de la propiedad codigoQS.</param>
        public static Excepciones_cliente CreateExcepciones_cliente(global::System.Int32 dELEGACION, global::System.Int32 codigoQS)
        {
            Excepciones_cliente excepciones_cliente = new Excepciones_cliente();
            excepciones_cliente.DELEGACION = dELEGACION;
            excepciones_cliente.codigoQS = codigoQS;
            return excepciones_cliente;
        }

        #endregion

        #region Propiedades primitivas
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 DELEGACION
        {
            get
            {
                return _DELEGACION;
            }
            set
            {
                if (_DELEGACION != value)
                {
                    OnDELEGACIONChanging(value);
                    ReportPropertyChanging("DELEGACION");
                    _DELEGACION = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("DELEGACION");
                    OnDELEGACIONChanged();
                }
            }
        }
        private global::System.Int32 _DELEGACION;
        partial void OnDELEGACIONChanging(global::System.Int32 value);
        partial void OnDELEGACIONChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 codigoQS
        {
            get
            {
                return _codigoQS;
            }
            set
            {
                if (_codigoQS != value)
                {
                    OncodigoQSChanging(value);
                    ReportPropertyChanging("codigoQS");
                    _codigoQS = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("codigoQS");
                    OncodigoQSChanged();
                }
            }
        }
        private global::System.Int32 _codigoQS;
        partial void OncodigoQSChanging(global::System.Int32 value);
        partial void OncodigoQSChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String cliente
        {
            get
            {
                return _cliente;
            }
            set
            {
                OnclienteChanging(value);
                ReportPropertyChanging("cliente");
                _cliente = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("cliente");
                OnclienteChanged();
            }
        }
        private global::System.String _cliente;
        partial void OnclienteChanging(global::System.String value);
        partial void OnclienteChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> camionConPluma
        {
            get
            {
                return _camionConPluma;
            }
            set
            {
                OncamionConPlumaChanging(value);
                ReportPropertyChanging("camionConPluma");
                _camionConPluma = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("camionConPluma");
                OncamionConPlumaChanged();
            }
        }
        private Nullable<global::System.Boolean> _camionConPluma;
        partial void OncamionConPlumaChanging(Nullable<global::System.Boolean> value);
        partial void OncamionConPlumaChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int16> tipoCamion
        {
            get
            {
                return _tipoCamion;
            }
            set
            {
                OntipoCamionChanging(value);
                ReportPropertyChanging("tipoCamion");
                _tipoCamion = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("tipoCamion");
                OntipoCamionChanged();
            }
        }
        private Nullable<global::System.Int16> _tipoCamion;
        partial void OntipoCamionChanging(Nullable<global::System.Int16> value);
        partial void OntipoCamionChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int16> medioDescarga
        {
            get
            {
                return _medioDescarga;
            }
            set
            {
                OnmedioDescargaChanging(value);
                ReportPropertyChanging("medioDescarga");
                _medioDescarga = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("medioDescarga");
                OnmedioDescargaChanged();
            }
        }
        private Nullable<global::System.Int16> _medioDescarga;
        partial void OnmedioDescargaChanging(Nullable<global::System.Int16> value);
        partial void OnmedioDescargaChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> noAbiertoTardes
        {
            get
            {
                return _noAbiertoTardes;
            }
            set
            {
                OnnoAbiertoTardesChanging(value);
                ReportPropertyChanging("noAbiertoTardes");
                _noAbiertoTardes = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("noAbiertoTardes");
                OnnoAbiertoTardesChanged();
            }
        }
        private Nullable<global::System.Boolean> _noAbiertoTardes;
        partial void OnnoAbiertoTardesChanging(Nullable<global::System.Boolean> value);
        partial void OnnoAbiertoTardesChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String horario
        {
            get
            {
                return _horario;
            }
            set
            {
                OnhorarioChanging(value);
                ReportPropertyChanging("horario");
                _horario = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("horario");
                OnhorarioChanged();
            }
        }
        private global::System.String _horario;
        partial void OnhorarioChanging(global::System.String value);
        partial void OnhorarioChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String horarioVerano
        {
            get
            {
                return _horarioVerano;
            }
            set
            {
                OnhorarioVeranoChanging(value);
                ReportPropertyChanging("horarioVerano");
                _horarioVerano = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("horarioVerano");
                OnhorarioVeranoChanged();
            }
        }
        private global::System.String _horarioVerano;
        partial void OnhorarioVeranoChanging(global::System.String value);
        partial void OnhorarioVeranoChanged();

        #endregion

    
    }
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="PlanificacionTransportesModel", Name="Maestro_Delegaciones")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Maestro_Delegaciones : EntityObject
    {
        #region Método de generador
    
        /// <summary>
        /// Crear un nuevo objeto Maestro_Delegaciones.
        /// </summary>
        /// <param name="dELEGACION">Valor inicial de la propiedad DELEGACION.</param>
        public static Maestro_Delegaciones CreateMaestro_Delegaciones(global::System.Int32 dELEGACION)
        {
            Maestro_Delegaciones maestro_Delegaciones = new Maestro_Delegaciones();
            maestro_Delegaciones.DELEGACION = dELEGACION;
            return maestro_Delegaciones;
        }

        #endregion

        #region Propiedades primitivas
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 DELEGACION
        {
            get
            {
                return _DELEGACION;
            }
            set
            {
                if (_DELEGACION != value)
                {
                    OnDELEGACIONChanging(value);
                    ReportPropertyChanging("DELEGACION");
                    _DELEGACION = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("DELEGACION");
                    OnDELEGACIONChanged();
                }
            }
        }
        private global::System.Int32 _DELEGACION;
        partial void OnDELEGACIONChanging(global::System.Int32 value);
        partial void OnDELEGACIONChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String NOMBRE
        {
            get
            {
                return _NOMBRE;
            }
            set
            {
                OnNOMBREChanging(value);
                ReportPropertyChanging("NOMBRE");
                _NOMBRE = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("NOMBRE");
                OnNOMBREChanged();
            }
        }
        private global::System.String _NOMBRE;
        partial void OnNOMBREChanging(global::System.String value);
        partial void OnNOMBREChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String BIBLIOTECA
        {
            get
            {
                return _BIBLIOTECA;
            }
            set
            {
                OnBIBLIOTECAChanging(value);
                ReportPropertyChanging("BIBLIOTECA");
                _BIBLIOTECA = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("BIBLIOTECA");
                OnBIBLIOTECAChanged();
            }
        }
        private global::System.String _BIBLIOTECA;
        partial void OnBIBLIOTECAChanging(global::System.String value);
        partial void OnBIBLIOTECAChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> DESDEALMACEN
        {
            get
            {
                return _DESDEALMACEN;
            }
            set
            {
                OnDESDEALMACENChanging(value);
                ReportPropertyChanging("DESDEALMACEN");
                _DESDEALMACEN = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("DESDEALMACEN");
                OnDESDEALMACENChanged();
            }
        }
        private Nullable<global::System.Int32> _DESDEALMACEN;
        partial void OnDESDEALMACENChanging(Nullable<global::System.Int32> value);
        partial void OnDESDEALMACENChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> HASTAALMACEN
        {
            get
            {
                return _HASTAALMACEN;
            }
            set
            {
                OnHASTAALMACENChanging(value);
                ReportPropertyChanging("HASTAALMACEN");
                _HASTAALMACEN = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("HASTAALMACEN");
                OnHASTAALMACENChanged();
            }
        }
        private Nullable<global::System.Int32> _HASTAALMACEN;
        partial void OnHASTAALMACENChanging(Nullable<global::System.Int32> value);
        partial void OnHASTAALMACENChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String EMPRESA
        {
            get
            {
                return _EMPRESA;
            }
            set
            {
                OnEMPRESAChanging(value);
                ReportPropertyChanging("EMPRESA");
                _EMPRESA = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("EMPRESA");
                OnEMPRESAChanged();
            }
        }
        private global::System.String _EMPRESA;
        partial void OnEMPRESAChanging(global::System.String value);
        partial void OnEMPRESAChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> DESDEZONA
        {
            get
            {
                return _DESDEZONA;
            }
            set
            {
                OnDESDEZONAChanging(value);
                ReportPropertyChanging("DESDEZONA");
                _DESDEZONA = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("DESDEZONA");
                OnDESDEZONAChanged();
            }
        }
        private Nullable<global::System.Int32> _DESDEZONA;
        partial void OnDESDEZONAChanging(Nullable<global::System.Int32> value);
        partial void OnDESDEZONAChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> HASTAZONA
        {
            get
            {
                return _HASTAZONA;
            }
            set
            {
                OnHASTAZONAChanging(value);
                ReportPropertyChanging("HASTAZONA");
                _HASTAZONA = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("HASTAZONA");
                OnHASTAZONAChanged();
            }
        }
        private Nullable<global::System.Int32> _HASTAZONA;
        partial void OnHASTAZONAChanging(Nullable<global::System.Int32> value);
        partial void OnHASTAZONAChanged();

        #endregion

    
    }

    #endregion

    
}
