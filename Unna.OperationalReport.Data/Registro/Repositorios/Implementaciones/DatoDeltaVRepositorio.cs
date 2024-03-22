using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones
{
    public class DatoDeltaVRepositorio : OperacionalRepositorio<DatoDeltaV, object>, IDatoDeltaVRepositorio
    {
        public DatoDeltaVRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public async Task EliminarDatosDeltaVAsync(long? idRegistroSupervisor)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Registro.DatosDeltaV WHERE IdRegistroSupervisor=@IdRegistroSupervisor";
                await conexion.QueryAsync(sql, new { IdRegistroSupervisor = idRegistroSupervisor }, commandType: CommandType.Text);
            }
        }
        public async Task GuardarDatosDeltaVAsync(DatoDeltaV entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.DatosDeltaV (Tanque,Nivel,Pres, Temp,IdRegistroSupervisor,Actualizado) VALUES(@Tanque,@Nivel,@Pres,@Temp,@IdRegistroSupervisor,@Actualizado)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        public async Task<List<DatoDeltaV>> BuscarDatosDeltaVAsync(long idRegistroSupervisor)
        {
            var lista = new List<DatoDeltaV>();
            var sql = "SELECT * FROM Registro.DatosDeltaV WHERE IdRegistroSupervisor=@IdRegistroSupervisor";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<DatoDeltaV>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroSupervisor = idRegistroSupervisor
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<List<DatoDeltaV>> BuscarDatosDeltaVPorDiaOperativoAsync(DateTime diaOperativo)
        {
            var lista = new List<DatoDeltaV>();
            var sql = "SELECT c.Producto,a.Tanque,a.Nivel FROM Registro.DatosDeltaV a INNER JOIN Reporte.RegistroSupervisor b ON a.IdRegistroSupervisor = b.IdRegistroSupervisor INNER JOIN Registro.Tanque c ON c.NroTanque = a.Tanque WHERE b.Fecha = CAST(@DiaOperativo AS DATE)";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<DatoDeltaV>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }




        public async Task EliminarVolumenDeltaVAsync(long? idRegistroSupervisor)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Registro.VolumeDeltaV WHERE IdRegistroSupervisor=@IdRegistroSupervisor";
                await conexion.QueryAsync(sql, new { IdRegistroSupervisor = idRegistroSupervisor }, commandType: CommandType.Text);
            }
        }

        public async Task GuardarVolumenDeltaVAsync(VolumenDeltaV entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.VolumeDeltaV (NombreLote,Volumen,IdRegistroSupervisor,Actualizado) VALUES(@NombreLote,@Volumen,@IdRegistroSupervisor,@Actualizado)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        public async Task<List<VolumenDeltaV>> BuscarVolumenDeltaVAsync(long idRegistroSupervisor)
        {
            var lista = new List<VolumenDeltaV>();
            var sql = "SELECT * FROM Registro.VolumeDeltaV WHERE IdRegistroSupervisor=@IdRegistroSupervisor";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<VolumenDeltaV>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroSupervisor = idRegistroSupervisor
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task EliminarProduccionDiariaMsAsync(long? idRegistroSupervisor)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Registro.ProduccionDiariaMs WHERE IdRegistroSupervisor=@IdRegistroSupervisor";
                await conexion.QueryAsync(sql, new { IdRegistroSupervisor = idRegistroSupervisor }, commandType: CommandType.Text);
            }
        }
        public async Task GuardarProduccionDiariaMsAsync(ProduccionDiariaMs entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.ProduccionDiariaMs (Producto,MedidoresMasicos,Actualizado,IdRegistroSupervisor) VALUES(@Producto,@MedidoresMasicos,@Actualizado,@IdRegistroSupervisor)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        public async Task<List<ProduccionDiariaMs>> BuscarProduccionDiariaMsAsync(long idRegistroSupervisor)
        {
            var lista = new List<ProduccionDiariaMs>();
            var sql = "SELECT * FROM Registro.ProduccionDiariaMs WHERE IdRegistroSupervisor=@IdRegistroSupervisor";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ProduccionDiariaMs>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroSupervisor = idRegistroSupervisor
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }


        public async Task EliminarGnsVolumeMsYPcBrutoAsync(long? idRegistroSupervisor, string? tipo)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Registro.GnsVolumeMsYPcBruto WHERE IdRegistroSupervisor = @IdRegistroSupervisor AND Tipo=@Tipo";
                await conexion.QueryAsync(sql, new { IdRegistroSupervisor  = idRegistroSupervisor, Tipo = tipo}, commandType: CommandType.Text);
            }
        }
        public async Task GuardarGnsVolumeMsYPcBrutoAsync(GnsVolumeMsYPcBruto entidad)
        {           

            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.GnsVolumeMsYPcBruto (Nombre,VolumeMs,PcBrutoRepCroma,Tipo,Actualizado,IdRegistroSupervisor) VALUES(@Nombre,@VolumeMs,@PcBrutoRepCroma,@Tipo,@Actualizado,@IdRegistroSupervisor)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        public async Task<List<GnsVolumeMsYPcBruto>> BuscarGnsVolumeMsYPcBrutoAsync(long idRegistroSupervisor,string? tipo)
        {
            var lista = new List<GnsVolumeMsYPcBruto>();
            var sql = "SELECT * FROM Registro.GnsVolumeMsYPcBruto WHERE IdRegistroSupervisor = @IdRegistroSupervisor AND Tipo=@Tipo";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<GnsVolumeMsYPcBruto>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroSupervisor = idRegistroSupervisor,
                        Tipo = tipo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }



        public async Task EliminarDatosCgnAsync(long? idRegistroSupervisor)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Registro.DatoCgn WHERE IdRegistroSupervisor = @IdRegistroSupervisor";
                await conexion.QueryAsync(sql, new { IdRegistroSupervisor = idRegistroSupervisor }, commandType: CommandType.Text);
            }
        }
        public async Task GuardarDatosCgnAsync(DatoCgn entidad)
        {

            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.DatoCgn (Tanque,Centaje,Volumen,Actualizado,IdRegistroSupervisor) VALUES(@Tanque,@Centaje,@Volumen,@Actualizado,@IdRegistroSupervisor)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        public async Task<List<DatoCgn>> BuscarDatosCgnAsync(long idRegistroSupervisor)
        {
            var lista = new List<DatoCgn>();
            var sql = "SELECT * FROM Registro.DatoCgn WHERE IdRegistroSupervisor = @IdRegistroSupervisor";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<DatoCgn>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroSupervisor = idRegistroSupervisor
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }



        public async Task EliminarVolumenDeDespachoAsync(long? idRegistroSupervisor, string? tipo)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Registro.VolumenDespacho WHERE IdRegistroSupervisor = @IdRegistroSupervisor AND Tipo=@Tipo";
                await conexion.QueryAsync(sql, new { IdRegistroSupervisor = idRegistroSupervisor, Tipo = tipo }, commandType: CommandType.Text);
            }
        }
        public async Task GuardarVolumenDeDespachoAsync(VolumenDespacho entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.VolumenDespacho (Tanque,Cliente,Placa,Volumen,Tipo,Actualizado,IdRegistroSupervisor) VALUES(@Tanque,@Cliente,@Placa,@Volumen,@Tipo,@Actualizado,@IdRegistroSupervisor)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        public async Task<List<VolumenDespacho>> BuscarVolumenDeDespachoAsync(long idRegistroSupervisor, string? tipo)
        {
            var lista = new List<VolumenDespacho>();
            var sql = "SELECT * FROM Registro.VolumenDespacho WHERE IdRegistroSupervisor = @IdRegistroSupervisor AND Tipo=@Tipo";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<VolumenDespacho>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroSupervisor = idRegistroSupervisor,
                        Tipo = tipo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }


        public async Task EliminarDespachoGlpEnvasadoAsync(long? idRegistroSupervisor)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Registro.DespachoGlpEnvasado WHERE IdRegistroSupervisor = @IdRegistroSupervisor";
                await conexion.QueryAsync(sql, new { IdRegistroSupervisor = idRegistroSupervisor}, commandType: CommandType.Text);
            }
        }
        public async Task GuardarDespachoGlpEnvasadoAsync(DespachoGlpEnvasado entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.DespachoGlpEnvasado (Nombre,Envasado,Granel,Actualizado,IdRegistroSupervisor) VALUES(@Nombre,@Envasado,@Granel,@Actualizado,@IdRegistroSupervisor)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public async Task<List<DespachoGlpEnvasado>> BuscarDespachoGlpEnvasadoAsync(long idRegistroSupervisor)
        {
            var lista = new List<DespachoGlpEnvasado>();
            var sql = "SELECT * FROM Registro.DespachoGlpEnvasado WHERE IdRegistroSupervisor = @IdRegistroSupervisor";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<DespachoGlpEnvasado>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroSupervisor = idRegistroSupervisor
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }


    }
}
