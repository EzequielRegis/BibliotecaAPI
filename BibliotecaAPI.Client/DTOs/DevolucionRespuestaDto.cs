namespace BibliotecaAPI.Client.DTOs
{
    public class DevolucionRespuestaDto
    {
        public int Id { get; set; }
        public DateTime? FechaDevolucionReal { get; set; }
        public decimal Multa { get; set; }
    }

}