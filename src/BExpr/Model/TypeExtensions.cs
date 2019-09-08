namespace BExpr.Model
{
    public static class TypeExtensions
    {
        public static decimal? UpCastDecimal(this object value)
        {
            switch (value)
            {
                case decimal dec:
                    return dec;
                case double d:
                    return (decimal)d;
                case float f:
                    return (decimal)f;
                case long l:
                    return l;
                case int i:
                    return i;
                case short s:
                    return s;
                case byte b:
                    return b;
            }

            return null;
        }

        public static double? CastDouble(this object value)
        {
            if (value is decimal d)
                return (double)d;
            return value.UpCastDouble();
        }

        public static double? UpCastDouble(this object value)
        {
            switch (value)
            {
                case double d:
                    return d;
                case float f:
                    return f;
                case long l:
                    return l;
                case int i:
                    return i;
                case short s:
                    return s;
                case byte b:
                    return b;
            }

            return null;
        }

        public static long? UpCastLong(this object value)
        {
            switch (value)
            {
                case long l:
                    return l;
                case int i:
                    return i;
                case short s:
                    return s;
                case byte b:
                    return b;
            }

            return null;
        }

        public static int? UpCastInt(this object value)
        {
            switch (value)
            {
                case int i:
                    return i;
                case short s:
                    return s;
                case byte b:
                    return b;
            }

            return null;
        }
    }
}