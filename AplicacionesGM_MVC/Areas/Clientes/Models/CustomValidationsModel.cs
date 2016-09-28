using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class EmailAttribute : RegularExpressionAttribute
{
    public EmailAttribute() : base("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$") { }
}

public class CustomValidations
{
    private class NumeroNif
    {
        /// <summary>
        /// Tipos de Códigos.
        /// </summary>
        /// <remarks>Aunque actualmente no se utilice el término CIF, se usa en la enumeración
        /// por comodidad</remarks>
        private enum TiposCodigosEnum { NIF, NIE, CIF }

        // Número tal cual lo introduce el usuario
        private string numero;
        private TiposCodigosEnum tipo;

        /// <summary>
        /// Parte de Nif: En caso de ser un Nif intracomunitario, permite obtener el cógido del país
        /// </summary>
        public string CodigoIntracomunitario { get; internal set; }
        internal bool EsIntraComunitario { get; set; }

        /// <summary>
        /// Parte de Nif: Letra inicial del Nif, en caso de tenerla
        /// </summary>
        public string LetraInicial { get; internal set; }

        /// <summary>
        /// Parte de Nif: Bloque numérico del NIF. En el caso de un NIF de persona física,
        /// corresponderá al DNI
        /// </summary>
        public int Numero { get; internal set; }

        /// <summary>
        /// Parte de Nif: Dígito de control. Puede ser número o letra
        /// </summary>
        public string DigitoControl { get; internal set; }

        /// <summary>
        /// Valor que representa si el Nif introducido es correcto
        /// </summary>
        public bool EsCorrecto { get; internal set; }

        /// <summary>
        /// Cadena que representa el tipo de Nif comprobado:
        ///     - NIF : Número de identificación fiscal de persona física
        ///     - NIE : Número de identificación fiscal extranjería
        ///     - CIF : Código de identificación fiscal (Entidad jurídica)
        /// </summary>
        public string TipoNif { get { return tipo.ToString(); } }

        /// <summary>
        /// Constructor. Al instanciar la clase se realizan todos los cálculos
        /// </summary>
        /// <param name="numero">Cadena de 9 u 11 caracteres que contiene el DNI/NIF
        /// tal cual lo ha introducido el usuario para su verificación</param>
        private NumeroNif(string numero)
        {
            // Se eliminan los carácteres sobrantes
            numero = EliminaCaracteres(numero);

            // Todo en maýusculas
            numero = numero.ToUpper();

            // Comprobación básica de la cadena introducida por el usuario
            if (numero.Length != 9 && numero.Length != 11)
            {
                throw new ApplicationException("El NIF no tiene un número de caracteres válidos");
            }
            else
            {
                //correccion para los NIE, substitucion de la primera letra X,Y,Z por 0,1,2
                if (numero.Substring(0, 1).ToString() == "X")
                    numero = 0 + numero.Substring(1, 8);
                if (numero.Substring(0, 1).ToString() == "Y")
                    numero = 1 + numero.Substring(1, 8);
                if (numero.Substring(0, 1).ToString() == "Z")
                    numero = 2 + numero.Substring(1, 8);

                this.numero = numero;
                Desglosa();

                switch (tipo)
                {
                    case TiposCodigosEnum.NIF:
                    case TiposCodigosEnum.NIE:
                        this.EsCorrecto = CompruebaNif();
                        break;
                    case TiposCodigosEnum.CIF:
                        this.EsCorrecto = CompruebaCif();
                        break;
                }
            }
        }

        #region Preparación del número (desglose)

        /// <summary>
        /// Realiza un desglose del número introducido por el usuario en las propiedades
        /// de la clase
        /// </summary>
        private void Desglosa()
        {
            Int32 n;
            if (numero.Length == 11)
            {
                // Nif Intracomunitario
                EsIntraComunitario = true;
                CodigoIntracomunitario = numero.Substring(0, 2);
                LetraInicial = numero.Substring(2, 1);
                Int32.TryParse(numero.Substring(3, 7), out n);
                DigitoControl = numero.Substring(10, 1);
                tipo = GetTipoDocumento(LetraInicial[0]);
            }
            else
            {
                // Nif español
                tipo = GetTipoDocumento(numero[0]);
                EsIntraComunitario = false;
                if (tipo == TiposCodigosEnum.NIF)
                {
                    LetraInicial = string.Empty;
                    Int32.TryParse(numero.Substring(0, 8), out n);
                }
                else
                {
                    LetraInicial = numero.Substring(0, 1);
                    Int32.TryParse(numero.Substring(1, 7), out  n);
                }
                DigitoControl = numero.Substring(8, 1);
            }
            Numero = n;
        }

        /// <summary>
        /// En base al primer carácter del código, se obtiene el tipo de documento que se intenta
        /// comprobar
        /// </summary>
        /// <param name="letra">Primer carácter del número pasado</param>
        /// <returns>Tipo de documento</returns>
        private TiposCodigosEnum GetTipoDocumento(char letra)
        {
            Regex regexNumeros = new Regex("[0-9]");
            if (regexNumeros.IsMatch(letra.ToString()))
                return TiposCodigosEnum.NIF;

            Regex regexLetrasNIE = new Regex("[XYZ]");
            if (regexLetrasNIE.IsMatch(letra.ToString()))
                return TiposCodigosEnum.NIE;

            Regex regexLetrasCIF = new Regex("[ABCDEFGHJPQRSUVNW]");
            if (regexLetrasCIF.IsMatch(letra.ToString()))
                return TiposCodigosEnum.CIF;

            throw new ApplicationException("El código no es reconocible");
        }



        /// <summary>
        /// Eliminación de todos los carácteres no numéricos o de texto de la cadena
        /// </summary>
        /// <param name="numero">Número tal cual lo escribe el usuario</param>
        /// <returns>Cadena de 9 u 11 carácteres sin signos</returns>
        private string EliminaCaracteres(string numero)
        {
            // Todos los carácteres que no sean números o letras
            string caracteres = "[^\\w]";
            Regex regex = new Regex(caracteres);
            return regex.Replace(numero, "");
        }

        #endregion

        #region Cálculos

        private bool CompruebaNif()
        {
            return DigitoControl == GetLetraNif();
        }

        /// <summary>
        /// Cálculos para la comprobación del Cif (Entidad jurídica)
        /// </summary>
        private bool CompruebaCif()
        {
            string[] letrasCodigo = { "J", "A", "B", "C", "D", "E", "F", "G", "H", "I" };

            string n = Numero.ToString("0000000");
            Int32 sumaPares = 0;
            Int32 sumaImpares = 0;
            Int32 sumaTotal = 0;
            Int32 i = 0;
            bool retVal = false;

            // Recorrido por todos los dígitos del número
            for (i = 0; i < n.Length; i++)
            {
                Int32 aux;
                Int32.TryParse(n[i].ToString(), out aux);

                if ((i + 1) % 2 == 0)
                {
                    // Si es una posición par, se suman los dígitos
                    sumaPares += aux;
                }
                else
                {
                    // Si es una posición impar, se multiplican los dígitos por 2 
                    aux = aux * 2;

                    // se suman los dígitos de la suma
                    sumaImpares += SumaDigitos(aux);
                }
            }
            // Se suman los resultados de los números pares e impares
            sumaTotal += sumaPares + sumaImpares;

            // Se obtiene el dígito de las unidades
            Int32 unidades = sumaTotal % 10;

            // Si las unidades son distintas de 0, se restan de 10
            if (unidades != 0)
                unidades = 10 - unidades;

            switch (LetraInicial)
            {
                // Sólo números
                case "A":
                case "B":
                case "E":
                case "H":
                    retVal = DigitoControl == unidades.ToString();
                    break;

                // Sólo letras
                case "K":
                case "P":
                case "Q":
                case "S":
                    retVal = DigitoControl == letrasCodigo[unidades];
                    break;

                default:
                    retVal = (DigitoControl == unidades.ToString())
                            || (DigitoControl == letrasCodigo[unidades]);
                    break;
            }

            return retVal;

        }

        /// <summary>
        /// Obtiene la suma de todos los dígitos
        /// </summary>
        /// <returns>de 23, devuelve la suma de 2 + 3</returns>
        private Int32 SumaDigitos(Int32 digitos)
        {
            string sNumero = digitos.ToString();
            Int32 suma = 0;

            for (Int32 i = 0; i < sNumero.Length; i++)
            {
                Int32 aux;
                Int32.TryParse(sNumero[i].ToString(), out aux);
                suma += aux;
            }
            return suma;
        }

        /// <summary>
        /// Obtiene la letra correspondiente al Dni
        /// </summary>
        private string GetLetraNif()
        {
            int indice = Numero % 23;
            return "TRWAGMYFPDXBNJZSQVHLCKET"[indice].ToString();
        }

        /// <summary>
        /// Obtiene una cadena con el número de identificación completo
        /// </summary>
        public override string ToString()
        {
            string nif;
            string formato = "{0:0000000}";

            if (tipo == TiposCodigosEnum.CIF && LetraInicial == "")
                formato = "{0:00000000}";
            if (tipo == TiposCodigosEnum.NIF)
                formato = "{0:00000000}";

            nif = EsIntraComunitario ? CodigoIntracomunitario :
                string.Empty + LetraInicial + string.Format(formato, Numero) + DigitoControl;
            return nif;
        }

        #endregion

        /// <summary>
        /// Comprobación de un número de identificación fiscal español
        /// </summary>
        /// <param name="numero">Numero a analizar</param>
        /// <returns>Instancia de <see cref="NumeroNif"/> con los datos del número.
        /// Destacable la propiedad <seealso cref="NumeroNif.EsCorrecto"/>, que contiene la verificación
        /// </returns>
        public static NumeroNif CompruebaNif(string numero)
        {
            return new NumeroNif(numero);
        }

    }

    private bool ISLaendercode(string code)
    {

        if (code.Length != 2)
            return false;
        else
        {
            code = code.ToUpper();
            string[] Laendercodes = { "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI",
                "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV",
                "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI",
                "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV",
                "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI",
                "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV",
                "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI",
                "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV",
                "DW", "DX", "DY", "DZ", "EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI",
                "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV",
                "EW", "EX", "EY", "EZ", "FA", "FB", "FC", "FD", "FE", "FF", "FG", "FH", "FI",
                "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV",
                "FW", "FX", "FY", "FZ", "GA", "GB", "GC", "GD", "GE", "GF", "GG", "GH", "GI",
                "GJ", "GK", "GL", "GM", "GN", "GO", "GP", "GQ", "GR", "GS", "GT", "GU", "GV",
                "GW", "GX", "GY", "GZ", "HA", "HB", "HC", "HD", "HE", "HF", "HG", "HH", "HI",
                "HJ", "HK", "HL", "HM", "HN", "HO", "HP", "HQ", "HR", "HS", "HT", "HU", "HV",
                "HW", "HX", "HY", "HZ", "IA", "IB", "IC", "ID", "IE", "IF", "IG", "IH", "II",
                "IJ", "IK", "IL", "IM", "IN", "IO", "IP", "IQ", "IR", "IS", "IT", "IU", "IV",
                "IW", "IX", "IY", "IZ", "JA", "JB", "JC", "JD", "JE", "JF", "JG", "JH", "JI",
                "JJ", "JK", "JL", "JM", "JN", "JO", "JP", "JQ", "JR", "JS", "JT", "JU", "JV",
                "JW", "JX", "JY", "JZ", "KA", "KB", "KC", "KD", "KE", "KF", "KG", "KH", "KI",
                "KJ", "KK", "KL", "KM", "KN", "KO", "KP", "KQ", "KR", "KS", "KT", "KU", "KV",
                "KW", "KX", "KY", "KZ", "LA", "LB", "LC", "LD", "LE", "LF", "LG", "LH", "LI",
                "LJ", "LK", "LL", "LM", "LN", "LO", "LP", "LQ", "LR", "LS", "LT", "LU", "LV",
                "LW", "LX", "LY", "LZ", "MA", "MB", "MC", "MD", "ME", "MF", "MG", "MH", "MI",
                "MJ", "MK", "ML", "MM", "MN", "MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV",
                "MW", "MX", "MY", "MZ", "NA", "NB", "NC", "ND", "NE", "NF", "NG", "NH", "NI",
                "NJ", "NK", "NL", "NM", "NN", "NO", "NP", "NQ", "NR", "NS", "NT", "NU", "NV",
                "NW", "NX", "NY", "NZ", "OA", "OB", "OC", "OD", "OE", "OF", "OG", "OH", "OI",
                "OJ", "OK", "OL", "OM", "ON", "OO", "OP", "OQ", "OR", "OS", "OT", "OU", "OV",
                "OW", "OX", "OY", "OZ", "PA", "PB", "PC", "PD", "PE", "PF", "PG", "PH", "PI",
                "PJ", "PK", "PL", "PM", "PN", "PO", "PP", "PQ", "PR", "PS", "PT", "PU", "PV",
                "PW", "PX", "PY", "PZ", "QA", "QB", "QC", "QD", "QE", "QF", "QG", "QH", "QI",
                "QJ", "QK", "QL", "QM", "QN", "QO", "QP", "QQ", "QR", "QS", "QT", "QU", "QV",
                "QW", "QX", "QY", "QZ", "RA", "RB", "RC", "RD", "RE", "RF", "RG", "RH", "RI",
                "RJ", "RK", "RL", "RM", "RN", "RO", "RP", "RQ", "RR", "RS", "RT", "RU", "RV",
                "RW", "RX", "RY", "RZ", "SA", "SB", "SC", "SD", "SE", "SF", "SG", "SH", "SI",
                "SJ", "SK", "SL", "SM", "SN", "SO", "SP", "SQ", "SR", "SS", "ST", "SU", "SV",
                "SW", "SX", "SY", "SZ", "TA", "TB", "TC", "TD", "TE", "TF", "TG", "TH", "TI",
                "TJ", "TK", "TL", "TM", "TN", "TO", "TP", "TQ", "TR", "TS", "TT", "TU", "TV",
                "TW", "TX", "TY", "TZ", "UA", "UB", "UC", "UD", "UE", "UF", "UG", "UH", "UI",
                "UJ", "UK", "UL", "UM", "UN", "UO", "UP", "UQ", "UR", "US", "UT", "UU", "UV",
                "UW", "UX", "UY", "UZ", "VA", "VB", "VC", "VD", "VE", "VF", "VG", "VH", "VI",
                "VJ", "VK", "VL", "VM", "VN", "VO", "VP", "VQ", "VR", "VS", "VT", "VU", "VV",
                "VW", "VX", "VY", "VZ", "WA", "WB", "WC", "WD", "WE", "WF", "WG", "WH", "WI",
                "WJ", "WK", "WL", "WM", "WN", "WO", "WP", "WQ", "WR", "WS", "WT", "WU", "WV",
                "WW", "WX", "WY", "WZ", "XA", "XB", "XC", "XD", "XE", "XF", "XG", "XH", "XI",
                "XJ", "XK", "XL", "XM", "XN", "XO", "XP", "XQ", "XR", "XS", "XT", "XU", "XV",
                "XW", "XX", "XY", "XZ", "YA", "YB", "YC", "YD", "YE", "YF", "YG", "YH", "YI",
                "YJ", "YK", "YL", "YM", "YN", "YO", "YP", "YQ", "YR", "YS", "YT", "YU", "YV",
                "YW", "YX", "YY", "YZ", "ZA", "ZB", "ZC", "ZD", "ZE", "ZF", "ZG", "ZH", "ZI",
                "ZJ", "ZK", "ZL", "ZM", "ZN", "ZO", "ZP", "ZQ", "ZR", "ZS", "ZT", "ZU", "ZV",
                "ZW", "ZX", "ZY", "ZZ" };

            if (Array.IndexOf(Laendercodes, code) == -1)
                return false;
            else
                return true;
        }
    }

    private string IBANCleaner(string sIBAN)
    {
        for (int x = 65; x < 90; x++)
        {
            int replacewith = x - 64 + 9;
            string replace = ((char)x).ToString();
            sIBAN = sIBAN.Replace(replace, replacewith.ToString());
        }
        return sIBAN;
    }

    private int Modulo(string sModulus, int iTeiler)
      {
         int iStart,iEnde,iErgebniss,iRestTmp,iBuffer;
         string iRest = "",sErg = "";

         iStart = 0;
         iEnde = 0;

        while (iEnde <= sModulus.Length - 1)
        {
            iBuffer = int.Parse(iRest + sModulus.Substring(iStart, iEnde - iStart + 1));

            if (iBuffer >= iTeiler)
            {
                iErgebniss = iBuffer / iTeiler;
                iRestTmp = iBuffer - iErgebniss * iTeiler;
                iRest = iRestTmp.ToString();

                sErg = sErg + iErgebniss.ToString();

                iStart = iEnde + 1;
                iEnde = iStart;
            }
            else
            {
                if (sErg != "")
                    sErg = sErg + "0";

                iEnde = iEnde + 1;
            }
        }

        if (iStart <= sModulus.Length)
            iRest = iRest + sModulus.Substring(iStart);

        return int.Parse(iRest);
    }

    private bool IsNumeric(string value)
    {
        try
        {
            int.Parse(value);
            return (true);
        }
        catch
        {
            return (false);
        }
    }		    


    public bool IsValidNIF(object value,ref string ErrorMessage)
    {
        try
        {
            NumeroNif objValidadorNIF = NumeroNif.CompruebaNif(Convert.ToString(value));
            return objValidadorNIF.EsCorrecto;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return false;
        }
    }

    public bool isValidEMail(object value, ref string ErrorMessage)
    {
        bool blnRes = true;
        try
        {
            if (!Regex.IsMatch(Convert.ToString(value), "^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$"))
            {
                blnRes = false;
                ErrorMessage = "El mail indicado no es correcto";
            }

            return blnRes;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return false;
        }
    }

    public bool isDecimalInRange(object value, decimal startRange, decimal endRange, ref string ErrorMessage)
    {
        bool blnRes = true;
        decimal decValue;
        try
        {
            string  strErrorMessage = "El valor debe de estar entre " + startRange + " y " + endRange;

            if (decimal.TryParse(value.ToString(), out decValue))
            {
                if (decValue < startRange || decValue > endRange)
                {
                    ErrorMessage = strErrorMessage;
                    blnRes = false;
                }
            }
            else
            {
                ErrorMessage = strErrorMessage;
                blnRes = false;
            }

            return blnRes;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return false;
        }
    }

    public bool isValidIBAN(object value, ref string ErrorMessage)
    {
        bool blnRes = true;

        string mysIBAN = (value ?? "").ToString().Replace(" ", "");

        if (mysIBAN.Length > 34 || mysIBAN.Length < 5)
            blnRes = false;
        else
        {
            string LaenderCode = mysIBAN.Substring(0, 2).ToUpper();
            string Pruefsumme = mysIBAN.Substring(2, 2).ToUpper();
            string BLZ_Konto = mysIBAN.Substring(4).ToUpper();

            if (!IsNumeric(Pruefsumme))
                return false; //No es numérico

            if (!ISLaendercode(LaenderCode))
                return false;

            string Umstellung = BLZ_Konto + LaenderCode + "00";
            string Modulus = IBANCleaner(Umstellung);

            if (98 - Modulo(Modulus, 97) != int.Parse(Pruefsumme))
                blnRes = false;  //No es Numerico
        }
        if (!blnRes){
            ErrorMessage="El IBAN introducino no es correcto.";
        }
        return blnRes;
    }    
}

