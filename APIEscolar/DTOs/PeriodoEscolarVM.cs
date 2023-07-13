namespace APIEscolar.DTOs
{
    public class PeriodoEscolarVM
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Abreviacion { get; set; }
        public string Status { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTemino { get; set; }
    }
}
