namespace Lab05_EDII.Models
{
    public class Key : IKey
    {
        /// <summary>
        /// Palabra clave para cifrado Cesar
        /// </summary>
        public string word { get; set; }
        /// <summary>
        /// Cantidad de filas para cifrado ruta espiral
        /// </summary>
        public int rows { get; set; }
        /// <summary>
        /// Cantidad de columnas para cifrado ruta espiral
        /// </summary>
        public int columns { get; set; }
        /// <summary>
        /// Cantidad de niveles para cifrado Zig Zag
        /// </summary>
        public int levels { get; set; }
    }
}