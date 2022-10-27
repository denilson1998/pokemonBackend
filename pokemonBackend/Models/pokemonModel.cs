using System.ComponentModel.DataAnnotations;

namespace pokemonBackend.Models
{
    public class pokemonModel
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; } = string.Empty;
        public string season { get; set; } = string.Empty;
        public string partner { get; set; } = string.Empty;
        public string imageUrl { get; set; } = string.Empty; 
    }
}
