namespace Shimakaze.Struct.Ini.Options
{
    public class IniSetOption
    {
        /// <summary>
        /// Cover Data when the Exists
        /// </summary>
        public bool Cover { get; set; } = true;
        /// <summary>
        /// Create Data when the not Exists
        /// </summary>
        public bool Create { get; set; } = true;
    }
}