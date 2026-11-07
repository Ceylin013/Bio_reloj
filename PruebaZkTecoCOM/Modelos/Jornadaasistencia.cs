namespace PruebaZkTecoCOM.Modelos;

public class JornadaAsistencia
{
    public string CodigoEmpleado { get; set; } = "";

    public DateTime FechaJornada { get; set; }

    public TimeSpan? HoraEntrada { get; set; }

    public TimeSpan? HoraSalida { get; set; }

    public decimal HorasTrabajadas { get; set; }

    public decimal HorasExtras { get; set; }
}