using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApi.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text {  get; set; }
        public int Rating {  get; set; }
        public Reviewer Reviewer {  get; set; }
        [ForeignKey("Reviewer")]
        public int ReviewrId {  get; set; }
        public Pokemon Pokemon {  get; set; }
        [ForeignKey("Pokemon")]
        public int PokemonId {  get; set; }
    }
}
