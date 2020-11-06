namespace Lab05_EDII.Models
{
    interface IKey
    {
        string word { get; set; }
        int levels { get; set; }
        int rows { get; set; }
        int columns { get; set; }
    }
}