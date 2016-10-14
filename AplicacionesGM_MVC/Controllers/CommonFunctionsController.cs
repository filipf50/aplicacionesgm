using System.Data.SqlTypes;

public class CommonFunctions
{
    public bool DecimalValidation(string strValue, int precisionConst, int scaleConst)
    {
        bool blnRes = false;
        decimal decValue = 0;
        int precision = 0, scale = 0;
        //numeric verification
        if (!string.IsNullOrEmpty(strValue) && decimal.TryParse(strValue, out decValue))
        {
            SqlDecimal sqlDecimal = new SqlDecimal(decValue);
            precision = (int)sqlDecimal.Precision;
            scale = (int)sqlDecimal.Scale;
            if (precision <= precisionConst && scale <= scaleConst)
            {
                blnRes = true;
            }            
        }

        return blnRes;
    }
}
    