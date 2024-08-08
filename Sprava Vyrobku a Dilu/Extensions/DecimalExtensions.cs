namespace SpravaVyrobkuaDilu
{
    /// <summary>
    /// Solution for Decimal not having propper rounding
    /// </summary>
    public static class DecimalExtensions
    {
        public static decimal RoundUp(decimal input, int places)
        {
            decimal multiplier = (decimal)Math.Pow(10, places);
            decimal roundedValue = Math.Ceiling(input * multiplier) / multiplier;

            return roundedValue;
        }
    }
}
