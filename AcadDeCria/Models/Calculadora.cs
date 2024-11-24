namespace AcadDeCria.Models
{
    public class Calculator
    {
        public int id { get; set; }
        public int? tipo_dano { get; set; }
        public int? tipo_veiculo { get; set; }
        public float? percent_valid { get; set; }
        public DateTime? created { get; set; }
        public float? resultado { get; set; }
        public float? valor_gasto { get; set; }
        public int? userid { get; set; }
        public void EnsureUtcDate()
        {
            if (created.HasValue && created.Value.Kind != DateTimeKind.Utc)
            {
                created = DateTime.SpecifyKind(created.Value, DateTimeKind.Utc);
            }
        }
    }
}
