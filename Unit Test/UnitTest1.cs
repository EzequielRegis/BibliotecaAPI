using BibliotecaAPI.Entidades;
using BibliotecaAPI.Repositories;

namespace Unit_Test
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(23, 0)]
        [InlineData(24, 5000)]
        [InlineData(47, 5000)]
        [InlineData(48, 10000)]
        [InlineData(24.5, 5000)]
        [InlineData(-24, 0)]
        [InlineData(0, 0)]

        
        public void CalcularMulta_UsaPeriodosCompletosDe24Horas(
            double horasRetraso,
            decimal multaEsperada)
        {
            decimal multa = PrestamoRepository.CalcularMulta(horasRetraso);

            Assert.Equal(multaEsperada, multa);
        }

        [Fact]
        public void RegistrarDevolucion_Con23HorasDeRetraso_NoGeneraMulta()
        {
            var libro = new Libro
            {
                Id = 1,
                Titulo = "Libro de prueba"
            };

            var ejemplar = new Ejemplar
            {
                Id = 1,
                LibroId = libro.Id,
                Libro = libro,
                Disponible = false
            };

            var fechaPrevista = new DateTime(2026, 10, 1, 10, 0, 0);

            var prestamo = new Prestamo
            {
                Id = 1,
                EjemplarId = ejemplar.Id,
                Ejemplar = ejemplar,
                FechaPrestamo = fechaPrevista.AddDays(-14),
                FechaDevolucionPrevista = fechaPrevista,
                EsDevuelto = false
            };

            var repository = new PrestamoRepository(
                null!);

            repository.RegistrarDevolucion(
                prestamo,
                fechaPrevista.AddHours(23));

            Assert.Equal(0m, prestamo.Multa);
            Assert.True(prestamo.EsDevuelto);
            Assert.True(ejemplar.Disponible);
        }
















        [Fact]
        public void RegistrarDevolucion_Con24HorasDeRetraso_CalculaMultaYLiberaEjemplar()
        {
            var libro = new Libro
            {
                Id = 1,
                Titulo = "Libro de prueba"
            };

            var ejemplar = new Ejemplar
            {
                Id = 1,
                LibroId = libro.Id,
                Libro = libro,
                Disponible = false
            };

            var fechaPrevista = new DateTime(2026, 10, 1, 10, 0, 0);

            var prestamo = new Prestamo
            {
                Id = 1,
                EjemplarId = ejemplar.Id,
                Ejemplar = ejemplar,
                FechaPrestamo = fechaPrevista.AddDays(-14),
                FechaDevolucionPrevista = fechaPrevista,
                EsDevuelto = false
            };

            var repository = new PrestamoRepository(
                null!);

            repository.RegistrarDevolucion(
                prestamo,
                fechaPrevista.AddHours(24));

            Assert.Equal(5000m, prestamo.Multa);
            Assert.True(prestamo.EsDevuelto);
            Assert.True(ejemplar.Disponible);
        }
        [Fact]
        public void RegistrarDevolucion_Con48HorasDeRetraso_CalculaMultaYLiberaEjemplar()
        {
            var libro = new Libro
            {
                Id = 1,
                Titulo = "Libro de prueba"
            };

            var ejemplar = new Ejemplar
            {
                Id = 1,
                LibroId = libro.Id,
                Libro = libro,
                Disponible = false
            };

            var fechaPrevista = new DateTime(2026, 10, 1, 10, 0, 0);

            var prestamo = new Prestamo
            {
                Id = 1,
                EjemplarId = ejemplar.Id,
                Ejemplar = ejemplar,
                FechaPrestamo = fechaPrevista.AddDays(-14),
                FechaDevolucionPrevista = fechaPrevista,
                EsDevuelto = false
            };

            var repository = new PrestamoRepository(
                null!);

            repository.RegistrarDevolucion(
                prestamo,
                fechaPrevista.AddHours(48));

            Assert.Equal(10000m, prestamo.Multa);
            Assert.True(prestamo.EsDevuelto);
            Assert.True(ejemplar.Disponible);
        }
    }
}
